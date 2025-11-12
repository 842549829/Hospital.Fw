using System.Text;
using System.Text.Json;

namespace Hospital.Fw.OpenApiToTsClient;

/// <summary>
/// 将 OpenAPI JSON 转换为 TypeScript 模块（按 tags 分组，生成 index.ts 和 type.ts）
/// 输出格式: (relativeFilePath, fileContent)
/// </summary>
public class OpenApiToTypeScriptConverter
{
    /// <summary>
    /// http 客户端
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// 构造函数
    /// </summary>
    public OpenApiToTypeScriptConverter()
    {
        _httpClient = new HttpClient();
    }
    
    /// <summary>
    /// 从 URL 异步加载 OpenAPI 文档并生成 TypeScript 客户端代码
    /// </summary>
    /// <returns>TypeScript 代码字符串</returns>
    public async Task<List<(string Path, string Content)>> ConvertFromUrlAsync(string openApiUrl)
    {
        if (string.IsNullOrWhiteSpace(openApiUrl))
        {
            throw new ArgumentException("OpenAPI URL cannot be null or empty.", nameof(openApiUrl));
        }

        var jsonText = await _httpClient.GetStringAsync(openApiUrl);
        return await ConvertAsync(jsonText);
    }
    
    /// <summary>
    /// 转换 OpenAPI JSON 为多个 TypeScript 文件内容
    /// </summary>
    /// <param name="openApiJson">OpenAPI JSON 字符串</param>
    /// <returns>文件路径与内容的元组列表</returns>
    private static Task<List<(string Path, string Content)>> ConvertAsync(string openApiJson)
    {
        if (string.IsNullOrWhiteSpace(openApiJson))
        {
            throw new ArgumentException("OpenAPI JSON cannot be null or empty.", nameof(openApiJson));
        }

        using var doc = JsonDocument.Parse(openApiJson);
        var root = doc.RootElement;

        var result = new List<(string, string)>();

        // 缓存所有 schema 名称用于 $ref 解析
        var schemaRefs = new Dictionary<string, string>();
        ExtractSchemaNames(root, schemaRefs);

        // 提取所有 schemas 并按 tag 分组 API
        var schemas = root.GetPropertyOrNull("components")?.GetPropertyOrNull("schemas");
        var paths = root.GetPropertyOrNull("paths");

        // 存储每个 tag 的操作
        var tagGroups = new Dictionary<string, List<ApiOperation>>();

        if (paths.HasValue)
        {
            foreach (var pathItem in paths.Value.EnumerateObject())
            {
                var route = pathItem.Name;
                var methods = pathItem.Value;

                foreach (var method in methods.EnumerateObject())
                {
                    var httpMethod = method.Name.ToUpperInvariant();
                    var operation = method.Value;

                    var tags = new List<string>();
                    if (operation.TryGetProperty("tags", out var tagsElement) && tagsElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var t in tagsElement.EnumerateArray())
                        {
                            tags.Add(t.GetString() ?? "default");
                        }
                    }
                    else
                    {
                        tags.Add("default");
                    }

                    var operationId = operation.GetPropertyOrNull("operationId")?.GetString();
                    var summary = operation.GetPropertyOrNull("summary")?.GetString() ?? "";

                    // 解析参数、请求体、响应
                    var parameters = operation.GetPropertyOrNull("parameters");
                    var requestBody = operation.GetPropertyOrNull("requestBody");
                    var responses = operation.GetPropertyOrNull("responses");

                    // 构建 API 操作模型
                    var apiOp = new ApiOperation
                    {
                        HttpMethod = httpMethod,
                        Route = route,
                        OperationId = operationId,
                        Summary = summary,
                        Parameters = parameters,
                        RequestBody = requestBody,
                        Responses = responses
                    };

                    foreach (var tag in tags)
                    {
                        if (!tagGroups.ContainsKey(tag))
                            tagGroups[tag] = new List<ApiOperation>();
                        tagGroups[tag].Add(apiOp);
                    }
                }
            }
        }

        // 为每个 tag 生成文件
        foreach (var (tagName, operations) in tagGroups)
        {
            var tagDir = ToKebabOrCamelCase(tagName); // 可选：转为 kebab-case 或 camelCase

            // 1. 生成 type.ts
            var typeBuilder = new StringBuilder();
            GenerateTypeFile(typeBuilder, operations, schemas, schemaRefs);
            result.Add(($"{tagDir}/type.ts", typeBuilder.ToString()));

            // 2. 生成 index.ts
            var indexBuilder = new StringBuilder();
            GenerateIndexFile(indexBuilder, tagName, operations, schemaRefs);
            result.Add(($"{tagDir}/index.ts", indexBuilder.ToString()));
        }

        // 可选：生成根 index.ts 导出所有模块
        var rootIndex = GenerateRootIndex(tagGroups.Keys);
        result.Add(($"index.ts", rootIndex));

        return Task.FromResult(result);
    }

    #region Helper Methods

    private static string ToKebabOrCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        // 转为小驼峰命名，作为目录名（也可改为 kebab-case）
        var camel = char.ToLower(input[0]) + input.Substring(1);
        return camel.Replace(" ", "").Replace("-", "");
    }

    private static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToLower(input[0]) + input.Substring(1);
    }

    private static string GetResourceNameFromPath(string route)
    {
        // 移除前缀和参数
        var clean = route.Replace("/api", "").Replace("/v1", "").Replace("/v2", "");
        var segments = clean.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0) return "resource";

        // 取最后一个非参数段作为资源名
        for (int i = segments.Length - 1; i >= 0; i--)
        {
            var seg = segments[i];
            if (!seg.StartsWith("{") && !seg.EndsWith("}"))
            {
                return ToPascalCase(seg);
            }
        }

        return "Resource";
    }

    private static string MapJsonType(string type) => type?.ToLower() switch
    {
        "string" => "string",
        "number" => "number",
        "integer" => "number",
        "boolean" => "boolean",
        "object" => "object",
        "array" => "any[]",
        _ => "any"
    };

    private static void ExtractSchemaNames(JsonElement root, Dictionary<string, string> schemaRefs)
    {
        var schemas = root.GetPropertyOrNull("components")?.GetPropertyOrNull("schemas");
        if (!schemas.HasValue) return;

        foreach (var prop in schemas.Value.EnumerateObject())
        {
            var refKey = $"#/components/schemas/{prop.Name}";
            schemaRefs[refKey] = prop.Name;
        }
    }

    private static string ResolveSchemaType(JsonElement schema, Dictionary<string, string> schemaRefs)
    {
        if (schema.TryGetProperty("$ref", out var refToken))
        {
            var refPath = refToken.GetString();
            return schemaRefs.TryGetValue(refPath, out var typeName) ? typeName : "any";
        }

        var type = schema.GetPropertyOrNull("type")?.GetString();
        if (type == "array" && schema.TryGetProperty("items", out var items))
        {
            var itemType = ResolveSchemaType(items, schemaRefs);
            return $"{itemType}[]";
        }

        return MapJsonType(type) ?? "any";
    }

    #endregion

    #region Type File Generation

    private static void GenerateTypeFile(StringBuilder builder, List<ApiOperation> operations,
        JsonElement? schemas, Dictionary<string, string> schemaRefs)
    {
        builder.AppendLine("// Auto-generated types from OpenAPI schemas and operations");
        builder.AppendLine("// Do not edit manually");
        builder.AppendLine("");

        var usedTypes = new HashSet<string>();

        // 收集所有用到的 schema 名称
        foreach (var op in operations)
        {
            // 请求参数类型
            if (op.Parameters.HasValue)
            {
                foreach (var p in op.Parameters.Value.EnumerateArray())
                {
                    var schema = p.GetPropertyOrNull("schema");
                    var type = ResolveSchemaType(schema!.Value, schemaRefs);
                    if (!type.Contains("[]") && !type.StartsWith("string") && !type.StartsWith("number") && type != "any")
                        usedTypes.Add(type);
                }
            }

            // 请求体类型
            if (op.RequestBody.HasValue)
            {
                var content = op.RequestBody.Value.GetPropertyOrNull("content");
                if (content.HasValue)
                {
                    var jsonContent = content.Value
                                          .GetPropertyOrNull("application/json") ??
                                      content.Value.EnumerateObject().FirstOrDefault().Value;

                    var schema = jsonContent.GetPropertyOrNull("schema");
                    if (schema.HasValue)
                    {
                        var tsType = ResolveSchemaType(schema.Value, schemaRefs);
                        if (!tsType.Contains("[]") && !tsType.StartsWith("string") && tsType != "any")
                            usedTypes.Add(tsType);
                    }
                }
            }

            // 响应类型
            var successResponse = op.Responses?.GetPropertyOrNull("200") ??
                                  op.Responses?.GetPropertyOrNull("201") ??
                                  op.Responses?.GetPropertyOrNull("default");
            if (successResponse.HasValue)
            {
                var respContent = successResponse.Value.GetPropertyOrNull("content");
                if (respContent.HasValue)
                {
                    var jsonResp = respContent.Value
                                           .GetPropertyOrNull("application/json") ??
                                       respContent.Value.EnumerateObject().FirstOrDefault().Value;

                    var schema = jsonResp.GetPropertyOrNull("schema");
                    if (schema.HasValue)
                    {
                        var tsType = ResolveSchemaType(schema.Value, schemaRefs);
                        if (!tsType.Contains("[]") && !tsType.StartsWith("string") && tsType != "any")
                            usedTypes.Add(tsType);
                    }
                }
            }
        }

        // 从 schemas 中提取这些类型
        if (schemas.HasValue)
        {
            foreach (var prop in schemas.Value.EnumerateObject())
            {
                var name = prop.Name;
                if (usedTypes.Contains(name))
                {
                    GenerateTsInterface(builder, name, prop.Value, schemaRefs);
                    builder.AppendLine();
                }
            }
        }

        // 添加通用分页类型（如果需要）
        if (usedTypes.Any(t => t.Contains("PageListDto")))
        {
            builder.AppendLine("export interface PageListDto<T> {");
            builder.AppendLine("  total: number;");
            builder.AppendLine("  list: T[];");
            builder.AppendLine("  pageNum: number;");
            builder.AppendLine("  pageSize: number;");
            builder.AppendLine("}");
            builder.AppendLine();
        }
    }

    private static void GenerateTsInterface(StringBuilder builder, string name, JsonElement schema, Dictionary<string, string> schemaRefs)
    {
        builder.AppendLine($"export interface {name} {{");

        var props = schema.GetPropertyOrNull("properties");
        var requiredProps = new HashSet<string>();
        var requiredArray = schema.GetPropertyOrNull("required");
        if (requiredArray.HasValue)
        {
            foreach (var r in requiredArray.Value.EnumerateArray())
            {
                requiredProps.Add(r.GetString());
            }
        }

        if (props.HasValue)
        {
            foreach (var prop in props.Value.EnumerateObject())
            {
                var propName = prop.Name;
                var propSchema = prop.Value;
                var type = ResolveSchemaType(propSchema, schemaRefs);
                var isRequired = requiredProps.Contains(propName);
                builder.AppendLine($"  {propName}{(isRequired ? "" : "?")}: {type};");
            }
        }
        else
        {
            builder.AppendLine("  [key: string]: any;");
        }

        builder.AppendLine("}");
    }

    #endregion

    #region Index File Generation

    private static void GenerateIndexFile(StringBuilder builder, string tagName, List<ApiOperation> operations, Dictionary<string, string> schemaRefs)
    {
        var camelTag = ToCamelCase(tagName);

        builder.AppendLine("// Auto-generated API functions");
        builder.AppendLine("// Do not edit manually");
        builder.AppendLine("import request from '@/axios';");
        builder.AppendLine($"import ApiConfig from '@/api/common/apiConfig';");
        builder.AppendLine($"import {{");
        var importedTypes = new HashSet<string>();

        foreach (var op in operations)
        {
            // 请求参数类型
            if (op.Parameters.HasValue)
            {
                foreach (var p in op.Parameters.Value.EnumerateArray())
                {
                    var schema = p.GetPropertyOrNull("schema");
                    var type = ResolveSchemaType(schema!.Value, schemaRefs);
                    if (type != "string" && type != "number" && type != "boolean" && !type.Contains("[]") && type != "any")
                        importedTypes.Add(type);
                }
            }

            // 请求体类型
            if (op.RequestBody.HasValue)
            {
                var content = op.RequestBody.Value.GetPropertyOrNull("content");
                if (content.HasValue)
                {
                    var jsonContent = content.Value
                                          .GetPropertyOrNull("application/json") ??
                                      content.Value.EnumerateObject().FirstOrDefault().Value;

                    var schema = jsonContent.GetPropertyOrNull("schema");
                    if (schema.HasValue)
                    {
                        var tsType = ResolveSchemaType(schema.Value, schemaRefs);
                        if (!tsType.Contains("[]") && tsType != "any")
                            importedTypes.Add(tsType);
                    }
                }
            }

            // 响应类型
            var successResponse = op.Responses?.GetPropertyOrNull("200") ??
                                  op.Responses?.GetPropertyOrNull("201") ??
                                  op.Responses?.GetPropertyOrNull("default");
            if (successResponse.HasValue)
            {
                var respContent = successResponse.Value.GetPropertyOrNull("content");
                if (respContent.HasValue)
                {
                    var jsonResp = respContent.Value
                                           .GetPropertyOrNull("application/json") ??
                                       respContent.Value.EnumerateObject().FirstOrDefault().Value;

                    var schema = jsonResp.GetPropertyOrNull("schema");
                    if (schema.HasValue)
                    {
                        var tsType = ResolveSchemaType(schema.Value, schemaRefs);
                        if (!tsType.Contains("[]") && tsType != "any")
                            importedTypes.Add(tsType);
                    }
                }
            }
        }

        // 导入分页类型
        if (importedTypes.Any(t => t.StartsWith("PageListDto<")))
        {
            builder.AppendLine("  PageListDto,");
        }

        if (importedTypes.Count > 0)
        {
            builder.AppendLine(string.Join(", ", importedTypes.OrderBy(x => x)));
        }

        builder.AppendLine("} from './type';");
        builder.AppendLine("");

        foreach (var op in operations)
        {
            var route = op.Route;
            var httpMethod = op.HttpMethod.ToLower();
            var operationId = op.OperationId;

            // 构造函数名：动词 + 资源名 + Api
            var resourceName = GetResourceNameFromPath(route);
            var verb = httpMethod switch
            {
                "get" => "get",
                "post" => "post",
                "put" => "put",
                "delete" => "delete",
                "patch" => "patch",
                _ => "call"
            };

            var methodName = $"{verb}{resourceName}Api";
            var pascalMethodName = char.ToUpper(methodName[0]) + methodName.Substring(1);

            // 参数列表
            var paramList = new List<string>();
            var usedNames = new HashSet<string>();

            // 查询/路径参数
            if (op.Parameters.HasValue)
            {
                foreach (var p in op.Parameters.Value.EnumerateArray())
                {
                    var name = p.GetPropertyOrNull("name")?.GetString() ?? "param";
                    var originalName = name;
                    var suffix = 1;
                    while (usedNames.Contains(name))
                        name = originalName + suffix++;
                    usedNames.Add(name);

                    var location = p.GetPropertyOrNull("in")?.GetString();
                    var required = p.GetPropertyOrNull("required")?.GetBoolean() == true;
                    var schema = p.GetPropertyOrNull("schema");
                    var type = ResolveSchemaType(schema!.Value, schemaRefs);

                    paramList.Add($"{name}{(required ? "" : "?")}: {type}");
                }
            }

            // 请求体
            string bodyType = null;
            if (op.RequestBody.HasValue)
            {
                var content = op.RequestBody.Value.GetPropertyOrNull("content");
                if (content.HasValue)
                {
                    var jsonContent = content.Value
                                          .GetPropertyOrNull("application/json") ??
                                      content.Value.EnumerateObject().FirstOrDefault().Value;

                    var schema = jsonContent.GetPropertyOrNull("schema");
                    if (schema.HasValue)
                    {
                        bodyType = ResolveSchemaType(schema.Value, schemaRefs);
                        paramList.Add($"data: {bodyType}");
                    }
                }
            }

            // 返回类型
            var returnType = "any";
            var successResponse = op.Responses?.GetPropertyOrNull("200") ??
                                  op.Responses?.GetPropertyOrNull("201") ??
                                  op.Responses?.GetPropertyOrNull("default");
            if (successResponse.HasValue)
            {
                var respContent = successResponse.Value.GetPropertyOrNull("content");
                if (respContent.HasValue)
                {
                    var jsonResp = respContent.Value
                                           .GetPropertyOrNull("application/json") ??
                                       respContent.Value.EnumerateObject().FirstOrDefault().Value;

                    var schema = jsonResp.GetPropertyOrNull("schema");
                    if (schema.HasValue)
                    {
                        returnType = ResolveSchemaType(schema.Value, schemaRefs);
                    }
                }
            }

            // 方法注释
            if (!string.IsNullOrEmpty(op.Summary))
            {
                builder.AppendLine($"/**");
                builder.AppendLine($" * {op.Summary}");
                builder.AppendLine($" * @{httpMethod} {route}");
                builder.AppendLine($" */");
            }

            // 函数签名
            builder.AppendLine($"export const {methodName} = ({string.Join(", ", paramList)}): Promise<{returnType}> => {{");

            // 构造请求
            builder.AppendLine($"  return request.{httpMethod}({{");
            builder.AppendLine($"    url: ApiConfig.{camelTag}.{pascalMethodName},");
            if (httpMethod == "get" && usedNames.Count > 0)
            {
                builder.AppendLine("    params,");
            }
            else if (bodyType != null)
            {
                builder.AppendLine("    data,");
            }
            builder.AppendLine("  });");

            builder.AppendLine("};");
            builder.AppendLine();
        }
    }

    #endregion

    #region Root Index Generation

    private static string GenerateRootIndex(IEnumerable<string> tags)
    {
        var builder = new StringBuilder();
        builder.AppendLine("// Export all API modules");
        builder.AppendLine("// Do not edit manually");
        foreach (var tag in tags)
        {
            var dir = ToKebabOrCamelCase(tag);
            builder.AppendLine($"export * from './{dir}';");
        }
        return builder.ToString();
    }

    #endregion

    #region Models

    private class ApiOperation
    {
        public required string HttpMethod { get; init; }
        public required string Route { get; init; }
        public string? OperationId { get; init; }
        public required string Summary { get; init; }
        public JsonElement? Parameters { get; init; }
        public JsonElement? RequestBody { get; init; }
        public JsonElement? Responses { get; init; }
    }

    #endregion
}