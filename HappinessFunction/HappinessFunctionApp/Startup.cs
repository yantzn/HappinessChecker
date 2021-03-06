﻿using HappinessFunctionApp.Common;
using HappinessFunctionApp.Extension;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(HappinessFunctionApp.Startup))]
namespace HappinessFunctionApp
{
	public class Startup : FunctionsStartup
	{
		// 依存関係
		public override void Configure(IFunctionsHostBuilder builder)
		{
			// IHttpClientFactory を使用する
			builder.Services.AddHttpClient<HttpClientService>();
			builder.Services.AddSingleton(provider =>
			{
				ConnectionPolicy ConnectionPolicy = new ConnectionPolicy
				{
					ConnectionMode = ConnectionMode.Direct,
					ConnectionProtocol = Protocol.Tcp
				};

				return new DocumentClient(new Uri(AppSettings.Instance.COSMOSDB_ENDPOINT), AppSettings.Instance.COSMOSDB_KEY, ConnectionPolicy);
			});
		}
	}
}
