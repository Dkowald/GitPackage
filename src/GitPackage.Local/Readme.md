# Overview
Basic project to verify the built GitPackage nuget.

This manages the nuget package cache so it always gets the latest build from the 
main project.

Use following steps to check package is ok
Reset project so it will get latest package:
dotnet clean

Get latest nuget package:
dotnet restore

Build project to verify
dotnet build