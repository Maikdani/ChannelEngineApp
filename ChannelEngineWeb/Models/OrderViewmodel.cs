using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChannelEngineWeb.Models
{
    public class OrderViewmodel
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int GlobalChannelId { get; set; }
        public string ChannelName { get; set; }
        public string GlobalChannelName { get; set; }
        public string ChannelOrderSupport { get; set; }
        public string ChannelOrderNo { get; set; }
        public string Status { get; set; }
    }
}

