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
        MerchantProduct GetMerchantProduct(string id);
        string GetOrdersInProgressJSON();
        IEnumerable<Order> GetOrdersInProgress();
        IEnumerable<Product> FilterTopXProductsSold(int takeCount, IEnumerable<Order> orders);
        HttpResponseMessage PatchProduct(JsonPatchDocument<MerchantProduct> patchDoc, string merchantProductNo);
    }
}
