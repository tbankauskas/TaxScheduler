using System.Collections.Generic;

namespace Taxes.Data.Entities
{
    public class TaxType
    {
        public int TaxTypeId { get; set; }
        public string TypeName { get; set; }
        public int Priority { get; set; }
        public ICollection<TaxScheduler> TaxesSchedulers { get; set; }
    }
}
