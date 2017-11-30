using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrleansDemo.Services.Interfaces;
using OrleansDemo.Models.ViewModels;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Http.Headers;
using System.Net;

namespace OrleansDemo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/File")]
    public class FileController : Controller
    {
        private readonly IDeviceTypeConfiguration deviceType;

        public FileController(IDeviceTypeConfiguration deviceTypeConfiguration)
        {
            deviceType = deviceTypeConfiguration;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetFile(int id)
        {
            DeviceTypeFileViewModel file = await deviceType.GetFileAsync(id);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            using (MemoryStream memoryStream = new MemoryStream(file.Data))
            {
                result.Content = new ByteArrayContent(memoryStream.ToArray());
            }
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(file.MimeType);
            result.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(7)
            };

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> PutFile(int id, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await deviceType.DeviceTypeExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            DeviceTypeFileViewModel model = new DeviceTypeFileViewModel
            {
                DecviceTypeId = id,
                Name = file.FileName,
                Extension = file.ContentType,
                MimeType = file.ContentDisposition,
                FileType = 1
            };

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                model.Data = memoryStream.ToArray();
            }

            await deviceType.SaveImageFileAsync(model);

            return NoContent();
        }
    }
}