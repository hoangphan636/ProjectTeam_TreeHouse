using AutoMapper;
using BusinessObject;
using BusinessObject.DataAccess;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_FamillyTreeApi.Mappers;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountRepository _jWTManager;
        private readonly IMapper _mapper;

        public AccountController(AccountRepository jWTManager, IMapper mapper)
        {
            _jWTManager = jWTManager;
            _mapper = mapper;
        }

        [HttpGet]
        public List<AccountAPI> Get()
        {
            var list = _jWTManager.GetAccount();
            var listAPI = _mapper.Map<List<AccountAPI>>(list);

            return listAPI;
        }

        [HttpPost]
        public ActionResult PostAccount(AccountAPI accountAPI)
        {
            var account = _mapper.Map<Account>(accountAPI);
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
        public ActionResult<AccountAPI> GetAccount(int id)
        {
            var account = _jWTManager.GetAccountID(id);
            var accountAPI = _mapper.Map<AccountAPI>(account);

            if (account == null)
            {
                return NotFound("Album isn't exist");
            }

            return accountAPI;
        }

        [HttpPut("{id}")]
        public IActionResult PutAccount(int id, AccountAPI accountAPI)
        {
            var account = _mapper.Map<Account>(accountAPI);
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
                if (_jWTManager.GetAccountID(id)==null)
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
                if (_jWTManager.GetAccountID(id)==null)
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


    }
}
