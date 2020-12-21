using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IntegrationTests.Infrastructure;
using IntegrationTests.Infrastructure.Utilities;
using Xunit;

namespace IntegrationTests.Tests
{
    public class MunicipalityControllerTests : BaseHttpApiTest
    {
        private const string ApiUrl = "api/municipalities-import/";
        private string fileContent = "Copenhagen\r\nVilnius\r\nLondon\r\nParis";

        [Fact]
        public async Task Post_should_import_municipalities_from_file()
        {
            // Arrange
            var formData = new MultipartFormDataContent
            {
                { new ByteArrayContent(Encoding.ASCII.GetBytes(fileContent)), "file", "filename"}
            };

            // Act
            await HttpClient.PostAsync(ApiUrl, formData);

            // Assert
            var dbContext = DatabaseUtility.CreateDbContext();
            var result = dbContext.Municipalities.ToList();
            Assert.Equal(4, result.Count);

        }
    }
}
