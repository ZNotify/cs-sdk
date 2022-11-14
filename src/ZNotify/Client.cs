// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;
using Newtonsoft.Json;
using ZNotify.Entity;
using ZNotify.Utils;

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
        _client = new HttpClient();
    }

    private async Task<Result<Client>> Create(string userId, string endpoint = Static.DefaultEndpoint)
    {
        var result = await Check(userId, endpoint);
        if (result)
        {
            return Result.Ok(new Client(userId, endpoint));
        }
        else
        {
            return Result.Fail("UserId not valid");
        }
    }

    public static async Task<bool> Check(string userId, string endpoint = Static.DefaultEndpoint)
    {
        var client = new Client(userId, endpoint);
        var response = await new HttpClient().GetAsync(client._endpoint + "/check");
        return response.IsSuccessStatusCode;
    }

    public async Task<Message> Send(MessageOption option)
    {
        var data = new List<KeyValuePair<string, string>>();

        if (option.Content.Length > 0)
        {
            data.Add(new KeyValuePair<string, string>("content", option.Content));
        }
        else
        {
            throw new Exception("Content is required");
        }

        if (option.Title.Length > 0)
        {
            data.Add(new KeyValuePair<string, string>("title", option.Title));
        }

        if (option.LongContent.Length > 0)
        {
            data.Add(new KeyValuePair<string, string>("long", option.LongContent));
        }

        data.Add(new KeyValuePair<string, string>("priority", option.Priority));

        var url = $"{_endpoint}/{_userId}/send";

        var response = await _client.PostAsync(url, new FormUrlEncodedContent(data));
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<Message>>(content)!.Body;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var errText = JsonConvert.DeserializeObject<Response<string>>(content)!.Body;
            throw new Exception(errText);
        }
    }

    public async Task<Result<bool>> Register(string deviceId, string token, string channel)
    {
        var data = new List<KeyValuePair<string, string>>
        {
            new("channel", channel),
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
}