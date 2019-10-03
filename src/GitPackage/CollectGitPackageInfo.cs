using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace GitPackage
{
    public class CollectGitPackageInfo : Task
    {
        /// <summary>
        /// Given a full url (file or web) present it as a
        /// deterministic (mostly) unique short folder name.
        /// </summary>
        public static string GenerateShortFolderName(string fullUri, HashAlgorithm hasher)
        {
            if (fullUri == null) 
                throw new ArgumentNullException(nameof(fullUri));

            if(!Uri.TryCreate(fullUri, UriKind.Absolute, out var uri) )
                uri = new Uri("file://"+fullUri);

            if (uri.IsFile)
            {
                fullUri = Path.GetFullPath(uri.LocalPath).ToLower()
                    .Replace('\\', '/');
            }
            else
            {
                fullUri = uri.ToString().ToLower();
            }

            var deterministicHash = BitConverter.ToString(
                    hasher.ComputeHash(Encoding.UTF8.GetBytes(fullUri)))
                .Replace("-","").ToLower();

            var result = $"{Path.GetFileNameWithoutExtension(fullUri)}_{deterministicHash}";
            return result;
        }
        
        public static string  GenerateShortFolderName(string fullUri)
        { using(var hasher = SHA1.Create())
            return GenerateShortFolderName(fullUri, hasher);
        }

        /// <summary>
        /// Root folder containing the current version files.
        /// </summary>
        [Required]
        public string Root { get; set; }

        /// <summary>
        /// Collection of GitPackage items to process.
        /// </summary>
        [Required]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// GitPackage items with metadata.
        /// </summary>
        [Output]
        public ITaskItem[] Info { get; set; }

        public override bool Execute()
        {
            var infoItems = (Items ?? Array.Empty<ITaskItem>())
                .Select(x => new TaskItem(x))
                .Cast<ITaskItem>().ToList();

            var dupes = string.Join("; ", 
                infoItems.GroupBy(x => x.ItemSpec).Where(x => x.Count() > 1).Select(x => x.Key));
            if (!string.IsNullOrWhiteSpace(dupes))
            {
                throw new Exception("Duplicate packages found: "+dupes);
            }

            infoItems = AppendMissingItemsFromFileSystem(infoItems);
            
            using (var sh1 = SHA1.Create())
            {
                foreach (var item in infoItems)
                {
                    var info = new PackageInfoMetaData(item);

                    info.VersionFile = Path.Combine(Root, info.ItemSpec + ".ver");
                    
                    info.Actual = ReadActualVersion(info);

                    info.WorkTreeFolder = Path.Combine(Root, info.ItemSpec);
                    
                    info.WorkTreeCommit = GetWorkTreeCommitFromVersion(info.Version);

                    GetCloneFolderName(sh1, item);

                }
            }

            Info = infoItems.ToArray();

            return true;
        }

        private List<ITaskItem> AppendMissingItemsFromFileSystem(List<ITaskItem> items)
        {
            foreach (var verFile in Directory.EnumerateFiles(Root, "*.ver"))
            {
                var id = Path.GetFileNameWithoutExtension(verFile);
                if (id == null)
                {
                    Log.LogError("Skipping version file with no name");
                    continue;
                }

                var found = items.SingleOrDefault(x => x.ItemSpec.Equals(id, StringComparison.OrdinalIgnoreCase));
                if (found == null)
                {
                    found = new TaskItem(id);
                    found.SetMetadata(nameof(PackageInfoMetaData.VersionFile), verFile);

                    items.Add(found);
                }
            }

            foreach (var dir in Directory.EnumerateDirectories(Root))
            {
                var id = Path.GetFileName(dir);
                var found = items.SingleOrDefault(x => x.ItemSpec.Equals(id, StringComparison.OrdinalIgnoreCase));
                if (found == null)
                {
                    found = new TaskItem(id);
                    items.Add(found);
                }
            }

            return items;
        }

        private ITaskItem GetCloneFolderName(HashAlgorithm hasher, ITaskItem data)
        {
            var folderName =
                data.GetMetadata(nameof(PackageInfoMetaData.CloneFolderName));

            if (string.IsNullOrWhiteSpace(folderName))
            {
                var uri = data.GetMetadata(nameof(PackageInfoMetaData.Uri));
                
                folderName = GenerateShortFolderName(uri, hasher);

                data.SetMetadata(nameof(PackageInfoMetaData.CloneFolderName), folderName);
            }
            return data;
        }

        private string GetWorkTreeCommitFromVersion(string version)
        {
            //No version, just use detached master
            if (string.IsNullOrWhiteSpace(version))
                return "--detach master";

            if (version[0] == '/' || version[0] == '\\')
                return "tags/" + version;

            return version;
        }

        private string ReadActualVersion(PackageInfoMetaData info)
        {
            if (!File.Exists(info.VersionFile)) return null;

            using (var rd = File.OpenText(info.VersionFile))
            {
                var data = rd.ReadLine()?.Split(':') ?? new string[]{null};
                return data.Length > 1 ? data.Last()?.Trim(): "";
            }
        }
    }
}
