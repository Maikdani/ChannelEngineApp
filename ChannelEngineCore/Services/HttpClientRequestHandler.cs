using ChannelEngineCore.Entities;
using ChannelEngineCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ChannelEngineCore.Services
{
    public class HttpClientRequestHandler : IRequestHandler
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public List<Order> GetOrdersInProgress()
        {
            try
            {
                var response = _httpClient.GetStringAsync(new Uri("https://api-dev.channelengine.net/api/v2/orders?statuses=IN_PROGRESS&apikey=541b989ef78ccb1bad630ea5b85c6ebff9ca3322")).Result;
                var channelEngineRoot = JsonConvert.DeserializeObject<ChannelEngineRoot>(response);
                return channelEngineRoot.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
