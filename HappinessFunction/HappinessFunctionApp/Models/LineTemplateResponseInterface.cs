using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HappinessFunctionApp.Models
{
	[DataContract]
	public class LineTemplateResponseInterface
	{
		[DataMember(Name = "to")]
		public string To { get; set; }

		[DataMember(Name = "messages")]
		public List<TemplateMessages> Messages { get; set; }

	}

	[DataContract]
	public class TemplateMessages
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[DataMember(Name = "altText")]
		public string AltText { get; set; }

		[DataMember(Name = "template")]
		public Template Template { get; set; }

	}

	[DataContract]
	public class Template
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[DataMember(Name = "thumbnailImageUrl")]
		public string ThumbnailImageUrl { get; set; }

		[DataMember(Name = "title")]
		public string Title { get; set; }

		[DataMember(Name = "text")]
		public string Text { get; set; }

		[DataMember(Name = "actions")]
		public List<Actions> Actions { get; set; }

	}

	[DataContract]
	public class Actions
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[DataMember(Name = "label")]
		public string Label { get; set; }

		[DataMember(Name = "uri")]
		public string Uri { get; set; }

	}
}