using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taxes.Services.Dtos;
using Taxes.Services.Services;
using TaxScheduler.Models;

namespace TaxScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxesSchedulerController : ControllerBase
    {
        private readonly TaxSchedulerService _taxSchedulerService;
        private readonly MunicipalitiesService _municipalitiesService;

        public TaxesSchedulerController(TaxSchedulerService taxSchedulerService, MunicipalitiesService municipalitiesService)
        {
            _taxSchedulerService = taxSchedulerService;
            _municipalitiesService = municipalitiesService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(TaxSchedulerModel model)
        {
            return await SaveTax(model, true);
        }

        [HttpPut]
        public async Task<ActionResult> Put(TaxSchedulerModel model)
        {
            return await SaveTax(model, false);
        }

        [HttpGet]
        [Route("{municipality}/{date}")]
        public async Task<TaxSchedulerDto> Get(string municipality, DateTime date)
        {
            var result = await _taxSchedulerService.GetTaxByMunicipalityAndDate(municipality, date);
            return result;
        }

        private async Task<ActionResult> SaveTax(TaxSchedulerModel model, bool isNew)
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
            if (isNew)
            {
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
            }
            else
            {
                if (existingTax == null)
                {
                    return BadRequest(
                        "Can't update tax because it doesn't exists for provided municipality and tax type");
                }

                existingTax.TaxValue = model.TaxValue;
                await _taxSchedulerService.UpdateExistingTax(existingTax);
            }

            return Ok();
        }
    }
}
