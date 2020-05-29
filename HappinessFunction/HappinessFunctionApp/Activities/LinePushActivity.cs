using HappinessFunctionApp.Common;
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
	public class LinePushActivity
	{
		[FunctionName("LinePushActivity")]
		public static async void LinePush([ActivityTrigger] IDurableActivityContext context, ILogger log)
		{
			var input = context.GetInput<(string UserId, string url, string score)>();

			var res = new LineTemplateResponseInterface();
			try
			{
				res.To = input.UserId;
				res.Messages = new List<TemplateMessages>()
				{
					new TemplateMessages
					{
						Type = "template",
						AltText = "写真のハピネス数値",
						Template = new Template()
						{
							Type = "buttons",
							ThumbnailImageUrl = input.url,
							Title = $"{input.score} pt",
							Text = $"この写真のハピネス数値です！",
							Actions = new List<Actions>()
							{
								new Actions
								{
									Type = "uri",
									Label = "ランキングを確認",
									Uri = AppSettings.Instance.LINE_POST_LIST,
								}
							}
						}
					}
				};

				var httpClient = HttpStart.GetInstance();
				await httpClient.PostLineJsonAsync<LineTemplateResponseInterface>("https://api.line.me/v2/bot/message/push", res);

			}
			catch (Exception ex){
				log.LogError(ex.Message);
				throw;
			}

		}
	}
}
