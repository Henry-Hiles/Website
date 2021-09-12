using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTrends.AwsClients;
using DevTrends.Mailer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HenryHiles
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
            var mvcBuilder = services.AddRazorPages();

            mvcBuilder.AddViewOptions(x => x.HtmlHelperOptions.ClientValidationEnabled = false);

            var emailSettings = new EmailSettings();
            Configuration.Bind("emailSettings", emailSettings);
            services.AddSingleton(emailSettings);


            services.AddMailer(new SesSettings(
                new AwsSettings(emailSettings.AwsAccessKeyId, emailSettings.AwsSecretAccessKey, "us-east-1", "ses"),
                emailSettings.FromAddress, "Henry Hiles"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
