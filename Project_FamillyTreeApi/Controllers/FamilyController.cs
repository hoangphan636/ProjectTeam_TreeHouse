using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : Controller
    {
        private readonly FamilyRepository _jWTManager;

        public FamilyController(FamilyRepository jWTManager)
        {
            _jWTManager = jWTManager;
        }

        [HttpGet("all")]
        public List<Family> GetAllFamily()
        {
            var list = _jWTManager.GetAllFamily();
            return list;
        }
        [HttpGet("{id}")]
        public Family GetFamilyById(int id)
        {
            return _jWTManager.GetFamily(id);
        }

        [HttpGet("search")]
        public List<Family> SearchFamilies(string family)
        {
            var list = _jWTManager.SearchFamily(family);
            return list;
        }

        [HttpPost]
        public IActionResult CreateFamily([FromBody] Family family)
        {
            _jWTManager.Add(family);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateFamily([FromBody] Family family)
        {
            _jWTManager.Update(family);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFamily(int id)
        {
            var family = _jWTManager.GetFamily(id);
            if (family == null)
            {
                return NotFound();
            }
            _jWTManager.Delete(family);
            return Ok();
        }
    }
}
