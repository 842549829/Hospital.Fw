# 获取脚本所在目录
$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

# 设置输出目录
$outputDir = "C:\Users\Administrator\Desktop\pack"

# 创建输出目录（如果不存在）
if (-not (Test-Path -Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir
}

# 定义要处理的父级目录
$parentDirs = @("src", "custom", "module")

foreach ($parent in $parentDirs) {
    # 每次循环父目录前，确保当前路径是脚本所在目录
    Set-Location $scriptDir

    if (-not (Test-Path -Path $parent)) {
        Write-Host "Parent directory not found: $parent" -ForegroundColor Yellow
        continue
    }

    Get-ChildItem -Path $parent -Directory | ForEach-Object {
        $projectDir = $_.FullName
        Write-Host "Entering directory: $projectDir" -ForegroundColor Cyan

        # 进入子目录并打包
        Set-Location $projectDir
        dotnet pack -c release -o $outputDir

        if ($LASTEXITCODE -eq 0) {
            Write-Host "Packing succeeded: $projectDir`n" -ForegroundColor Green
        } else {
            Write-Host "Packing failed: $projectDir`n" -ForegroundColor Red
        }

        # 打包完成后，切换回脚本所在目录
        Set-Location $scriptDir
    }
}

Write-Host "All projects have been packed successfully!"