
Write-Host -ForegroundColor Blue "Test1"
dotnet msbuild testdata.targets /t:Test1

Write-Host -ForegroundColor Blue "Test1-1"
dotnet msbuild testdata.targets /t:test1-1

Write-Host -ForegroundColor Blue "Test1-2"
dotnet msbuild testdata.targets /t:test1-2

Write-Host -ForegroundColor Blue "Test1-3"
dotnet msbuild testdata.targets /t:Test1-3

Write-Host -ForegroundColor Blue "Test2"
dotnet msbuild testdata.targets /t:Test2

Write-Host -ForegroundColor Blue "Test2-1"
dotnet msbuild testdata.targets /t:Test2-1

Write-Host -ForegroundColor Blue "Test2-2"
dotnet msbuild testdata.targets /t:Test2-2

Write-Host -ForegroundColor Blue "Test3"
dotnet msbuild testdata.targets /t:Test3

Write-Host -ForegroundColor Blue "Test3-1"
dotnet msbuild testdata.targets /t:Test3-1

Write-Host -ForegroundColor Blue "Test3-2"
dotnet msbuild testdata.targets /t:Test3-2

Write-Host -ForegroundColor Blue "Test4"
dotnet msbuild testdata.targets /t:Test4