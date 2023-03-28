using CerealAPI.DTO;
using CerealAPI.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CerealAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        string sqlString = "Server=localhost;Database=master;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate([FromBody] UserDTO userdto)
        {
            //step 1 validate user
            var user = ValidateUserCredentials( userdto.Name, userdto.Password );

            if(user.Name != userdto.Name || user.Password != userdto.Password)
            {
                return Unauthorized();
            }

            //step 2 : create token
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials =  new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("name", user.Name.ToString()));
            claimsForToken.Add(new Claim("password", user.Password));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        
        private UserDTO ValidateUserCredentials(string name, string password)
        {
            UserDTO check;
            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    check = UserService.GetUser(conn, transaction, name, password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    conn.Close();
                    throw new BadHttpRequestException("Error: " + ex.Message, 400);
                }
                transaction.Commit();
                conn.Close();
            }
            //here: check the input against the database

            //return a userDTO of the logged in person

            return check;
        }

        public UserController(IConfiguration configuration) { 
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
