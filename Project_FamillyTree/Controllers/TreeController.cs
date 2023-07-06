using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_FamillyTree.Controllers
{
    public class TreeController : Controller
    {
        private HttpClient client;
        public string TreeUrl;
        public TreeController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            TreeUrl = "http://localhost:45571/api/Family";
        }

        public async Task<IActionResult> Index()
        {
            var   memberId = HttpContext.Session.GetString("MemberFamilyId");

            if (memberId == null)
            {
                return RedirectToAction("index", "Login");
            }
            var response = await client.GetAsync("http://localhost:45571/api/Family/1/tree");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var familyTree = System.Text.Json.JsonSerializer.Deserialize<List<FamilyMemberNode>>(strData,options);

                return View(familyTree);
            }

            return View();
        }

        [HttpPost, ActionName("Logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "Login");
        }
    }
}