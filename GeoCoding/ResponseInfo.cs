using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MapQuest2
{
    public class ResponseInfo
    {
        /// <summary>
        /// Extended copyright info
        /// </summary>
        //[JsonDictionary]
        [JsonProperty("copyright")]
        public IDictionary<string, string> Copyright { get; set; }

        /// <summary>
        /// Maps to HTTP response code generally
        /// </summary>
        [JsonProperty("statuscode")]
        public ResponseStatus Status { get; set; }

        /// <summary>
        /// Error or status messages if applicable
        /// </summary>
        //[JsonArray(AllowNullItems=true)]
        [JsonProperty("messages")]
        public IList<string> Messages { get; set; }
    }
}