using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChannelEngineCore.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Lines")]
        public IList<Product> Products { get; set; }
    }
}


//●	Fetch all orders with status IN_PROGRESS from the API
//●	From these orders, compile a list of the top 5 products sold (product name, GTIN and total quantity), order these by the total quantity sold in descending order.
//●	Pick one of the products from these orders and use the API to set the stock of this product to 25.
