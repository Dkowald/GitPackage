using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitPackage.Tests
{
    [TestClass]
    public class CollectGitPackageInfoTests
    {
        private readonly string _root;

        private readonly string _samplePackage = "Sample";

        public CollectGitPackageInfoTests()
        {
            _root = Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), "../../../", "TestData", "GitPackageInfo"));
        }

        [TestMethod]
        public void MergeFileAndItemMetData()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[]
                {
                    new TaskItem(_samplePackage,
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "https://gist.git"}})
                }
            };
            
            var result = target.Execute();

            Assert.IsTrue(result);

            var sample = target.Info.Single(x => x.ItemSpec == _samplePackage);

            var data = new PackageInfoMetaData(sample);

            Assert.AreEqual("1.0.0", data.Version);
            Assert.AreEqual("https://gist.git", data.Uri);

            Assert.AreEqual($"{_samplePackage}.ver", Path.GetFileName(data.VerFile));
            Assert.AreEqual("1.0.0", data.Actual);
            Assert.AreEqual(_samplePackage, Path.GetFileName(data.Workspace));
        }

        [TestMethod]
        public void CollectItemMissingFromBuild()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[] { }
            };

            target.Execute();

            var sample = target.Info.SingleOrDefault(x => x.ItemSpec == _samplePackage);

            var data = new PackageInfoMetaData(sample);

            Assert.IsNotNull(sample, "Item not in project, but .ver file exists");
            Assert.AreEqual("", data.Version);
            Assert.AreEqual("", data.Uri);
            Assert.AreEqual($"{_samplePackage}.ver", Path.GetFileName(data.VerFile));
        }
    }
}
