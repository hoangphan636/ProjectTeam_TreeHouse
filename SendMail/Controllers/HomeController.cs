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
            email.From.Add(MailboxAddress.Parse("minhduy1511@gmail.com"));
            email.To.Add(MailboxAddress.Parse("minhduy15112023@gmail.com"));

            email.Subject = "Send email Minh Duy";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("minhduy1511@gmail.com", "dxhuwemtdtkobzoj");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
