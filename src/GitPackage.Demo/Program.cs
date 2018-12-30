using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GitPackage.Demo
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder<Startup>(args)
                .Build();

            host.Run();
        }
    }
}
