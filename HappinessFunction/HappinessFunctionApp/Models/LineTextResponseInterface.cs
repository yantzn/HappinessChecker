using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HappinessFunctionApp.Models
{
	[DataContract]
	public class LineTextResponseInterface
	{
		[DataMember(Name = "replyToken")]
		public string ReplyToken { get; set; }

		[DataMember(Name = "messages")]
		public List<TextMessages> Messages { get; set; }

	}

	[DataContract]
	public class TextMessages
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[DataMember(Name = "text")]
		public string Text { get; set; }
	}
}