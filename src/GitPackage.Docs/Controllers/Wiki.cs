using System.IO;
using System.Linq;
using System.Text;

using CommonMark;
using GitPackage.Docs.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace GitPackage.Docs.Controllers
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
            var srcPage = _pages.Pages.SingleOrDefault(x => x.Name == path);

            if (srcPage == null) return NotFound("No such file");

            var html = new StringBuilder();
            using (var rd = new StreamReader(srcPage.ContentFile.CreateReadStream()))
            using (var wr = new StringWriter(html))
            {
                CommonMarkConverter.Convert(rd, wr, CommonMarkSettings.Default);
            }

            return View(new Model
            {

                Title = srcPage.DisplayName,
                Content = new HtmlString(html.ToString())
            });
        }

        public class Model
        {
            public string Title;
            public HtmlString Content;
        }
    }
}
