using ChannelEngineCore.Entities;
using ChannelEngineCore.Interfaces;
using ChannelEngineCore.Services;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChannelEngineConsoleApp
{
    public class Program
    {
        private static readonly IRequestHandler _requestHandler = new HttpClientRequestHandler();
        static void Main(string[] args)
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
                            DisplayOrdersInProgress();
                            break;
                        case 2:
                            DisplayTopFiveProductsSold();
                            break;
                        case 3:
                            UpdateStock();
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

        public static void DisplayOrdersInProgress()
        {
            var json = _requestHandler.GetOrdersInProgressJSON();
            string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);
            Console.WriteLine(jsonFormatted);
        }

        public static IEnumerable<Product> DisplayTopFiveProductsSold()
        {
            Console.WriteLine("Retrieving top 5 products sold from Orders In Progress by Quantity sold...");
            var products = _requestHandler.FilterTopXProductsSold(5, _requestHandler.GetOrdersInProgress());
            int i = 1;
            foreach(var product in products)
            {
                Console.WriteLine($"{i}. Product name: {product.Name} - GTIN: {product.GTIN} - Quantity: {product.Quantity}");
                i++;
            }
            return products;
        }

        public static void UpdateStock()
        {
            var patchDoc = new JsonPatchDocument<MerchantProduct>();
            patchDoc.Replace(p => p.Stock, 25);
            Console.WriteLine("Select product...");
            List<Product> products = DisplayTopFiveProductsSold().ToList();

            if (int.TryParse(Console.ReadLine(), out int selectedOption) && selectedOption > 0 && selectedOption <= products.Count)
            {
                var merchantProductNo = products.ElementAt(selectedOption - 1)?.MerchantProductNo;
                var response = _requestHandler.PatchProduct(patchDoc, merchantProductNo);
                Console.WriteLine(response.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var product = _requestHandler.GetMerchantProduct(merchantProductNo);
                    Console.WriteLine($"Successfully updated stock of Product: {product.MerchantProductNo} - {product.Name} - Stock: {product.Stock}");
                }
            } else
            {
                Console.WriteLine("Invalid input.");
            }            
        }
    }
}
