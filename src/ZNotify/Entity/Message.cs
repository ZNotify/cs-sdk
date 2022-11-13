using Newtonsoft.Json;

namespace ZNotify.Entity;

public record Message
{
    Message(string id, string userId, string content, string title, string longContent, string createdAt)
    {
        Id = id;
        UserId = userId;
        Content = content;
        Title = title;
        LongContent = longContent;
        CreatedAt = createdAt;
    }

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "user_id")]
    public string UserId { get; set; }

    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }

    [JsonProperty(PropertyName = "title")] 
    public string Title { get; set; }

    [JsonProperty(PropertyName = "long")] 
    public string LongContent { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public string CreatedAt { get; set; }
}