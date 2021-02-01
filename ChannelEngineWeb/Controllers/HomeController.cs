using ChannelEngineCore.Entities;
using ChannelEngineCore.Interfaces;
using ChannelEngineWeb.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChannelEngineWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestHandler _requestHandler;

        public HomeController(
            ILogger<HomeController> logger, 
            IRequestHandler requestHandler)
        {
            _logger = logger;
            this._requestHandler = requestHandler;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var orders = await this._requestHandler.GetOrdersInProgressJSONAsync();
            var channelEngineRoot = JsonConvert.DeserializeObject<ChannelEngineRoot<List<OrderViewmodel>>>(orders);
            return View(channelEngineRoot.Content);
        }

        public async Task<IActionResult> ProductsAsync()
        {
            var orders = await this._requestHandler.GetOrdersInProgressAsync();
            var products = this._requestHandler.FilterTopXProductsSold(5, orders);
            return View(products);
        }

        public async Task<IActionResult> PatchProductAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patchDoc = new JsonPatchDocument<MerchantProduct>();
            patchDoc.Replace(p => p.Stock, 25);
            var response = await _requestHandler.PatchProductAsync(patchDoc, id);

            if (response.IsSuccessStatusCode)
            {
                var product = await _requestHandler.GetMerchantProductAsync(id);
                return View(product);
            }

            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
