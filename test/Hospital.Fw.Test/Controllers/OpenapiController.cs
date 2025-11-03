using System.Text;
using Hospital.Fw.OpenApiToTsClient;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.Test.Controllers;

[ApiController]
[Route("/api/open")]
public class OpenapiController : ControllerBase
{
    [HttpGet]
    public async Task<List<PathAndContent>> Test()
    {
        var openApiToTypeScriptConverter = new OpenApiToTypeScriptConverter();
        var tsClientCode =
            await openApiToTypeScriptConverter.ConvertFromUrlAsync("http://localhost:5268/swagger/v1/swagger.json");
        var tsClientCodes = tsClientCode.Select(x => new PathAndContent(x.Path, x.Content)).ToList();
        
        var path = Path.Combine(AppContext.BaseDirectory, "ts");
        foreach (var item in tsClientCodes)
        {
            var filePath = Path.Combine(path, item.Path);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? string.Empty);
            await System.IO.File.WriteAllTextAsync(filePath, item.Content, Encoding.UTF8);
        }
        
        
        return tsClientCodes;
    }
}

public record PathAndContent(string Path, string Content);  