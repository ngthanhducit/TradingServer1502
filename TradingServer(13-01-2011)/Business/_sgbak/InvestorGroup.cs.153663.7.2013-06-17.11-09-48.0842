using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class InvestorGroup
    {
        #region Create Instance Class DBWInvestorGroup
        private static DBW.DBWInvestorGroup dbwInvestorGroup;
        private static DBW.DBWInvestorGroup DBWInvestorGroupInstance
        {
            get
            {
                if (InvestorGroup.dbwInvestorGroup == null)
                {
                    InvestorGroup.dbwInvestorGroup = new DBW.DBWInvestorGroup();
                }

                return InvestorGroup.dbwInvestorGroup;
            }
        }
        #endregion

        #region Create Instance Class DBWIGroupSymbol
        private static TradingServer.DBW.DBWIGroupSymbol dbwIGroupSymbol;
        private static TradingServer.DBW.DBWIGroupSymbol DBWIGroupSymbolInstance
        { 
            get
            {
                if (InvestorGroup.dbwIGroupSymbol == null)
                {
                    InvestorGroup.dbwIGroupSymbol = new DBW.DBWIGroupSymbol();
                }

                return InvestorGroup.dbwIGroupSymbol;
            }
        
        }
        #endregion

        public int InvestorGroupID { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public double DefautDeposite { get; set; }
        public List<Business.ParameterItem> ParameterItems { get; set; }

        #region Property Investor Group Setting Default
        public string FreeMargin { get; set; }
        public double MarginCall { get; set; }
        public double MarginStopOut { get; set; }
        public string InMargin { get; set; }
        public bool IsEnable { get; set; }
        public bool IsManualStopOut { get; set; }
        #endregion
    }
}
