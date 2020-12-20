using System;
using System.Globalization;
using Taxes.Data.Ef;
using Taxes.Data.Entities;
using Taxes.Services.Dtos;
using Taxes.Services.Enums;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Taxes.Services.Services
{
    public class TaxSchedulerService
    {
        private readonly TaxesDbContext _dbContext;
        private static Calendar Calendar => CultureInfo.CurrentCulture.Calendar;

        public TaxSchedulerService(TaxesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddNewTax(TaxSchedulerDto taxScheduler)
        {
            var taxEntity = CreateTaxScheduler(taxScheduler);
            _dbContext.TaxSchedulers.Add(taxEntity);
            _dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<TaxSchedulerDto> GetSpecificTax(int municipalityId, TaxTypeEnum taxType, DateTime date)
        {
            var query = _dbContext.TaxSchedulers.Where(x =>
                x.TaxTypeId == (int)taxType && x.MunicipalityId == municipalityId);
            if (taxType == TaxTypeEnum.Yearly)
            {
                query = query.Where(x => x.Year == date.Year);
            }
            else if (taxType == TaxTypeEnum.Monthly)
            {
                query = query.Where(x => x.Year == date.Year && x.Month == date.Month);
            }
            else if (taxType == TaxTypeEnum.Weekly)
            {
                query = query.Where(x => x.Year == date.Year && x.Week == GetWeekOfYear(date));
            }
            else if (taxType == TaxTypeEnum.Daily)
            {
                query = query.Where(x => x.Date == date);
            }

            var tax = await query.FirstOrDefaultAsync();
            return tax == null
                ? null
                : new TaxSchedulerDto
                {
                    MunicipalityId = tax.MunicipalityId,
                    TaxType = (TaxTypeEnum)tax.TaxTypeId,
                    DateTime = date,
                    TaxValue = tax.TaxValue
                };
        }

        public async Task<TaxSchedulerDto> GetTaxByMunicipalityAndDate(string munucipality, DateTime date)
        {
            var result = await _dbContext.TaxSchedulers
                .Where(x => x.Municipality.Name == munucipality &&
                            (x.Date == date
                             || x.Month == date.Month && x.Year == date.Year
                             || x.Week == GetWeekOfYear(date) && x.Year == date.Year
                             || x.Year == date.Year && x.Date == null && x.Month == null && x.Week == null))
                .OrderBy(x => x.TaxType.Priority).FirstOrDefaultAsync();
            if (result == null)
                return null;
            return new TaxSchedulerDto
            {
                TaxType = (TaxTypeEnum)result.TaxTypeId,
                MunicipalityId = result.MunicipalityId,
                DateTime = date,
                TaxValue = result.TaxValue
            };
        }

        private TaxScheduler CreateTaxScheduler(TaxSchedulerDto taxScheduler)
        {
            var taxEntity = new TaxScheduler
            {
                MunicipalityId = taxScheduler.MunicipalityId,
                Year = taxScheduler.DateTime.Year,
                TaxTypeId = (int)taxScheduler.TaxType,
                TaxValue = taxScheduler.TaxValue
            };

            if (taxScheduler.TaxType == TaxTypeEnum.Monthly)
            {
                taxEntity.Month = taxScheduler.DateTime.Month;
            }
            else if (taxScheduler.TaxType == TaxTypeEnum.Weekly)
            {
                taxEntity.Week = GetWeekOfYear(taxScheduler.DateTime);
            }
            else if (taxScheduler.TaxType == TaxTypeEnum.Monthly)
            {
                taxEntity.Month = taxScheduler.DateTime.Month;
            }
            else if (taxScheduler.TaxType == TaxTypeEnum.Daily)
            {
                taxEntity.Date = taxScheduler.DateTime;
            }

            return taxEntity;
        }

        private int GetWeekOfYear(DateTime date)
        {
            return Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
