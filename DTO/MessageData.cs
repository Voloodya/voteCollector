using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLmessanger.Models
{
    public class MessageData
    {
        [JsonProperty("urlFrom")]
        public string UrlFrom { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("userFirstName")]
        public string UserFirstName { get; set; }
        [JsonProperty("userLastName")]
        public string UserLastName { get; set; }
        [JsonProperty("idUser")]
        public long IdUser { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("textMessage")]
        public string TextMessage { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

    }
}
