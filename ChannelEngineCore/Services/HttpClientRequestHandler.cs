using ChannelEngineCore.Entities;
using ChannelEngineCore.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChannelEngineCore.Services
{
    public class HttpClientRequestHandler : IRequestHandler
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public string GetOrdersInProgressJSON()
        {
            try
            {
                return _httpClient.GetStringAsync(new Uri("https://api-dev.channelengine.net/api/v2/orders?statuses=IN_PROGRESS&apikey=541b989ef78ccb1bad630ea5b85c6ebff9ca3322")).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Order> GetOrdersInProgress()
        {
            var response = GetOrdersInProgressJSON();
            var channelEngineRoot = JsonConvert.DeserializeObject<ChannelEngineRoot>(response);
            return channelEngineRoot.Content;
        }

        public IEnumerable<Product> GetTopXProductsSold(int takeCount)
        {
            var products = this.GetOrdersInProgress()
                .SelectMany(order => order.Products)
                .GroupBy(product => product.Name)
                .Select(g => new Product
                {
                    Name = g.Key,
                    GTIN = g.FirstOrDefault()?.GTIN,
                    Quantity = g.Sum(x => x.Quantity),
                    MerchantProductNo = g.FirstOrDefault()?.MerchantProductNo
                })
                .OrderByDescending(x => x.Quantity)
                .Take(takeCount);
            return products;
        }

        public HttpResponseMessage PatchProduct(JsonPatchDocument<MerchantProduct> patchDoc, string merchantProductNo)
        {
            var serializedItemToUpdate = JsonConvert.SerializeObject(patchDoc);

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, new Uri("https://api-dev.channelengine.net/api/v2/products/" + merchantProductNo + "?apikey=541b989ef78ccb1bad630ea5b85c6ebff9ca3322"))
            {
                Content = new StringContent(serializedItemToUpdate,
                Encoding.UTF8, "application/json-patch+json")
            };
            try
            {
                var response = _httpClient.SendAsync(request).Result;
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
