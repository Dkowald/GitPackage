using System;
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

        [Required]
        public string Root { get; set; }

        [Required]
        public ITaskItem[] Items { get; set; }

        [Output]
        public ITaskItem[] Info { get; set; }

        public override bool Execute()
        {
            var infoItems = Items.Select(x => new TaskItem(x))
                .Cast<ITaskItem>().ToList();

            var dupes = string.Join("; ", 
                infoItems.GroupBy(x => x.ItemSpec).Where(x => x.Count() > 1).Select(x => x.Key));
            if (!string.IsNullOrWhiteSpace(dupes))
            {
                throw new Exception("Duplicate packages found: "+dupes);
            }

            foreach (var verFile in Directory.EnumerateFiles(Root, "*.ver"))
            {
                var id = Path.GetFileNameWithoutExtension(verFile);
                if (id == null)
                {
                    Log.LogError("Skipping version file with no name");
                    continue;
                }

                var info = infoItems.SingleOrDefault(x => x.ItemSpec == id);
                if (info == null)
                {
                    info = new TaskItem(id);
                    infoItems.Add(info);
                }

                info.SetMetadata(nameof(PackageInfoMetaData.VerFile), verFile);

                var workSpace = Path.Combine(Root, id);
                if (Directory.Exists(workSpace))
                { info.SetMetadata(nameof(PackageInfoMetaData.Workspace), workSpace); }

                using (var rd = File.OpenText(verFile))
                {
                    var data = rd.ReadLine()?.Split(':') ?? new string[]{null};
                    info.SetMetadata(nameof(PackageInfoMetaData.Actual), data.Last()?.Trim());
                }
            }
            
            using(var sh1 = SHA1.Create())
                foreach (var item in infoItems)
                {
                    MetadataForCloneFileName(sh1, item);
                }

            Info = infoItems.ToArray();

            return true;
        }

        private ITaskItem MetadataForCloneFileName(HashAlgorithm hasher, ITaskItem data)
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
    }
}
