using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace mvcapp
{
    public class Startup
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public Startup( ILogger<Startup> logger
                      , IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {     
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {               
                app.UseGlobalExceptionHandler( _logger
                                             , errorPagePath: "/Home/Error"
                                             , respondWithJsonErrorDetails: true);
                

                //Replaced UseExceptionHandler with UseGlobalExceptionHandler
                //app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
