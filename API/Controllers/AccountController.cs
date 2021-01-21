using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost ("register")]
        public async Task<ActionResult<AppUser>>Register(RegisterDTO registerDTO)
        {
if  (await UserExists(registerDTO.Username)) return BadRequest ("UserName is taken");

using var hmac = new HMACSHA512();
 var user = new AppUser
 {

UserName= registerDTO.Username.ToLower(),
PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
PasswordSalt=hmac.Key
 };

 _context.Users.Add(user);
 await  _context.SaveChangesAsync();

return user;    
        }

        [HttpPost ("login")]
        public async Task<ActionResult<AppUser>>Login(LoginDTO loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync
            (x => x.UserName == loginDto.Username);

            if(user == null) return Unauthorized("Invalid Username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
             
             var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

             for(int i = 0; i < computeHash.Length; i++ )
             {
                    if(computeHash[i] != user.PasswordHash[i]) 
                    return Unauthorized("invalid password");

             }

             return user;

        }
        private async Task<bool> UserExists(string username)
        {
return await _context.Users.AnyAsync(x=> x.UserName ==username.ToLower());

        }
    }
}