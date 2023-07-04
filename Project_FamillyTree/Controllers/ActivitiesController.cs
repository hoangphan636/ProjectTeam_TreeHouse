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
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Project_FamillyTree.Controllers
{
    public class ActivitiesController : Controller
    {

        private HttpClient client;
        private string ActivitiesApiUrl;
        private string FamilyApiApiUrl;

        public ActivitiesController()
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ActivitiesApiUrl = "http://localhost:45571/api/Activity";
            FamilyApiApiUrl = "http://localhost:45571/api/Family";
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:45571/api/Activity/activities-by-member/1");
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Activity> listActivity = System.Text.Json.JsonSerializer.Deserialize<List<Activity>>(strData, options);

            // Lấy thông tin Family cho mỗi Activity
            foreach (var activity in listActivity)
            {
                var familyResponse = await client.GetAsync($"{FamilyApiApiUrl}/{activity.FamilyId}");
                if (familyResponse.IsSuccessStatusCode)
                {
                    var familyJsonString = await familyResponse.Content.ReadAsStringAsync();
                    var family = JsonConvert.DeserializeObject<Family>(familyJsonString);
                    activity.Family = family;
                }
            }

            return View(listActivity);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.GetAsync($"{ActivitiesApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var activity = JsonConvert.DeserializeObject<Activity>(jsonString);

                if (activity == null)
                {
                    return NotFound();
                }

                // Lấy thông tin Family cho activity
                var familyResponse = await client.GetAsync($"{FamilyApiApiUrl}/{activity.FamilyId}");
                if (familyResponse.IsSuccessStatusCode)
                {
                    var familyJsonString = await familyResponse.Content.ReadAsStringAsync();
                    var family = JsonConvert.DeserializeObject<Family>(familyJsonString);
                    activity.Family = family;
                }

                return View(activity);
            }

            return NotFound();
        }


        public async Task<IActionResult> Create()
        {
            // Lấy danh sách Family để hiển thị trong dropdown list
            var familyListResponse = await client.GetAsync($"{FamilyApiApiUrl}");
            if (familyListResponse.IsSuccessStatusCode)
            {
                var familyListJsonString = await familyListResponse.Content.ReadAsStringAsync();
                var familyList = JsonConvert.DeserializeObject<List<Family>>(familyListJsonString);
                ViewBag.FamilyList = new SelectList(familyList, "Id", "FamilyName");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(activity);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(ActivitiesApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // Nếu có lỗi, cần lấy danh sách Family để hiển thị lại trong dropdown list
            var familyListResponse = await client.GetAsync($"{FamilyApiApiUrl}");
            if (familyListResponse.IsSuccessStatusCode)
            {
                var familyListJsonString = await familyListResponse.Content.ReadAsStringAsync();
                var familyList = JsonConvert.DeserializeObject<List<Family>>(familyListJsonString);
                ViewBag.FamilyList = new SelectList(familyList, "Id", "FamilyName");
            }

            return View(activity);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityResponse = await client.GetAsync($"{ActivitiesApiUrl}/{id}");
            if (activityResponse.IsSuccessStatusCode)
            {
                var activityJsonString = await activityResponse.Content.ReadAsStringAsync();
                var activity = JsonConvert.DeserializeObject<Activity>(activityJsonString);

                if (activity == null)
                {
                    return NotFound();
                }

                // Lấy danh sách Family để hiển thị trong dropdown list
                var familyListResponse = await client.GetAsync($"{FamilyApiApiUrl}");
                if (familyListResponse.IsSuccessStatusCode)
                {
                    var familyListJsonString = await familyListResponse.Content.ReadAsStringAsync();
                    var familyList = JsonConvert.DeserializeObject<List<Family>>(familyListJsonString);
                    ViewBag.FamilyList = new SelectList(familyList, "Id", "FamilyName");
                }

                return View(activity);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(activity);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{ActivitiesApiUrl}/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // Nếu có lỗi, cần lấy danh sách Family để hiển thị lại trong dropdown list
            var familyListResponse = await client.GetAsync($"{FamilyApiApiUrl}");
            if (familyListResponse.IsSuccessStatusCode)
            {
                var familyListJsonString = await familyListResponse.Content.ReadAsStringAsync();
                var familyList = JsonConvert.DeserializeObject<List<Family>>(familyListJsonString);
                ViewBag.FamilyList = new SelectList(familyList, "Id", "FamilyName");
            }

            return View(activity);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await client.GetAsync($"{ActivitiesApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var activity = JsonConvert.DeserializeObject<Activity>(jsonString);

                if (activity == null)
                {
                    return NotFound();
                }
                var familyResponse = await client.GetAsync($"{FamilyApiApiUrl}/{activity.FamilyId}");
                if (familyResponse.IsSuccessStatusCode)
                {
                    var familyJsonString = await familyResponse.Content.ReadAsStringAsync();
                    var family = JsonConvert.DeserializeObject<Family>(familyJsonString);
                    activity.Family = family;
                }
                return View(activity);
            }

            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await client.DeleteAsync($"{ActivitiesApiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public async Task<IActionResult> SendEmail(int memberId)
        {
            var response = await client.PostAsync($"{"http://localhost:45571/api/Activity/send-email/1"}/send-email/{memberId}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}