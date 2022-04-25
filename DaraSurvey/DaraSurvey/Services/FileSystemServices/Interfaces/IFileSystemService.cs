using DaraSurvey.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DaraSurvey.Interfaces
{
    public interface IFileSystemService
    {
        IEnumerable<Container> GetContainers();
        Container GetContainer(string containerName);
        Bucket GetBucket(string containerName, string bucketName);
        IEnumerable<File> GetFiles(string containerName, string bucketName);
        FileServiceStatus CreateContainer(string containerName);
        FileServiceStatus CreateBucket(string containerName, string bucketName);
        FileServiceStatus DeleteContainer(string containerName);
        FileServiceStatus DeleteBucket(string containerName, string bucketName);
        FileServiceStatus DeleteFiles(string filePaths);
        FileServiceStatus DeleteTempFiles(string filePaths);
        FileServiceStatus DeleteFiles(IEnumerable<string> filePaths);
        IEnumerable<string> Upload(IEnumerable<IFormFile> files);
        string ManipulateAttachments(string oldFilePaths, string newFilePaths, string containerName, string bucketName);
        void ManipulateAttachments<T>(T oldModel, T newModel) where T : class;
        DownloadResult Download(string containerName, string bucketName, string fileName);
        DownloadResult Download(string filePath);
    }

    // ----------

    public enum FileServiceStatus
    {
        succeeded,
        alreadyExists,
        worngParameters,
        notEmpty,
        operationFailed,
        accessDenied
    }
}
