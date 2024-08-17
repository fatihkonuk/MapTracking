using System.Collections.Generic;
using System.Threading.Tasks;
using MapTracking.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace MapTracking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointController(DbService dbService) : ControllerBase
    {
        private readonly DbService _dbService = dbService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var points = await _dbService.GetAll();
                return Ok(Response<List<Point>>.SuccessResponse(points));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<List<Point>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var point = await _dbService.GetById(id);
                if (point == null)
                {
                    return NotFound(Response<Point>.ErrorResponse("Nokta bulunamadı"));
                }

                return Ok(Response<Point>.SuccessResponse(point));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Point>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Point point)
        {
            if (point == null)
            {
                return BadRequest(Response<Point>.ErrorResponse("Geçersiz nokta verisi"));
            }

            try
            {
                var createdPoint = await _dbService.Add(point);
                return CreatedAtAction(nameof(GetById), new { id = createdPoint.Id }, Response<Point>.SuccessResponse(createdPoint, "Yeni nokta başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Point>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateById(int id, Point point)
        {
            if (point == null)
            {
                return BadRequest(Response<Point>.ErrorResponse("Geçersiz nokta verisi."));
            }

            try
            {
                var updatedPoint = await _dbService.UpdateById(id, point);
                if (updatedPoint == null)
                {
                    return NotFound(Response<Point>.ErrorResponse("Nokta bulunamadı."));
                }

                return Ok(Response<Point>.SuccessResponse(updatedPoint, "Nokta başarıyla güncellendi."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Point>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var result = await _dbService.DeleteById(id);
                if (!result)
                {
                    return NotFound(Response<Point>.ErrorResponse("Nokta bulunamadı."));
                }

                return Ok(Response<string>.SuccessResponse(null, "Nokta başarıyla silindi."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
