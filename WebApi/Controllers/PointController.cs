using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointController(PointService pointService) : ControllerBase
    {
        private readonly PointService _pointService = pointService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var points = await _pointService.GetAllPointsAsync();
                return Ok(Response<IEnumerable<Point>>.SuccessResponse(points));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<IEnumerable<Point>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var point = await _pointService.GetPointByIdAsync(id);
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
                var newPoint = new Point()
                {
                    Name = point.Name,
                    PointX = point.PointX,
                    PointY = point.PointY
                };
                var createdPoint = await _pointService.CreatePointAsync(newPoint);
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
                var existingPoint = await _pointService.GetPointByIdAsync(id);

                if (existingPoint == null)
                {
                    return NotFound(Response<Point>.ErrorResponse("Nokta bulunamadı."));
                }

                existingPoint.Name = point.Name;
                existingPoint.PointX = point.PointX;
                existingPoint.PointY = point.PointY;

                var updatedPoint = await _pointService.UpdatePointAsync(existingPoint);
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
                var result = await _pointService.DeletePointAsync(id);
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
