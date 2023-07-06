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

        [HttpPut("{id}")]
        public IActionResult UpdateActivity(int id, [FromBody] Activity activity)
        {
            _jWTManager.Update(id, activity);
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
                var emailContent = GenerateEmailContents(member.FullName, activities);

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
        [HttpPost("send-email/{memberId}/{activityId}")]
        public IActionResult SendActivitiesEmailByMemberId(int memberId, int activityId)
        {
            var members = _familyRepository.GetAllFamilyMemberByMemberId(memberId).ToList();
            if (members == null)
            {
                return NotFound();
            }

            var activities = _jWTManager.GetActivitiesByMemberId(memberId);
            var selectedActivity = activities.FirstOrDefault(a => a.Id == activityId);
            if (selectedActivity == null)
            {
                return BadRequest("The specified activity does not exist for the member.");
            }

            foreach (var member in members.ToList())
            {
                var emailContent = GenerateEmailContent(member.FullName, selectedActivity);

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


        [HttpPost("send-email/{familyId}")]
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
                var emailContent = GenerateEmailContents(member.FullName, activities);

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

        private static string GenerateEmailContents(string memberName, List<Activity> activities)
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
        private static string GenerateEmailContent(string memberName, Activity activity)
        {
            // Tạo nội dung email từ hoạt động và thông tin thành viên gia đình
            var emailContent = $"Dear {memberName},\n\n";
            emailContent += "Here is the latest activity for your family:\n\n";
            emailContent += $"- {activity.ActivityName}: {activity.Description}\n";
            emailContent += $"   Start Date: {activity.StartDate}\n";
            emailContent += $"   End Date: {activity.EndDate}\n\n";
            emailContent += "Best regards,\nYour Family";

            return emailContent;
        }
    }
}