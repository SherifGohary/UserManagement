using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.API.DataAccess.Entities;
using UserManagement.API.Models;
using AutoMapper;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly TokenManager _tokenManager;
        private readonly IMapper _mapper;
        public AccountController(SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            IConfiguration config, 
            TokenManager tokenManager,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _tokenManager = tokenManager;
            _mapper = mapper;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        // Create the Token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          _config["Tokens:Issuer"],
                          _config["Tokens:Audience"],
                          claims,
                          expires: DateTime.UtcNow.AddMinutes(180),
                          signingCredentials: creds);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            isAuthenticated = true,
                            userDetails = _mapper.Map<UserViewModel>(user)
                        };

                        return Created("", results);
                    }
                }
            }

            return BadRequest();
        }

        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User userEmailExist = await _userManager.FindByEmailAsync(model.Email);
                User usernamelExist = await _userManager.FindByNameAsync(model.UserName);

                if (userEmailExist == null && usernamelExist == null)
                {
                    var newUser = _mapper.Map<User>(model);
                    var result = await _userManager.CreateAsync(newUser, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        return BadRequest("Couldn't be saved at the moment");
                    }
                    return Created("", result);
                }
                return BadRequest("Username or Email already exists!");
            }
            return BadRequest(ModelState);
        }

        [HttpGet, Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            //await _tokenManager.DeactivateCurrentAsync();
            return Ok();
        }
    }
}
