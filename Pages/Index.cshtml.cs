﻿using System.ComponentModel.DataAnnotations;
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
        private readonly EmailSettings _emailSettings;

        public IndexModel(IMailSender mailer, EmailSettings emailSettings)
        {
            _mailer = mailer;
            _emailSettings = emailSettings;
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
                    To = _emailSettings.FromAddress,
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
