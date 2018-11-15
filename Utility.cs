﻿using System;
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
        public async Task EnsureGzipFiles(CloudBlobContainer containerS, string constring)
        {

            var storageAccount = CloudStorageAccount.Parse(constring);

            //segmented and await
            var blobInfos = containerS.ListBlobs("", true, BlobListingDetails.Metadata);
            List<string> names = new List<string>();
             
             foreach(var blob in blobInfos){
                var path = blob.Uri.AbsolutePath.Substring(1, blob.Uri.AbsolutePath.LastIndexOf('/')-1);
                names.Add(path);
             }

            names = names.Distinct().ToList();

            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("gzip");

            foreach(var name in names){
                var message = new CloudQueueMessage(name);
                await queue.AddMessageAsync(message);
            }

        }
    }
}
