namespace DepMap.Tests.Mock.Services;

public class MockServiceUserParams : IMockServiceUser
{
    public IMockService MockService { get; set; }
    public MockService1 MockService1 { get; set; }
}