using System.Xml.Linq;
using GitPackage.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitPackage.Tests
{
    [TestClass]
    public class MSBuildPrep
    {
        [TestMethod]
        public void GenerateTestDataProperties()
        {
            var doc = new XDocument(new XElement("Project",
                new XComment("Generated file:" +
                             $" use {nameof(GenerateTestDataProperties)} " +
                             " to refresh for local"),
                new XElement("PropertyGroup",
                    new XElement("TestRepository_CacheFolder", 
                        CollectGitPackageInfo.GenerateShortFolderName(Files.TestRepository.FullName)),
                    new XElement("WebSample1_CacheFolder", 
                        CollectGitPackageInfo.GenerateShortFolderName("https://gist.github.com/b0e78c837464e463aded4f7336605db6.git")),
                    new XElement("WebSample2_CacheFolder", 
                        CollectGitPackageInfo.GenerateShortFolderName("https://gist.github.com/859715d703f5c1030c679aa394c48679.git")),
                    new XElement("WebSample3_CacheFolder", 
                        CollectGitPackageInfo.GenerateShortFolderName("https://gist.github.com/Dkowald/28ea6f6bd38562c131a76052587c6268"))
                ))
            );

            using(var wr = Files.MSBuildExtraData.CreateText())
                doc.Save(wr);

            Assert.IsTrue(Files.MSBuildExtraData.Exists);
        }
    }
}