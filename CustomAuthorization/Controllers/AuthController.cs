using JWTAuthentication.Data;
using JWTAuthentication.Dtos;
using JWTAuthentication.Helpers;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace JWTAuthentication.Controllers
{
    [Route(template:"api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repo, JwtService jwtService)
        {
            _repo = repo;
            _jwtService = jwtService;
        }


        [HttpPost(template:"register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User()
            {
                Username= dto.Username,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            return Created("success", _repo.Create(user));
        }
        //      First Action -- Where JWT token is created
        [HttpPost(template:"login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _repo.GetByUsername(dto.Username);

            if(user == null)
            {
                return BadRequest(new { message = "Invalid credentials" });
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid credentials" });
            }
            var jwt = _jwtService.Generate(user);
            //Response.Cookies.Append("jwt", jwt, new CookieOptions()
            //{
            //    HttpOnly = true
            //});
            return Ok(jwt);
        }

        // Second Action -- Passing this generated token as a cookie
        //[HttpGet(template: "user")]
        //public new IActionResult User()
        //{
        //    try
        //    {
        //        var jwt = Request.Cookies["jwt"];
        //        var token = _jwtService.Verify(jwt);
        //        int userId = int.Parse(token.Issuer);
        //        var user = _repo.GetById(userId);
        //        return Ok(user);
        //    }
        //    catch (Exception)
        //    {
        //        return Unauthorized();
        //    }
        //}


        // Check for token but not in cookie (second version)
        [HttpGet(template:"check")]
        public IActionResult OnlyForAuthenticatedUsersMethod()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                try
                {
                    var userClaims = identity.Claims;
                    var currentUser = new LoginDto()
                    {
                        Username = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value,
                    };
                    return Ok($"Hey {currentUser.Username}, you are authenticated, well done");
                }
                catch (Exception)
                {

                    return Unauthorized(new { message = "You are not authenticated, sorry!" });
                }
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "success"
            });
            
        }

    }
}
