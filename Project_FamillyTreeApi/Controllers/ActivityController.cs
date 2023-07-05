using Azure.Communication.Sms;
using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
using System;

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

        //[HttpPost("SendSMS")]
        //public IActionResult SendSMS(List<string> phoneNo, string content)
        //{
        //    var accountSid = "AC4def4fce1704e2d8c2439f574a819b0e";
        //    var authToken = "6e40501c1d86dc3e121a8ffb5eea5e92";
        //    TwilioClient.Init(accountSid, authToken);

        //    foreach (var num in phoneNo)
        //    {
        //        var messageOptions = new CreateMessageOptions(
        //            new PhoneNumber(num));
        //        messageOptions.From = new PhoneNumber("+14176412659");
        //        messageOptions.Body = content;
        //        MessageResource.Create(messageOptions);
        //    }
        //    return Ok("Send SMS success");
        //}

        [HttpPost("SendSMS")]
        public IActionResult SendSMS(List<string> phoneNo, string content)
        {
            // This code retrieves your connection string
            // from an environment variable.
            string connectionString = "endpoint=https://sendsms622.communication.azure.com/;accesskey=6++YFgPqDE/S1G1wGpYyU/AtJjb58y/ZDYVtahYI3c5VGpQTGWAMHrcgpe2c2kRnc/3jogntecXOUPLEgzjWvg==";
            SmsClient smsClient = new SmsClient(connectionString);

            SmsSendResult sendResult = smsClient.Send(
                from: "+84877660374",
                to: "+84822616468",
                message: "Hello World via SMS"
            );

            return Ok($"Sms id: {sendResult.MessageId}");
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

        [HttpGet("activities-by-family/{familyId}")]
        public List<Activity> GetActivitiesByFamilyId(int familyId)
        {
            return _jWTManager.GetActivitiesByFamilyId(familyId);
        }

        [HttpGet("activities-by-member/{memberId}")]
        public IActionResult GetActivitiesByMemberId(int memberId)
        {
            var activities = _jWTManager.GetActivitiesByMemberId(memberId);
            return Ok(activities);
        }

        [HttpGet("api/family-members-by-member/{memberId}")]
        public IActionResult GetAllFamilyMemberByMemberId(int memberId)
        {
            var familyMembers = _familyRepository.GetAllFamilyMemberByMemberId(memberId);
            return Ok(familyMembers);
        }

        [HttpPost("send-email/{memberId}")]
        public IActionResult SendActivitiesEmailByMemberId(int memberId)
        {
            var members = _familyRepository.GetAllFamilyMemberByMemberId(memberId).ToList();
            if (members == null)
            {
                return NotFound();
            }

            var activities = _jWTManager.GetActivitiesByMemberId(memberId);
            if (activities == null || activities.Count == 0)
            {
                return BadRequest("No activities found for the member.");
            }

            foreach (var member in members.ToList())
            {
                var emailContent = GenerateEmailContent(member.FullName, activities);

                try
                {
                    // Tạo đối tượng MailMessage
                    var message = new MailMessage();
                    message.From = new MailAddress("minhduy1511@gmail.com");
                    message.To.Add(member.Email);
                    message.Subject = "Activities Update";
                    message.Body = emailContent;
                    message.IsBodyHtml = false;

                    // Tạo đối tượng SmtpClient và cấu hình thông tin SMTP
                    var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    smtpClient.Credentials = new NetworkCredential("minhduy1511@gmail.com", "dxhuwemtdtkobzoj");
                    smtpClient.EnableSsl = true;

                    // Gửi email
                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu gửi email không thành công
                    return StatusCode(500, $"Failed to send email to {member.Email}. Error: {ex.Message}");
                }
            }

            return Ok();
        }


        [HttpPost("send-email")]
        public IActionResult SendActivitiesEmailByFamilyId(int familyId)
        {
            var family = _familyRepository.GetAllFamilyMemberByFamily(familyId);
            if (family == null)
            {
                return NotFound();
            }

            var activities = _jWTManager.GetActivitiesByFamilyId(familyId);
            if (activities == null || activities.Count == 0)
            {
                return BadRequest("No activities found for the family.");
            }
            //var listMember = family.FamilyMembers.ToList();
            foreach (var member in family.ToList())
            {
                var emailContent = GenerateEmailContent(member.FullName, activities);

                try
                {
                    // Tạo đối tượng MailMessage
                    var message = new MailMessage();
                    message.From = new MailAddress("minhduy1511@gmail.com");
                    message.To.Add(member.Email);
                    message.Subject = "Activities Update";
                    message.Body = emailContent;
                    message.IsBodyHtml = false;

                    // Tạo đối tượng SmtpClient và cấu hình thông tin SMTP
                    var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    smtpClient.Credentials = new NetworkCredential("minhduy1511@gmail.com", "dxhuwemtdtkobzoj");
                    smtpClient.EnableSsl = true;

                    // Gửi email
                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu gửi email không thành công
                    return StatusCode(500, $"Failed to send email to {member.Email}. Error: {ex.Message}");
                }
            }

            return Ok();
        }

        private static string GenerateEmailContent(string memberName, List<Activity> activities)
        {
            // Tạo nội dung email từ danh sách activities và thông tin thành viên gia đình
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
        }

    }
}