# EFCore.Extensions.AESEncrypt
 
Entity Framework Core extension to encrypt and decrypt entities using AES mode CBC and PKCS7 padding.

## How to use
On the entity use `[Encrypted]` attribute on propeties that needs to be encrypted and decrypted. It will only works on `string` properties.
``` C#
public class Message
{
    public int Id { get; set; }

    [Encrypted]
    public string Text { get; set; }
}
```
## Examples
All examples can be found in `MessagesController` in the TestAPI project. 

### Add entity and encrypt
``` C#
await context.AddEncryptAsync(message, encryptionKey);
await context.SaveChangesAsync();
```

### Get a list entities
``` C#
var result = await context.Messages.AsNoTracking().ToListDecryptAsync(encryptionKey);
```

### Select a single property and decrypt
``` C#
var result = await context.Messages.AsNoTracking()
                                   .Where(x => x.Id == id)
                                   .Select(x => x.Text.Decrypt(encryptionKey))
                                   .FirstOrDefaultAsync(key);
```
