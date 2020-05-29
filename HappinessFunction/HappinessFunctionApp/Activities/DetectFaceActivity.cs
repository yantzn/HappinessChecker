using HappinessFunctionApp.Common;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappinessFunctionApp.Activities
{
	public class DetectFaceActivity
	{
		[FunctionName("DetectFaceActivity")]
		public static async Task<string> DetectFace([ActivityTrigger] string url, ILogger log)
		{

			// FaseAPIの認証を行う
			IFaceClient client = Authenticate(AppSettings.Instance.FACE_ENDPOINT, AppSettings.Instance.FACE_SUBSCRIPTION_KEY);

			try
			{
				// 感情分析を行う
				var detectedFaces = await DetectFaceExtract(client, url, RecognitionModel.Recognition01);

				double sum = 0;

				// 感情分析結果が1件も無い場合、0ptで返却する
				if (detectedFaces.Count == 0)
				{
					return "00.000";
				}

				// Happiness数の加算して合計を算出
				foreach (var item in detectedFaces)
				{
					sum += item.FaceAttributes.Emotion.Happiness;
				}

				sum = (sum * 100000) / 1000;

				return sum.ToString("00.000");

			}
			catch (Exception ex)
			{
				log.LogError(ex.Message);
				throw;
			}

		}

		/// <summary>
		/// Faceサービスのインスタンス生成処理
		/// </summary>
		/// <param name="endpoint">FaceサービスのURL</param>
		/// <param name="key">Faseサービスの認証キー</param>
		/// <returns></returns>
		private static IFaceClient Authenticate(string endpoint, string key)
		{
			return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
		}

		/// <summary>
		/// 感情分析処理
		/// </summary>
		/// <param name="client">Faseサービスインスタンス</param>
		/// <param name="url">画像のURL</param>
		/// <param name="recognitionModel">検出モデル</param>
		/// <returns></returns>
		private static async Task<IList<DetectedFace>> DetectFaceExtract(IFaceClient client, string url, string recognitionModel)
		{
			IList<DetectedFace> detectedFaces = await client.Face.DetectWithUrlAsync($"{url}",
					returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Emotion },
					recognitionModel: recognitionModel);

			return detectedFaces;
		}

	}
}
