using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.IPresenter
{
    interface ICalculatorClient
    {
        string Comment { get; set; }
        Business.TaskComplete TaskWork { get; set; }
        Business.TaskDelegate TaskJob { get; set; }
        string TaskName { get; set; }        
        //Business.Tick Tick { get; set; }
        bool IsActive { get; set; }
    }
}
