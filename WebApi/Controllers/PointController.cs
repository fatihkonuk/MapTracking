using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapTracking.Models;
using Microsoft.AspNetCore.Mvc;

namespace MapTracking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointController : ControllerBase
    {
        private static readonly List<Point> _points = [];
        private static int _count = 1;

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_points);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            Point? _point = _points.FirstOrDefault(e => e.Id == id);
            if (_point == null)
                return NotFound();

            return Ok(_point);
        }

        [HttpPost("Add")]
        public IActionResult Add(Point point)
        {
            point.Id = _count++;
            _points.Add(point);
            return CreatedAtAction(nameof(GetById), new { id = point.Id }, point);
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateById(int id, Point point)
        {
            Point? existingPoint = _points.FirstOrDefault(e => e.Id == id);
            if (existingPoint == null)
                return NotFound();

            int index = _points.IndexOf(existingPoint);
            _points[index] = point;
            _points[index].Id = id;

            return Ok(_points[index]);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            Point? point = _points.FirstOrDefault(e => e.Id == id);
            if (point == null)
                return NotFound();

            _points.Remove(point);
            return Ok();
        }
    }
}
