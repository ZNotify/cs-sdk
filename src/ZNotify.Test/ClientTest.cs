using System.Threading.Tasks;
using ZNotify.Entity;

namespace ZNotify.Test;

public class ClientTests
{
    private const string TestEndpoint = "http://localhost:14444";

    [Test]
    public async Task TestClientCreateFailed()
    {
        var result = await Client.Create("error", TestEndpoint);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors[0], Is.Not.Null);
        });
    }

    [Test]
    public async Task TestClientCreateSuccess()
    {
        var result = await Client.Create("test", TestEndpoint);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task TestSendFailed()
    {
        var result = await Client.Create("test", TestEndpoint);
        var client = result.Value;
        var sendResult = await client.Send(new MessageOption()
        {
            Title = "Test",
            Content = "",
            Priority = PriorityType.HIGH,
            LongContent = "LongTest"
        });
        Assert.Multiple(() =>
        {
            Assert.That(sendResult.IsSuccess, Is.False);
            Assert.That(sendResult.Errors[0], Is.Not.Null);
        });
    }

    [Test]
    public async Task TestSendSuccess()
    {
        var result = await Client.Create("test", TestEndpoint);
        var client = result.Value;
        var sendResult = await client.Send(new MessageOption()
        {
            Title = "Test",
            Content = "",
            Priority = PriorityType.HIGH,
            LongContent = "LongTest"
        });
        Assert.Multiple(() =>
        {
            Assert.That(sendResult.IsSuccess, Is.False);
            Assert.That(sendResult.Errors[0], Is.Not.Null);
        });
    }

    [Test]
    public async Task TestRegisterFailed()
    {
        var result = await Client.Create("test", TestEndpoint);
        var client = result.Value;
        var registerResult = await client.Register("error", "", ChannelType.WNS);
        Assert.Multiple(() =>
        {
            Assert.That(registerResult.IsSuccess, Is.False);
            Assert.That(registerResult.Errors[0], Is.Not.Null);
        });
    }

    [Test]
    public async Task TestRegisterSuccess()
    {
        var result = await Client.Create("test", TestEndpoint);
        var client = result.Value;
        var registerResult = await client.Register("41811964-643f-11ed-81ce-0242ac120002", "", ChannelType.WNS);
        Assert.Multiple(() =>
        {
            Assert.That(registerResult.IsSuccess, Is.True);
            Assert.That(registerResult.Value, Is.True);
        });
    }
}