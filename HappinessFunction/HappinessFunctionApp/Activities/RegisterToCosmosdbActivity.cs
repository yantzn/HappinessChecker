using HappinessFunctionApp.Common;
using HappinessFunctionApp.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace HappinessFunctionApp.Activities
{
	public class RegisterToCosmosdbActivity
	{

		[FunctionName("RegisterToCosmosdbActivity")]
		public static async void RegisterToCosmosdb([ActivityTrigger] IDurableActivityContext context, ILogger log)
		{

			var feedOptions = new FeedOptions
			{
				MaxItemCount = 1,
				EnableCrossPartitionQuery = true
			};

			var input = context.GetInput<(string user_id, string url, string score, string image_id)>();

			var httpClient = HttpStart.GetInstance();			
			var userInfo = await httpClient.GetAsync($"https://api.line.me/v2/bot/profile/{input.user_id}");	
			var data = JsonConvert.DeserializeObject<LineUserInfoInterface>(userInfo);

			TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

			var item = new DocumentInterface()
			{
				ImageId = input.image_id,
				User = data.Name,
				UserIcon = data.PictureUrl,
				PictureUrl = input.url.Replace(AppSettings.Instance.BLOB_URL, AppSettings.Instance.PROXY_URL),
				Score = double.Parse(input.score),
				GoodCnt = 0,
				CreatedDatatime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), tst),
				UpdatedDatatime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), tst)
			};

			var ducumentClient = HttpStart.GetDocumentInstance();
			await ducumentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = AppSettings.Instance.DATABASE_ID });

			PartitionKeyDefinition pkDefn = new PartitionKeyDefinition() { Paths = new Collection<string>() { "/image_id" } };
			await ducumentClient.CreateDocumentCollectionIfNotExistsAsync(
							UriFactory.CreateDatabaseUri(AppSettings.Instance.DATABASE_ID), new DocumentCollection { Id = AppSettings.Instance.COLLECTION_ID, PartitionKey = pkDefn },
							new RequestOptions { OfferThroughput = 400, PartitionKey = new PartitionKey("/image_id") });

			await ducumentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(AppSettings.Instance.DATABASE_ID, AppSettings.Instance.COLLECTION_ID), item);

			var documentQuery = ducumentClient.CreateDocumentQuery<DocumentInterface>(UriFactory.CreateDocumentCollectionUri(AppSettings.Instance.DATABASE_ID, AppSettings.Instance.COLLECTION_ID), feedOptions)
								   .OrderByDescending(x => x.Score)
								   .ToList();

			var rank = new RankResponseInterface();
			rank.Images = new List<Images>();
			var count = 0;
			foreach (var res in documentQuery)
			{
				var image = new Images
				{
					ImageId = res.ImageId,
					User = res.User,
					UserIcon = res.UserIcon,
					PictureUrl = res.PictureUrl,
					Score = res.Score,
					GoodCnt = res.GoodCnt,
					Rank = count += 1
				};
				rank.Images.Add(image);
			}

			await httpClient.PostJsonAsync<RankResponseInterface>(AppSettings.Instance.SIGNALR_URL, rank);
		}
	}
}
