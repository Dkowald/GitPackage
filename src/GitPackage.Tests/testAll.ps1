
Write-Host -ForegroundColor Blue "----------------"
Write-Host -ForegroundColor Blue "Test Run started"
Write-Host -ForegroundColor Blue "----------------"

Write-Host -ForegroundColor Blue "AddAndRemoveVersioned"
dotnet msbuild Tests.proj /t:AddAndRemoveVersioned

Write-Host -ForegroundColor Blue "AddAndRemoveUnversioned"
dotnet msbuild Tests.proj /t:AddAndRemoveUnversioned

Write-Host -ForegroundColor Blue "RefreshUnversioned"
dotnet msbuild Tests.proj /t:RefreshUnversioned

Write-Host -ForegroundColor Blue "RefreshVersioned"
dotnet msbuild Tests.proj /t:RefreshVersioned

Write-Host -ForegroundColor Blue "AutoRefreshUnversioned"
dotnet msbuild Tests.proj /t:AutoRefreshUnversioned

Write-Host -ForegroundColor Blue "AutoRefreshVersioned"
dotnet msbuild Tests.proj /t:AutoRefreshVersioned

Write-Host -ForegroundColor Blue "VersionedPackageUseFetchToGetNewVersion"
dotnet msbuild Tests.proj /t:VersionedPackageUseFetchToGetNewVersion

Write-Host -ForegroundColor Blue "RestoreUnversionedDoesNotFetch"
dotnet msbuild Tests.proj /t:RestoreUnversionedDoesNotFetch

Write-Host -ForegroundColor Blue "PackageUsedInMultipleProjects"
dotnet msbuild Tests.proj /t:PackageUsedInMultipleProjects

Write-Host -ForegroundColor Blue "UseFetchToGetSpecifiedVersion"
dotnet msbuild Tests.proj /t:UseFetchToGetSpecifiedVersion

Write-Host -ForegroundColor Blue "SwapBetweenVersionAndUnversioned"
dotnet msbuild Tests.proj /t:UseFetchToGetSpecifiedVersion

#Put fail cases at end so easier to verify
Write-Host -ForegroundColor Blue "VersionedPackageInvalidVersion"
dotnet msbuild Tests.proj /t:VersionedPackageInvalidVersion
