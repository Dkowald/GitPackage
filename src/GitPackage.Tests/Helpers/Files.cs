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

        public static DirectoryInfo SampleRepo => 
            new DirectoryInfo(Path.Combine(
                TestProject.FullName, "TestData/SampleRepo"));
    }
}
