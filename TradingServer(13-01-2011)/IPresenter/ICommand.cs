using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.IPresenter
{
    interface ICommand
    {
        void ExtractCommand(string Cmd, string ipAddress);
        string ExtractCommandServer(string Cmd, string ipAddress, string code);
        List<string> ExtractServerCommand(string Cmd, string ipAddress, string code);
    }
}
