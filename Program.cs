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

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach(var container in blobClientS.ListContainers()){
                var blobContainerS = blobClientS.GetContainerReference(container.Name);
                await new Utility().EnsureGzipFiles(blobContainerS, options.ConnectionStringSource);
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
           
            Console.WriteLine("RunTime " + ts);
           
        }
    }
}
    