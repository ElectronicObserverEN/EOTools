using EOTools.Models.KancolleApi;
using EOTools.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.IO;
using System.IO.Compression;

namespace EOTools.DataBase;

public class ElectronicObserverContext : DbContext
{
    public DbSet<ApiFile> ApiFiles { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={AppSettings.EoDbPath}");

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApiFile>()
            .HasKey(a => a.Id);

        builder.Entity<ApiFile>()
            .Property(a => a.Content)
            .HasConversion(new ValueConverter<string, byte[]>
            (
                s => CompressBytes(System.Text.Encoding.UTF8.GetBytes(s)),
                b => System.Text.Encoding.UTF8.GetString(DecompressBytes(b))
            ));
    }

    public override int SaveChanges()
    {
        // Throw if they try to call this
        throw new InvalidOperationException("This context is read-only.");
    }

    private static byte[] CompressBytes(byte[] bytes)
    {
        using MemoryStream outputStream = new();
        using (BrotliStream compressStream = new(outputStream, CompressionLevel.SmallestSize))
        {
            compressStream.Write(bytes, 0, bytes.Length);
        }

        return outputStream.ToArray();
    }

    private static byte[] DecompressBytes(byte[] bytes)
    {
        using MemoryStream inputStream = new(bytes);
        using MemoryStream outputStream = new();
        using (BrotliStream decompressStream = new(inputStream, CompressionMode.Decompress))
        {
            decompressStream.CopyTo(outputStream);
        }

        return outputStream.ToArray();
    }
}
