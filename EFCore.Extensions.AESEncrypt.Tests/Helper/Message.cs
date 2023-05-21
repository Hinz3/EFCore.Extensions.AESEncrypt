namespace EFCore.Extensions.AESEncrypt.Tests.Helper;

public class Message
{
    public int Id { get; set; }

    [Encrypted]
    public string Text { get; set; }
}
