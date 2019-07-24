using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class ForgotPassWordController : Controller
    {
        private readonly NittanDBcontext _Context;
        private IEmailService _emailService;

        public ForgotPassWordController(NittanDBcontext context, IEmailService emailService)
        {
            this._Context = context;
            this._emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Joey Tribbiani", "admin@friends.com"));
            message.To.Add(new MailboxAddress("Mrs. Waranyu", "waranyu@m-focus.co.th"));
            message.Subject = "How you doin'?";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.inetmail.cloud", 25, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("waranyu@m-focus.co.th", "");

                client.Send(message);
                client.Disconnect(true);
            }
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string email, string subject, string submit)
        {

            return Ok();
        }
    }
}