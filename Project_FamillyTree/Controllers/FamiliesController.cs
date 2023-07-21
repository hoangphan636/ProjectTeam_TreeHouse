using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DataAccess;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace Project_FamillyTree.Controllers
{
    public class FamiliesController : Controller
    {
        private readonly HttpClient client = null;
        private string apiUrl = "";
        private string apiUrlGetAll = "";

        public FamiliesController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            apiUrl = "http://localhost:45571/api/Family";
            apiUrlGetAll = "http://localhost:45571/api/Family/all";

        }

        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("role");
            if (role != "Admin")
            {
                return RedirectToAction("Index", "Tree");
            }

            HttpResponseMessage response = await client.GetAsync(apiUrlGetAll);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Family> families = System.Text.Json.JsonSerializer.Deserialize<List<Family>>(strData, options);

            return View(families);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Family family)
        {

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(family);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(family);
        }

        public async Task<IActionResult> Details(int id)
        {

            var response = await client.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var family = JsonConvert.DeserializeObject<Family>(jsonString);
                return View(family);
            }

            return NotFound();
        }


        public async Task<IActionResult> Edit(int id)
        {

            var response = await client.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var family = JsonConvert.DeserializeObject<Family>(jsonString);
                return View(family);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Family family)
        {
            if (id != family.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(family);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(family);

        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await client.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var family = JsonConvert.DeserializeObject<Family>(jsonString);
                return View(family);
            }

            return NotFound();
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await client.DeleteAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}