using ChannelEngineCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChannelEngineCore.Interfaces
{
    public interface IRequestHandler
    {
        List<Order> GetOrdersInProgress();
    }
}
