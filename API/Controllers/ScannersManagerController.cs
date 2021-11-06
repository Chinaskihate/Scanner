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

        /// <summary>
        /// Get status of scan.
        /// </summary>
        /// <param name="id"> Id of scan. </param>
        /// <returns> Action result. </returns>
        [HttpGet("get-scan-status")]
        public IActionResult GetScanStatus([FromQuery] int id)
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

        /// <summary>
        /// Create new scan.
        /// </summary>
        /// <param name="path"> Path to directory. </param>
        /// <returns> Action result. </returns>
        [HttpGet("create-scan")]
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
