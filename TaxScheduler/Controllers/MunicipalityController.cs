using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taxes.Services.Services;

namespace TaxScheduler.Controllers
{
    [Route("api/municipalities-import")]
    [ApiController]
    public class MunicipalityController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromForm] IFormFile file, [FromServices] MunicipalitiesService municipalitiesImportService)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var municipalities = reader.ReadToEnd().Split(Environment.NewLine);
            municipalitiesImportService.Import(municipalities);
            return Ok();
        }
    }
}