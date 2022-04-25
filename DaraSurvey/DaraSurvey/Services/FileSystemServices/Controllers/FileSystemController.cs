using DaraSurvey.Interfaces;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace DaraSurvey.Controllers
{
    [ApiController]
    [Route("api/v1/file")]
    //[JwtAuth("superAdmin")]
    [Produces("application/json")]
    public class FileSystemController : ControllerBase
    {
        private IFileSystemService _fileSystemService;

        public FileSystemController(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        // ------------------------

        [HttpGet("countainers")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<Container>> GetContainers()
        {
            return Ok(_fileSystemService.GetContainers());
        }

        // ------------------------


        [HttpGet("countainers/{name}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<Container> GetContainer([FromRoute] string name)
        {
            return Ok(_fileSystemService.GetContainer(name));
        }

        // ------------------------

        [HttpGet("countainers/{containerName}/buckets/{bucketName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<Bucket> GetBucket([FromRoute] string containerName, [FromRoute] string bucketName)
        {
            return Ok(_fileSystemService.GetBucket(containerName, bucketName));
        }

        // ------------------------

        [HttpGet("countainers/{containerName}/buckets/{bucketName}/files")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<File>> GetFiles([FromRoute] string containerName, [FromRoute] string bucketName)
        {
            return Ok(_fileSystemService.GetFiles(containerName, bucketName));
        }

        // ------------------------

        [HttpPost("containers/{name}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<FileServiceStatus> CreateContainer([FromRoute] string name)
        {
            return Ok(_fileSystemService.CreateContainer(name));
        }

        // ------------------------

        [HttpPost("countainers/{containerName}/buckets/{bucketName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<FileServiceStatus> CreateBucket([FromRoute] string containerName, [FromRoute] string bucketName)
        {
            return Ok(_fileSystemService.CreateBucket(containerName, bucketName));
        }

        // ------------------------

        [HttpDelete("containers/{name}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<FileServiceStatus> DeleteContainer([FromRoute] string name)
        {
            return Ok(_fileSystemService.DeleteContainer(name));
        }

        // ------------------------

        [HttpDelete("countainers/{containerName}/buckets/{bucketName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<FileServiceStatus> DeleteBucket([FromRoute] string containerName, [FromRoute] string bucketName)
        {
            return _fileSystemService.DeleteBucket(containerName, bucketName);
        }

        // ------------------------

        [HttpDelete("temp/{fileName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public ActionResult<FileServiceStatus> DeleteFile(string fileName)
        {
            return Ok(_fileSystemService.DeleteTempFiles(fileName));
        }

        // ------------------------

        [HttpPost("temp")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> UploadFiles(IFormFile[] files)
        {
            var result = _fileSystemService.Upload(files);
            return Ok(result);
        }

        // ------------------------

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public ActionResult<DownloadResult> UploadFiles([FromQuery] string filePath)
        {
            var result = _fileSystemService.Download(filePath);
            return Ok(result);
        }
    }
}
