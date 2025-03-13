using System.Net;

namespace DepMap.Tests.Controllers;

public class DepMapControllerTest : IClassFixture<MockApplicationFactory>
{
    private HttpClient _client;
    
    public DepMapControllerTest(MockApplicationFactory factory)
    {
        _client = factory.Client;
    }

    [Fact]
    public async Task Route_depmapui_ShoudRespond_HtmlPage()
    {
        var result = await _client.GetAsync("depmapui");
        Assert.True(result.Content.Headers.ContentType?.MediaType == "text/html");
        Assert.True(result.StatusCode == HttpStatusCode.OK);
    }
}