using System.ComponentModel.DataAnnotations;
using DevTrends.Mailer;
using HenryHiles.Pages._EmailTemplates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HenryHiles.Helpers;
using DevTrends.AwsClients;

namespace HenryHiles.Pages;
public class IndexModel : PageModel
{
    private readonly IMailSender _mailer;
    private readonly SesSettings _emailSettings;

    public IndexModel(IMailSender mailer, SesSettings emailSettings)
    {
        _mailer = mailer;
        _emailSettings = emailSettings;
    }

    [BindProperty]
    public ViewModel Data { get; set; } = default!;

    public async Task<PartialViewResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            if (Request.IsAjaxRequest() && (Request.GetIp() != "156.146.63.17"))
                await _mailer.SendEmail(
                    new ContactUsEmail(Data.Email, Data.Name, Data.Message, _emailSettings.FromAddress, Request.GetIp()),
                    replyTo: Data.Email
                );
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
