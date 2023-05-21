using AutoFixture.Xunit2;
using EFCore.Extensions.AESEncrypt.Tests.Configurations;
using EFCore.Extensions.AESEncrypt.Tests.Helper;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.AESEncrypt.Tests;

public class QueryExtensionsTest
{
    private string encryptionKey = "Zg3s4sJsqRmhG6kl7u5bVmW/HgKJMi9cu8Kv7KzU/0o=";

    [Theory]
    [AutoMoqSQLiteData]
    public void AddEncrypt_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        Assert.Throws<ArgumentNullException>(() => context.AddEncrypt(entity, null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void AddEncrypt_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "Test";
        context.AddEncrypt(entity, encryptionKey);
        context.SaveChanges();

        var db = await context.Messages.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.NotEqual("Test", db.Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void AddEncryptAsync_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => context.AddEncryptAsync(entity, null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void AddEncryptAsync_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "Test";
        await context.AddEncryptAsync(entity, encryptionKey);
        await context.SaveChangesAsync();

        var db = await context.Messages.AsNoTracking().FirstOrDefaultAsync();

        Assert.NotNull(db);
        Assert.NotEqual("Test", db.Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public void AddRange_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        Assert.Throws<ArgumentNullException>(() => context.AddRangeEncrypt(new List<Message> { entity }, null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void AddRangeEncrypt_Success([Frozen] TestContext context, Message entity, Message otherEntity)
    {
        entity.Text = "Test";
        otherEntity.Text = "Hello";
        context.AddRangeEncrypt(new List<Message> { entity, otherEntity }, encryptionKey);
        context.SaveChanges();

        var db = await context.Messages.AsNoTracking().ToListAsync();

        Assert.NotEmpty(db);
        Assert.DoesNotContain(db, x => x.Text == "Test");
        Assert.DoesNotContain(db, x => x.Text == "Hello");
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void AddRangeEncryptAsync_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await context.AddRangeEncryptAsync(new List<Message> { entity }, null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void AddRangeEncryptAsync_Success([Frozen] TestContext context, Message entity, Message otherEntity)
    {
        entity.Text = "Test";
        otherEntity.Text = "Hello";
        await context.AddRangeEncryptAsync(new List<Message> { entity, otherEntity }, encryptionKey);
        await context.SaveChangesAsync();

        var db = await context.Messages.AsNoTracking().ToListAsync();

        Assert.NotEmpty(db);
        Assert.DoesNotContain(db, x => x.Text == "Test");
        Assert.DoesNotContain(db, x => x.Text == "Hello");
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecrypt_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.Throws<ArgumentNullException>(() => context.Messages.FirstOrDefaultDecrypt(null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecrypt_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = context.Messages.FirstOrDefaultDecrypt(encryptionKey);

        Assert.Equal("Test", result.Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecryptPredicate_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.Throws<ArgumentNullException>(() => context.Messages.FirstOrDefaultDecrypt(x => x.Id == entity.Id, null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecryptPredicate_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = context.Messages.FirstOrDefaultDecrypt(x => x.Id == entity.Id, encryptionKey);

        Assert.Equal("Test", result.Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecryptAsync_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await context.Messages.FirstOrDefaultDecryptAsync(null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecryptAsync_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await context.Messages.FirstOrDefaultDecryptAsync(encryptionKey);

        Assert.Equal("Test", result.Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecryptAsyncPredicate_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await context.Messages.FirstOrDefaultDecryptAsync(x => x.Id == entity.Id, null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void FirstOrDefaultDecryptAsyncPredicate_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await context.Messages.FirstOrDefaultDecryptAsync(x => x.Id == entity.Id, encryptionKey);

        Assert.Equal("Test", result.Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToListDecrypt_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.Throws<ArgumentNullException>(() => context.Messages.ToListDecrypt(null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToListDecrypt_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = context.Messages.ToListDecrypt(encryptionKey);

        Assert.NotEmpty(result);
        Assert.Equal("Test", result.First().Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToListDecryptAsync_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await context.Messages.ToListDecryptAsync(null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToListDecryptAsync_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await context.Messages.ToListDecryptAsync(encryptionKey);

        Assert.NotEmpty(result);
        Assert.Equal("Test", result.First().Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToArrayDecrypt_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        Assert.Throws<ArgumentNullException>(() => context.Messages.ToArryDecrypt(null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToArrayDecrypt_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = context.Messages.ToArryDecrypt(encryptionKey);

        Assert.NotEmpty(result);
        Assert.Equal("Test", result.First().Text);
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToArrayDecryptAsync_MissingEncryptionKey([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await context.Messages.ToArryDecryptAsync(null));
    }

    [Theory]
    [AutoMoqSQLiteData]
    public async void ToArrayDecryptAsync_Success([Frozen] TestContext context, Message entity)
    {
        entity.Text = "FBF/QXVD5rSqyaIIMIunUbcLs7rHJOWLBvwrVZZ1qGI=";

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var result = await context.Messages.ToArryDecryptAsync(encryptionKey);

        Assert.NotEmpty(result);
        Assert.Equal("Test", result.First().Text);
    }

}
