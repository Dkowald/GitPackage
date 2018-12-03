using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace GitPackage.Demo.Components
{
  public class WikiPages : ViewComponent
  {
    private IFileInfo[] _wikiPages;

    public WikiPages(IHostingEnvironment env)
    {
      Links = env.ContentRootFileProvider.GetDirectoryContents("./gist/wiki")
        .Where(x => !x.IsDirectory && Path.GetExtension(x.Name) == ".md")
        .Select(x => new HtmlLink
        {
          Name = Path.GetFileNameWithoutExtension(x.Name),
          Url = $"/Wiki/{x.Name}"
        })
        .OrderBy(x => x.Name != "Home")
        .ThenBy(x => x.Name)
        .ToArray();
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      return View(this);
    }

    public readonly HtmlLink[] Links;

    public class HtmlLink
    {
      public string Name { get; set; }
      public string Url { get; set; }
    }
  }
}
