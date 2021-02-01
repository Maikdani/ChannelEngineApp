using ChannelEngineCore.Entities;
using ChannelEngineCore.Interfaces;
using ChannelEngineCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChannelEngineTestProject
{
    public class ProductUnitTest
    {
        // A unit test testing the expected outcome of the “top 5” functionality based on dummy input.
        [Fact]
        public void TopProductsTest()
        {
            IRequestHandler requestHandler = new HttpClientRequestHandler();
            Product product1 = new Product { Name = "A", GTIN = "1233", Quantity = 1 };
            Product product2 = new Product { Name = "B", GTIN = "1234", Quantity = 2 };
            Product product3 = new Product { Name = "C", GTIN = "1235", Quantity = 3 };
            Product product4 = new Product { Name = "D", GTIN = "1236", Quantity = 4 };
            Product product5 = new Product { Name = "E", GTIN = "1237", Quantity = 5 };
            Product product6 = new Product { Name = "F", GTIN = "1238", Quantity = 6 };

            List<Order> orders = new List<Order>
            {
                new Order { Products = new List<Product> { product1, product2 } },
                new Order { Products = new List<Product> { product3, product2 } },
                new Order { Products = new List<Product> { product4, product5, product2 } },
                new Order { Products = new List<Product> { product6, product2 } }
            };

            var products = requestHandler.FilterTopXProductsSold(5, orders).ToList();
            Assert.Equal(5, products.Count);
            Assert.Equal("B", products.ElementAt(0).Name);
            Assert.Equal("F", products.ElementAt(1).Name);
            Assert.Equal("E", products.ElementAt(2).Name);
            Assert.Equal("D", products.ElementAt(3).Name);
            Assert.Equal("C", products.ElementAt(4).Name);
        }
    }
}
