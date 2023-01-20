using System.Threading.Tasks;

namespace ZNotify.Test;

public class ClientTests
{
    private const string TestEndpoint = "http://localhost:14444";

    private readonly Client _client = Client.Create("test", TestEndpoint).Value!;

    [Test]
    public void TestClientCreateFailed()
    {
        var result = Client.Create("error", TestEndpoint);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            // Assert.That(result., Is.True);
        });
    }

    [Test]
    public void TestClientCreateSuccess()
    {
        var result = Client.Create("test", TestEndpoint);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task TestSendFailed()
    {
        var sendResult = await _client.Send(new MessageOption
        {
            Title = "Test",
            Content = "",
            Priority = Priority2.High,
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
        var sendResult = await _client.Send(new MessageOption
        {
            Title = "Test",
            Content = "",
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
        var registerResult = await _client.Register(new DeviceOption
        {
            Channel = Channel.FCM,
            Token = "test",
            DeviceId = "test"
        });
        Assert.Multiple(() =>
        {
            Assert.That(registerResult.IsSuccess, Is.False);
            Assert.That(registerResult.Errors[0], Is.Not.Null);
        });
    }

    [Test]
    public async Task TestRegisterSuccess()
    {
        var registerResult = await _client.Register(new DeviceOption
        {
            DeviceId = "41811964-643f-11ed-81ce-0242ac120002"
        });
        Assert.Multiple(() =>
        {
            Assert.That(registerResult.IsSuccess, Is.True);
            Assert.That(registerResult.Value, Is.True);
        });
    }

    [Test]
    public async Task TestFetchMessage()
    {
        var fetchResult = await _client.FetchMessage();
        Assert.Multiple(() =>
        {
            Assert.That(fetchResult.IsSuccess, Is.True);
            Assert.That(fetchResult.Value, Is.Not.Null);
        });
    }
}