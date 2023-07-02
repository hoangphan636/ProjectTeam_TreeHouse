using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace SendMail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendMail(string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("kacie8@ethereal.emai"));
            email.To.Add(MailboxAddress.Parse("kacie8@ethereal.emai"));

            email.Subject = "Send email Minh Duy";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("kacie8@ethereal.email", "M5RZSXYG4NfGkcgnm3");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
