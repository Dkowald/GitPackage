using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitPackage.Tests.Helpers;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitPackage.Tests
{
    [TestClass]
    public class CollectGitPackageInfoTests
    {
        private readonly string _root = 
            Files.InfoRead.InfoReadFolder.FullName;

        [TestMethod]
        public void MergedDataIgnoresFileSystemCase()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new[]
                {
                    new TaskItem("versioned"),
                    new TaskItem("orphan"),
                    new TaskItem("UnVersioned"), 
                }
            };

            target.Execute();
            Assert.AreEqual(3, target.Info.Length, "match same item even if case differs");

            var versioned = new PackageInfoMetaData(target.Info.Single(x => x.ItemSpec == "versioned"));
            Assert.AreEqual("1.0.0", versioned.Actual);
        }
        
        [TestMethod]
        public void BuildCheckoutCommitish()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[]
                {
                    new TaskItem("Versioned",
                        new Dictionary<string, string>
                        {
                            {"Uri", "git://repo1"},
                            {"Version", "1.0.0"}
                        }),
                    new TaskItem("UnVersioned",
                        new Dictionary<string, string>
                        {
                            {"Uri", "git://repo2"}
                        })
                }
            };

            target.Execute();

            var versioned = new PackageInfoMetaData(target.Info.Single(x => x.ItemSpec == "Versioned"));
            var unVersioned = new PackageInfoMetaData(target.Info.Single(x => x.ItemSpec == "UnVersioned"));

            Assert.AreEqual("1.0.0", versioned.WorkTreeCommit, "use provided version ");
            Assert.AreEqual("--detach master", unVersioned.WorkTreeCommit, "Use master for un-versioned");
        }

        [TestMethod]
        public void GenerateCloneFolderFromFileUri()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[]
                {
                    new TaskItem("Sample",
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "c:/temp/sub/../repo.git"}}),
                    new TaskItem("Sample2",
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "file:///c:\\TEMP\\repo.git"}}),
                    new TaskItem("Sample3",
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "/TEMP/repo.git"}}),
                }
            };

            target.Execute();

            var sample = new PackageInfoMetaData(target.Info[0]);
            var sampleSameRepo = new PackageInfoMetaData(target.Info[1]);
            
            
            Assert.IsNotNull(sample.CloneFolderName, "Got a repo folder to use");
            Assert.AreEqual(sample.CloneFolderName, sampleSameRepo.CloneFolderName, 
                "Same repo folder for same repo");

            var sample3 = new PackageInfoMetaData(target.Info[2]);
            Assert.IsNotNull(sample3.CloneFolderName);

        }

        [TestMethod]
        public void GenerateCloneFolderFromUri()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[]
                {
                    new TaskItem("Sample",
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "https://server1/gist.git"}}),
                    new TaskItem("Sample_SameRepo",
                    new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "https://server1/gist.git"}}),
                    new TaskItem("Sample_DifRepo",
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "https://server2/gist.git"}})
                }
            };

            target.Execute();

            var sample = new PackageInfoMetaData(target.Info[0]);
            var sampleSameRepo = new PackageInfoMetaData(target.Info[1]);
            var difRepo = new PackageInfoMetaData(target.Info[2]);

            Assert.IsNotNull(sample.CloneFolderName, "Got a repo folder to use");
            Assert.AreEqual(sample.CloneFolderName, sampleSameRepo.CloneFolderName, 
                "Same repo folder for same repo");

            Assert.AreNotEqual(sample.CloneFolderName, difRepo.CloneFolderName, 
                "Different repo folders");

            Assert.IsFalse(sample.CloneFolderName.IndexOfAny(Path.GetInvalidPathChars()) > -1, "No invalid path chars");
            Assert.IsFalse(difRepo.CloneFolderName.IndexOfAny(Path.GetInvalidPathChars()) > -1, "No invalid path chars");
        }

        [TestMethod]
        public void MergeFileAndItemMetData()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[]
                {
                    new TaskItem("Versioned",
                        new Dictionary<string, string>
                        {
                            {"Version", "2.0.0"}, 
                            {"Uri", "https://gist.git"}
                        }),
                    new TaskItem("MyPackage")
                }
            };
            
            var result = target.Execute();

            Assert.IsTrue(result);

            var versioned = target.Info.Single(x => x.ItemSpec == "Versioned");

            var data = new PackageInfoMetaData(versioned);

            Assert.AreEqual("2.0.0", data.Version, "TaskItem is for version 2");
            Assert.AreEqual("https://gist.git", data.Uri);
            Assert.AreEqual("Versioned.ver", Path.GetFileName(data.VersionFile));
            Assert.AreEqual("1.0.0", data.Actual, "Version in file");
            Assert.AreEqual("Versioned", Path.GetFileName(data.WorkTreeFolder));

            var myPackage = target.Info.SingleOrDefault(x => x.ItemSpec == "MyPackage");
            Assert.IsNotNull(myPackage, "Keeps the provided task item");
        }

        [TestMethod]
        public void ReadsItemsFromFolder()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root
            };

            target.Execute();

            Assert.IsNotNull(target.Info.SingleOrDefault(x => x.ItemSpec == "Versioned"), 
                "Found versioned");

            Assert.IsNotNull(target.Info.SingleOrDefault(x => x.ItemSpec == "UnVersioned"),
                "Found unVersioned");

            Assert.IsNotNull(target.Info.SingleOrDefault(x => x.ItemSpec == "Orphan"),
                "Found orphan folder");
        }

        [TestMethod]
        public void CollectItemMissingFromBuild()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[] { }//no packages in project
            };

            target.Execute();

            var unVersioned = target.Info.Single(x => x.ItemSpec == "UnVersioned");

            var data = new PackageInfoMetaData(unVersioned);

            Assert.IsNotNull(unVersioned, "Item not in project, but .ver file exists");
            Assert.AreEqual("", data.Actual);
            Assert.AreEqual("", data.Uri);
            Assert.AreEqual("UnVersioned.ver", Path.GetFileName(data.VersionFile));

            var versioned = new PackageInfoMetaData(target.Info.Single(x => x.ItemSpec == "Versioned"));
            Assert.AreEqual("1.0.0", versioned.Actual);
        }

        [TestMethod]
        public void PackageRemovedFromProject()
        {
            var projectItems = new ITaskItem[]
            {
                new TaskItem("UnVersioned", new Dictionary<string, string>
                    {{"Uri", "https://gist.git"}})
            };

            var target = new CollectGitPackageInfo
                {Root = _root, Items = projectItems};

            Assert.IsTrue(target.Execute());

            var data = target.Info.Select(x => new PackageInfoMetaData(x));
            var toRemove = data.Where(x => string.IsNullOrWhiteSpace(x.Uri));
            
            Assert.AreEqual(2, toRemove.Count(), "Expect to remove orphan and versioned");
        }
    }
}
