using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace gzip
{
	public class GzipBlober : IActor
	{
		private readonly CloudStorageAccount account;
		private readonly string blobName = "gzip";

		public GzipBlober(CloudStorageAccount account)
		{
			this.account = account ?? throw new ArgumentNullException(nameof(account));
			var blobClient = account.CreateCloudBlobClient();
			var blobContainer = blobClient.GetContainerReference(blobName);
			blobContainer.CreateIfNotExists();
		}

		public async Task Act(string content)
		{
			var blobName = String.Copy(content);
			Regex.Replace(blobName, @"\s+", "");
			var blobClient = account.CreateCloudBlobClient();
			var blobContainer = blobClient.GetContainerReference(blobName);
			CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(blobName);
			await cloudBlockBlob.UploadTextAsync(content);
		}
	}
}
