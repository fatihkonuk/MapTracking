using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var features = await _unitOfWork.FeatureRepository.GetAllAsync();
                features = features.Where(x => x.UserId == Convert.ToInt32(userId));
                return Ok(Response<IEnumerable<Feature>>.SuccessResponse(features, "Features listed successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<IEnumerable<Feature>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var features = await _unitOfWork.FeatureRepository.GetAllAsync();
                features = features.Where(x => x.UserId == id);
                return Ok(Response<IEnumerable<Feature>>.SuccessResponse(features, "Features listed successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<IEnumerable<Feature>>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var feature = await _unitOfWork.FeatureRepository.GetByIdAsync(id);
                if (feature == null)
                {
                    return NotFound(Response<Feature>.ErrorResponse("Feature is not found"));
                }

                return Ok(Response<Feature>.SuccessResponse(feature, "Feature listed successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Feature>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Feature feature)
        {
            if (feature == null)
            {
                return BadRequest(Response<Feature>.ErrorResponse("Invalid feature data"));
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = User.FindFirst(ClaimTypes.Name)?.Value;

                var newFeature = new Feature()
                {
                    Name = feature.Name,
                    WKT = feature.WKT,
                    UserId = Convert.ToInt32(userId),
                };
                var createdFeature = await _unitOfWork.FeatureRepository.AddAsync(newFeature);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetById), new { id = createdFeature.Id }, Response<Feature>.SuccessResponse(createdFeature, "Feature created successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Feature>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Feature feature)
        {
            if (feature == null)
            {
                return BadRequest(Response<Feature>.ErrorResponse("Invalid feature data"));
            }

            try
            {
                var existingFeature = await _unitOfWork.FeatureRepository.GetByIdAsync(id);
                if (existingFeature == null)
                {
                    return NotFound(Response<Feature>.ErrorResponse("Feature is not found"));
                }

                // Mevcut veriyi güncelle
                existingFeature.Name = feature.Name;
                existingFeature.WKT = feature.WKT;

                var updatedFeature = await _unitOfWork.FeatureRepository.UpdateAsync(existingFeature);
                await _unitOfWork.CompleteAsync();
                return Ok(Response<Feature>.SuccessResponse(updatedFeature, "Feature updated successfuly."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Feature>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var feature = await _unitOfWork.FeatureRepository.GetByIdAsync(id);
                if (feature == null)
                {
                    return NotFound(Response<Feature>.ErrorResponse("Feature is not found"));
                }

                var result = await _unitOfWork.FeatureRepository.DeleteAsync(feature);
                await _unitOfWork.CompleteAsync();
                if (!result)
                {
                    return NotFound(Response<Feature>.ErrorResponse("An error occurred while deleting the feature"));
                }

                return Ok(Response<string>.SuccessResponse(null, "Feature deleted successfuly"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.ErrorResponse(ex.Message));
            }
        }
    }
}