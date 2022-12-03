// ReSharper disable MemberCanBePrivate.Global

using System.ComponentModel;

namespace ZNotify.Entity;

public record struct MessageOption
{
    public MessageOption(string content)
    {
        Content = content;
    }
    
    public string Content = "";
    public string LongContent = "";
    public string Title = "Notification";
    public PriorityType Priority = PriorityType.Normal;
}

public enum PriorityType
{
    [Description("low")]
    Low,
    
    [Description("normal")]
    Normal,
    
    [Description("high")]
    High
}

public static class PriorityExtension
{
    public static string GetDescription(this PriorityType priorityType)
    {
        var type = priorityType.GetType();
        var memInfo = type.GetMember(priorityType.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        return ((DescriptionAttribute)attributes[0]).Description;
    }
}

