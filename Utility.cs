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

namespace gzip
{
    class Utility
    {
        public async Task EnsureGzipFiles(CloudBlobContainer containerS)
        {
            //segmented and await
             var blobInfos = containerS.ListBlobs("", true, BlobListingDetails.Metadata);
             List<string> names = new List<string>();
             
             foreach(var blob in blobInfos){
                var path = blob.Uri.AbsolutePath.Substring(1, blob.Uri.AbsolutePath.LastIndexOf('/'));
                names.Add(path);
             }
        }
    }
}
