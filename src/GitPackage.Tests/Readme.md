# GitPackage Demo / Test project

A test project for GitPackage.

At this stage, most of the testing is manually performed (yuk .. I know)

To perform tests; create a local git repository and configure it with a few versions:
e.g repo : c:\tmp\Sample.
Add a file : 1.txt commit and tag as (version) 1.
Replace 1.txt with 2.txt. Commit and tag as (version) 2.

Replace 2.txt with 3.txt. Commit.

## Core Manual tests

1. Clear prj; no git packages and no cloned repos.
2. Build - reports no git package work.
3. Add versioned package - restore - clones and checkout version.
4. Update package version - refresh - checkout new version.
5. Update package version - restore - fetches but checkout fail (no such version).
6. Update repo with new version tag - restore - fetches and checkout with version.
7. Add unversioned package - restore - clones and checkout
8.  - restore - no work.
9.  - refresh - fetch and checkout unversioned.
10. Update unversioned repo - restore - no work
11. - refresh - fetch and checkout update.
12. Remove versioned package - restore - deletes version package files
13. Remove unversioned package - restore - deletes un versioned package.
14.  - refresh - no work.


### Manual refresh via file delete.

1. Build with an unversioned package.
2. Update the repo.
3. Delete the local files (ver file and folder).
4. - restore - fetches latest and checks out changes.