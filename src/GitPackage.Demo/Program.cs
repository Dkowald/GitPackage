using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GitPackage.Demo
{
    static class Program
    {
        static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder<Startup>(args)
                .Build();

            host.Run();
        }
    }
}
