using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : Controller
    {
        private readonly ActivitiesRepository _jWTManager;

        public ActivityController(ActivitiesRepository jWTManager)
        {
            _jWTManager = jWTManager;
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
    }
}
