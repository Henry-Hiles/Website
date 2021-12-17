using DevTrends.AwsClients;
using DevTrends.ConfigurationExtensions;
using DevTrends.Mailer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var mvcBuilder = builder.Services.AddRazorPages();

mvcBuilder.AddViewOptions(x => x.HtmlHelperOptions.ClientValidationEnabled = false);

var emailSettings = builder.Configuration.Bind<SesSettings>();

builder.Services.AddSingleton(emailSettings);

builder.Services.AddMailer(emailSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/error/{0}");
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();