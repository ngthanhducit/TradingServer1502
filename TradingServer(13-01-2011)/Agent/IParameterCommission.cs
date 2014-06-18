using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Agent
{
    public class IParameterCommission
    {
        public int IParameterID { get; set; }
        public int FirstParameterID { get; set; }
        public int SecondParameterID { get; set; }
        public int GroupID { get; set; }
        public int SymbolID { get; set; }
        public double Comission { get; set; }
        public bool IsDelete { get; set; }
        public double ParentCommission { get; set; }
        public double ChildCommission { get; set; }
        public double ParentPipReBate { get; set; }
        public double ChildPipReBate { get; set; }
    }
}
