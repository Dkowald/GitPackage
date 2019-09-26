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

        /// <summary>
        /// The optional version requested from project file
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The required source repository url.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The version file corresponding to the task item, or located file.
        /// </summary>
        public string VerFile { get; set; }

        /// <summary>
        /// The current version found in the file.
        /// </summary>
        public string Actual { get; set; }

        /// <summary>
        /// The expected folder to use for checked out work-tree.
        /// </summary>
        public string Workspace { get; set; }

        /// <summary>
        /// The local cashed folder for the cloned bare repository.
        /// </summary>
        public string CloneFolderName { get; set; }
    }
}