using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCore.Extensions.AESEncrypt;

public static class QueryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <param name="encryptionKey"></param>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static void AddEncrypt<TEntity>(this DbContext context, TEntity entity, string encryptionKey)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        EncryptEntity(entity, encryptionKey);

        context.Add(entity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <param name="encryptionKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static async Task AddEncryptAsync<TEntity>(this DbContext context, TEntity entity, string encryptionKey, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        EncryptEntity(entity, encryptionKey);

        await context.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="context"></param>
    /// <param name="entities"></param>
    /// <param name="encryptionKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static async Task AddRangeEncryptAsync<TEntity>(this DbContext context, List<TEntity> entities, string encryptionKey, 
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        foreach (var entity in entities)
        {
            EncryptEntity(entity, encryptionKey);
        }

        await context.AddRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="encryptionKey"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static TEntity FirstOrDefaultDecrypt<TEntity>(this IQueryable<TEntity> source, string encryptionKey)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        var entity = source.FirstOrDefault();
        return DecryptEntity(entity, encryptionKey);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="encryptionKey"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static TEntity FirstOrDefaultDecrypt<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> predicate, string encryptionKey)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        var entity = source.FirstOrDefault(predicate);
        return DecryptEntity(entity, encryptionKey);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="encryptionKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static async Task<TEntity> FirstOrDefaultDecryptAsync<TEntity>(this IQueryable<TEntity> source, string encryptionKey, 
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        var entity = await source.FirstOrDefaultAsync(cancellationToken);
        return DecryptEntity(entity, encryptionKey);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="encryptionKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static async Task<TEntity> FirstOrDefaultDecryptAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> predicate, 
        string encryptionKey, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        var entity = await source.FirstOrDefaultAsync(predicate, cancellationToken);
        return DecryptEntity(entity, encryptionKey);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="encryptionKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static async Task<List<TEntity>> ToListDecryptAsync<TEntity>(this IQueryable<TEntity> source, string encryptionKey, 
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        var entities = await source.ToListAsync(cancellationToken);

        foreach (var entity in entities)
        {
            DecryptEntity(entity, encryptionKey);
        }

        return entities;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="encryptionKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Throws if encryption key is not set</exception>
    public static async Task<TEntity[]> ToArryDecryptAsync<TEntity>(this IQueryable<TEntity> source, string encryptionKey,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Missing encryption key");
        }

        var result = await ToListDecryptAsync(source, encryptionKey, cancellationToken);
        return result.ToArray();
    }

    /// <summary>
    /// Encrypt all strings on entity using <paramref name="encryptionKey"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="encryptionKey"></param>
    private static void EncryptEntity<TEntity>(TEntity entity, string encryptionKey)
        where TEntity : class
    {
        var properties = entity.GetType().GetPublicProperties();
        foreach (var property in properties)
        {
            if (!property.CanEncrypt())
            {
                continue;
            }

            var encryptedValue = property.GetValue(entity, null).Encrypt(property, encryptionKey);
            property.SetValue(entity, encryptedValue);
        }
    }

    /// <summary>
    /// Decrypt all string on entity using <paramref name="encryptionKey"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="encryptionKey"></param>
    /// <returns></returns>
    private static TEntity DecryptEntity<TEntity>(TEntity entity, string encryptionKey)
        where TEntity : class
    {
        if (entity == null)
        {
            return null;
        }

        var properties = entity.GetType().GetPublicProperties();
        foreach (var property in properties)
        {
            if (!property.CanEncrypt())
            {
                continue;
            }

            var encryptedValue = property.GetValue(entity, null).Decrypt(property, encryptionKey);
            property.SetValue(entity, encryptedValue);
        }

        return entity;
    }
}
