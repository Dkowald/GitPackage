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
        public CollectGitPackageInfoTests()
        {
            _root = Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), "../../../", "gist"));
        }

        [TestMethod]
        public void MergeFileAndItemMetData()
        {
            var target = new CollectGitPackageInfo
            {
                Root = _root,
                Items = new ITaskItem[]
                {
                    new TaskItem("AbsUrl",
                        new Dictionary<string, string> {{"Version", "1.0.0"}, {"Uri", "https://gist.git"}})
                }
            };
            
            var result = target.Execute();

            Assert.IsTrue(result);

            var absUrl = target.Info.Single(x => x.ItemSpec == "AbsUrl");

            var data = new PackageInfoMetaData(absUrl);

            Assert.AreEqual("1.0.0", data.Version);
            Assert.AreEqual("https://gist.git", data.Uri);

            Assert.AreEqual("AbsUrl.ver", Path.GetFileName(data.VerFile));
            Assert.AreEqual("1.0.0", data.Actual);
            Assert.AreEqual("AbsUrl", Path.GetFileName(data.Workspace));
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

            var absUrl = target.Info.SingleOrDefault(x => x.ItemSpec == "AbsUrl");

            var data = new PackageInfoMetaData(absUrl);

            Assert.IsNotNull(absUrl, "Item not in project, but .ver file exists");
            Assert.AreEqual("", data.Version);
            Assert.AreEqual("", data.Uri);
            Assert.AreEqual("AbsUrl.ver", Path.GetFileName(data.VerFile));
        }
    }
}
