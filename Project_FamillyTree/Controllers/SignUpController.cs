using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DataAccess;
using System.Net.Http.Headers;
using System.Net.Http;
using BusinessObject.DTO;
using Newtonsoft.Json;

namespace Project_FamillyTree.Controllers
{
    public class SignUpController : Controller
    {
        private readonly PRN231FamilyTreeContext _context;
        private readonly HttpClient client;
        private readonly string AccountUrl;

        public SignUpController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AccountUrl = "http://localhost:45571/CreateAccount";
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountDTO account)
        {

            var json = JsonConvert.SerializeObject(account);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(AccountUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "Login");
            }
            else
            {
                ModelState.AddModelError("", "Error to create");
                return View();
            }
        }

    }
}
