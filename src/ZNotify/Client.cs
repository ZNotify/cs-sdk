// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;
using Newtonsoft.Json;

namespace ZNotify;

public class Client
{
    private readonly APIClient _client;
    private readonly string _userSecret;
    private readonly string _endpoint;

    private Client(string userSecret, string endpoint = Static.DefaultEndpoint)
    {
        _userSecret = userSecret;
        _endpoint = endpoint;
        _client = new APIClient(endpoint, GetHttpClient());
    }

    private static HttpClient GetHttpClient()
    {
        var client = new HttpClient();
        var version = typeof(Client).Assembly.GetName().Version.ToString();
        client.DefaultRequestHeaders.Add("User-Agent", $"znotify-cs-sdk/{version}");
        return client;
    }

    public static Result<Client> Create(string userSecret, string endpoint = Static.DefaultEndpoint)
    {
        var result = Result.Try(() => Check(userSecret, endpoint).ConfigureAwait(false).GetAwaiter().GetResult());
        
        if (result.IsFailed)
        {
            return Result.Fail<Client>(result.Errors);
        }

        return result.Value ? Result.Ok(new Client(userSecret, endpoint)) : Result.Fail("User secret not valid");
    }

    private static async Task<bool> Check(string userSecret, string endpoint)
    {
        var client = new APIClient(endpoint, GetHttpClient());
        var result = await client.CheckUserSecretAsync(userSecret).ConfigureAwait(false);
        return result.Body ?? false;
    }

    public async Task<Result<Message>> Send(MessageOption option)
    {
        async Task<ResponseEntity_Message> Action()
        {
            return await _client.SendMessageAsync(
                _userSecret,
                option.Title,
                option.Content,
                option.LongContent,
                option.Priority
            );
        }

        var ret = await Result.Try(Action);
        return ret.IsFailed ? Result.Fail<Message>(ret.Errors) : Result.Ok(ret.Value!.Body);
    }

    public async Task<Result<bool>> Register(DeviceOption option)
    {
        if (!Guid.TryParse(option.DeviceId, out _))
        {
            return Result.Fail("DeviceId is not UUID");
        }

        async Task<ResponseBool> Action()
        {
            return await _client.CreateDeviceAsync(
                _userSecret,
                option.DeviceId,
                option.Channel,
                option.DeviceName,
                option.DeviceMeta,
                option.Token
            );
        }

        var ret = await Result.Try(Action);
        return ret.IsSuccess ? Result.Ok(ret.Value!.Body ?? false) : Result.Fail<bool>(ret.Errors);
    }

    public async Task<Result<List<Message>>> FetchMessage(int skip = 0, int limit = 20)
    {
        async Task<ResponseArray_entity_Message> Action()
        {
            return await _client.GetMessagesByUserSecretAsync(this._userSecret, skip, limit);
        }

        var ret = await Result.Try(Action);
        return ret.IsFailed ? Result.Fail<List<Message>>(ret.Errors) : Result.Ok(ret.Value!.Body.ToList());
    }
}

public record struct MessageOption(
    string Content,
    string Title = "Notification",
    string LongContent = "",
    Priority2 Priority = Priority2.Normal);

public record struct DeviceOption(
    string DeviceId,
    Channel Channel,
    string Token,
    string DeviceName = "",
    string DeviceMeta = ""
);