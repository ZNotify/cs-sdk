// ReSharper disable MemberCanBePrivate.Global
namespace ZNotify.Entity;

public record MessageOption
{
    public class Priorities
    {
        public const string Low = "low";
        public const string Normal = "normal";
        public const string High = "high";
    }

    MessageOption(string content, string longContent = "", string title = "Notification",
        string priority = Priorities.Normal)
    {
        Content = content;
        LongContent = longContent;
        Title = title;
        Priority = priority;
    }

    public string Content;
    public string LongContent;
    public string Title;
    public string Priority;
}