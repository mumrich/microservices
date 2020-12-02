using DotNetify;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using VueCliMiddleware;

namespace frontend_vue
{
  public class Startup
  {
    private const string CLIENT_APP_NAME = "client-app";
    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
      // NOTE: PRODUCTION Ensure this is the same path that is specified in your webpack output
      services.AddSpaStaticFiles(opt => opt.RootPath = $"{CLIENT_APP_NAME}/dist");
      services.AddControllers();
      services.AddMemoryCache();
      services.AddSignalR();
      services.AddDotNetify();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // PRODUCTION uses webpack static files
      app.UseSpaStaticFiles();
      app.UseRouting();
      app.UseWebSockets();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<DotNetifyHub>("/dotnetify");
        endpoints.MapToVueCliProxy(
          "{*path}",
          new SpaOptions { SourcePath = CLIENT_APP_NAME },
          npmScript: env.IsDevelopment() ? "serve" : null,
          regex: "Compiled successfully",
          forceKill: true,
          wsl: false, // Set to true if you are using WSL on windows. For other operating systems it will be ignored
          https: false);
      });
    }
  }
}
