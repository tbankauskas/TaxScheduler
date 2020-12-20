using System.Collections.Generic;
using Taxes.Data.Ef;
using Taxes.Data.Entities;
using System.Linq;

namespace Taxes.Services.Services
{
    public class MunicipalitiesService
    {
        private readonly TaxesDbContext _dbContext;

        public MunicipalitiesService(TaxesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int? GetMunicipalityId(string name)
        {
            return _dbContext.Municipalities.FirstOrDefault(x => x.Name == name)?.MunicipalityId;
        }

        public void Import(IEnumerable<string> municipalities)
        {
            foreach (var municipality in municipalities.Distinct())
            {
                var entity = _dbContext.Municipalities.FirstOrDefault(x => x.Name == municipality);
                if (entity == null)
                {
                    _dbContext.Municipalities.Add(new Municipality { Name = municipality });
                }
                else
                {
                    entity.Name = municipality;
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
