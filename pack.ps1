# 获取脚本所在目录
$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

# 设置输出目录
$outputDir = "C:\Users\Administrator\Desktop\pack"

# 创建输出目录（如果不存在）
if (-not (Test-Path -Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force # 使用 -Force 确保必要时创建父目录
}

# 定义要处理的父级目录
$parentDirs = @("src", "custom", "module")
# 定义需要再深入一层的特殊子目录名
$specialSubDir = "automaticApi"

foreach ($parent in $parentDirs) {
    # 每次循环父目录前，确保当前路径是脚本所在目录
    Set-Location $scriptDir

    $fullParentPath = Join-Path -Path $scriptDir -ChildPath $parent

    if (-not (Test-Path -Path $fullParentPath)) {
        Write-Host "Parent directory not found: $fullParentPath" -ForegroundColor Yellow
        continue
    }

    Write-Host "`n--- Processing Parent Directory: $fullParentPath ---" -ForegroundColor Magenta

    # 获取当前父目录下的所有子目录
    $childDirs = Get-ChildItem -Path $fullParentPath -Directory

    foreach ($childDirInfo in $childDirs) {
        $childDirPath = $childDirInfo.FullName

        # 检查当前子目录是否是特殊子目录 (只在 'module' 目录下检查)
        if ($childDirInfo.Name -eq $specialSubDir -and $parent -eq "module") {
            Write-Host "`nFound special directory '$specialSubDir' in '$parent'. Processing its subdirectories..." -ForegroundColor Yellow

            # 遍历 specialSubDir 下的子目录
            $specialSubDirPath = $childDirPath
            Get-ChildItem -Path $specialSubDirPath -Directory | ForEach-Object {
                $projectDir = $_.FullName
                Write-Host "`nEntering directory for packing: $projectDir" -ForegroundColor Cyan

                # 进入项目子目录并打包
                Set-Location $projectDir
                # 尝试获取 .csproj 文件以确认是否为项目目录
                $csprojFiles = Get-ChildItem -Path $projectDir -Filter "*.csproj" -File
                if ($csprojFiles.Count -gt 0) {
                    Write-Host "  Found .csproj file(s), proceeding with packing..." -ForegroundColor Green
                    dotnet pack -c release -o $outputDir
                } else {
                    Write-Host "  No .csproj file found in $projectDir, skipping." -ForegroundColor Yellow
                }

                if ($LASTEXITCODE -eq 0) {
                    Write-Host "Packing succeeded: $projectDir" -ForegroundColor Green
                } else {
                    Write-Host "Packing failed: $projectDir" -ForegroundColor Red
                }
                # 打包完成后，切换回脚本所在目录
                Set-Location $scriptDir
            }
        } else {
            # 如果不是特殊子目录，或者不是在 'module' 目录下，则直接将其作为项目目录处理
            Write-Host "`nProcessing regular directory: $childDirPath" -ForegroundColor Cyan

            # 进入子目录并打包
            Set-Location $childDirPath
            # 尝试获取 .csproj 文件以确认是否为项目目录
            $csprojFiles = Get-ChildItem -Path $childDirPath -Filter "*.csproj" -File
            if ($csprojFiles.Count -gt 0) {
                Write-Host "  Found .csproj file(s), proceeding with packing..." -ForegroundColor Green
                dotnet pack -c release -o $outputDir
            } else {
                Write-Host "  No .csproj file found in $childDirPath, skipping." -ForegroundColor Yellow
            }

            if ($LASTEXITCODE -eq 0) {
                Write-Host "Packing succeeded: $childDirPath" -ForegroundColor Green
            } else {
                Write-Host "Packing failed: $childDirPath" -ForegroundColor Red
            }
            # 打包完成后，切换回脚本所在目录
            Set-Location $scriptDir
        }
    }
}

# 最后，再次确保回到脚本目录
Set-Location $scriptDir

Write-Host "`nAll projects have been packed successfully!" -ForegroundColor Green