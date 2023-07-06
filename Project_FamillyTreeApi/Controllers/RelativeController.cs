using AutoMapper;
using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_FamillyTreeApi.Mappers;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RelativeController : ControllerBase
    {
        private readonly RelativeRepository _repository;
        private readonly IMapper _mapper;

        public RelativeController(RelativeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult PostRelative(RelativeAPI relativeAPI)
        {
            var relative = _mapper.Map<Relative>(relativeAPI);
            try
            {
                _repository.Add(relative);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok("Add success!");
        }

        [HttpGet]
        public ActionResult<IEnumerable<RelativeAPI>> GetRelatives()
        {
            var listRelative = _repository.List();
            var listRelativeAPI = _mapper.Map<List<RelativeAPI>>(listRelative);

            return listRelativeAPI;
        }

        [HttpGet("{id}")]
        public ActionResult<RelativeAPI> GetRelative(int id)
        {
            var relative = _repository.Find(id);
            var relativeAPI = _mapper.Map<RelativeAPI>(relative);

            if (relative == null)
            {
                return NotFound("Relative isn't exist");
            }

            return relativeAPI;
        }

        [HttpPut("{id}")]
        public IActionResult PutRelative(int id, RelativeAPI relativeAPI)
        {
            var relative = _mapper.Map<Relative>(relativeAPI);
            if (id != relative.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(id, relative);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Relative isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Update success!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRelative(int id)
        {
            var relative = _repository.Find(id);
            if (relative == null)
            {
                return NotFound("Relative isn't exist");
            }

            try
            {
                _repository.Delete(relative);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Relative isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Delete success!");
        }

        [HttpGet("{memberId}")]
        public ActionResult<IEnumerable<RelativeAPI>> GetRelativesByMemberId(int memberId)
        {
            var listRelative = _repository.GetRelativesByMemberId(memberId);
            var listRelativeAPI = _mapper.Map<List<RelativeAPI>>(listRelative);

            if (listRelativeAPI.IsNullOrEmpty())
            {
                return NotFound("This member don't have any relative.");
            }

            return listRelativeAPI;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RelativeAPI>> GetRelativesByRelationWithMemberId(int memberId, int relationId)
        {
            var listRelative = _repository.GetRelativesByRelationWithMemberId(memberId, relationId);
            var listRelativeAPI = _mapper.Map<List<RelativeAPI>>(listRelative);

            if (listRelativeAPI.IsNullOrEmpty())
            {
                return NotFound("The member isn't exist or This member don't have any relative.");
            }

            return listRelativeAPI;
        }
    }
}
