using System;
using System.ComponentModel.DataAnnotations;
using Taxes.Services.Enums;

namespace TaxScheduler.Models
{
    public class TaxSchedulerModel
    {
        [Required]
        public string Municipality { get; set; }
        [Required]
        [EnumDataType(typeof(TaxTypeEnum))]
        public TaxTypeEnum TaxType { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        [Required]
        public decimal TaxValue { get; set; }
    }
}
