using System.IO;
using System.Linq;
using GitPackage.Demo.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace GitPackage.Demo.Model
{
  public class WikiPageProvider
  {
    public WikiPageProvider(IHostingEnvironment env)
    {
      Pages = env.ContentRootFileProvider.GetDirectoryContents("./gist/wiki")
        .Where(x => !x.IsDirectory && Path.GetExtension(x.Name) == ".md")
        .OrderBy(x => x.Name != "Home")
        .ThenBy(x => x.Name)
        .ToArray();
    }

    public readonly IFileInfo[] Pages;
  }
}