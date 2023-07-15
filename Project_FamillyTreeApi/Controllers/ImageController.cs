using BusinessObject.DataAccess;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Project_FamillyTreeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImage()
        {
            return Ok(await _imageRepository.GetAllImage());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            return Ok(await _imageRepository.GetImage(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Image image)
        {
            return Ok(await _imageRepository.Create(image));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Image image)
        {
            return Ok(await _imageRepository.Update(id, image));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _imageRepository.Delete(id);
            return NoContent();
        }

        [HttpGet("/image-by-album/{id}")]
        public async Task<IActionResult> GetImageByAlbum(int id)
        {
            return Ok(await _imageRepository.GetImageByAlbum(id));
        }
    }
}