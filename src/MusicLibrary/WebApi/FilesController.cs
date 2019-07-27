﻿using BusinessLayer.Facades;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MusicLibrary.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly StorageFileFacade storageFileFacade;

        public FilesController(StorageFileFacade storageFileFacade)
        {
            this.storageFileFacade = storageFileFacade;
        }

        [HttpGet("{id}")]
        public FileStreamResult DownloadFile(int id)
        {
            var file = storageFileFacade.GetFile(id);
            var stream = new FileStream(Path.Combine(storageFileFacade.GetUploadPath(), file.FileName), FileMode.Open);

            return File(stream, "application/octet-stream", file.DisplayName);
        }
    }
}
