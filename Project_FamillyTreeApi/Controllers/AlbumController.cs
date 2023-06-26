using AutoMapper;
using AutoMapper.Execution;
using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_FamillyTreeApi.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly AlbumRepository _repository;
        private readonly IMapper _mapper;

        public AlbumController(AlbumRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult PostAlbum(AlbumAPI albumAPI)
        {
            var album = _mapper.Map<Album>(albumAPI);
            try
            {
                _repository.Add(album);
                _repository.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok("Add success!");
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlbumAPI>> GetAlbums()
        {
            var listAlbum = _repository.List();
            var listAlbumAPI = _mapper.Map<List<AlbumAPI>>(listAlbum);

            return listAlbumAPI;
        }

        [HttpGet("{id}")]
        public ActionResult<AlbumAPI> GetAlbum(int id)
        {
            var album = _repository.Find(id);
            var albumAPI = _mapper.Map<AlbumAPI>(album);

            if (album == null)
            {
                return NotFound("Album isn't exist");
            }

            return albumAPI;
        }

        [HttpPut("{id}")]
        public IActionResult PutAlbum(int id, AlbumAPI albumAPI)
        {
            var album =  _mapper.Map<Album>(albumAPI);
            if (id != album.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(id, album);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Album isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Update success!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAlbum(int id)
        {
            var album = _repository.Find(id);
            if (album == null)
            {
                return NotFound("Album isn't exist");
            }

            try
            {
                _repository.Delete(album);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Exists(id))
                {
                    return NotFound("Album isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }
            
            return Ok("Delete success!");
        }

        [HttpGet("{familyId}")]
        public ActionResult<IEnumerable<AlbumAPI>> GetAlbumsByFamilyId(int familyId)
        {
            var listAlbum = _repository.GetAlbumsByFamilyId(familyId);
            var listAlbumAPI = _mapper.Map<List<AlbumAPI>>(listAlbum);

            if (listAlbumAPI.IsNullOrEmpty())
            {
                return NotFound("This family don't have any album.");
            }

            return listAlbumAPI;
        }
    }
}
