using Microsoft.Extensions.FileProviders;

namespace GitPackage.Docs.Model
{
    public class WikiPage
    {
        public string Name;
        public IFileInfo ContentFile;
        public string DisplayName;
    }
}