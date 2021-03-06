﻿using HappinessFunctionApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HappinessFunctionApp.Extension
{
	public class HttpClientService
	{
		private readonly HttpClient _client;

		public HttpClientService(HttpClient client)
		{
			_client = client;
		}
		public async Task<HttpResponseMessage> PostJsonAsync<T>(string requestUri, T value)
		{
			_client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
			return await _client.PostAsJsonAsync(requestUri, value, CancellationToken.None).ConfigureAwait(false);
		}

		public async Task<HttpResponseMessage> PostLineJsonAsync<T>(string requestUri, T value)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.Instance.LINE_CHANNEL_ACCESS_TOKEN);
			_client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
			return await _client.PostAsJsonAsync(requestUri, value, CancellationToken.None).ConfigureAwait(false);
		}

		public async Task<Stream> GetStreamAsync(string requestUri)
		{
			var response = await _client.GetAsync(requestUri);
			return response.Content.ReadAsStreamAsync().Result;
		}

		public async Task<string> GetAsync(string requestUri)
		{
			var response = await _client.GetAsync(requestUri);
			return response.Content.ReadAsStringAsync().Result;
		}
	}
}
