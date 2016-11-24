using BL.Facades;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MusicLibrary.WebApi
{
    public class FilesController : ApiController
    {
        public StorageFileFacade StorageFileFacade { get; set; }
       
        [HttpGet]
        [ActionName("download")]
        public HttpResponseMessage DownloadFile(int id)
        {
            var file = StorageFileFacade.GetFile(id);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(Path.Combine(StorageFileFacade.GetUploadPath(), file.FileName), FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.DisplayName
            };

            return result;
        }
    }
}
