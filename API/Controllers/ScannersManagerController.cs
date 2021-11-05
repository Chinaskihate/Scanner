using API.Models;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ScannersManagerController : Controller
    {
        private static ScannersManager _scannersManager = new ScannersManager();

        [HttpGet("get-task-status")]
        public IActionResult GetTaskStatus([FromQuery] int id)
        {
            try
            {
                return Ok(_scannersManager.GetStatus(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("create-scan")]
        public IActionResult CreateScan([FromQuery] string path)
        {
            int id = _scannersManager.CreateScan(path);
            if (id != -1)
            {
                return Ok(id);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
