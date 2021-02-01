using ChannelEngineCore.Constants;
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
        
        public HttpClientRequestHandler()
        { }

        public MerchantProduct GetMerchantProduct(string id)
        {
            try
            {
                return JsonConvert.DeserializeObject<ChannelEngineRoot<MerchantProduct>>(_httpClient.GetStringAsync(new Uri($"{RequestConstants.BaseUrl}/products/{id}?apikey={RequestConstants.ApiKey}")).Result).Content;
            }
            catch (Exception)
            {
                throw;
            }
        } 

        public string GetOrdersInProgressJSON()
        {
            try
            {
                return _httpClient.GetStringAsync(new Uri($"{RequestConstants.BaseUrl}/orders?statuses=IN_PROGRESS&apikey={RequestConstants.ApiKey}")).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Order> GetOrdersInProgress()
        {
            var response = GetOrdersInProgressJSON();
            var channelEngineRoot = JsonConvert.DeserializeObject<ChannelEngineRoot<List<Order>>>(response);
            return channelEngineRoot.Content;
        }

        public IEnumerable<Product> FilterTopXProductsSold(int takeCount, IEnumerable<Order> orders)
        {
            var products = orders.SelectMany(order => order.Products)
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
            var request = new HttpRequestMessage(method, new Uri($"{RequestConstants.BaseUrl}/products/{merchantProductNo}?apikey={RequestConstants.ApiKey}"))
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
