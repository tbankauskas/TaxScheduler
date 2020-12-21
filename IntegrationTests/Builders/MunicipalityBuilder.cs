using Taxes.Data.Entities;

namespace IntegrationTests.Builders
{
    public class MunicipalityBuilder : EntityBuilder<Municipality>
    {
        private string _name = "Copenhagen";
        public override Municipality Create()
        {
            return new Municipality { Name = _name };
        }
    }
}
