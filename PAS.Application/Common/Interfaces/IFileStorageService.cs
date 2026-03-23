namespace Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(byte[] content, string fileName, string folder, CancellationToken cancellationToken = default);
        Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
        Task<string> GetFileUrlAsync(string filePath, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}
