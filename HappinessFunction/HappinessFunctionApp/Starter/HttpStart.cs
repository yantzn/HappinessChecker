using HappinessFunctionApp.Extension;
using HappinessFunctionApp.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HappinessFunctionApp
{
	public class HttpStart
	{

		private static HttpClientService _httpClientService;
		private static DocumentClient _documentClient;

		public HttpStart(HttpClientService httpClientService, DocumentClient documentClient)
		{
			_httpClientService = httpClientService;
			_documentClient = documentClient;
		}

		public static HttpClientService GetInstance()
		{
			return _httpClientService;
		}

		public static DocumentClient GetDocumentInstance()
		{
			return _documentClient;
		}

		[FunctionName("LineBotHttpStart")]
		public async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req,
			[DurableClient]IDurableOrchestrationClient starter,
			ILogger log)
		{

			var jsonContent = await req.Content.ReadAsStringAsync();
			log.LogInformation($"Request:{jsonContent}");

			var data = JsonConvert.DeserializeObject<LineRequestInterface>(jsonContent);

			if (data.Events[0].ReplyToken.Equals("00000000000000000000000000000000"))
			{
				return req.CreateResponse(HttpStatusCode.OK);
			}


			// Function input comes from the request content.
			string instanceId = await starter.StartNewAsync("LineBotOrchestrator", data);
			log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
			return await starter.WaitForCompletionOrCreateCheckStatusResponseAsync(req, instanceId);
		}
	}
}