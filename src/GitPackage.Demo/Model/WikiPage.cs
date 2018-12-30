using Microsoft.Extensions.FileProviders;

namespace GitPackage.Demo.Model
{
    public class WikiPage
    {
        public string Name;
        public IFileInfo ContentFile;
        public string DisplayName;
    }
}