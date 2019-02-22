﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.XivApi.SearchCharacter;
using MonkeyButler.XivApi.Services;
using Moq;
using Xunit;
using Xunit.Categories;

namespace MonkeyButler.XivApi.Tests.Integration.Commands
{
    public class SearchCharacter
    {
        [Fact]
        [IntegrationTest]
        public async Task ShouldGetResponseAndDeserialize()
        {
            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.SendAsync(It.IsAny<Uri>())).ReturnsAsync(new HttpResponseMessage()
            {
                Content = new HttpContentMock(@"Integration\SampleResponses\SearchCharacter.json"),
                StatusCode = HttpStatusCode.OK
            });

            var services = IntegrationHelper.GetServiceCollection()
                .AddSingleton(httpServiceMock.Object)
                .BuildServiceProvider();

            var response = await services.GetService<ISearchCharacter>().Process(new SearchCharacterCriteria());

            Assert.Equal("Jolinar Cast", response.Body.Results[0].Name);
            Assert.Equal("Diabolos", response.Body.Results[0].Server);
        }
    }
}