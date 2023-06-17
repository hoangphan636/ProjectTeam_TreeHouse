using BusinessObject;
using BusinessObject.DataAccess;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountRepository _jWTManager;

        public AccountController(AccountRepository jWTManager)
        {
            _jWTManager = jWTManager;
        }

        [HttpGet]
        public List<Account> Get()
        {
            var list = new List<Account>();

           list =  _jWTManager.GetAccount();

            return list;
        }






    }
}
