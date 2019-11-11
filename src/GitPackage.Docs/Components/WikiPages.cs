using System.Linq;
using System.Threading.Tasks;
using GitPackage.Docs.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitPackage.Docs.Components
{
    public class WikiPages : ViewComponent
    {
        public WikiPages(WikiPageProvider pages)
        {
            Links = pages.Pages
              .Select(x => new HtmlLink
              {
                  Name = x.DisplayName,
                  Url = $"/Wiki/{x.Name}"
              })
              .ToArray();
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View(this));
        }

        public readonly HtmlLink[] Links;

        public class HtmlLink
        {
            public string Name;
            public string Url { get; set; }
        }
    }
}
