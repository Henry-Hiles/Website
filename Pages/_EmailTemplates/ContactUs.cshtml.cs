using DevTrends.Mailer;

namespace HenryHiles.Pages._EmailTemplates;

public class ContactUsEmail : IEmail
{
    public string Subject => $"New Contact Us Message";
    public string Template => "~/Pages/_EmailTemplates/ContactUs.cshtml";

    public string EmailAddress { get; set; }

    public string To { get; set; }

    public string Name { get; set; }

    public string Message { get; set; }

    public string Ip { get; set; }
}
