using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointController(IGenericRepository<Point> pointService) : ControllerBase
    {
        private readonly IGenericRepository<Point> _pointService = pointService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var points = await _pointService.GetAllAsync();
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
                var point = await _pointService.GetByIdAsync(id);
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
                var createdPoint = await _pointService.AddAsync(newPoint);

                return CreatedAtAction(nameof(GetById), new { id = createdPoint.Id }, Response<Point>.SuccessResponse(createdPoint, "Yeni nokta başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<Point>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Point point)
        {
            if (point == null)
            {
                return BadRequest(Response<Point>.ErrorResponse("Geçersiz nokta verisi."));
            }

            try
            {
                var existingPoint = await _pointService.GetByIdAsync(id);
                if (existingPoint == null)
                {
                    return NotFound(Response<Point>.ErrorResponse("Güncellenecek nokta bulunamadı."));
                }

                // Mevcut veriyi güncelle
                existingPoint.Name = point.Name;
                existingPoint.PointX = point.PointX;
                existingPoint.PointY = point.PointY;

                var updatedPoint = await _pointService.UpdateAsync(existingPoint);
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
                var point = await _pointService.GetByIdAsync(id);
                if (point == null)
                {
                    return NotFound(Response<Point>.ErrorResponse("Nokta bulunamadı"));
                }

                var result = await _pointService.DeleteAsync(point);
                if (!result)
                {
                    return NotFound(Response<Point>.ErrorResponse("Nokta silinirken bir hata oluştu."));
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