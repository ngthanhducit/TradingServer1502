using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{
    public partial class Market
    {
        #region FIND COMMAND IN THREE COMMAND LIST
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandID"></param>
        /// <returns></returns>
        internal bool ProcessCheckCommandIDInListCommand(int commandID, string symbolName, int investorID)
        {
            bool result = false;
            bool resultExecutor = false;
            bool resultSymbol = false;
            bool resultInvestor = false;

            if (commandID < 0 || string.IsNullOrEmpty(symbolName) || investorID < 0)
            {
                TradingServer.Facade.FacadeAddNewSystemLog(1, "prameter invalid", "Process Check Three Command", "", "");
                return false;
            }

            int countDB = TradingServer.Facade.FacadeCountOnlineCommand();
            int countExecutor = TradingServer.Facade.FacadeCountCommandExecutor();
            int countSymbol = TradingServer.Facade.FacadeCountCommandInSymbol();
            int countInvestor = TradingServer.Facade.FacadeCountCommandInInvestor();            

            if (countExecutor == countSymbol && countExecutor == countInvestor && countExecutor == countDB)
                return result = true;
            else
            {   
                #region CHECK COMMAND IN COMMAND EXECUTOR
                if (countExecutor != countDB && countExecutor < countDB)
                {
                    TradingServer.Facade.FacadeAddNewSystemLog(1, "command executor: " + countExecutor + " command DB: " + countDB, "Process Check Three Command", "", "");

                    #region PROCESS COMMAND EXECUTOR
                    if (Business.Market.CommandExecutor != null && Business.Market.CommandExecutor.Count > 0)
                    {
                        for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
                        {
                            if (Business.Market.CommandExecutor[i] != null)
                            {
                                if (Business.Market.CommandExecutor[i].ID == commandID)
                                {
                                    resultExecutor = true;
                                    break;
                                }
                            }
                            else
                            {
                                TradingServer.Facade.FacadeAddNewSystemLog(1, "command in executor list is null", "Process Check Three Command", "", "");
                                Business.Market.CommandExecutor.Remove(Business.Market.CommandExecutor[i]);
                                i--;
                            }
                        }
                    }
                    #endregion

                    if (!resultExecutor)
                    {
                        Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(commandID);
                        Business.Market.CommandExecutor.Add(newOpenTrade);
                    }
                    else
                    {
                        if (countSymbol == countDB || countInvestor == countDB)
                        {
                            if (countSymbol == countDB)
                            {
                                this.ProcessCheckCommandIDInListCommand(1, 0);
                            }
                            else
                            {
                                this.ProcessCheckCommandIDInListCommand(3, 0);
                            }
                        }
                        else
                        {
                            //COMMING SOON......
                        }
                    }
                }
                #endregion                

                #region CHECK COMMAND IN INVESTOR LIST
                if (countInvestor != countDB && countInvestor < countDB)
                {
                    TradingServer.Facade.FacadeAddNewSystemLog(1, "command investor: " + countInvestor + " command DB: " + countDB, "Process Check Three Command", "", "");

                    #region PROCESS IN SYMBOL LIST
                    if (Business.Market.SymbolList != null && Business.Market.SymbolList.Count > 0)
                    {
                        for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                        {
                            if (Business.Market.SymbolList[i].Name.Trim() == symbolName.Trim())
                            {
                                if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                                {
                                    for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                                    {
                                        if (Business.Market.SymbolList[i].CommandList[j] != null)
                                        {
                                            if (Business.Market.SymbolList[i].CommandList[j].ID == commandID)
                                            {
                                                resultSymbol = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            TradingServer.Facade.FacadeAddNewSystemLog(1, "command in symbol list is null", "Process Check Three Command", "", "");
                                            Business.Market.SymbolList[i].CommandList.Remove(Business.Market.SymbolList[i].CommandList[j]);
                                            j--;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                    #endregion

                    if (!resultInvestor)
                    {
                        #region ADD NEW COMMAND TO INVESTOR LIST
                        if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
                        {
                            Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(commandID);
                            for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                            {
                                if (Business.Market.InvestorList[i].InvestorID == investorID)
                                {
                                    if (Business.Market.InvestorList[i].CommandList != null)
                                    {
                                        Business.Market.InvestorList[i].CommandList.Add(newOpenTrade);
                                    }
                                    else
                                    {
                                        Business.Market.InvestorList[i].CommandList = new List<OpenTrade>();
                                        Business.Market.InvestorList[i].CommandList.Add(newOpenTrade);
                                    }

                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (countExecutor == countDB || countSymbol == countDB)
                        {
                            if (countExecutor == countDB)
                            {
                                this.ProcessCheckCommandIDInListCommand(0, 2);
                            }
                            else
                            {
                                this.ProcessCheckCommandIDInListCommand(1, 2);
                            }
                        }
                        else
                        {
                            //COMMING SOON.......
                        }
                    }
                }
                #endregion

                #region CHECK COMMAND IN SYMBOL LIST
                if (countSymbol != countDB && countSymbol < countDB)
                {
                    TradingServer.Facade.FacadeAddNewSystemLog(1, "command symbol: " + countSymbol + " command DB: " + countDB, "Process Check Three Command", "", "");

                    #region PROCESS IN INVESTOR LIST
                    if (Business.Market.InvestorList != null && Business.Market.InvestorList.Count > 0)
                    {
                        for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                        {
                            if (Business.Market.InvestorList[i].InvestorID == investorID)
                            {
                                if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                {
                                    for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                    {
                                        if (Business.Market.InvestorList[i].CommandList[j] != null)
                                        {
                                            if (Business.Market.InvestorList[i].CommandList[j].ID == commandID)
                                            {
                                                resultInvestor = true;

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Business.Market.InvestorList[i].CommandList.Remove(Business.Market.InvestorList[i].CommandList[j]);
                                            TradingServer.Facade.FacadeAddNewSystemLog(1, "command in investor list is null", "Process Check Three Command", "", "");
                                            j--;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                    #endregion

                    if (!resultSymbol)
                    {
                        #region ADD NEW COMMAND TO SYMBOL LIST
                        Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(commandID);
                        if (Business.Market.SymbolList != null && Business.Market.SymbolList.Count > 0)
                        {
                            for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                            {
                                if (Business.Market.SymbolList[i].Name == symbolName)
                                {
                                    if (Business.Market.SymbolList[i].CommandList != null)
                                    {
                                        Business.Market.SymbolList[i].CommandList.Add(newOpenTrade);
                                    }
                                    else
                                    {
                                        Business.Market.SymbolList[i].CommandList = new List<OpenTrade>();
                                        Business.Market.SymbolList[i].CommandList.Add(newOpenTrade);
                                    }

                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (countExecutor == countDB || countInvestor == countDB)
                        {
                            if (countExecutor == countDB)
                            {
                                this.ProcessCheckCommandIDInListCommand(0, 1);
                            }
                            else
                            {
                                this.ProcessCheckCommandIDInListCommand(2, 1);
                            }
                        }
                        else
                        {
                            //COMMING SOON.....
                        }
                    }
                }
                #endregion               
            }

            return true;
        }

        /// <summary>
        /// 0: CommandExecutor | 1: Command In Symbol | 2: Command In Investor
        /// </summary>
        /// <param name="sourceCompare"></param>
        /// <param name="sourceUpdate"></param>
        /// 
        private void ProcessCheckCommandIDInListCommand(int sourceCompare, int sourceUpdate)
        {
            switch (sourceCompare)
            {
                case 0:
                    {
                        switch (sourceUpdate)
                        {
                            case 1:
                                {
                                    #region COMPARE COMMAND EXECUTOR VS SYMBOL LIST
                                    if (Business.Market.CommandExecutor != null)
                                    {
                                        for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
                                        {
                                            bool flag = false;
                                            if (Business.Market.CommandExecutor[i] != null)
                                            {
                                                #region SEARCH COMMAND IN SYMBOL LIST
                                                if (Business.Market.SymbolList != null)
                                                {
                                                    for (int j = 0; j < Business.Market.SymbolList.Count; j++)
                                                    {
                                                        if (Business.Market.SymbolList[j].Name == Business.Market.CommandExecutor[i].Symbol.Name)
                                                        {
                                                            if (Business.Market.SymbolList[j].CommandList != null)
                                                            {
                                                                for (int n = 0; n < Business.Market.SymbolList[j].CommandList.Count; n++)
                                                                {
                                                                    if (Business.Market.SymbolList[j].CommandList[n] != null)
                                                                    {
                                                                        if (Business.Market.SymbolList[j].CommandList[n].ID == Business.Market.CommandExecutor[i].ID)
                                                                        {
                                                                            flag = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion                                                

                                                #region ADD NEW COMMAND TO SYMBOL LIST
                                                if (!flag)
                                                {
                                                    Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(Business.Market.CommandExecutor[i].ID);
                                                    for (int j = 0; j < Business.Market.SymbolList.Count; j++)
                                                    {
                                                        if (Business.Market.SymbolList[j].Name == newOpenTrade.Symbol.Name)
                                                        {
                                                            if (Business.Market.SymbolList[j].CommandList != null)
                                                            {
                                                                Business.Market.SymbolList[j].CommandList.Add(newOpenTrade);
                                                            }
                                                            else
                                                            {
                                                                Business.Market.SymbolList[j].CommandList = new List<OpenTrade>();
                                                                Business.Market.SymbolList[j].CommandList.Add(newOpenTrade);
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion                                                
                                            }
                                        }
                                    }
                                    #endregion                                    
                                }
                                break;

                            case 2:
                                {
                                    #region COMPARE COMMAND EXECUTOR VS INVESTOR LIST
                                    if (Business.Market.CommandExecutor != null)
                                    {
                                        for (int i = 0; i < Business.Market.CommandExecutor.Count; i++)
                                        {
                                            bool flag = false;
                                            if (Business.Market.CommandExecutor[i] != null)
                                            {
                                                #region SEARCH COMMAND IN INVESTOR LIST
                                                if (Business.Market.InvestorList != null)
                                                {
                                                    for (int j = 0; j < Business.Market.InvestorList.Count; j++)
                                                    {
                                                        if (Business.Market.InvestorList[j].InvestorID == Business.Market.CommandExecutor[i].Investor.InvestorID)
                                                        {
                                                            if (Business.Market.InvestorList[j].CommandList != null)
                                                            {
                                                                for (int n = 0; n < Business.Market.InvestorList[j].CommandList.Count; n++)
                                                                {
                                                                    if (Business.Market.InvestorList[j].CommandList[n] != null)
                                                                    {
                                                                        if (Business.Market.InvestorList[j].CommandList[n].ID == Business.Market.CommandExecutor[i].ID)
                                                                        {
                                                                            flag = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }
                                                #endregion                                                

                                                #region ADD NEW COMMAND TO INVESTOR LIST
                                                if (!flag)
                                                {
                                                    Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(Business.Market.CommandExecutor[i].ID);
                                                    for (int j = 0; j < Business.Market.InvestorList.Count; j++)
                                                    {
                                                        if (Business.Market.InvestorList[j].InvestorID == newOpenTrade.Investor.InvestorID)
                                                        {
                                                            if (Business.Market.InvestorList[j].CommandList != null)
                                                            {
                                                                Business.Market.InvestorList[j].CommandList.Add(newOpenTrade);
                                                            }
                                                            else
                                                            {
                                                                Business.Market.InvestorList[j].CommandList = new List<OpenTrade>();
                                                                Business.Market.InvestorList[j].CommandList.Add(newOpenTrade);
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion                                                
                                            }
                                        }
                                    }
                                    #endregion                                    
                                }
                                break;
                        }
                    }
                    break;

                case 1:
                    {
                        switch (sourceUpdate)
                        {
                            case 0:
                                {
                                    #region COMPARE COMMAND IN SYMBOL LIST VS COMMAND EXECUTOR
                                    if (Business.Market.SymbolList != null)
                                    {
                                        for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                                        {
                                            if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                                            {                                                
                                                for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                                                {
                                                    bool flag = false;
                                                    if (Business.Market.CommandExecutor != null)
                                                    {
                                                        #region SEARCH IN COMMAND EXECUTOR
                                                        for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                        {
                                                            if (Business.Market.SymbolList[i].CommandList[j].ID == Business.Market.CommandExecutor[n].ID)
                                                            {
                                                                flag = true;
                                                                break;
                                                            }
                                                        }
                                                        #endregion

                                                        #region ADD COMMAND TO COMMAND EXECUTOR
                                                        if (!flag)
                                                        {
                                                            Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(Business.Market.CommandExecutor[i].ID);
                                                            if (Business.Market.CommandExecutor != null)
                                                            {
                                                                Business.Market.CommandExecutor.Add(newOpenTrade);
                                                            }
                                                            else
                                                            {
                                                                Business.Market.CommandExecutor = new List<OpenTrade>();
                                                                Business.Market.CommandExecutor.Add(newOpenTrade);
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion                                    
                                }
                                break;

                            case 2:
                                {
                                    #region COMPARE COMMAND IN SYMBOL LIST VS INVESTOR LIST
                                    if (Business.Market.SymbolList != null)
                                    {
                                        for (int i = 0; i < Business.Market.SymbolList.Count; i++)
                                        {
                                            if (Business.Market.SymbolList[i].CommandList != null && Business.Market.SymbolList[i].CommandList.Count > 0)
                                            {
                                                for (int j = 0; j < Business.Market.SymbolList[i].CommandList.Count; j++)
                                                {
                                                    if (Business.Market.InvestorList != null)
                                                    {
                                                        #region SEARCH IN INVESTOR LIST
                                                        bool flag = false;
                                                        for (int n = 0; n < Business.Market.InvestorList.Count; n++)
                                                        {
                                                            if (Business.Market.InvestorList[n].InvestorID == Business.Market.SymbolList[i].CommandList[j].Investor.InvestorID)
                                                            {
                                                                if (Business.Market.InvestorList[n].CommandList != null &&
                                                                    Business.Market.InvestorList[n].CommandList.Count > 0)
                                                                {
                                                                    for (int m = 0; m < Business.Market.InvestorList[n].CommandList.Count; m++)
                                                                    {
                                                                        if (Business.Market.InvestorList[n].CommandList[m].ID == Business.Market.SymbolList[i].CommandList[j].ID)
                                                                        {
                                                                            flag = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                break;
                                                            }                                                            

                                                            if (flag)
                                                                break;
                                                        }
                                                        #endregion

                                                        #region ADD NEW COMMAND TO INVESTOR LIST
                                                        if (!flag)
                                                        {
                                                            Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(Business.Market.CommandExecutor[i].ID);
                                                            if (Business.Market.InvestorList != null)
                                                            {
                                                                for (int n = 0; n < Business.Market.InvestorList.Count; n++)
                                                                {
                                                                    if (Business.Market.InvestorList[n].InvestorID == newOpenTrade.Investor.InvestorID)
                                                                    {
                                                                        if (Business.Market.InvestorList[n].CommandList != null)
                                                                        {
                                                                            Business.Market.InvestorList[n].CommandList.Add(newOpenTrade);
                                                                        }
                                                                        else
                                                                        {
                                                                            Business.Market.InvestorList[n].CommandList = new List<OpenTrade>();
                                                                            Business.Market.InvestorList[n].CommandList.Add(newOpenTrade);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion                                    
                                }
                                break;
                        }
                    }
                    break;

                case 2:
                    {
                        switch (sourceUpdate)
                        {
                            case 0:
                                {
                                    #region COMPARE COMMAND IN INVESTOR LIST VS COMMAND EXECUTOR
                                    if (Business.Market.InvestorList != null)
                                    {
                                        for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                                        {
                                            if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                            {
                                                for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                                {
                                                    if (Business.Market.CommandExecutor != null)
                                                    {
                                                        #region SEARCH COMMAND IN COMMAND EXECUTOR
                                                        bool flag = false;
                                                        for (int n = 0; n < Business.Market.CommandExecutor.Count; n++)
                                                        {
                                                            if (Business.Market.CommandExecutor[n].ID == Business.Market.InvestorList[i].CommandList[j].ID)
                                                            {
                                                                flag = true;
                                                                break;
                                                            }
                                                        }
                                                        #endregion                                                        

                                                        #region ADD NEW COMMAND TO COMMAND EXECUTOR
                                                        if (!flag)
                                                        {
                                                            Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(Business.Market.CommandExecutor[i].ID);
                                                            if (Business.Market.CommandExecutor != null)
                                                            {
                                                                Business.Market.CommandExecutor.Add(newOpenTrade);
                                                            }
                                                            else
                                                            {
                                                                Business.Market.CommandExecutor = new List<OpenTrade>();
                                                                Business.Market.CommandExecutor.Add(newOpenTrade);
                                                            }
                                                        }
                                                        #endregion                                                        
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                break;

                            case 1:
                                {
                                    #region COMPARE COMMAND IN INVESTOR LIST VS SYMBOL LÍT
                                    if (Business.Market.InvestorList != null)
                                    {
                                        for (int i = 0; i < Business.Market.InvestorList.Count; i++)
                                        {
                                            if (Business.Market.InvestorList[i].CommandList != null && Business.Market.InvestorList[i].CommandList.Count > 0)
                                            {
                                                for (int j = 0; j < Business.Market.InvestorList[i].CommandList.Count; j++)
                                                {
                                                    #region SEARCH COMMAND IN SYMBOL LIST
                                                    bool flag = false;
                                                    if (Business.Market.SymbolList != null)
                                                    {
                                                        for (int n = 0; n < Business.Market.SymbolList.Count; n++)
                                                        {
                                                            if (Business.Market.InvestorList[i].CommandList[j].Symbol.Name == Business.Market.SymbolList[n].Name)
                                                            {
                                                                if (Business.Market.SymbolList[n].CommandList != null)
                                                                {
                                                                    for (int m = 0; m < Business.Market.SymbolList[n].CommandList.Count; m++)
                                                                    {
                                                                        if (Business.Market.SymbolList[n].CommandList[m].ID == Business.Market.InvestorList[i].CommandList[j].ID)
                                                                        {
                                                                            flag = true;
                                                                            break;
                                                                        }
                                                                    }

                                                                    if (flag)
                                                                        break;
                                                                }

                                                                break;
                                                            }                                                            
                                                        }
                                                    }
                                                    #endregion                                                    

                                                    #region ADD NEW COMMAND TO SYMBOL LIST
                                                    if (!flag)
                                                    {
                                                        Business.OpenTrade newOpenTrade = TradingServer.Facade.FacadeGetCommandByID(Business.Market.CommandExecutor[i].ID);
                                                        for (int n = 0; n < Business.Market.SymbolList.Count; n++)
                                                        {
                                                            if (Business.Market.SymbolList[n].Name == Business.Market.InvestorList[i].CommandList[j].Symbol.Name)
                                                            {
                                                                if (Business.Market.SymbolList[n].CommandList != null)
                                                                {
                                                                    Business.Market.SymbolList[n].CommandList.Add(newOpenTrade);
                                                                }
                                                                else
                                                                {
                                                                    Business.Market.SymbolList[n].CommandList = new List<OpenTrade>();
                                                                    Business.Market.SymbolList[n].CommandList.Add(newOpenTrade);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion                                                    
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                break;
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}



