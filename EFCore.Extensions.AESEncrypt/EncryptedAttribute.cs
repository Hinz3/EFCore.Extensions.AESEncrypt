namespace EFCore.Extensions.AESEncrypt;

/// <summary>
/// Can only encrypt string.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EncryptedAttribute : Attribute
{
}
