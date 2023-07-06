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

namespace Project_FamillyTree.Controllers
{
    public class FamilyMembersController : Controller
    {
        private readonly HttpClient client;
        private readonly string FamilyMemberUrl;

        public FamilyMembersController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FamilyMemberUrl = "http://localhost:45571/api/FamilyMember/CreateFamilyMember";
        }

        // GET: FamilyMembers
      

        // GET: FamilyMembers/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: FamilyMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(familyMember);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(FamilyMemberUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error to create");
                    return View();
                }
            }
            return View(familyMember);
        }

        // GET: FamilyMembers/Edit/5
        
        // GET: FamilyMembers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
            
        //}

        //// POST: FamilyMembers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.FamilyMembers == null)
        //    {
        //        return Problem("Entity set 'PRN231FamilyTreeContext.FamilyMembers'  is null.");
        //    }
        //    var familyMember = await _context.FamilyMembers.FindAsync(id);
        //    if (familyMember != null)
        //    {
        //        _context.FamilyMembers.Remove(familyMember);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool FamilyMemberExists(int id)
        //{
        //  return _context.FamilyMembers.Any(e => e.Id == id);
        //}
    }
}
