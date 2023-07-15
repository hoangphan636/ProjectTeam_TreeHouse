using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DataAccess;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using Firebase.Storage;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;

namespace Project_FamillyTree.Controllers
{
    public class ImagesController : Controller
    {
        private HttpClient client;
        private readonly string ImageUrl;
        private readonly IConfiguration _configuration;

        public ImagesController(IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ImageUrl = "http://localhost:45571/api/Image";
            _configuration = configuration;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ImageUrl);
            if (response.IsSuccessStatusCode)
            {
                var strData = await response.Content.ReadAsStringAsync();
                var images = JsonSerializer.Deserialize<List<Image>>(strData);
                return View(images);
            }
            else
            {
                ModelState.AddModelError("", "Error retrieving images");
                return View(new List<Image>());
            }
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:45571/image-by-album/{id}");
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var image = JsonSerializer.Deserialize<List<Image>>(strData, options);

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            var albumId = HttpContext.Session.GetString("AlbumId");
            ViewData["AlbumId"] = albumId;
            return View();

        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Images/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Image image, IFormFile file)
        {
            var albumId = HttpContext.Session.GetString("AlbumId");

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString(); 
                    string fileExtension = Path.GetExtension(file.FileName); 

                    var options = new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(_configuration["Firebase:apiKey"])
                    };

                    var firebaseStorage = new FirebaseStorage(_configuration["Firebase:Bucket"], options)
                        .Child("images")
                        .Child(fileName + fileExtension);

                    await firebaseStorage.PutAsync(file.OpenReadStream());

                    string imageUrl = await firebaseStorage.GetDownloadUrlAsync();

                    image.UrlImage = imageUrl;
                }

                var json = JsonSerializer.Serialize(image);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{ImageUrl}/Create", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", "Images", new { id = albumId });
                }
            }
            ModelState.AddModelError("", "Error to create");
            return View(image);
        }



        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await client.DeleteAsync($"{ImageUrl}/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albumId = HttpContext.Session.GetString("AlbumId");
            var response = await client.DeleteAsync($"{ImageUrl}/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", "Images", new { id = albumId });
            }

            return NotFound();
        }




    }
}