using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("OK")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("InternalError")]
        public async Task<IActionResult> Get2()
        {
            var fileName = "Excel2Blob.xlsx";
            var path = Environment.CurrentDirectory;
            var localFile = Path.Combine(path, fileName);


            ////Creating the conection to the blob Storage
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=backupstorage4invoices;AccountKey=Dre2LxY5rPiLrZmOY9nU0cbjbTYOdXNH4mHnDQatw2WYmM7oOUMm8kU6+ola/KUXxQEtFO9aI9IFlULk+XgmRw==;EndpointSuffix=core.windows.net";
            string containerName = "excelinvoices";
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            //Uploading the file to the blob
            using FileStream uploadFileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            return Ok();
        }


        [HttpGet("BadRequest")]
        public IActionResult post()
        {
            return BadRequest();

        }
    }
}
