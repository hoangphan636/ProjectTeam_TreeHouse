using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace TestSendMailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        public IActionResult TestSendMail(string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("emmie.kreiger73@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("alize.pacocha62@ethereal.emailm"));

            email.Subject = "Send email Minh Duy";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body};

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("emmie.kreiger73@ethereal.email", "wEFa1qzeSaCZ2CDpAA");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
        
    }
}
