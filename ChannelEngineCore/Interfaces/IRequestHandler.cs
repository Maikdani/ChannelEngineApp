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
        IEnumerable<Order> GetOrdersInProgress();
        IEnumerable<Product> GetTopXProductsSold(int topX);
        HttpResponseMessage PutProduct(JsonPatchDocument<MerchantProduct> patchDoc);
    }
}
