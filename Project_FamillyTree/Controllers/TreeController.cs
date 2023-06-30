using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var response = await client.GetAsync("http://localhost:45571/api/Family/1/tree");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var familyTree = JsonConvert.DeserializeObject<List<FamilyMemberNode>>(jsonString);

                return View(familyTree);
            }

            return View();
        }


    }
}