using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IntegrationTests.Builders;
using IntegrationTests.Infrastructure;
using IntegrationTests.Infrastructure.Utilities;
using Taxes.Services.Enums;
using TaxScheduler.Models;
using Xunit;

namespace IntegrationTests.Tests
{
    public class TaxesSchedulerControllerTests : BaseHttpApiTest
    {
        private const string ApiUrl = "api/TaxesScheduler/";
        private readonly TaxSchedulerModel _model;

        public TaxesSchedulerControllerTests()
        {
            _model = new TaxSchedulerModel
            {
                Municipality = "Copenhagen",
                TaxType = TaxTypeEnum.Yearly,
                TaxValue = 0.5m

            };
        }

        [Fact]
        public async Task Post_should_return_badrequest_when_date_is_not_provided()
        {
            // Arrange

            // Act
            var response = await HttpClient.PostJson(ApiUrl, _model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Date value is required", await response.Content.ReadAsStringAsync());

        }

        [Fact]
        public async Task Post_should_return_badrequest_when_provided_municipality_not_exists()
        {
            // Arrange
            _model.DateTime = DateTime.Now;

            // Act
            var response = await HttpClient.PostJson(ApiUrl, _model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Provided municipality doesn't exist!", await response.Content.ReadAsStringAsync());

        }

        [Fact]
        public async Task Post_should_return_badrequest_when_tax_already_exists()
        {
            // Arrange
            var municipality = await new MunicipalityBuilder().SaveToDb();

            await new TaxSchedulerBuilder()
                .WithMunicipalityId(municipality.MunicipalityId)
                .WithTaxType(TaxTypeEnum.Yearly)
                .WithYear(2020)
                .WithTaxValue(0.2m)
                .SaveToDb();
            _model.DateTime = DateTime.Now;

            // Act
            var response = await HttpClient.PostJson(ApiUrl, _model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Can't create tax because it is already exists for provided municipality and tax type",
                await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Post_should_save_new_tax()
        {
            // Arrange
            await new MunicipalityBuilder().SaveToDb();
            _model.DateTime = DateTime.Now;
            _model.TaxType = TaxTypeEnum.Daily;

            // Act
            await HttpClient.PostJson(ApiUrl, _model);

            // Assert
            var tax = DatabaseUtility.CreateDbContext().TaxSchedulers.FirstOrDefault();
            Assert.NotNull(tax);
            Assert.True(tax.TaxTypeId == (int)TaxTypeEnum.Daily);
            Assert.Equal(DateTime.Now.Day, tax.Day);
        }

        [Fact]
        public async Task Post_should_save_update_existing_tax()
        {
            // Arrange
            var municipality = await new MunicipalityBuilder().SaveToDb();
            await new TaxSchedulerBuilder()
                .WithMunicipalityId(municipality.MunicipalityId)
                .WithTaxType(TaxTypeEnum.Yearly)
                .WithYear(2020)
                .WithTaxValue(0.2m)
                .SaveToDb();
            _model.DateTime = DateTime.Now;
            _model.TaxValue = 10;

            // Act
            await HttpClient.PutJson(ApiUrl, _model);

            // Assert
            var result = DatabaseUtility.CreateDbContext().TaxSchedulers.FirstOrDefault();
            Assert.Equal(_model.TaxValue, result?.TaxValue);
        }

        [Fact]
        public async Task Post_should_return_badrequest_when_updating_none_existing_tax()
        {
            // Arrange
            await new MunicipalityBuilder().SaveToDb();
            _model.DateTime = DateTime.Now;

            // Act
            var response = await HttpClient.PutJson(ApiUrl, _model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Can't update tax because it doesn't exists for provided municipality and tax type",
                await response.Content.ReadAsStringAsync());
        }
    }
}
