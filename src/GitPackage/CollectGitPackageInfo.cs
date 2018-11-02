using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace GitPackage
{
    public class CollectGitPackageInfo : Task
    {
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
                    var data = rd.ReadLine();
                    info.SetMetadata(nameof(PackageInfoMetaData.Actual), data);
                }
            }

            Info = infoItems.ToArray();

            return true;
        }
    }
}
