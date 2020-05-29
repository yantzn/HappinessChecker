using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Azure.Documents;
using HappinessFunctionApp.Models;
using HappinessFunctionApp.Common;
using HappinessFunctionApp.Extension;

namespace HappinessFunctionApp.Functions
{
    public  class UpdateGoodCntFunction: AbstractDocument
    {
        private static HttpClientService _httpClientService;
        private static DocumentClient _documentClient;

        public UpdateGoodCntFunction(HttpClientService httpClientService,DocumentClient documentClient)
        {
            _httpClientService = httpClientService;
            _documentClient = documentClient;
        }

        [FunctionName("GoodUpdateFunction")]
        public async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/good")] HttpRequest req,
            ILogger log)
        {

            // POSTデータからパラメータを取得
            var jsonContent = await req.ReadAsStringAsync();

            log.LogInformation($"Request:{jsonContent}");

            var feedOptions = new FeedOptions
            {
                MaxItemCount = 1,
                EnableCrossPartitionQuery = true
            };

            try
            {

                dynamic data = JsonConvert.DeserializeObject(jsonContent);
                string image_id = data?.id;

                // バリデーションチェック
                if (string.IsNullOrWhiteSpace(image_id) || !(Regex.IsMatch(image_id, @"^[0-9]{14}$")))
                {
                    log.LogInformation("バリデーションエラー");
                    return;
                }

                var doc = _documentClient.CreateDocumentQuery<DocumentInterface>(
                                                    UriFactory.CreateDocumentCollectionUri(AppSettings.Instance.DATABASE_ID, AppSettings.Instance.COLLECTION_ID), feedOptions)
                                                   .Where(x => x.ImageId == image_id)
                                                   .AsEnumerable()
                                                   .FirstOrDefault();

                if(doc == null)
                {
                    log.LogInformation("更新対象なし");
                    return;
                }

                var requestOptions = new RequestOptions()
                {
                    PartitionKey = new PartitionKey(doc.ImageId),
                    AccessCondition = new AccessCondition()
                    {
                        Condition = doc.Etag,
                        Type = AccessConditionType.IfMatch
                    }
                };

                TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

                var update = new DocumentInterface()
                {
                    Id = doc.Id,
                    ImageId = doc.ImageId,
                    User = doc.User,
                    UserIcon = doc.UserIcon,
                    PictureUrl = doc.PictureUrl,
                    Score = doc.Score,
                    GoodCnt = doc.GoodCnt + 1,
                    CreatedDatatime = doc.CreatedDatatime,
                    UpdatedDatatime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), tst)
                };

                await _documentClient.ReplaceDocumentAsync(
                                UriFactory.CreateDocumentUri(AppSettings.Instance.DATABASE_ID, AppSettings.Instance.COLLECTION_ID, doc.Id), update, requestOptions);

                var imageInfo = _documentClient.CreateDocumentQuery<DocumentInterface>(UriFactory.CreateDocumentCollectionUri(AppSettings.Instance.DATABASE_ID, AppSettings.Instance.COLLECTION_ID), feedOptions)
                                   .OrderByDescending(x => x.Score)
                                   .ToList();

                var rank = new RankResponseInterface();
                rank.Images = new List<Images>();
                var count = 0;
                foreach (var res in imageInfo)
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

                await _httpClientService.PostJsonAsync<RankResponseInterface>(AppSettings.Instance.SIGNALR_URL, rank);

            }
            catch (Exception ex)
            {
                log.LogError($"Error:{ex.Message}");
                log.LogError($"StackTrace:{ex.StackTrace}");
            }
        }
    }
}
