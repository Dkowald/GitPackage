using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitPackage.Demo
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc()
				.AddControllersAsServices();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseMvc();
		}
	}
}