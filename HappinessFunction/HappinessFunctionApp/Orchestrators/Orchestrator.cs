using HappinessFunctionApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HappinessFunctionApp
{
	public static class Orchestrator
	{
		[FunctionName("LineBotOrchestrator")]
		public static async Task RunOrchestrator(
			[OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
		{
			var req = context.GetInput<LineRequestInterface>();
			foreach (var contents in req.Events)
			{
				try
				{
					if (!(contents.Message.Type.Equals("image")))
					{
						await context.CallActivityAsync<LineTextResponseInterface>("LineReplyActivity", (contents.ReplyToken, Constants.LINE_ERROR_MESSAGE));
					}
					else
					{
						await context.CallActivityAsync<LineTextResponseInterface>("LineReplyActivity", (contents.ReplyToken, Constants.LINE_SUCCESS_MESSAGE));
						var url = await context.CallActivityAsync<string>("UploadFileToStorageActivity", contents.Message.Id);
						var score = await context.CallActivityAsync<string>("DetectFaceActivity", url);
						await context.CallActivityAsync<string>("RegisterToCosmosdbActivity", (contents.Source.UserId, url, score, contents.Message.Id));
						await context.CallActivityAsync<LineTemplateResponseInterface>("LinePushActivity", (contents.Source.UserId, url, score));
					}
				}
				catch (FunctionFailedException ex) when (ex.InnerException is HttpRequestException httpRequestException)
				{
					log.LogError(httpRequestException.Message);
					throw;
				}
			}
		}
	}
}