using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Project_FamillyTree.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private string productApiUrl = "";

        public LoginController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productApiUrl = "http://localhost:45571/odata/CompanyProjects/authenticate";
        }


        public IActionResult index()
        {
            return View();
        }



        public async Task<IActionResult> Login(Login p)
        {


            string strDatas = JsonSerializer.Serialize(p);
            var contentData = new StringContent(strDatas, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(productApiUrl, contentData);
            string strData = await response.Content.ReadAsStringAsync();
            if (strData == "\"Login Fail\"")
            {
                HttpContext.Session.SetString("CustomerName", "Seem Like wrong password or Email");
                return RedirectToAction("Index", "Login");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Tokens tokens = JsonSerializer.Deserialize<Tokens>(strData, options);
            string token = tokens.Token;
          //  HttpContext.Session.SetString("token", token);
            // Giải mã token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.First(c => c.Type == "email").Value;
            var role = jwtToken.Claims.First(c => c.Type == "role").Value;
            var memberFamilyId = jwtToken.Claims.First(c => c.Type == "MemberFamilyId").Value;

            if (emailClaim == null && role == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("emailClaim", emailClaim);
            HttpContext.Session.SetString("role", role);
            HttpContext.Session.SetString("MemberFamilyId", memberFamilyId);



            return RedirectToAction("Index","Tree");
        }





    }
}
