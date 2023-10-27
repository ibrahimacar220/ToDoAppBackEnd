using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.Business.SqlServer.Business.Interface;
using ToDoApp.Business.SqlServer.Models;
using ToDoApp.WebApi.Dto.Login;

namespace ToDoApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        readonly ToDoAppDbContext dbContext;

        public AuthController()
        {
            dbContext = new ToDoAppDbContext();
        }

        [HttpPost]
        public string Login([FromBody] UserLoginCriteriaDto userLoginCriteriaDto)
        {
            // i don't recommended this usage !!!!
            // bu kullanımı tavsiye etmiyorum !!!!
            string userName = userLoginCriteriaDto.UserName;
            string password = userLoginCriteriaDto.Password;
            //dont write sql query in controller
            User user = dbContext.Users.FirstOrDefault(p => p.Username == userName);
            if (user != null)
            {
                string userPswd = user.Password;
                if (userPswd == password)
                {
                    return JwtTokenBuilder.BuildToken(userName);
                }
            }
            
            return "user Not Found";
           
        }
        //[HttpGet("ValidateToken")]
        //public bool loginController(string token)
        //{
        //    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingkey));
        //    try
        //    {
        //        JwtSecurityTokenHandler handler = new();
        //        handler.ValidateToken(token, new TokenValidationParameters()
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = securitykey,
        //            ValidateLifetime = true,
        //            ValidateAudience = false,
        //            ValidateIssuer = false,
        //        }, out SecurityToken validatedToken);


        //        var jwtToken = (JwtSecurityToken)validatedToken;

        //        var claims = jwtToken.Claims.ToList();
        //        return true;
        //    }
        //    catch (System.Exception)
        //    {
        //        return false;
        //    }

        //}
    }

    public class JwtTokenBuilder
    {

        public static string BuildToken(string userName)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, userName)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Stc.JwtKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
            Stc.JwtIssuer,
            Stc.JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddYears(50),
            signingCredentials: creds
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
