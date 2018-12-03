using GitPackage.Demo.Components;
using GitPackage.Demo.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens.Saml;

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

		  services.AddScoped<WikiPageProvider>();
      services.AddScoped<WikiPages>();
		}

		public void Configure(IApplicationBuilder app)
		{
		  app.UseDeveloperExceptionPage();

			app.UseMvcWithDefaultRoute();
		}
	}
}