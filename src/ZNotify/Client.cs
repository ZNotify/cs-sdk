// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;
using Newtonsoft.Json;
using ZNotify.Entity;

namespace ZNotify;

public class Client
{
    private readonly HttpClient _client;
    private readonly string _userId;
    private readonly string _endpoint;

    private Client(string userId, string endpoint = Static.DefaultEndpoint)
    {
        _userId = userId;
        _endpoint = endpoint;
        _client = GetHttpClient();
    }

    private static HttpClient GetHttpClient()
    {
        var client = new HttpClient();
        var version = typeof(Client).Assembly.GetName().Version.ToString();
        client.DefaultRequestHeaders.Add("User-Agent", $"znotify-cs-sdk/{version}");
        return client;
    }

    public static async Task<Result<Client>> Create(string userId, string endpoint = Static.DefaultEndpoint)
    {
        var result = await Check(userId, endpoint);
        return result ? Result.Ok(new Client(userId, endpoint)) : Result.Fail("UserId not valid");
    }

    public static async Task<bool> Check(string userId, string endpoint = Static.DefaultEndpoint)
    {
        var urlBase = $"{endpoint}/check";
        // query string encode
        var url = $"{urlBase}?user_id={Uri.EscapeDataString(userId)}";
        
        var response = await GetHttpClient().GetAsync(url);
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Response<bool>>(responseString)!.Body;
    }

    public async Task<Result<Message>> Send(MessageOption option)
    {
        var data = new List<KeyValuePair<string, string>>();

        if (option.Content.Length > 0)
        {
            data.Add(new KeyValuePair<string, string>("content", option.Content));
        }
        else
        {
            return Result.Fail("Content is required");
        }

        if (option.Title.Length > 0)
        {
            data.Add(new KeyValuePair<string, string>("title", option.Title));
        }

        if (option.LongContent.Length > 0)
        {
            data.Add(new KeyValuePair<string, string>("long", option.LongContent));
        }

        data.Add(new KeyValuePair<string, string>("priority", option.Priority.GetDescription()));

        var url = $"{_endpoint}/{_userId}/send";

        var response = await _client.PostAsync(url, new FormUrlEncodedContent(data));
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Result.Ok(JsonConvert.DeserializeObject<Response<Message>>(content)!.Body);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var errText = JsonConvert.DeserializeObject<Response<string>>(content)!.Body;
            return Result.Fail(errText);
        }
    }

    public async Task<Result<bool>> Register(string deviceId, string token, ChannelType channel)
    {
        if (!Guid.TryParse(deviceId, out _))
        {
            return Result.Fail("DeviceId is not UUID");
        }
        
        var data = new List<KeyValuePair<string, string>>
        {
            new("channel", channel.GetDescription()),
            new("token", token)
        };

        var url = $"{_endpoint}/{_userId}/token/{deviceId}";
        var response = await _client.PutAsync(url, new FormUrlEncodedContent(data));
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Result.Ok(JsonConvert.DeserializeObject<Response<bool>>(content)!.Body);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var errText = JsonConvert.DeserializeObject<Response<string>>(content)!.Body;
            return Result.Fail(errText);
        }
    }

    public async Task<Result<List<Message>>> FetchMessage()
    {
        var url = $"{_endpoint}/{_userId}/record";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Result.Ok(JsonConvert.DeserializeObject<Response<List<Message>>>(content)!.Body);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var errText = JsonConvert.DeserializeObject<Response<string>>(content)!.Body;
            return Result.Fail(errText);
        }
    }
}