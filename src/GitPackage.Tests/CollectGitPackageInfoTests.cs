using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
        public void PrepMSBuildTestData()
        {
            var doc = new XDocument(new XElement("Project",
                new XComment("Generated file:" +
                            $" use {nameof(PrepMSBuildTestData)} " +
                             " to refresh for local"),
                new XElement("PropertyGroup",
                    new XElement("TestRepository_CacheFolder", 
                        CollectGitPackageInfo.GenerateShortFolderName(Files.TestRepository.FullName))
                    ))
            );

            using(var wr = Files.MSBuildExtraData.CreateText())
                doc.Save(wr);

            Assert.IsTrue(Files.MSBuildExtraData.Exists);
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
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "https://gist.git"}})
                }
            };
            
            var result = target.Execute();

            Assert.IsTrue(result);

            var sample = target.Info.Single(x => x.ItemSpec == "Versioned");

            var data = new PackageInfoMetaData(sample);

            Assert.AreEqual("1.0.0", data.Version);
            Assert.AreEqual("https://gist.git", data.Uri);

            Assert.AreEqual("Versioned.ver", Path.GetFileName(data.VerFile));
            Assert.AreEqual("1.0.0", data.Actual);
            Assert.AreEqual("Versioned", Path.GetFileName(data.Workspace));
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
            Assert.AreEqual("UnVersioned.ver", Path.GetFileName(data.VerFile));

            var versioned = new PackageInfoMetaData(target.Info.Single(x => x.ItemSpec == "Versioned"));
            Assert.AreEqual("1.0.0", versioned.Actual);
        }
    }
}
