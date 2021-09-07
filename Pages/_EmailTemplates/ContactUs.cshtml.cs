using DevTrends.Mailer;

namespace HenryHiles.Pages._EmailTemplates
{
    public class ContactUsEmail : IEmail
    {
        public string To => Constants.AdminEmailRecipient;
        public string Subject => $"New Contact Us Message";
        public string Template => "~/Pages/_EmailTemplates/ContactUs.cshtml";

        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }
    }
}