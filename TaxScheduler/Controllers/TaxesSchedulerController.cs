using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Taxes.Services.Dtos;
using Taxes.Services.Services;
using TaxScheduler.Models;

namespace TaxScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxesSchedulerController : ControllerBase
    {
        private readonly ILogger<TaxesSchedulerController> _logger;
        private readonly TaxSchedulerService _taxSchedulerService;
        private readonly MunicipalitiesService _municipalitiesService;

        public TaxesSchedulerController(ILogger<TaxesSchedulerController> logger,
            TaxSchedulerService taxSchedulerService,
            MunicipalitiesService municipalitiesService
            )
        {
            _logger = logger;
            _taxSchedulerService = taxSchedulerService;
            _municipalitiesService = municipalitiesService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(TaxSchedulerModel model)
        {
            if (model.DateTime == DateTime.MinValue)
            {
                return BadRequest("Date value is required");
            }

            var municipalityId = _municipalitiesService.GetMunicipalityId(model.Municipality);

            if (!municipalityId.HasValue)
            {
                return BadRequest("Provided municipality doesn't exist!");
            }

            var existingTax = await _taxSchedulerService.GetSpecificTax((int)municipalityId, model.TaxType, model.DateTime);
            if (existingTax != null)
            {
                return BadRequest(
                    "Can't create tax because it is already exists for provided municipality and tax type");
            }

            await _taxSchedulerService.AddNewTax(new TaxSchedulerDto
            {
                MunicipalityId = (int)municipalityId,
                TaxType = model.TaxType,
                DateTime = model.DateTime,
                TaxValue = model.TaxValue
            });

            return Ok();
        }

        [HttpGet]
        [Route("{municipality}/{date}")]
        public async Task<TaxSchedulerDto> Get(string municipality, DateTime date)
        {
            var result = await _taxSchedulerService.GetTaxByMunicipalityAndDate(municipality, date);
            return result;
        }
    }
}
