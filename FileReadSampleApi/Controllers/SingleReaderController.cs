using System.IO;
using System.Threading.Tasks;
using FileReadSampleApi.Services;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FileReadSampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingleReaderController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public SingleReaderController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        /// <summary>
        /// Читает файл и возвращает содержимое.
        /// </summary>
        /// <param name="filename">произвольное имя файла, файл читается и выдается в Reponse</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/{filename}")]
       
        public async Task<IActionResult> Get(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return NotFound();

            string contentDir = Path.Combine(env.ContentRootPath, "content root");

            var bytes = await FileReader.ReadFileAync(contentDir, filename);
            return File(bytes, "text/html", filename);
        }

        /// <summary>
        /// Метрики по чтениям / запросам
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/metrics")]
        public JsonResult metrics()
        {
            return new JsonResult(new { Metrics.FileReads, Metrics.Requests });
        }


    }
}
