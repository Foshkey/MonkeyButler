using System;
using MonkeyButler.XivApi.Infrastructure;
using Moq;
using Xunit;
using SUT = MonkeyButler.XivApi.Infrastructure.ExecutionService;

namespace MonkeyButler.XivApi.Tests.Infrastructure
{
    public class CommandServiceTests
    {
        private readonly Mock<IHttpService> _httpServiceMock = new Mock<IHttpService>();
        private readonly Mock<IDeserializer> _deserializerMock = new Mock<IDeserializer>();

        private SUT BuildTarget() => new SUT(_httpServiceMock.Object, _deserializerMock.Object);

        [Fact]
        public void ValidationShouldThrowExceptionIfCriteriaIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => BuildTarget().ValidateCriteriaBase(null));
            Assert.Contains("criteria", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ValidationShouldThrowExceptionIfCriteriaKeyIsNullOrEmpty(string key)
        {
            var ex = Assert.Throws<ArgumentException>(() => BuildTarget().ValidateCriteriaBase(new CriteriaBase()
            {
                Key = key
            }));

            Assert.Contains("Key cannot be null or empty.", ex.Message);
        }
    }
}
