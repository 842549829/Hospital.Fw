# 模板



## 入门

为了让您轻松开始使用 GitLab，这里列出了推荐的后续步骤。

已经是专业人士了？只需编辑此 README.md 并使其成为您自己的。想轻松一点吗 [使用底部的模板](#editing-this-readme)!

## 模板制作
- 创建文件夹Template
- 在Template下创建template.csproj，内容如下
```
  <Project Sdk="Microsoft.NET.Sdk">
  
    <PropertyGroup>
      <PackageType>Template</PackageType>
      <PackageVersion>1.0.0</PackageVersion>
      <PackageId>KnownTemplate</PackageId>
      <Title>KnownTemplate</Title>
      <Authors>KnownChen</Authors>
      <Description>Project Template for Known.</Description>
      <PackageTags>dotnet-new;templates;Known</PackageTags>
  
      <TargetFramework>net7.0</TargetFramework>
  
      <IncludeContentInPack>true</IncludeContentInPack>
      <IncludeBuildOutput>false</IncludeBuildOutput>
      <ContentTargetFolders>content</ContentTargetFolders>
      <NoWarn>$(NoWarn);NU5128</NoWarn>
    </PropertyGroup>
  
    <ItemGroup>
      <Content Include="Template\**\*" Exclude="Template\**\bin\**;Template\**\obj\**" />
      <Compile Remove="**\*" />
    </ItemGroup>
  
  </Project>
```
- 在Template下再创建文件夹Template，把解决方案模板复制进去(即本项目git clone 下的所有内容全部拷贝)
- 注意：解决方案所有文件不要使用Template词语，否则创建项目时将文件中的Template替换成项目名称
- 在Template/Template文件夹在创建.template.config文件夹
- 在.template.config文件夹下创建template.json文件，内容如下
```
{
  "$schema": "http://json.schemastore.org/template",
  "author": "KnownChen",
  "classifications": [ "Template" ],
  "name": "KnownTemplate",
  "identity": "KnownTemplate", 
  "shortName": "known",
  "tags": {
    "language": "C#" 
  },
  "sourceName": "Template" /*此处与解决方案名称一致，安装项目时自动替换成项目名称*/
}
```
- 本项目已经有了.template.config 和 template.json 根据需求自行修改

## 打包生成nuget模板包
命令行进入template.csproj文件所在的Template目录，输入如下命令回车
```
dotnet pack
```
## 发布和安装模板包
将生成的KnownTemplate.1.0.0.nupkg包复制到本地nuget源或上传到nuget网站
上传到nuget服务器
```
dotnet nuget push KnownTemplate.nupkg -k xxxxxxx -s http://xxxxxxxxxxxx  
```

```
# 如果已安装则卸载
dotnet new uninstall KnownTemplate
dotnet new install KnownTemplate::1.0.2 --add-source http://xxxxxxxxxxxx  --force     
#如果sdk是.net6.0以下则用
dotnet new --install KnownTemplate::1.0.2 --add-source http://xxxxxxxxxxxx  --force     

```

## 安装步骤

```
# 查看所有的模板
dotnet new list

# 如果已安装则卸载
dotnet new uninstall HisTemplate

#安装
dotnet new install HisTemplate::1.0.0 --add-source http://192.168.5.245:8885/nuget  --force   

# 根据模板创建项目
dotnet new his --name= ValueCore
```

