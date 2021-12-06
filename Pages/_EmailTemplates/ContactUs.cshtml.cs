using DevTrends.Mailer;

namespace HenryHiles.Pages._EmailTemplates;

public record ContactUsEmail(string EmailAddress, string Name, string Message, string To, string? Ip) : IEmail
{
    public string Subject => $"New Contact Us Message";
    public string Template => "~/Pages/_EmailTemplates/ContactUs.cshtml";
};