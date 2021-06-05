using DotVVM.Core.Storage;
using DotVVM.Framework.Hosting;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.Presenters
{
    public class TempFilePresenter : IDotvvmPresenter
    {
        private readonly IUploadedFileStorage uploadedFileStorage;

        public TempFilePresenter(IUploadedFileStorage uploadedFileStorage)
        {
            this.uploadedFileStorage = uploadedFileStorage;
        }

        public async Task ProcessRequest(IDotvvmRequestContext context)
        {
            var fileId = new Guid(context.Parameters["FileId"].ToString());
            var fileExtension = context.Parameters["FileExtension"].ToString();

            using var file = await uploadedFileStorage.GetFileAsync(fileId);
            context.HttpContext.Response.Headers["Content-Disposition"] = $"attachment; filename=temp.{fileExtension}";
            await file.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
}
