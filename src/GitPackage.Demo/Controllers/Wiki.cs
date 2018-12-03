using System.IO;
using System.Linq;
using System.Text;
using CommonMark;
using GitPackage.Demo.Components;
using GitPackage.Demo.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace GitPackage.Demo.Controllers
{
  public class Wiki : Controller
  {
    private readonly WikiPageProvider _pages;

    public Wiki(WikiPageProvider pages)
    {
      _pages = pages;
    }

    [Route("/[controller]/{path?}")]
    public IActionResult Index(string path)
    {
      ViewBag.Path = path;

      var srcPage = _pages.Pages.SingleOrDefault(x => x.Name == path);

      if (srcPage == null) return NotFound("No such file");

      var html = new StringBuilder();
      using (var rd = new StreamReader(srcPage.CreateReadStream()))
      using (var wr = new StringWriter(html))
      {
        CommonMarkConverter.Convert(rd, wr, CommonMarkSettings.Default);
      }

      ViewBag.Content = new HtmlString( html.ToString());
      return View();
    }
  }
}
