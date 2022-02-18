using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GreetingService.Core.Entities;
using GreetingService.Core.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Function
{
    public class ConvertGreetingToCsv
    {
        [FunctionName("ConvertGreetingToCsv")]
        public async Task Run([BlobTrigger("greetings/{name}", Connection = "LoggingStorageAccount")] Stream greetingJsonBlob,
            [Blob("greetings-csv/{name}", FileAccess.Write)] Stream greetingCsvBlob,
            ILogger log)
        {
            StreamReader blobStreamReader = new(greetingJsonBlob);           
            string jsonString = await blobStreamReader.ReadToEndAsync();
                Greeting g = jsonString.ToGreeting();

            StreamWriter streamWriter = new(greetingCsvBlob);            
            streamWriter.WriteLine("id;from;to;message;timestamp");
            streamWriter.WriteLine(g.ToCsvString());

            await streamWriter.FlushAsync();

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{g.Id.ToString()} \n Size: {greetingJsonBlob.Length} Bytes");

        }
    }
}
