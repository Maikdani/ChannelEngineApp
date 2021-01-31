using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChannelEngineCore.Entities
{
    public class Product
    {
        [JsonProperty(PropertyName = "Description")] 
        public string Name { get; set; }
        public string GTIN { get; set; }
        public int Quantity { get; set; }
    }
}
