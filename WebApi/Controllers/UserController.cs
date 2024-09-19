using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services.Implementations;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                return Ok(Response<IEnumerable<User>>.SuccessResponse(users, "Users listed successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<IEnumerable<User>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(Response<User>.ErrorResponse("User is not found"));
                }

                return Ok(Response<User>.SuccessResponse(user, "User listed successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<User>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(Response<User>.ErrorResponse("User is not found"));
                }

                var result = await _unitOfWork.UserRepository.DeleteAsync(user);
                if (!result)
                {
                    return NotFound(Response<User>.ErrorResponse("An error occurred while deleting the user"));
                }

                var features = await _unitOfWork.FeatureRepository.GetByUserIdAsync(id);
                foreach (var item in features)
                {
                    await _unitOfWork.FeatureRepository.DeleteAsync(item);
                }

                return Ok(Response<User>.SuccessResponse(null, "User deleted successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.ErrorResponse(ex.Message));
            }
        }
    }
}