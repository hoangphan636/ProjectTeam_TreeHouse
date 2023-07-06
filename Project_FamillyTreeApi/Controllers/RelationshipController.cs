using AutoMapper;
using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_FamillyTreeApi.Mappers;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RelationshipController : ControllerBase
    {
        private readonly RelationshipRepository _repository;
        private readonly IMapper _mapper;

        public RelationshipController(RelationshipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult PostRelationship(RelationshipAPI relationshipAPI)
        {
            var relationship = _mapper.Map<Relationship>(relationshipAPI);
            try
            {
                _repository.Add(relationship);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok("Add success!");
        }

        [HttpGet]
        public ActionResult<IEnumerable<RelationshipAPI>> GetRelationships()
        {
            var listRelationship = _repository.List();
            var listRelationshipAPI = _mapper.Map<List<RelationshipAPI>>(listRelationship);

            return listRelationshipAPI;
        }

        [HttpGet("{id}")]
        public ActionResult<RelationshipAPI> GetRelationship(int id)
        {
            var relationship = _repository.Find(id);
            var relationshipAPI = _mapper.Map<RelationshipAPI>(relationship);

            if (relationship == null)
            {
                return NotFound("Relationship isn't exist");
            }

            return relationshipAPI;
        }

        [HttpPut("{id}")]
        public IActionResult PutRelationship(int id, RelationshipAPI relationshipAPI)
        {
            var relationship = _mapper.Map<Relationship>(relationshipAPI);
            if (id != relationship.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(id, relationship);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Relationship isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Update success!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRelationship(int id)
        {
            var relationship = _repository.Find(id);
            if (relationship == null)
            {
                return NotFound("Relationship isn't exist");
            }

            try
            {
                _repository.Delete(relationship);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Relationship isn't exist");
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
