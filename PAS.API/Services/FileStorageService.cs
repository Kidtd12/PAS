using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using PAS.API.Configurations;

namespace PAS.API.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageSettings _settings;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(IOptions<FileStorageSettings> settings, ILogger<FileStorageService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(byte[] content, string fileName, string folder, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(content);
        return await SaveFileAsync(stream, fileName, folder, cancellationToken);
    }

    public async Task<string> SaveFileAsync(Stream stream, string fileName, string folder, CancellationToken cancellationToken = default)
    {
        try
        {
            var directoryPath = Path.Combine(_settings.LocalPath, folder);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(directoryPath, uniqueFileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream, cancellationToken);

            _logger.LogInformation("File saved: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save file {FileName}", fileName);
            throw;
        }
    }

    public async Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            return await File.ReadAllBytesAsync(filePath, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get file from {FilePath}", filePath);
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("File deleted: {FilePath}", filePath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file {FilePath}", filePath);
            throw;
        }
    }

    public async Task<string> GetFileUrlAsync(string filePath)
    {
        var relativePath = filePath.Replace(_settings.LocalPath, "").Replace("\\", "/");
        return $"{_settings.BaseUrl}{relativePath}";
    }
}