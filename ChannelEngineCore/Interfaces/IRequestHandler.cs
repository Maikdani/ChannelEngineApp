using ChannelEngineCore.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChannelEngineCore.Interfaces
{
    public interface IRequestHandler
    {
        Task<MerchantProduct> GetMerchantProductAsync(string id);
        Task<string> GetOrdersInProgressJSONAsync();
        Task<IEnumerable<Order>> GetOrdersInProgressAsync();
        IEnumerable<Product> FilterTopXProductsSold(int takeCount, IEnumerable<Order> orders);
        Task<HttpResponseMessage> PatchProductAsync(JsonPatchDocument<MerchantProduct> patchDoc, string merchantProductNo);
    }
}
