using DaraSurvey.Core;
using DaraSurvey.Interfaces;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DaraSurvey.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly string _rootPhysicalPath;
        private readonly string _tempPhysicalPath;

        public FileSystemService(IWebHostEnvironment env, IOptionsSnapshot<AppSettings> appSetting)
        {
            _rootPhysicalPath = Path.Combine(env.ContentRootPath, "wwwroot");
            _tempPhysicalPath = Path.Combine(_rootPhysicalPath, "Temp");

            InitContainers(appSetting.Value.Containers);
        }

        // ------------------

        public IEnumerable<Container> GetContainers()
        {
            var directories = Directory.GetDirectories(_rootPhysicalPath);

            if (directories != null && directories.Any())
            {
                return directories.Select(d => GetContainer(Path.GetFileName(d)));
            }
            return new Container[0];
        }

        // ------------------

        public Container GetContainer(string containerName)
        {
            if (!string.IsNullOrEmpty(containerName))
            {
                var d = Path.Combine(_rootPhysicalPath, containerName);
                if (Directory.Exists(d))
                {
                    return new Container
                    {
                        Name = Path.GetFileName(d),
                        Created = Directory.GetCreationTime(d),
                        Buckets = Directory.GetDirectories(d).Select(o => GetBucket(containerName, Path.GetFileName(o))),
                        Size = GetDirectorySize(d)
                    };
                }
            }
            return null;
        }

        // ------------------

        public Bucket GetBucket(string containerName, string bucketName)
        {
            if (!string.IsNullOrEmpty(containerName) && !string.IsNullOrEmpty(bucketName))
            {
                var d = Path.Combine(_rootPhysicalPath, containerName, bucketName);
                if (Directory.Exists(d))
                {
                    return new Bucket
                    {
                        Name = Path.GetFileName(d),
                        Created = Directory.GetCreationTime(d),
                        Size = GetDirectorySize(d),
                    };
                }
            }
            return null;
        }

        // ------------------

        public IEnumerable<DaraSurvey.Models.File> GetFiles(string containerName, string bucketName)
        {
            if (!string.IsNullOrEmpty(containerName) && !string.IsNullOrEmpty(bucketName))
            {
                var d = Path.Combine(_rootPhysicalPath, containerName, bucketName);
                if (Directory.Exists(d))
                {
                    var files = Directory.GetFiles(d);
                    return files.Select(o => new DaraSurvey.Models.File
                    {
                        Created = System.IO.File.GetCreationTime(o),
                        Name = Path.GetFileName(o),
                        Size = new FileInfo(o).Length
                    });
                }
            }
            return new DaraSurvey.Models.File[0];
        }

        // ------------------

        public FileServiceStatus CreateContainer(string containerName)
        {
            if (!string.IsNullOrEmpty(containerName))
            {
                var d = Path.Combine(_rootPhysicalPath, containerName);
                if (!Directory.Exists(d))
                {
                    var info = Directory.CreateDirectory(d);
                    return info.Exists ? FileServiceStatus.succeeded : FileServiceStatus.operationFailed;
                }
                return FileServiceStatus.alreadyExists;
            }
            return FileServiceStatus.worngParameters;
        }

        // ------------------

        public FileServiceStatus CreateBucket(string containerName, string bucketName)
        {
            if (!string.IsNullOrEmpty(containerName) && !string.IsNullOrEmpty(bucketName))
            {
                var d = Path.Combine(_rootPhysicalPath, containerName, bucketName);
                if (!Directory.Exists(d))
                {
                    var info = Directory.CreateDirectory(d);
                    return info.Exists ? FileServiceStatus.succeeded : FileServiceStatus.operationFailed;
                }
                return FileServiceStatus.alreadyExists;
            }
            return FileServiceStatus.worngParameters;
        }

        // ------------------

        public FileServiceStatus DeleteContainer(string containerName)
        {
            if (!string.IsNullOrEmpty(containerName))
            {
                var d = Path.Combine(_rootPhysicalPath, containerName);
                if (Directory.Exists(d))
                {
                    if (Directory.GetDirectories(d).Length > 0)
                    {
                        Directory.Delete(d);
                        return FileServiceStatus.succeeded;
                    }
                    return FileServiceStatus.notEmpty;
                }
                return FileServiceStatus.succeeded;
            }
            return FileServiceStatus.worngParameters;
        }

        // ------------------

        public FileServiceStatus DeleteBucket(string containerName, string bucketName)
        {
            if (!string.IsNullOrEmpty(containerName) && !string.IsNullOrEmpty(bucketName))
            {
                var d = Path.Combine(_rootPhysicalPath, containerName, bucketName);
                if (Directory.Exists(d))
                {
                    if (Directory.GetFiles(d).Length > 0)
                    {
                        Directory.Delete(d);
                        return FileServiceStatus.succeeded;
                    }
                    return FileServiceStatus.notEmpty;
                }
                return FileServiceStatus.succeeded;
            }
            return FileServiceStatus.worngParameters;
        }

        // ------------------

        public FileServiceStatus DeleteFiles(string filePaths)
        {
            if (!string.IsNullOrEmpty(filePaths))
            {
                foreach (var fileName in filePaths.Split(','))
                {
                    var filePhysicalPath = Path.Combine(_rootPhysicalPath, fileName);
                    if (System.IO.File.Exists(filePhysicalPath))
                    {
                        System.IO.File.Delete(filePhysicalPath);
                    }
                }
            }

            return FileServiceStatus.succeeded;
        }

        // ------------------


        public FileServiceStatus DeleteFiles(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                var filePhysicalPath = Path.Combine(_rootPhysicalPath, filePath);
                if (System.IO.File.Exists(filePhysicalPath))
                {
                    System.IO.File.Delete(filePhysicalPath);
                }
            }

            return FileServiceStatus.succeeded;
        }

        // ------------------

        public FileServiceStatus DeleteTempFiles(string fileNames)
        {
            if (!string.IsNullOrEmpty(fileNames))
            {
                foreach (var fileName in fileNames.Split(','))
                {
                    var filePath = Path.Combine(_tempPhysicalPath, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            return FileServiceStatus.succeeded;
        }

        // ------------------

        public IEnumerable<string> Upload(IEnumerable<IFormFile> files)
        {
            return files.Select(o =>
            {
                var fileName = string.Concat(Guid.NewGuid().ToString().Replace("-", ""), Path.GetExtension(o.FileName));
                var fileStream = new FileStream(Path.Combine(_tempPhysicalPath, fileName), FileMode.Create);
                o.CopyTo(fileStream);
                return fileName;
            });
        }

        // ------------------

        public DownloadResult Download(string containerName, string bucketName, string fileName)
        {
            var fileInPhysicalPath = Path.Combine(_rootPhysicalPath, containerName, bucketName, fileName);
            return new DownloadResult
            {
                File = System.IO.File.ReadAllBytes(fileInPhysicalPath),
                MimeType = MimeTypeMap.GetMimeType(Path.GetExtension(fileName))
            };
        }

        // ------------------

        public DownloadResult Download(string filePath)
        {
            var fileInPhysicalPath = Path.Combine(_rootPhysicalPath, filePath);
            return new DownloadResult
            {
                File = System.IO.File.ReadAllBytes(fileInPhysicalPath),
                MimeType = MimeTypeMap.GetMimeType(Path.GetExtension(filePath))
            };
        }

        private long GetDirectorySize(string dirPath)
        {
            var d = new DirectoryInfo(dirPath);
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += GetDirectorySize(di.FullName);
            }
            return size;
        }

        // ------------------

        // Move file from temp directory to specified container and bucket
        public string ManipulateAttachments(string oldFilePaths, string newFilePaths, string containerName, string bucketName)
        {
            var oldFilePathsArr = string.IsNullOrEmpty(oldFilePaths) ? new string[0] : oldFilePaths.Replace('\\', '/').Split(',');
            var newFilePathsArr = string.IsNullOrEmpty(newFilePaths) ? new string[0] : newFilePaths.Replace('\\', '/').Split(',');

            if (!Enumerable.SequenceEqual(oldFilePathsArr.OrderBy(t => t), newFilePathsArr.OrderBy(t => t)))
            {
                var manipulatedFilePaths = new List<string>();

                // Delete Old Values
                DeleteFiles(oldFilePathsArr.Except(newFilePathsArr));

                // Move if new file is in temp folder
                var tempFilePaths = newFilePathsArr.Where(url => url.ToLower().Replace('\\', '/').Split('/').Count() == 2).ToList();
                foreach (var tempFilePath in tempFilePaths)
                {
                    var fileName = Path.GetFileName(tempFilePath);
                    var destinationFilePath = Path.Combine(containerName, bucketName, fileName);

                    var sourceFilePhysicalPath = Path.Combine(_rootPhysicalPath, tempFilePath);
                    var destinationFilePhysicalPath = Path.Combine(_rootPhysicalPath, destinationFilePath);

                    if (System.IO.File.Exists(sourceFilePhysicalPath))
                        System.IO.File.Move(sourceFilePhysicalPath, destinationFilePhysicalPath);

                    manipulatedFilePaths.Add(destinationFilePath);
                }

                manipulatedFilePaths.AddRange(newFilePathsArr.Except(tempFilePaths));

                return manipulatedFilePaths.Any() ? manipulatedFilePaths.GetString(",") : null;
            }

            return newFilePaths;
        }

        // ------------------

        public void ManipulateAttachments<T>(T oldModel, T newModel) where T : class
        {
            var attachments = AttachmentAttributeInfo.GetAttachmentAttributes(typeof(T));
            foreach (var attachment in attachments)
            {
                var oldValue = (oldModel == null) ? null : attachment.Property.GetValue(oldModel) as string;
                var newValue = (newModel == null) ? null : attachment.Property.GetValue(newModel) as string;

                var manipulatedValue = ManipulateAttachments(oldValue, newValue, attachment.Container, attachment.Bucket);

                if (newModel != null)
                    attachment.Property.SetValue(newModel, manipulatedValue);
            }
        }

        // ------------------

        // create default containers and buckets if they don't exist
        private void InitContainers(IEnumerable<FileContainer> fileContainers)
        {
            var dirPath = _rootPhysicalPath;

            if (fileContainers != null && fileContainers.Any())
            {
                foreach (var fileContainer in fileContainers)
                {
                    // Create File Containers
                    dirPath = Path.Combine(_rootPhysicalPath, fileContainer.Name);
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    // Create Buckets
                    if (fileContainer.Buckets != null && fileContainer.Buckets.Any())
                        foreach (var bucket in fileContainer.Buckets)
                        {
                            dirPath = Path.Combine(dirPath, bucket);
                            if (!Directory.Exists(dirPath))
                                Directory.CreateDirectory(dirPath);

                            // Remover bucket name from path for read an other bucket
                            dirPath = Path.GetDirectoryName(dirPath);
                        }
                    // Remove file container name form path for read an other file container
                    dirPath = Path.GetDirectoryName(dirPath);
                }
            }
        }
    }
}
