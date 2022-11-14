using Newtonsoft.Json;

namespace ZNotify.Entity;

public record Response<T>
{
    [JsonConstructor]
    public Response(int code, T body)
    {
        Code = code;
        Body = body;
    }

    [JsonProperty(PropertyName = "code")]
    public int Code { get; set; }
    
    [JsonProperty(PropertyName = "body")]
    public T Body { get; set; }
}