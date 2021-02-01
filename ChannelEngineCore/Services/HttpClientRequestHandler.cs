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

        public async Task<MerchantProduct> GetMerchantProductAsync(string id)
        {
            try
            {
                return JsonConvert.DeserializeObject<ChannelEngineRoot<MerchantProduct>>(await _httpClient.GetStringAsync(new Uri($"{RequestConstants.BaseUrl}/products/{id}?apikey={RequestConstants.ApiKey}"))).Content;
            }
            catch (Exception)
            {
                throw;
            }
        } 

        public async Task<string> GetOrdersInProgressJSONAsync()
        {
            try
            {
                return await _httpClient.GetStringAsync(new Uri($"{RequestConstants.BaseUrl}/orders?statuses=IN_PROGRESS&apikey={RequestConstants.ApiKey}"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersInProgressAsync()
        {
            var response = await GetOrdersInProgressJSONAsync();
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

        public async Task<HttpResponseMessage> PatchProductAsync(JsonPatchDocument<MerchantProduct> patchDoc, string merchantProductNo)
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
                var response = await _httpClient.SendAsync(request);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
