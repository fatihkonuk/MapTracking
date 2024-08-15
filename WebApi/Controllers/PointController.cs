using System.Collections.Generic;
using WebApi.Models;
using MapTracking.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapTracking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointController : ControllerBase
    {
        private readonly IPointService _pointService;

        public PointController(IPointService pointService)
        {
            _pointService = pointService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var points = _pointService.GetAll();
            return Ok(Response<List<Point>>.SuccessResponse(points));
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var point = _pointService.GetById(id);
            if (point == null)
            {
                return NotFound(Response<Point>.ErrorResponse("Point not found."));
            }

            return Ok(Response<Point>.SuccessResponse(point));
        }

        [HttpPost("Add")]
        public IActionResult Add(Point point)
        {
            if (point == null)
            {
                return BadRequest(Response<Point>.ErrorResponse("Invalid point data."));
            }

            var createdPoint = _pointService.Add(point);
            return CreatedAtAction(nameof(GetById), new { id = createdPoint.Id }, Response<Point>.SuccessResponse(createdPoint, "Point created successfully."));
        }

        [HttpPut("UpdateById/{id}")]
        public IActionResult UpdateById(int id, Point point)
        {
            if (point == null)
            {
                return BadRequest(Response<Point>.ErrorResponse("Invalid point data."));
            }

            var updatedPoint = _pointService.UpdateById(id, point);
            if (updatedPoint == null)
            {
                return NotFound(Response<Point>.ErrorResponse("Point not found."));
            }

            return Ok(Response<Point>.SuccessResponse(updatedPoint, "Point updated successfully."));
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult DeleteById(int id)
        {
            var result = _pointService.DeleteById(id);
            if (!result)
            {
                return NotFound(Response<Point>.ErrorResponse("Point not found."));
            }

            return Ok(Response<string>.SuccessResponse(null, "Point deleted successfully."));
        }
    }
}
