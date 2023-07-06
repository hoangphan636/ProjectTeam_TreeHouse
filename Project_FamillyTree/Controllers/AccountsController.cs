using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DataAccess;
using System.Linq;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Project_FamillyTree.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient client = null;
        private string apiUrl = "";
        public AccountController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            apiUrl = "http://localhost:45571/api/Account";

        }
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("role");
            if(role != "Admin" )
            {
                return RedirectToAction("Index","Tree");
            }

            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Account> listActivity = System.Text.Json.JsonSerializer.Deserialize<List<Account>>(strData, options);

            return View(listActivity);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(account);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(account);
        }

        public async Task<IActionResult> Details(int id)
        {

            var response = await client.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(jsonString);
                return View(account);
            }

            return NotFound();
        }


        public async Task<IActionResult> Edit(int id)
        {

            var response = await client.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(jsonString);
                return View(account);
            }

            return NotFound();
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(account);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(account);

        }

        public async Task<IActionResult> Deleted(int id)
        {
            var response = await client.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(jsonString);
                return View(account);
            }

            return NotFound();
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
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