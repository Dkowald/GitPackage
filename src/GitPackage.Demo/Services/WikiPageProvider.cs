using System.IO;
using System.Linq;

using GitPackage.Demo.Model;

using Microsoft.AspNetCore.Hosting;

namespace GitPackage.Demo.Services
{
    public class WikiPageProvider
    {
        public WikiPageProvider(IHostingEnvironment env)
        {
            Pages = env.ContentRootFileProvider.GetDirectoryContents("./gist/GitPackageWiki")
              .Where(x => !x.IsDirectory && Path.GetExtension(x.Name) == ".md")
              .OrderBy(x => x.Name != "Home.md")
              .ThenBy(x => x.Name)
              .Select(x => new WikiPage
              {
                  Name = x.Name,
                  ContentFile = x,
                  DisplayName = Path.GetFileNameWithoutExtension(x.Name).Replace('-', ' ')
              })
              .ToArray();
        }

        public readonly WikiPage[] Pages;
    }
}