# GitPackage Demo / Test project

A test project for GitPackage.

At this stage, most of the testing is manually performed (yuk .. I know)
But it is quite a challenge to automate testing for this kind of project.

# Sample repo
A sample repo used for testing.
Repo created via: buildRepo.ps1

# Manual tests

## Clean / reset
1. Make sample repo as above.
2. Remove Sample repos from cache: %HOME%/.gitpack
3. Ensure test project has no current gitpackage's

### Attached package.
1. Add SampleAttached package.
2. Add SampleAttachedAlt package.
3. dotnet build - clones sample, master and alt branches checked out.
4. delete SampleAttachedAlt.ver
5. dotnet build - clones sample alt branch
6. dotnet build - no work;

### With unversioned.   
4. Add SampleUnversioned
5. dotnet build. Sample now in cache, and unversioend has v4
6. Use cli; update the SampleAttached : replace 4 with 5 and push.
7. dotnet msbuild /t:GitPackRefresh (manual refresh)
8. SampleUnversioned now has v5.
11. update SampleAttached replace 5 with 6; push.
12. dotnet build; no work unversioned NOT updated automaticly.
13. delete SampleUnversioned.ver (auto refresh)
14. dotnet build SampleUnversioned updated to 6

### With versioned
1. Add SampleTaged v1
2. dotnet build. Sample cloned to cache. v1 file in SampleTaged
3. Replace SampleTaged with v2
4. dotnet build
5. Sample now has v2
6. dotnet build; no work all up to date.
7. Cli: tag latest Sample with v6. (git tag -a 6 -m 6 && git push --tags)
8. Replace SampleTaged with v6.
9. dotnet build; fetches latest from repo, check out v6
10. Replace SampleTaged with invalidVersion
11. dotnet build; fetch latest; build failed: version tag not found.
12. Replace SampleTaged with v3
13. dotnet build; no fetch; and v3 now checked out

### Multiple projects using same versions
--Must used detached checkout's so other projects can use repo.
 In TestProjects/GetWithOther
1. dotnet build; no work, no git packages yet.
3. Add SampleUnversioned
4. Add SampleTaged v1
5. dotnet build; has files from sample repo.
6. Replace SampleTaged with v3
7. dotnet build; now has sample check out as main test proj.

### Clean up.
1. Comment out all gitpackages.
2. Build - all git packages removed.
3. delete Sample repo.