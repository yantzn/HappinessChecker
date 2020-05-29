using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HappinessFunctionApp.Models
{
    [DataContract]
    public abstract class AbstractDocument
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "_etag")]
        public string Etag { get; set; }

        [DataMember(Name = "createdDatatime")]
        public DateTime CreatedDatatime { get; set; }

        [DataMember(Name = "updatedDatatime")]
        public DateTime UpdatedDatatime { get; set; }

    }
}
