using DepMap.Test;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DepMap.Tests;

public class MockApplicationFactory : WebApplicationFactory<MockApplication>
{
    public MockApplicationFactory()
    {
        CreateClient();
    }
}