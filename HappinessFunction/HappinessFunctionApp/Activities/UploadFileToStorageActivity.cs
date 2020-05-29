using HappinessFunctionApp.Common;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HappinessFunctionApp.Activities
{
	public class UploadFileToStorageActivity
	{
		[FunctionName("UploadFileToStorageActivity")]
		public static async Task<string> UploadFileToStorage([ActivityTrigger] string id, ILogger log)
		{

			// Blob取得オプション
			BlobRequestOptions options = new BlobRequestOptions
			{
				ParallelOperationThreadCount = 8,      // 同時にアップロードできるブロックの数
				DisableContentMD5Validation = true,    // BLOBのダウンロード時にMD5検証を無効にする
				StoreBlobContentMD5 = false            // アップロードするときにMD5ハッシュを計算して保存しない
			};

			try
			{

				var credential = new StorageCredentials(AppSettings.Instance.STORAGE_ACCOUNT_NAME, AppSettings.Instance.STORAGE_ACCESS_KEY);
				CloudStorageAccount storageAccount = new CloudStorageAccount(credential, true);

				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
				CloudBlobContainer container = blobClient.GetContainerReference(AppSettings.Instance.BLOB_NAME);

				await container.CreateIfNotExistsAsync();
				await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

				var filename = id + ".jpg";

				CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
				
				var client = HttpStart.GetInstance();
				var stream = await client.GetStreamAsync($"https://api.line.me/v2/bot/message/{id}/content");

				await blockBlob.UploadFromStreamAsync(stream, null, options, null);
				
				var blobUrl = blockBlob.Uri.AbsoluteUri;
				
				return blobUrl;
				
			}
			catch (Exception ex)
			{
				log.LogError(ex.Message);
				throw;
			}

		}

	}
}
