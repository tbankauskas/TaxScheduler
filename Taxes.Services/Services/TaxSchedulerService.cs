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

        public async Task AddNewTax(TaxSchedulerDto taxScheduler)
        {
            var taxEntity = CreateOrUpdateTaxScheduler(taxScheduler);
            _dbContext.TaxSchedulers.Add(taxEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateExistingTax(TaxSchedulerDto taxScheduler)
        {
            var taxEntity = _dbContext.TaxSchedulers.First(x => x.TaxSchedulerId == taxScheduler.TaxSchedulerId);
            CreateOrUpdateTaxScheduler(taxScheduler, taxEntity);
            await _dbContext.SaveChangesAsync();
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
                query = query.Where(x => x.Day == date.Day && x.Month == date.Month && x.Year == date.Year);
            }

            var tax = await query.FirstOrDefaultAsync();
            return tax == null
                ? null
                : new TaxSchedulerDto
                {
                    TaxSchedulerId = tax.TaxSchedulerId,
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
                            (x.Day == date.Day && x.Month == date.Month && x.Year == date.Year
                             || x.Month == date.Month && x.Year == date.Year
                             || x.Week == GetWeekOfYear(date) && x.Year == date.Year
                             || x.Year == date.Year && x.Day == null && x.Month == null && x.Week == null))
                .OrderBy(x => x.TaxType.Priority).FirstOrDefaultAsync();
            if (result == null)
                return null;
            return new TaxSchedulerDto
            {
                TaxSchedulerId = result.TaxSchedulerId,
                TaxType = (TaxTypeEnum)result.TaxTypeId,
                MunicipalityId = result.MunicipalityId,
                DateTime = date,
                TaxValue = result.TaxValue
            };
        }

        private TaxScheduler CreateOrUpdateTaxScheduler(TaxSchedulerDto taxScheduler, TaxScheduler taxEntity = null)
        {
            if (taxEntity == null)
            {
                taxEntity = new TaxScheduler
                {
                    MunicipalityId = taxScheduler.MunicipalityId,
                    Year = taxScheduler.DateTime.Year,
                    TaxTypeId = (int)taxScheduler.TaxType,
                    TaxValue = taxScheduler.TaxValue
                };
            }

            taxEntity.TaxValue = taxScheduler.TaxValue;

            if (taxScheduler.TaxType == TaxTypeEnum.Monthly)
            {
                taxEntity.Month = taxScheduler.DateTime.Month;
            }
            else if (taxScheduler.TaxType == TaxTypeEnum.Weekly)
            {
                taxEntity.Week = GetWeekOfYear(taxScheduler.DateTime);
            }
            else if (taxScheduler.TaxType == TaxTypeEnum.Daily)
            {
                taxEntity.Month = taxScheduler.DateTime.Month;
                taxEntity.Day = taxScheduler.DateTime.Day;
            }

            return taxEntity;
        }

        private int GetWeekOfYear(DateTime date)
        {
            return Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
