using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HappinessFunctionApp.Models
{
	[DataContract]
	public class RankResponseInterface
	{
		[DataMember(Name = "images")]
		public List<Images> Images { get; set; }
	}

	[DataContract]
	public class Images
	{
		[DataMember(Name = "image_id")]
		public string ImageId { get; set; }

		[DataMember(Name = "user")]
		public string User { get; set; }

		[DataMember(Name = "user_icon")]
		public string UserIcon { get; set; }

		[DataMember(Name = "picture_Url")]
		public string PictureUrl { get; set; }

		[DataMember(Name = "score")]
		public double Score { get; set; }

		[DataMember(Name = "good_cnt")]
		public int GoodCnt { get; set; }

		[DataMember(Name = "rank")]
		public int Rank { get; set; }

	}
}
