﻿namespace Taxes.Data.Entities
{
    public class TaxScheduler
    {
        public int TaxSchedulerId { get; set; }
        public int MunicipalityId { get; set; }
        public int TaxTypeId { get; set; }
        public int Year { get; set; }
        public int? Month { get; set; }
        public int? Week { get; set; }
        public int? Day { get; set; }
        public decimal TaxValue { get; set; }
        public Municipality Municipality { get; set; }
        public TaxType TaxType { get; set; }
    }
}
