using BusinessObject;
using BusinessObject.DataAccess;
using BusinessObject.DTO;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Project_FamillyTreeApi.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountRepository _jWTManager;
        private readonly PRN231FamilyTreeContext _context;

        public AccountController(AccountRepository jWTManager, PRN231FamilyTreeContext context)
        {
            _jWTManager = jWTManager;
            _context = context;
        }

        [HttpGet]
        public List<Account> Get()
        {
            var list = new List<Account>();

            list = _jWTManager.GetAccount();

            return list;
        }

        [HttpPost]
        public ActionResult PostAccount(Account account)
        {

            try
            {
                _jWTManager.SaveCustomer(account);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok("Add success!");
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            var account = _jWTManager.GetAccountID(id);


            if (account == null)
            {
                return NotFound("Album isn't exist");
            }

            return account;
        }

        [HttpPut("{id}")]
        public IActionResult PutAccount(int id, Account account)
        {

            if (id != account.Id)
            {
                return BadRequest();
            }

            try
            {
                _jWTManager.UpdateCustomer(account);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_jWTManager.GetAccountID(id) == null)
                {
                    return NotFound("Album isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Update success!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAlbum(int id)
        {
            var account = _jWTManager.GetAccountID(id);
            if (account == null)
            {
                return NotFound("Album isn't exist");
            }

            try
            {
                _jWTManager.DeleteCustomer(account);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_jWTManager.GetAccountID(id) == null)
                {
                    return NotFound("Album isn't exist");
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok("Delete success!");
        }

        [HttpPost("/CreateAccount")]
        public IActionResult CreateAccount(AccountDTO account)
        {
            var accountNew = _context.Accounts.SingleOrDefault(a => a.Email == account.Email);
            if (accountNew == null)
            {
                var family = new Family();
                family.FamilyName = account.FullName;
                _context.Families.Add(family);
                _context.SaveChanges();

                account.Role = 2; 
                var familyMember = new FamilyMember
                {
                    Accounts = new Account
                    {
                        FullName = account.FullName,
                        Email = account.Email,
                        Password = account.Password,
                        Role = account.Role
                    },
                    FamilyId = family.Id,
                    FullName = account.FullName,
                    Dob = System.DateTime.Now,
                    Phone = account.Phone,
                    Gender =account.Gender,
                };
                _context.FamilyMembers.Add(familyMember);
                _context.SaveChanges();

                return Ok(account);
            }
            else
            {
                return Conflict(); 
            }
        }


    }

}
