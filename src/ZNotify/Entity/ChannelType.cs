using System.ComponentModel;

namespace ZNotify.Entity;

public enum ChannelType
{
    [Description("WNS")]
    WNS
}

public static class ChannelTypeExtensions
{
    public static string GetDescription(this ChannelType channelType)
    {
        var fieldInfo = channelType.GetType().GetField(channelType.ToString());
        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}