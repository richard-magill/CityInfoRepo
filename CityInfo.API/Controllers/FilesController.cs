using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new ArgumentException(
                    nameof(FileExtensionContentTypeProvider));
        }

        [HttpGet]
        public ActionResult GetFile()
        {
            var filePath = "C:\\LocalGitRepo\\CityInfo\\CityInfo.API\\How-To-Cook-Indian-Food-Over-150-Recipes-for-Curry-More.pdf";
            if (System.IO.File.Exists(filePath))
            {
                var bytes = System.IO.File.ReadAllBytes(filePath);
                if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                return File(bytes, contentType, Path.GetFileName(filePath));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
