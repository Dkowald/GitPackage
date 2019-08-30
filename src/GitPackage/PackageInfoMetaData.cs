using Microsoft.Build.Framework;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace GitPackage
{
    public class PackageInfoMetaData
    {
        public PackageInfoMetaData(ITaskItem fromItem = null)
        {
            Version = fromItem?.GetMetadata(nameof(Version));
            Uri = fromItem?.GetMetadata(nameof(Uri));
            VerFile = fromItem?.GetMetadata(nameof(VerFile));
            Actual = fromItem?.GetMetadata(nameof(Actual));
            Workspace = fromItem?.GetMetadata(nameof(Workspace));
            CloneFolderName = fromItem?.GetMetadata(nameof(CloneFolderName));
        }

        public string Version { get; set; }
        public string Uri { get; set; }

        public string VerFile { get; set; }
        public string Actual { get; set; }
        public string Workspace { get; set; }

        public string CloneFolderName { get; set; }
    }
}