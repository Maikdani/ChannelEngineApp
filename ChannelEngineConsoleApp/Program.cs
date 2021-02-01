using ChannelEngineCore.Entities;
using ChannelEngineCore.Interfaces;
using ChannelEngineCore.Services;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChannelEngineConsoleApp
{
    public class Program
    {
        private static readonly IRequestHandler _requestHandler = new HttpClientRequestHandler();
        static async Task Main(string[] args)
        {
            Program program = new Program();
            DisplayStart();

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int selectedOption))
                {
                    Console.WriteLine();
                    switch (selectedOption)
                    {
                        case 1:
                            await DisplayOrdersInProgressAsync();
                            break;
                        case 2:
                            await DisplayTopFiveProductsSoldAsync();
                            break;
                        case 3:
                            await UpdateStockAsync();
                            break;
                        case 4:
                            Console.Clear();
                            DisplayStart();
                            break;
                        default:
                            Console.WriteLine("Not a valid input");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        public static void DisplayStart()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Fetch Orders In Progress");
            Console.WriteLine("2. Get top 5 products sold from Orders In Progress by Quantity sold");
            Console.WriteLine("3. Set stock of product to 25");
            Console.WriteLine("4. Clear console");
        }

        public static async Task DisplayOrdersInProgressAsync()
        {
            var json = await _requestHandler.GetOrdersInProgressJSONAsync();
            string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);
            Console.WriteLine(jsonFormatted);
        }

        public static async Task<IEnumerable<Product>> DisplayTopFiveProductsSoldAsync()
        {
            Console.WriteLine("Retrieving top 5 products sold from Orders In Progress by Quantity sold...");
            var products = _requestHandler.FilterTopXProductsSold(5, await _requestHandler.GetOrdersInProgressAsync());
            int i = 1;
            foreach(var product in products)
            {
                Console.WriteLine($"{i}. Product name: {product.Name} - GTIN: {product.GTIN} - Quantity: {product.Quantity}");
                i++;
            }
            return products;
        }

        public static async Task UpdateStockAsync()
        {
            var patchDoc = new JsonPatchDocument<MerchantProduct>();
            patchDoc.Replace(p => p.Stock, 25);
            Console.WriteLine("Select product...");
            List<Product> products = (await DisplayTopFiveProductsSoldAsync()).ToList();

            if (int.TryParse(Console.ReadLine(), out int selectedOption) && selectedOption > 0 && selectedOption <= products.Count)
            {
                var merchantProductNo = products.ElementAt(selectedOption - 1)?.MerchantProductNo;
                var response = await _requestHandler.PatchProductAsync(patchDoc, merchantProductNo);
                Console.WriteLine(response.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var product = await _requestHandler.GetMerchantProductAsync(merchantProductNo);
                    Console.WriteLine($"Successfully updated stock of Product: {product.MerchantProductNo} - {product.Name} - Stock: {product.Stock}");
                }
            } else
            {
                Console.WriteLine("Invalid input.");
            }            
        }
    }
}
