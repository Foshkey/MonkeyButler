using System;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.XivApi.Infrastructure;
using Moq;

namespace MonkeyButler.Mocks
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddHttpServiceMock(this IServiceCollection services) => services
            .AddSingleton(new Mock<IHttpService>().Object);

        public static IServiceCollection AddHttpServiceMock(this IServiceCollection services, Action<HttpServiceMockOptions> optionsDelegate) => services
            .AddSingleton(new Mock<IHttpService>().SetupResponse(optionsDelegate).Object);
    }
}
