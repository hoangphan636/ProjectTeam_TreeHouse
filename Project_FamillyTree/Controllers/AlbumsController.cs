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
using Newtonsoft.Json;
using System.Text.Json;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Project_FamillyTree.Controllers
{
    public class AlbumsController : Controller
    {
        private HttpClient client;
        private string AlbumApiUrl;
        private string FamilyApiApiUrl;

        public AlbumsController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AlbumApiUrl = "http://localhost:45571/api/Album";
            FamilyApiApiUrl = "http://localhost:45571/api/Family";
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetString("MemberFamilyId");
            var familyMemberResponse = await client.GetAsync($"http://localhost:45571/api/FamilyMember/GetGetFamilyMemberById/{id}");

            FamilyMember familyMember = null;
            if (familyMemberResponse.IsSuccessStatusCode)
            {
                var familyMemberJsonString = await familyMemberResponse.Content.ReadAsStringAsync();
                familyMember = JsonConvert.DeserializeObject<FamilyMember>(familyMemberJsonString);
            }

            if (familyMember == null)
            {
                var role = HttpContext.Session.GetString("role");
                if (role == "Admin")
                {
                    HttpResponseMessage res = await client.GetAsync($"{AlbumApiUrl}/GetAlbums");
                    string stringData = await res.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    List<Album> listByAdmin = System.Text.Json.JsonSerializer.Deserialize<List<Album>>(stringData, option);

                    return View(listByAdmin);
                }
                return RedirectToAction(nameof(Index));
            }


            HttpResponseMessage response = await client.GetAsync($"{AlbumApiUrl}/GetAlbumsByFamilyId/{familyMember.FamilyId}");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            List<Album> list = null;

            if (response.IsSuccessStatusCode)
            {
                list = System.Text.Json.JsonSerializer.Deserialize<List<Album>>(strData, options);

                foreach (var album in list)
                {
                    var familyResponse = await client.GetAsync($"{FamilyApiApiUrl}/{album.FamilyId}");
                    if (familyResponse.IsSuccessStatusCode)
                    {
                        var familyJsonString = await familyResponse.Content.ReadAsStringAsync();
                        var family = JsonConvert.DeserializeObject<Family>(familyJsonString);
                        album.Family = family;
                    }
                }
            }

            return View(list);
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.GetAsync($"{AlbumApiUrl}/GetAlbum/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var album = JsonConvert.DeserializeObject<Album>(jsonString);

                if (album == null)
                {
                    return NotFound();
                }

                var familyResponse = await client.GetAsync($"{FamilyApiApiUrl}/{album.FamilyId}");
                if (familyResponse.IsSuccessStatusCode)
                {
                    var familyJsonString = await familyResponse.Content.ReadAsStringAsync();
                    var family = JsonConvert.DeserializeObject<Family>(familyJsonString);
                    album.Family = family;
                }
                HttpContext.Session.SetString("AlbumId", album.Id.ToString());
                return RedirectToAction("Details", "Images", new {id=album.Id});
                
            }

            return NotFound();
        }

        // GET: Albums/Create
        public async Task<IActionResult> Create()
        {

            var id = HttpContext.Session.GetString("MemberFamilyId");
            var familyMemberResponse = await client.GetAsync($"http://localhost:45571/api/FamilyMember/GetGetFamilyMemberById/{id}");
            if (familyMemberResponse.IsSuccessStatusCode)
            {
                var familyMemberJsonString = await familyMemberResponse.Content.ReadAsStringAsync();
                var familyMember = JsonConvert.DeserializeObject<FamilyMember>(familyMemberJsonString);
                var familyId = familyMember.FamilyId;
                ViewData["familyId"] = familyId;
            }

            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FamilyId,AlbumName,UrlAlbum,Description")] Album album)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(album);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{AlbumApiUrl}/PostAlbum", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }



            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.GetAsync($"{AlbumApiUrl}/GetAlbum/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var album = JsonConvert.DeserializeObject<Album>(jsonString);

                if (album == null)
                {
                    return NotFound();
                }

                var memberFamilyId = HttpContext.Session.GetString("MemberFamilyId");
                var familyMemberResponse = await client.GetAsync($"http://localhost:45571/api/FamilyMember/GetGetFamilyMemberById/{memberFamilyId}");
                if (familyMemberResponse.IsSuccessStatusCode)
                {
                    var familyMemberJsonString = await familyMemberResponse.Content.ReadAsStringAsync();
                    var familyMember = JsonConvert.DeserializeObject<FamilyMember>(familyMemberJsonString);
                    var familyId = familyMember.FamilyId;
                    ViewData["familyId"] = familyId;
                }

                return View(album);
            }

            return NotFound();
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(album);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{AlbumApiUrl}/PutAlbum/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // Nếu có lỗi, cần lấy danh sách Family để hiển thị lại trong dropdown list
            var familyListResponse = await client.GetAsync($"{FamilyApiApiUrl}/all");
            if (familyListResponse.IsSuccessStatusCode)
            {
                var familyListJsonString = await familyListResponse.Content.ReadAsStringAsync();
                var familyList = JsonConvert.DeserializeObject<List<Family>>(familyListJsonString);
                ViewBag.FamilyList = new SelectList(familyList, "Id", "FamilyName");
            }

            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.GetAsync($"{AlbumApiUrl}/GetAlbum/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var album = JsonConvert.DeserializeObject<Album>(jsonString);

                if (album == null)
                {
                    return NotFound();
                }

                //var familyResponse = await client.GetAsync($"{FamilyApiApiUrl}/{album.FamilyId}");
                //if (familyResponse.IsSuccessStatusCode)
                //{
                //    var familyJsonString = await familyResponse.Content.ReadAsStringAsync();
                //    var family = JsonConvert.DeserializeObject<Family>(familyJsonString);
                //    activity.Family = family;
                //}
                return View(album);
            }

            return NotFound();
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await client.DeleteAsync($"{AlbumApiUrl}/DeleteAlbum/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}