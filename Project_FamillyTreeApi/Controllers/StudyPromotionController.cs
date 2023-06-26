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
    public class StudyPromotionController : ControllerBase
    {
        private readonly StudyPromotionRepository _repository;
        private readonly IMapper _mapper;

        public StudyPromotionController(StudyPromotionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult PostStudyPromotion(StudyPromotionAPI studyPromotionAPI)
        {
            var studyPromotion = _mapper.Map<StudyPromotion>(studyPromotionAPI);
            try
            {
                _repository.Add(studyPromotion);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok("Add success!");
        }

        [HttpGet]
        public ActionResult<IEnumerable<StudyPromotionAPI>> GetStudyPromotions()
        {
            var listStudyPromotion = _repository.List();
            var listStudyPromotionAPI = _mapper.Map<List<StudyPromotionAPI>>(listStudyPromotion);

            return listStudyPromotionAPI;
        }

        [HttpGet("{id}")]
        public ActionResult<StudyPromotionAPI> GetStudyPromotion(int id)
        {
            var studyPromotion = _repository.Find(id);
            var studyPromotionAPI = _mapper.Map<StudyPromotionAPI>(studyPromotion);

            if (studyPromotion == null)
            {
                return NotFound("StudyPromotion isn't exist");
            }

            return studyPromotionAPI;
        }

        [HttpPut("{id}")]
        public IActionResult PutStudyPromotion(int id, StudyPromotionAPI studyPromotionAPI)
        {
            var studyPromotion = _mapper.Map<StudyPromotion>(studyPromotionAPI);
            if (id != studyPromotion.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(id, studyPromotion);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("StudyPromotion isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Update success!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudyPromotion(int id)
        {
            var studyPromotion = _repository.Find(id);
            if (studyPromotion == null)
            {
                return NotFound("StudyPromotion isn't exist");
            }

            try
            {
                _repository.Delete(studyPromotion);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("StudyPromotion isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Delete success!");
        }

        [HttpGet("{familyId}")]
        public ActionResult<IEnumerable<StudyPromotionAPI>> GetStudyPromotionsByFamilyId(int familyId)
        {
            var listStudyPromotion = _repository.GetStudyPromotionsByFamilyId(familyId);
            var listStudyPromotionAPI = _mapper.Map<List<StudyPromotionAPI>>(listStudyPromotion);

            if (listStudyPromotionAPI.IsNullOrEmpty())
            {
                return NotFound("This family don't have any studyPromotion.");
            }

            return listStudyPromotionAPI;
        }
    }
}
