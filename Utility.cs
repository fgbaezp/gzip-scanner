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
          
             var blobInfos = containerS.ListBlobs("", true, BlobListingDetails.Metadata);
             List<string> names = new List<string>();
             
             foreach(var blob in blobInfos){
                names.Add(blob.Uri.AbsolutePath);
             }
        }

        public async Task Upload(IListBlobItem blobInfo, CloudBlobContainer containerD, string prefix){
            
                var blob = (CloudBlob)blobInfo; 
                containerD.CreateIfNotExistsAsync().Wait();
                var destinationBlob = containerD.GetBlockBlobReference(prefix+"/"+Guid.NewGuid()+".json");
            
                // Upload the compressed bytes to the new blob
               
                var x = await blob.OpenReadAsync();
                await destinationBlob.UploadFromStreamAsync(x);
                            
                // Set the blob headers
                destinationBlob.Properties.ContentType = blob.Properties.ContentType;
                destinationBlob.SetProperties();
                
        }



    }
}
