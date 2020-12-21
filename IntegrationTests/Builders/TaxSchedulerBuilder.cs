using Taxes.Services.Enums;
using Entities = Taxes.Data.Entities;

namespace IntegrationTests.Builders
{
    public class TaxSchedulerBuilder : EntityBuilder<Entities.TaxScheduler>
    {
        private int _municipalityId;
        private TaxTypeEnum _taxType;
        private int _year;
        private int _month;
        private int _week;
        private int _day;
        private decimal _taxValue;

        public override Entities.TaxScheduler Create()
        {
            return new Entities.TaxScheduler()
            {
                MunicipalityId = _municipalityId,
                TaxTypeId = (int)_taxType,
                Year = _year,
                Month = _month,
                Week = _week,
                Day = _day,
                TaxValue = _taxValue
            };
        }

        public TaxSchedulerBuilder WithTaxValue(decimal taxValue)
        {
            _taxValue = taxValue;
            return this;
        }

        public TaxSchedulerBuilder WithMunicipalityId(int municipalityId)
        {
            _municipalityId = municipalityId;
            return this;
        }

        public TaxSchedulerBuilder WithTaxType(TaxTypeEnum taxType)
        {
            _taxType = taxType;
            return this;
        }

        public TaxSchedulerBuilder WithYear(int year)
        {
            _year = year;
            return this;
        }

        public TaxSchedulerBuilder WithMonth(int month)
        {
            _month = month;
            return this;
        }

        public TaxSchedulerBuilder WithWeek(int week)
        {
            _week = week;
            return this;
        }

        public TaxSchedulerBuilder WithDay(int day)
        {
            _day = day;
            return this;
        }
    }

}
