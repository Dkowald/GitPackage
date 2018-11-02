# Overview
.NET Tooling to use git repositories as packages.

## Why?
Sometimes I don't want to take a dependency on a NuGet package.
I might be just wanting one or two classes.

This is an approach to import files directly from a git repository into my project(s).

## Get started
Add the following to your project .csproj
```XML
<ItemGroup>
  <PackageReference Include="GitPackage" Version="0.1.0" /> 
  <!-- sample git packages -->
  <GitPackage Include="AbsUrl" Version="1.0.0" Uri="https://gist.github.com/383acd462242194024981fbe53a84980.git" />
  <GitPackage Include="HttpError" Version="1.0.0" Uri="https://gist.github.com/6414276f4e3745473c699c1941549710.git" />
</ItemGroup>
```

## Usage Details

The Git command **MUST** be available on the path.

The git repository should use **Tags** to identify specific version(s).

Add reference to the package.

```XML
<ItemGroup>
  <GitPackage 
    Include="Sample" 
    Version="2" 
    Uri="C:\tmp\Sample" />
</ItemGroup>
```
Where

|Attribute| Sample|  Description|
|-------- |--------|-----------|
| Include | Sample | Name used for package |
| Version | 2      |Git tag used to identify version |
| Uri     | C:\tmp\Sample | Uri to git repository |

## How it works
A custom build task is used to manage the **GitPackages**

On restore (build)

1. Current data from project's gist folder is read
2. **Clone** any missing repository's to local cache folder (HOME/.gitpack)
3. **Fetch** if local clone doesn't have Version (tag).
4. Delete current project gists that don't match.
5. **add workspace** in project for each package; using Version tag.

## Credits
Thanks to Jeff Kluge for his [RoslynCodeTaskFactory](https://github.com/jeffkl/RoslynCodeTaskFactory)
project. Used to work out some tech.