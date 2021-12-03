using DevTrends.AwsClients;
using DevTrends.Mailer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var mvcBuilder = builder.Services.AddRazorPages();

mvcBuilder.AddViewOptions(x => x.HtmlHelperOptions.ClientValidationEnabled = false);

var emailSettings = new EmailSettings();
builder.Configuration.Bind("emailSettings", emailSettings);
builder.Services.AddSingleton(emailSettings);


builder.Services.AddMailer(new SesSettings(
    new AwsSettings(emailSettings.AwsAccessKeyId, emailSettings.AwsSecretAccessKey, "us-east-1", "ses"),
    emailSettings.FromAddress, "Henry Hiles"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
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

app.MapRazorPages();

app.Run();