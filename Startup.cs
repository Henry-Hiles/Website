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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddRazorPages();

            mvcBuilder.AddViewOptions(x => x.HtmlHelperOptions.ClientValidationEnabled = false);

            var awsKeyId = Configuration.GetValue<string>("SesSettings:AwsSettings:AccessKeyId");
            var fromAddress = Configuration.GetValue<string>("SesSettings:FromAddress");
            var secretAccessKey = Configuration.GetValue<string>("SesSettings:AwsSettings:SecretAccessKey");

            services.AddMailer(new SesSettings(
                new AwsSettings(awsKeyId, secretAccessKey, "us-east-1", "ses"),
                fromAddress, "Henry Hiles"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
