using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services.Helpers;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUnitOfWork unitOfWork, IJwtService jwtService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtService _jwtservice = jwtService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByUsernameAsync(loginDto.Username);
                if (user == null) return NotFound(Response<User>.ErrorResponse("User is not found"));

                if (PasswordHasher.VerifyPassword(loginDto.Password, user.Password, user.Salt))
                {
                    string token = _jwtservice.GenerateToken(user);
                    bool success = true;
                    string message = "success";
                    var data = new { token, user.Id, user.Fullname, user.Role };
                    return Ok(new { message, data, success });
                }

                return Unauthorized(Response<User>.ErrorResponse("Username or password is incorrect"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Feature>.ErrorResponse(ex.Message));
            }


        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var (hashedPassword, salt) = PasswordHasher.HashPassword(registerDto.Password);

                var newUser = new User
                {
                    Fullname = registerDto.FullName,
                    Username = registerDto.Username,
                    Password = hashedPassword,
                    Salt = salt,
                    Role = registerDto.Role ?? "User"
                };

                await _unitOfWork.UserRepository.AddAsync(newUser);
                await _unitOfWork.CompleteAsync();
                return Ok(Response<User>.SuccessResponse(null, "Registered successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<User>.ErrorResponse(ex.Message));
            }

        }
    }
}