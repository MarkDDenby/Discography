using Discography.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceClient;
using ServiceClient.Contracts;

namespace Discography
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddLogging(o =>
            {
                o.AddConsole();
            });
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddTransient<IDiscographyHelper, DiscographyHelper>();
            services.AddTransient<IWordCounter, WordCounter>();
            services.AddTransient<IDiscographyServiceClient,  DiscographyServiceClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationHub>("/resultFeedback");
            });

            //app.UseSignalR(routes => { routes.MapHub<NotificationHub>("/resultFeedback"); });
        }
    }
}
