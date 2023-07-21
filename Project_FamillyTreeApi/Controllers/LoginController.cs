using BusinessObject;
using BusinessObject;
using BusinessObject.DataAccess;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_FamillyTreeApi.Controllers
{
  
    [Route("odata/CompanyProjects")]
    [EnableQuery]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly LoginDAO _jWTManager;
        private readonly PRN231FamilyTreeContext _context;

        public LoginController(LoginDAO jWTManager, PRN231FamilyTreeContext context)
        {
            _jWTManager = jWTManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]

        public IActionResult Authenticate(Login usersdata)
        {
            var token = _jWTManager.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [HttpGet]
        [Route("merge-account")]
        public async Task<IActionResult> MergeAccount(string email)
        {
            var familyMember = await _context.FamilyMembers.FirstOrDefaultAsync(a => a.Email == email);
            if (familyMember != null)
            {
                var account = new Account
                {
                    FullName = familyMember.FullName,
                    Email = familyMember.Email,
                    Role = 2,
                    MemberId = familyMember.Id,
                    //Member = familyMember
                };
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return Ok(account);
            }
            return NotFound();
        }

    }
}
