using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HappinessFunctionApp.Models
{
	[DataContract]
	public class LineRequestInterface
	{
		[DataMember(Name = "events")]
		public List<Event> Events { get; set; }
	}

	[DataContract]
	public class Event
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[DataMember(Name = "replyToken")]
		public string ReplyToken { get; set; }

		[DataMember(Name = "source")]
		public Source Source { get; set; }

		[DataMember(Name = "timestamp")]
		public long Timestamp { get; set; }

		[DataMember(Name = "mode")]
		public string Mode { get; set; }

		[DataMember(Name = "message")]
		public EventMessage Message { get; set; }

	}

	[DataContract]
	public class Source
	{
		[DataMember(Name = "userId")]
		public string UserId { get; set; }

		[DataMember(Name = "type")]
		public string Type { get; set; }
	}

	[DataContract]
	public class EventMessage
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }


		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "contentProvider")]
		public MessageContentProvider ContentProvider { get; set; }
	}

	[DataContract]
	public class MessageContentProvider
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }

	}
}
