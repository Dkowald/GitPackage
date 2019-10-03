using System;
using Microsoft.Build.Framework;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace GitPackage
{
    public class PackageInfoMetaData
    {
        private readonly ITaskItem _taskItem;

        public PackageInfoMetaData(ITaskItem taskItem)
        {
            _taskItem = taskItem ?? throw new ArgumentNullException(nameof(taskItem));
        }

        #region Project provided data
        public string ItemSpec => _taskItem.ItemSpec;

        /// <summary>
        /// The optional version requested from project file
        /// </summary>
        public string Version 
        {
            get => _taskItem.GetMetadata(nameof(Version));
            set => _taskItem.SetMetadata(nameof(Version), value);
        }

        /// <summary>
        /// The required source repository url.
        /// </summary>
        public string Uri
        {
            get => _taskItem.GetMetadata(nameof(Uri));
            set => _taskItem.SetMetadata(nameof(Uri), value);
        }
        #endregion

        #region Version file found data.

        /// <summary>
        /// The version file corresponding to the task item, or located file.
        /// </summary>
        public string VersionFile
        {
            get =>_taskItem.GetMetadata(nameof(VersionFile));
            set => _taskItem.SetMetadata(nameof(VersionFile), value);
        }

        /// <summary>
        /// The current version found in the file.
        /// </summary>
        public string Actual
        {
            get =>_taskItem.GetMetadata(nameof(Actual));
            set => _taskItem.SetMetadata(nameof(Actual), value);
        }
        
        /// <summary>
        /// Full path to the projects target workTree folder.
        /// </summary>
        public string WorkTreeFolder
        {
            get => _taskItem.GetMetadata(nameof(WorkTreeFolder));
            set => _taskItem.SetMetadata(nameof(WorkTreeFolder), value);
        }

        /// <summary>
        /// The git commitish string to use to identify the requested checkout version
        /// </summary>
        public string WorkTreeCommit
        {
            get => _taskItem.GetMetadata(nameof(WorkTreeCommit));
            set => _taskItem.SetMetadata(nameof(WorkTreeCommit), value);
        }
        #endregion

        /// <summary>
        /// The local cashed folder for the cloned bare repository.
        /// </summary>
        public string CloneFolderName
        {
            get => _taskItem.GetMetadata(nameof(CloneFolderName)); 
            set => _taskItem.SetMetadata(nameof(CloneFolderName), value);
        }
    }
}