using BusinessObject;
using BusinessObject.DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Collections.Generic;

namespace Project_FamillyTreeApi.Controllers
{

    [Route("odata/CompanyProjects")]
    [EnableQuery]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly LoginDAO _jWTManager;

        public LoginController(LoginDAO jWTManager)
        {
            _jWTManager = jWTManager;
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
    }
}
