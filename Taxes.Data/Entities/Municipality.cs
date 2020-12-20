using System.Collections.Generic;

namespace Taxes.Data.Entities
{
    public class Municipality
    {
        public int MunicipalityId { get; set; }
        public string Name { get; set; }
        public ICollection<TaxScheduler> TaxesSchedulers { get; set; }
    }
}
