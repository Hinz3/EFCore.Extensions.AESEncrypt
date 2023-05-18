using EFCore.Extensions.AESEncrypt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEstAPI.Entities;

namespace TEstAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly TestContext context;
    private string key = "Zg3s4sJsqRmhG6kl7u5bVmW/HgKJMi9cu8Kv7KzU/0o=";

    public MessagesController(TestContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var db = context.Messages.AsNoTracking().FirstOrDefaultDecrypt(x => x.Id == 3, key);
        return Ok(db);
    }

    [HttpGet("Decrypted")]
    public async Task<IActionResult> GetAllDecrpyted()
    {
        var db = await context.Messages.AsNoTracking().ToListDecryptAsync(key);
        return Ok(db);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Message message)
    {
        await context.AddEncryptAsync(message, key);
        await context.SaveChangesAsync();

        return Ok();
    }
}
