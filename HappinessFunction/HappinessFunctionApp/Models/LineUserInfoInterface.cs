using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HappinessFunctionApp.Models
{
	[DataContract]
	public class LineUserInfoInterface
	{
		[DataMember(Name = "userId")]
		public string Id { get; set; }

		[DataMember(Name = "displayName")]
		public string Name { get; set; }

		[DataMember(Name = "pictureUrl")]
		public string PictureUrl { get; set; }
	}
}
