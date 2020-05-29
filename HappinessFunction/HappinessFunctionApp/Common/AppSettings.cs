using Microsoft.Extensions.Configuration;

namespace HappinessFunctionApp.Common
{
	class AppSettings
	{
		private readonly IConfigurationRoot _configuration;

		/// <summary>
		/// 環境変数に設定された項目を取得する
		/// </summary>
		public AppSettings()
		{
			var builder = new ConfigurationBuilder()
							.AddJsonFile("local.settings.json", true)
							.AddEnvironmentVariables();

			_configuration = builder.Build();
		}

		public string FACE_SUBSCRIPTION_KEY => _configuration[nameof(FACE_SUBSCRIPTION_KEY)];

		public string FACE_ENDPOINT => _configuration[nameof(FACE_ENDPOINT)];

		public string STORAGE_ACCOUNT_NAME => _configuration[nameof(STORAGE_ACCOUNT_NAME)];

		public string STORAGE_ACCESS_KEY => _configuration[nameof(STORAGE_ACCESS_KEY)];

		public string BLOB_NAME => _configuration[nameof(BLOB_NAME)];

		public string COSMOSDB_ENDPOINT => _configuration[nameof(COSMOSDB_ENDPOINT)];

		public string COSMOSDB_KEY => _configuration[nameof(COSMOSDB_KEY)];

		public string DATABASE_ID => _configuration[nameof(DATABASE_ID)];

		public string COLLECTION_ID => _configuration[nameof(COLLECTION_ID)];

		public string LINE_CHANNEL_ACCESS_TOKEN => _configuration[nameof(LINE_CHANNEL_ACCESS_TOKEN)];

		public string LINE_CHANNEL_SECRET => _configuration[nameof(LINE_CHANNEL_SECRET)];

		public string LINE_POST_LIST => _configuration[nameof(LINE_POST_LIST)];

		public string BLOB_URL => _configuration[nameof(BLOB_URL)];

		public string PROXY_URL => _configuration[nameof(PROXY_URL)];
		public string SIGNALR_URL => _configuration[nameof(SIGNALR_URL)];

		public static AppSettings Instance { get; } = new AppSettings();
	}
}
