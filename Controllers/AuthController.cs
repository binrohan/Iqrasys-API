using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Dtos;
using iqrasys.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace iqrasys.api.Controllers
{

    public class AuthController : ApiController
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration config,
        IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {

            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login(UserForLoginDto userForLogin)
        {
            var user = await _userManager.FindByNameAsync(userForLogin.Username);
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLogin.Password, false);

            if (result.Succeeded)
            {
                var appUser = _mapper.Map<UserForReturnDto>(user);

                Response.Headers.Add("X-Authorization-Token", GenerateJwtToken(user).Result);
                Response.Headers.Add("X-Refresher-Token", GenerateRefresherJwtToken(user));

                return Ok(appUser);
            }
            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim> {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefresherJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = creds,
                Audience = _config.GetSection("AppSettings:Audience").Value,
                Issuer = _config.GetSection("AppSettings:Issuer").Value
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (!result.Succeeded)
                throw new Exception("User unable to create");
            
            var newUser = await _userManager.GetUserNameAsync(userToCreate);

            var userToReturn = _mapper.Map<UserForReturnDto>(newUser);
            
            return Ok(userToReturn);
            
        }

        [HttpGet("validity")]
        public Boolean TokenValidity()
        {
            return true;
        }

        [HttpPatch]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Headers["Proxy-Authorization"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Session expired!");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = _config["AppSettings:Issuer"];
            var audience = _config["AppSettings:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidIssuer = issuer;
            validationParameters.ValidAudience = audience;
            validationParameters.IssuerSigningKey = key;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateAudience = true;
            validationParameters.ValidateIssuer = true;

            if (!tokenHandler.CanReadToken(refreshToken))
                return Unauthorized("WHY");


            ClaimsPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(refreshToken,
                                                    validationParameters,
                                                    out validatedToken);
            }
            catch (Exception e)
            {
                return Unauthorized(e);
            }

            var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(claim.Value);
            var appUser = _mapper.Map<UserForReturnDto>(user);

            Response.Headers.Add("X-Authorization-Token", GenerateJwtToken(user).Result);

            return Ok(appUser);

        }
    }
}