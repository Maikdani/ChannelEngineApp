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

        public IActionResult Index()
        {
            var orders = this._requestHandler.GetOrdersInProgressJSON();
            var channelEngineRoot = JsonConvert.DeserializeObject<ChannelEngineRoot<List<OrderViewmodel>>>(orders);
            return View(channelEngineRoot.Content);
        }

        public IActionResult Products()
        {
            var products = this._requestHandler.GetTopXProductsSold(5);
            return View(products);
        }

        public IActionResult PatchProduct(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patchDoc = new JsonPatchDocument<MerchantProduct>();
            patchDoc.Replace(p => p.Stock, 25);
            var response = _requestHandler.PatchProduct(patchDoc, id);

            if (response.IsSuccessStatusCode)
            {
                var product = _requestHandler.GetMerchantProduct(id);
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
