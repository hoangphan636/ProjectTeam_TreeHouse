using AutoMapper;
using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_FamillyTreeApi.Mappers;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FamilyMemberController : ControllerBase
    {
        private readonly FamilyMemberRepository _repository;
        private readonly IMapper _mapper;
        public FamilyMemberController(FamilyMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FamilyMemberAPI>> GetFamilyMembers()
        {
            var listMember = _repository.List();
            var listMemberApi = _mapper.Map<List<FamilyMemberAPI>>(listMember);

            return listMemberApi;
        }

        [HttpGet("{id}")]
        public ActionResult<FamilyMemberAPI> GetGetFamilyMemberById(int id)
        {
            var member = _repository.Find(id);
            var memberApi = _mapper.Map<FamilyMemberAPI>(member);

            if (member == null)
            {
                return NotFound("Member isn't exist");
            }

            return memberApi;
        }

        [HttpPost]
        public ActionResult CreateFamilyMember(FamilyMemberAPI familyMemberApi)
        {
            var familyMember = _mapper.Map<FamilyMember>(familyMemberApi);
            try
            {
                _repository.Add(familyMember);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok("Create success!");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFamilyMember(int id, FamilyMemberAPI memberApi)
        {
            var member = _mapper.Map<FamilyMember>(memberApi);
            if (id != member.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(id, member);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Member isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Update success!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFamilyMember(int id)
        {
            var member = _repository.Find(id);
            if (member == null)
            {
                return NotFound("Member isn't exist");
            }

            try
            {
                _repository.Delete(member);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Member isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Delete success!");
        }
    }
}
