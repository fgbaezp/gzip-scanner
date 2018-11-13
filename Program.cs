using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace gzip
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var options = new Options();
            options.ConnectionStringSource = args[0];
            var storageAccountS = CloudStorageAccount.Parse(options.ConnectionStringSource);
            var blobClientS = storageAccountS.CreateCloudBlobClient();
            var blobContainerS = blobClientS.GetContainerReference(options.Container);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await new Utility().EnsureGzipFiles(blobContainerS);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
           
            Console.WriteLine("RunTime " + ts);
           
        }
    }
}
    