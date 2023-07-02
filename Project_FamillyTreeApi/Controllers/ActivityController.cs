using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;

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

        [HttpPost("send-email")]
        public IActionResult SendActivitiesEmail(int familyId)
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

        private string GenerateEmailContent(string memberName, List<Activity> activities)
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
