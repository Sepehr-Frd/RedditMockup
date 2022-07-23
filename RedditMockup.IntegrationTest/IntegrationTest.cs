using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;
using Xunit;

namespace RedditMockup.IntegrationTest;

internal class IntegrationTest
{
    #region [Field(s)]

    protected readonly HttpClient _client;
    protected readonly WebApplicationFactory<Program> _factory;

    #endregion

    #region [Constructor]

    internal IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _client = factory.CreateClient();

    }

    #endregion
}
