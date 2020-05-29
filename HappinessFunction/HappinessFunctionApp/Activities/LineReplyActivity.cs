using HappinessFunctionApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace HappinessFunctionApp.Activities
{
	public class LineReplyActivity
	{
		[FunctionName("LineReplyActivity")]
		public static async void LineReply([ActivityTrigger] IDurableActivityContext context, ILogger log)
		{

			var input = context.GetInput<(string ReplyToken, string message)>();

			var res = new LineTextResponseInterface();
			try
			{
				res.ReplyToken = input.ReplyToken;
				res.Messages = new List<TextMessages>()
				{
					new TextMessages
					{
						Type = "text",
						Text = input.message,
					}
				};

				var httpClient = HttpStart.GetInstance();
				await httpClient.PostLineJsonAsync<LineTextResponseInterface>("https://api.line.me/v2/bot/message/reply", res);

			}
			catch (Exception ex)
			{
				log.LogError(ex.Message);
				throw;
			}
		}
	}
}
