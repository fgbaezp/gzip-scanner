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
        public async Task EnsureGzipFiles(CloudBlobContainer containerS, CloudBlobContainer containerD)
        {
            var blobInfo = containerS.GetBlockBlobReference("3c9c75741ecc4a75a7303fa4bc96f5d9_0000000000.json");
            var blobInfo2 = containerS.GetBlockBlobReference("88c9682d426548388fc1bf2b60c7d599_0000000000.json");
            var blobInfo3 = containerS.GetBlockBlobReference("afda4a126fb94c2a976f9543473caa1e_0000000000.json");
            
            
            for(var i = 0;i<5000;i++){
                await Upload(blobInfo, containerD, $"2018/april/{RandomString(random.Next(1,5))}/{RandomString(random.Next(4,8))}");
                Console.WriteLine("Doc: " + i);
            }

           
            for(var i = 0;i<5000;i++){
                await Upload(blobInfo2, containerD, $"2018/may/{RandomString(random.Next(1,5))}/{RandomString(random.Next(4,8))}");
                Console.WriteLine("Doc: " + i);
            }

            
            for(var i = 0;i<5000;i++){
                await Upload(blobInfo3, containerD, $"2018/march/{RandomString(random.Next(1,5))}/{RandomString(random.Next(4,8))}");
                Console.WriteLine("Doc: " + i);
            }

            for(var i = 0;i<5000;i++){
                await Upload(blobInfo, containerD, $"2017/april/{RandomString(random.Next(1,5))}/{RandomString(random.Next(4,8))}");
                Console.WriteLine("Doc: " + i);
            }

           
            for(var i = 0;i<5000;i++){
                await Upload(blobInfo2, containerD, $"2017/may/{RandomString(random.Next(1,5))}/{RandomString(random.Next(4,8))}");
                Console.WriteLine("Doc: " + i);
            }

            
            for(var i = 0;i<5000;i++){
                await Upload(blobInfo3, containerD, $"2017/march/{RandomString(random.Next(1,5))}/{RandomString(random.Next(4,8))}");
                Console.WriteLine("Doc: " + i);
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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
