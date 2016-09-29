using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MDHandbookAppService.Common.Models.RequestMessages
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GetUpdatesMessage
    {
        [JsonProperty]
        public List<string> BookItemIds { get; set; }

        [JsonProperty]
        public List<string> FullpageItemIds { get; set; }
    }
}
