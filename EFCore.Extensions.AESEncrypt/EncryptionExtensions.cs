using System.Reflection;
using System.Security.Cryptography;

namespace EFCore.Extensions.AESEncrypt;

public static class EncryptionExtensions
{
    public static object Encrypt(this object value, PropertyInfo property, string key)
    {
        if (value == null)
        {
            return null;
        }

        if (property.PropertyType != typeof(string))
        {
            return value;
        }
        
        try
        {
            return value.ToString().EncryptString(key);
        }
        catch 
        { 
            return value;
        }
    }

    public static object Decrypt(this object value, PropertyInfo property, string key)
    {
        if (value == null)
        {
            return null;
        }

        if (property.PropertyType != typeof(string))
        {
            return value;
        }

        try
        {
            return value.ToString().DecrpytString(key);
        }
        catch
        {
            return value;
        }
    }

    public static string EncryptString(this string str, string key)
    {
        byte[] encrypted;
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);

        aes.GenerateIV();

        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream msEncrypt = new();
        msEncrypt.Write(aes.IV, 0, aes.IV.Length);
        ICryptoTransform encoder = aes.CreateEncryptor();
        using (CryptoStream csEncrypt = new(msEncrypt, encoder, CryptoStreamMode.Write))
        using (StreamWriter swEncrypt = new(csEncrypt))
        {
            swEncrypt.Write(str);
        }
        encrypted = msEncrypt.ToArray();

        return Convert.ToBase64String(encrypted);
    }

    public static string DecrpytString(this string str, string key)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        byte[] cipherText = Convert.FromBase64String(str);

        using var aes = Aes.Create();

        aes.Key = Convert.FromBase64String(key);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream msDecryptor = new(cipherText);
        
        byte[] readIV = new byte[16];
        msDecryptor.Read(readIV, 0, 16);
        
        aes.IV = readIV;
        
        ICryptoTransform decoder = aes.CreateDecryptor();

        using CryptoStream csDecryptor = new(msDecryptor, decoder, CryptoStreamMode.Read);
        using StreamReader srReader = new(csDecryptor);

        return srReader.ReadToEnd();
    }
}
