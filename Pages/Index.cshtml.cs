using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DevTrends.Mailer;
using HenryHiles.Pages._EmailTemplates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HenryHiles.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMailSender _mailer;

        public IndexModel(IMailSender mailer)
        {
            _mailer = mailer;
        }

        [BindProperty]
        public ViewModel Data { get; set; }

        public async Task<PartialViewResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await _mailer.SendEmail(new ContactUsEmail
                {
                    EmailAddress = Data.Email,
                    Name = Data.Name,
                    Message = Data.Message
                }, replyTo: Data.Email);

                return Partial("_ContactSuccess");
            }
            return Partial("_ContactForm");
        }

        public record ViewModel(
            [Required]
            string Name,

            [Required]
            string Message,

            [Required]
            [EmailAddress]
            string Email
        );
    }
}
