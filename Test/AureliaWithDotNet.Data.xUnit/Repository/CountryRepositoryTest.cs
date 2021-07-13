using AureliaWithDotNet.Data.Repository;
using System.Threading.Tasks;
using Xunit;

namespace AureliaWithDotNet.Data.xUnit.Repository
{
    public class CountryRepositoryTest
    {
        [Fact]
        public void IsValidTest()
        {
            //Arrange
            CountryRepository countryRepository = new();

            //Act
            var isValid1 = countryRepository.IsValid("Brasil");
            var isValid2 = countryRepository.IsValid("Brasil123");

            //Assert
            Assert.True(isValid1);
            Assert.False(isValid2);
        }
    }
}
