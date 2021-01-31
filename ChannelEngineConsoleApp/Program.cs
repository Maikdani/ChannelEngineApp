using ChannelEngineCore.Interfaces;
using ChannelEngineCore.Services;
using System;

namespace ChannelEngineConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IRequestHandler requestHandler = new HttpClientRequestHandler();
            requestHandler.GetOrdersInProgress();
            Console.WriteLine("Hello World!");
        }
    }
}
