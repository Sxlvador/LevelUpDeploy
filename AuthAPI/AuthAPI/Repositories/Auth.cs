using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Repositories
{
    public class Auth : IAuth
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public Auth(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _appDbContext = appDbContext;
        }

        public async Task<ResponseVM> LoginUserAsync(LoginVM loginVM)
        {
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user == null)
            {
                return new ResponseVM
                {
                    Message = "No user found with this email.",
                    isSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, loginVM.Password);
            if (!result)
            {
                return new ResponseVM
                {
                    Message = "Invalid password.",
                    isSuccess = false
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new ResponseVM
            {
                Message = tokenString,
                isSuccess = true
            };
        }

        public async Task<ResponseVM> RegisterUserAsync(RegisterVM registerVM)
        {
            var newUser = new User
            {
                UserName = registerVM.Email,
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Address = registerVM.Address,
                PhoneNumber = registerVM.PhoneNumber,
                Age = registerVM.Age,
                UserCreated = DateTime.UtcNow
            };

            var existingEmail = await _appDbContext.Users.AnyAsync(u => u.Email == newUser.Email);
            if (existingEmail)
            {
                return new ResponseVM
                {
                    Message = "A user with this email already exists.",
                    isSuccess = false
                };
            }

            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                return new ResponseVM
                {
                    Message = "User registration failed.",
                    isSuccess = false
                };
            }

            return new ResponseVM
            {
                Message = "User registered successfully.",
                isSuccess = true
            };
        }
    }
}
