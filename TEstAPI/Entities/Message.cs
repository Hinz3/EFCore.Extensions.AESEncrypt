using EFCore.Extensions.AESEncrypt;

namespace TEstAPI.Entities;

public class Message
{
    public int Id { get; set; }

    [Encrypted]
    public string Text { get; set; }
}
