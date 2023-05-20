using System.Reflection;
using System.Security.Cryptography;

namespace EFCore.Extensions.AESEncrypt;

public static class EncryptionExtensions
{
    public static string Encrypt(this string value, string key)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        try
        {
            return value.EncryptString(key);
        }
        catch (Exception)
        {
            return value;
        }
    }

    public static string Decrypt(this string value, string key)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        try
        {
            return value.DecryptString(key);
        }
        catch (Exception)
        {
            return value;
        }
    }

    public static object EncryptValue(this object value, PropertyInfo property, string key)
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

    public static object DecryptValue(this object value, PropertyInfo property, string key)
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
            return value.ToString().DecryptString(key);
        }
        catch
        {
            return value;
        }
    }

    /// <summary>
    /// Encrypt string using AES mode CBC and Padding PKCS7
    /// </summary>
    /// <param name="str">String to encrypt</param>
    /// <param name="key">Key to encrypt</param>
    /// <returns>Encrypted value in base64 encoded</returns>
    private static string EncryptString(this string str, string key)
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

    /// <summary>
    /// Decrypt string using AES mode CBC and Padding PKCS7
    /// </summary>
    /// <param name="str">Base64 encoded string</param>
    /// <param name="key">Key to decrypt</param>
    /// <returns>Decrypted value</returns>
    private static string DecryptString(this string str, string key)
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
