﻿using DotVVM.Framework.Hosting;
using DotVVM.Framework.Storage;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.Presenters
{
    public class TempFilePresenter : IDotvvmPresenter
    {
        public async Task ProcessRequest(IDotvvmRequestContext context)
        {
            var fileId = new Guid(context.Parameters["FileId"].ToString());
            var fileExtension = context.Parameters["FileExtension"].ToString();
            var fileStorage = (IUploadedFileStorage)context.Configuration.ServiceProvider.GetService(typeof(IUploadedFileStorage));
            var file = fileStorage.GetFile(fileId);

            using (file)
            {
                context.HttpContext.Response.Headers["Content-Disposition"] = $"attachment; filename=temp.{fileExtension}";
                await file.CopyToAsync(context.HttpContext.Response.Body);
            }
        }
    }
}