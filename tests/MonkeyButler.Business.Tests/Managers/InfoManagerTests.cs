using System;
using System.Net;
using System.Threading.Tasks;
using MonkeyButler.Abstractions.Business.Models.Info;
using MonkeyButler.Abstractions.Data.Api;
using MonkeyButler.Abstractions.Data.Api.Models.PublicIp;
using MonkeyButler.Business.Managers;
using Moq;
using Xunit;

namespace MonkeyButler.Business.Tests.Managers
{
    public class InfoManagerTests
    {
        private readonly IPAddress _ipAddress = IPAddress.Parse("192.168.1.128");

        private readonly Mock<IPublicIpAccessor> _publicIpMock = new();

        public InfoManagerTests()
        {
            _publicIpMock.Setup(x => x.GetIp())
                .ReturnsAsync(new IpData()
                {
                    Ip = _ipAddress
                });
        }

        private InfoManager _target => Resolver
            .Add(_publicIpMock.Object)
            .Resolve<InfoManager>();

        [Fact]
        public async Task ShouldGetIpAddress()
        {
            var criteria = new InfoCriteria()
            {
                InfoRequest = InfoRequest.IpAddress
            };

            var result = await _target.GetInfo(criteria);

            Assert.Equal(_ipAddress, result.IpAddress);
        }

        [Fact]
        public async Task ShouldNotGetIpAddress()
        {
            var criteria = new InfoCriteria()
            {
                InfoRequest = InfoRequest.None
            };

            var result = await _target.GetInfo(criteria);

            Assert.Null(result.IpAddress);
        }

        [Fact]
        public async Task ShouldHandleException()
        {
            var criteria = new InfoCriteria()
            {
                InfoRequest = InfoRequest.IpAddress
            };
            _publicIpMock.Setup(x => x.GetIp()).Throws(new Exception());

            var result = await _target.GetInfo(criteria);

            Assert.NotNull(result);
        }
    }
}
