using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Queue;

namespace gzip
{
    class Utility
    {
		private string DestinationConnectionString { get; }
		private readonly string queueName = "gzip";
		private readonly IActor someone;

		public Utility(IActor someone)
		{
			someone = someone ?? throw new ArgumentNullException(nameof(someone));
		}

        public async Task EnsureGzipFiles(CloudBlobContainer containerS, string constring)
        {
            //segmented and await
            var blobInfos = containerS.ListBlobs("", true, BlobListingDetails.Metadata);
            List<string> names = new List<string>();
             
             foreach(var blob in blobInfos){
                var path = blob.Uri.AbsolutePath.Substring(1, blob.Uri.AbsolutePath.LastIndexOf('/')-1);
                names.Add(path);
             }

            names = names.Distinct().ToList();

			foreach(var name in names){
				await someone.Act(name);
            }
        }
    }
}
