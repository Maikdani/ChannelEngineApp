using ChannelEngineCore.Entities;
using ChannelEngineCore.Interfaces;
using ChannelEngineCore.Services;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace ChannelEngineConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IRequestHandler requestHandler = new HttpClientRequestHandler();
            requestHandler.GetOrdersInProgress();
            requestHandler.GetTopXProductsSold(5);
            requestHandler.PutProduct(new JsonPatchDocument<MerchantProduct>());
            Console.WriteLine("Hello World!");
        }
    }
}
