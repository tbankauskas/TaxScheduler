using System;
using Taxes.Services.Enums;

namespace Taxes.Services.Dtos
{
    public class TaxSchedulerDto
    {
        public int TaxSchedulerId { get; set; }
        public int MunicipalityId { get; set; }
        public TaxTypeEnum TaxType { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TaxValue { get; set; }
    }
}
