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

namespace Project_FamillyTree.Controllers
{
    public class ImagesController : Controller
    {
        private HttpClient client;
        private readonly string ImageUrl;

        public ImagesController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ImageUrl = "http://localhost:45571/api/Image";
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ImageUrl);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var images = JsonSerializer.Deserialize<List<Image>>(strData,options);
            return View(images);
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
            var image = JsonSerializer.Deserialize<List<Image>>(strData,options);

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            var albumId = HttpContext.Session.GetString("AlbumId");
           ViewData["AlbumId"]=albumId;
            return View();

        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Image image)
        {
            var json = JsonSerializer.Serialize(image);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ImageUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error to create");
                return View(image);
            }
        }

        //// GET: Images/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Images == null)
        //    {
        //        return NotFound();
        //    }

        //    var image = await _context.Images.FindAsync(id);
        //    if (image == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "UrlAlbum", image.AlbumId);
        //    return View(image);
        //}

        //// POST: Images/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,AlbumId,CreateDate,UrlImage")] Image image)
        //{
        //    if (id != image.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(image);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ImageExists(image.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "UrlAlbum", image.AlbumId);
        //    return View(image);
        //}

        //// GET: Images/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Images == null)
        //    {
        //        return NotFound();
        //    }

        //    var image = await _context.Images
        //        .Include(i => i.Album)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (image == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(image);
        //}

        //// POST: Images/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Images == null)
        //    {
        //        return Problem("Entity set 'PRN231FamilyTreeContext.Images'  is null.");
        //    }
        //    var image = await _context.Images.FindAsync(id);
        //    if (image != null)
        //    {
        //        _context.Images.Remove(image);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ImageExists(int id)
        //{
        //  return _context.Images.Any(e => e.Id == id);
        //}
    }
}
