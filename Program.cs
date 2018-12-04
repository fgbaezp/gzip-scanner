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
			// From where am I going to retrieve the paths
            options.ConnectionStringSource = args[0];
			// Where am I going to enqueue the paths
			options.ConnectionStringDestination = args[1];

			var containerSourceName = args[2];
            var storageAccountS = CloudStorageAccount.Parse(options.ConnectionStringSource);
			var storageAccountDestination = CloudStorageAccount.Parse(options.ConnectionStringDestination);

            var blobClientS = storageAccountS.CreateCloudBlobClient();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

			var util = new Utility(new GzipEnqueuer(storageAccountDestination));

			if (string.IsNullOrEmpty(containerSourceName))
			{
				foreach (var container in blobClientS.ListContainers())
				{
					var blobContainerS = blobClientS.GetContainerReference(container.Name);
					await util.EnsureGzipFiles(blobContainerS, options.ConnectionStringDestination);
				}
			}
            else
			{
				var container = blobClientS.GetContainerReference(containerSourceName);
				await util.EnsureGzipFiles(container, options.ConnectionStringDestination);
			}

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
           
            Console.WriteLine("RunTime " + ts);
        }
    }
}
    