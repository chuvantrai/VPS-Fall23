using System.ComponentModel;
using System.Reflection;

namespace Service.ManagerCRM.Extensions.StaticLogic;

public static class EnumExtension
{
    public static string GetEnumDescription(Enum value)
    {
        var fileInfo = value.GetType().GetField(value.ToString())!;

        if (fileInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes
            && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }
    public static T CoverIntToEnum<T>(int value)
    {
        return (T)Enum.ToObject(typeof(T), value);
    }
}