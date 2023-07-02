using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;
using Twilio.Types;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : Controller
    {
        private readonly ActivitiesRepository _jWTManager;
        private readonly FamilyRepository _familyRepository;

        public ActivityController(ActivitiesRepository jWTManager, FamilyRepository familyRepository)
        {
            _jWTManager = jWTManager;
            _familyRepository = familyRepository;
        }

        [HttpGet("all")]
        public List<Activity> GetAllActivities()
        {
            var list = _jWTManager.GetAllActivity();
            return list;
        }

        [HttpGet("{id}")]
        public Activity GetActivityById(int id)
        {
            return _jWTManager.GetActivity(id);
        }

        [HttpGet("search")]
        public List<Activity> SearchActivities(string activity)
        {
            var list = _jWTManager.SearchActivity(activity);
            return list;
        }

        [HttpPost]
        public IActionResult CreateActivity([FromBody] Activity activity)
        {
            _jWTManager.Add(activity);
            return Ok();
        }

        [HttpPost("SendSMS")]
        public IActionResult SendSMS(List<string> phoneNo, string content)
        {
            var accountSid = "AC4def4fce1704e2d8c2439f574a819b0e";
            var authToken = "6e40501c1d86dc3e121a8ffb5eea5e92";
            TwilioClient.Init(accountSid, authToken);

            foreach (var num in phoneNo)
            {
                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber(num));
                messageOptions.From = new PhoneNumber("+14176412659");
                messageOptions.Body = content;
                MessageResource.Create(messageOptions);
            }
            return Ok("Send SMS success");
        }

        [HttpPost("SendMail")]
        public IActionResult SendMail(List<string> toMails, string content)
        {
            MailMessage message = new MailMessage();
            string from = "rongbay.dt@gmail.com";
            string pass = "sjdoqwhhfhrlliyq";

            foreach(var toMail in toMails)
            {
                message.To.Add(toMail);
            }
            message.From = new MailAddress(from);
            message.Body = content;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(from, pass)
            };
            smtp.Send(message);

            return Ok("Send mail success");
        }


        [HttpPut]
        public IActionResult UpdateActivity([FromBody] Activity activity)
        {
            _jWTManager.Update(activity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteActivity(int id)
        {
            var activity = _jWTManager.GetActivity(id);
            if (activity == null)
            {
                return NotFound();
            }
            _jWTManager.Delete(activity);
            return Ok();
        }
        /*[HttpPost("send-email")]
        public async Task<IActionResult> SendActivitiesEmail(int familyId)
        {
            var family = _familyRepository.GetFamily(familyId);
            if (family == null)
            {
                return NotFound();
            }

            var activities = _jWTManager.GetActivitiesByFamilyId(familyId);
            if (activities.Count == 0)
            {
                return BadRequest("No activities found for the family.");
            }

            foreach (var member in family.FamilyMembers)
            {
                var emailContent = GenerateEmailContent(member.FullName, activities);

                var message = new SendGridMessage();
                message.AddTo(member.Email);
                message.SetFrom("your-email@example.com");
                message.SetSubject("Activities Update");
                message.AddContent(MimeType.Text, emailContent);

                // Gửi email bằng SendGrid
                await sendGridClient.SendEmailAsync(message);
            }

            return Ok();
        }

        private string GenerateEmailContent(string memberName, List<Activity> activities)
        {
            // Tạo nội dung email từ danh sách activities và thông tin thành viên gia đình
            // Đây chỉ là một ví dụ đơn giản, bạn có thể tùy chỉnh nội dung email theo yêu cầu của bạn
            var emailContent = $"Dear {memberName},\n\n";
            emailContent += "Here are the latest activities for your family:\n\n";
            foreach (var activity in activities)
            {
                emailContent += $"- {activity.ActivityName}: {activity.Description}\n";
                emailContent += $"   Start Date: {activity.StartDate}\n";
                emailContent += $"   End Date: {activity.EndDate}\n\n";
            }
            emailContent += "Best regards,\nYour Family";

            return emailContent;
        }*/
        

    }
}
