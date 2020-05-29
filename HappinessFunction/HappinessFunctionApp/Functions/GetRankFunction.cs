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
using HappinessFunctionApp.Models;
using HappinessFunctionApp.Common;

namespace HappinessFunctionApp.Functions
{
    public  class GetRankFunction
    {
        private static DocumentClient _documentClient;

        public GetRankFunction(DocumentClient documentClient)
        {
            _documentClient = documentClient;
        }

        [FunctionName("GetRankFunction")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/rank")] HttpRequestMessage req,
            ILogger log)
        {
            var feedOptions = new FeedOptions
            {
                MaxItemCount = 1,
                EnableCrossPartitionQuery = true
            };

            try
            {
                var documentQuery = _documentClient.CreateDocumentQuery<DocumentInterface>(UriFactory.CreateDocumentCollectionUri(AppSettings.Instance.DATABASE_ID, AppSettings.Instance.COLLECTION_ID), feedOptions)
                                                   .OrderByDescending(x => x.Score)
                                                   .ToList();

                var rank = new RankResponseInterface();
                rank.Images = new List<Images>();
                var count = 0;
                foreach (var item in documentQuery)
                {
                    var image = new Images
                    {
                        ImageId = item.ImageId,
                        User = item.User,
                        UserIcon = item.UserIcon,
                        PictureUrl = item.PictureUrl,
                        Score = item.Score,
                        GoodCnt = item.GoodCnt,
                        Rank = count += 1
                    };
                    rank.Images.Add(image);
                }

                var res = req.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(JsonConvert.SerializeObject(rank));

                log.LogInformation($"Response:{JsonConvert.SerializeObject(rank)}");
                return res;

            }
            catch (Exception ex)
            {
                log.LogError($"Error:{ex.Message}");
                log.LogError($"StackTrace:{ex.StackTrace}");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
