using System.IO;

namespace GitPackage.Tests.Helpers
{
    public static class Files
    {
        public static DirectoryInfo TestProject => 
            new DirectoryInfo(
                Path.GetFullPath("../../../", Path.GetDirectoryName(typeof(Files).Assembly.Location)));

        public static FileInfo MSBuildExtraData =>
            new FileInfo(Path.Combine(TestProject.FullName, "TestData.generated.props"));

        public static DirectoryInfo TestRepository => 
            new DirectoryInfo(Path.Combine(
                TestProject.FullName, "App_Data/TestRepository"));

        public static class InfoRead
        {
            public static DirectoryInfo InfoReadFolder=>
            new DirectoryInfo(
                Path.Combine(TestProject.FullName, "TestData/InfoRead"));

            public static FileInfo VersionedFile => new FileInfo(
                Path.Combine(InfoReadFolder.FullName, "Versioned.ver"));

            public static FileInfo UnVersionedFile => new FileInfo(
                Path.Combine(InfoReadFolder.FullName, "UnVersioned.ver"));
        }
    }
}
