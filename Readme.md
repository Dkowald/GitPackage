# Archived
No longer progressing this repo.

Originally I was using this to include source directly into other code.
It was an alternative to using Git sub-modules; which I find problematic.

As of now; a better approach would be to create a Nuget source package.

# GitPackage
.NET Tooling to use git repositories as packages.

Simple source-code based packages for .Net projects

## Get started
Add the following to your project .csproj
```XML
<ItemGroup>
  <!--Nuget package to use git packages-->
  <PackageReference Include="GitPackage" Version="0.5.0"
    PrivateAssets="All" />

  <!-- git package(s) -->
  <GitPackage
    Include="FileInfo" 
    Version="1.0.0" 
    Uri="https://gist.github.com/df49cda62ea033be1eda60333921675a.git" />
</ItemGroup>
```
Your project now includes source files for some handy FileInfo extensions.

More details at the [wiki](https://github.com/Dkowald/GitPackage/wiki) 
including [some other git packages](https://github.com/Dkowald/GitPackage/wiki/50-MyGitPackages).
