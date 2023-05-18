using System.Reflection;

namespace EFCore.Extensions.AESEncrypt;

public static class PropertyInfoExtensions
{
    public static List<PropertyInfo> GetPublicProperties(this Type type)
    {
        return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)?.ToList() ?? new List<PropertyInfo>();
    }

    public static bool CanEncrypt(this PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType != typeof(string))
        {
            return false;
        }

        return propertyInfo.GetCustomAttribute(typeof(EncryptedAttribute)) != null;
    }
}
