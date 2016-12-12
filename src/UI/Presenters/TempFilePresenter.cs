using DotVVM.Framework.Hosting;
using DotVVM.Framework.Storage;
using System;
using System.Threading.Tasks;

namespace MusicLibrary.Presenters
{
    public class TempFilePresenter : IDotvvmPresenter
    {
        public async Task ProcessRequest(DotvvmRequestContext context)
        {
            var fileId = new Guid(context.Parameters["FileId"].ToString());
            var fileExtension = context.Parameters["FileExtension"].ToString();
            var fileStorage = context.Configuration.ServiceLocator.GetService<IUploadedFileStorage>();
            var file = fileStorage.GetFile(fileId);

            using (file)
            {
                context.OwinContext.Response.Headers["Content-Disposition"] = $"attachment; filename=temp.{fileExtension}";
                await file.CopyToAsync(context.OwinContext.Response.Body);
            }
        }
    }
}
