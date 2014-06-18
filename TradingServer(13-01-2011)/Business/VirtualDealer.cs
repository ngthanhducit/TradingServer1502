using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Business
{

    public class VirtualDealer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double StartVolume { get; set; }
        public double EndVolume { get; set; }
        public double ProfitMaxPip { get; set; }
        public double LossMaxPip { get; set; }
        public double AdditionalPip { get; set; }
        public string GroupCondition { get; set; }
        public string SymbolCondition { get; set; }
        public List<IVirtualDealer> IVirtualDealer = new List<IVirtualDealer>();
        public int Delay { get; set; }
        public bool IsEnable { get; set; }
        public bool IsLimitAuto { get; set; }
        public bool IsStopAuto { get; set; }
        public bool IsStopSlippage { get; set; }
        public int Mode { get; set; }
        #region Create Instance Class DBWVirtualDealer
        private static DBW.DBWVirtualDealer dbwVirtualDealer;
        private static DBW.DBWVirtualDealer DBWAgentInstance
        {
            get
            {
                if (VirtualDealer.dbwVirtualDealer == null)
                {
                    VirtualDealer.dbwVirtualDealer = new DBW.DBWVirtualDealer();
                }
                return VirtualDealer.dbwVirtualDealer;
            }
        }
        #endregion
        public VirtualDealer()
        {
        }

        internal string AddDealer()
        {
            string result = VirtualDealer.DBWAgentInstance.AddNewVirtualDealer(this);
            int id;
            int.TryParse(result, out id);
            if (id > 0)
            {
                this.ID = id;
                Business.Agent agent = new Agent();
                agent.LoginVirtualDealer(this);
            }
            return result;
        }

        internal string UpdateDealer()
        {
            string result = VirtualDealer.DBWAgentInstance.UpdateVirtualDealer(this);
            int id;
            int.TryParse(result, out id);
            if (id > 0)
            {
                for (int i = Market.AgentList.Count - 1; i >= 0; i--)
                {
                    if (Market.AgentList[i].IsVirtualDealer)
                    {
                        if (Market.AgentList[i].VirtualDealer.ID == this.ID)
                        {
                            Market.AgentList[i].VirtualDealer.Name = this.Name;
                            Market.AgentList[i].VirtualDealer.StartVolume = this.StartVolume;
                            Market.AgentList[i].VirtualDealer.EndVolume = this.EndVolume;
                            Market.AgentList[i].VirtualDealer.ProfitMaxPip = this.ProfitMaxPip;
                            Market.AgentList[i].VirtualDealer.LossMaxPip = this.LossMaxPip;
                            Market.AgentList[i].VirtualDealer.AdditionalPip = this.AdditionalPip;
                            Market.AgentList[i].VirtualDealer.Delay = this.Delay;
                            Market.AgentList[i].VirtualDealer.IsEnable = this.IsEnable;
                            Market.AgentList[i].VirtualDealer.IsLimitAuto = this.IsLimitAuto;
                            Market.AgentList[i].VirtualDealer.IsStopAuto = this.IsStopAuto;
                            Market.AgentList[i].VirtualDealer.IsStopSlippage = this.IsStopSlippage;
                            Market.AgentList[i].VirtualDealer.Mode = this.Mode;
                            Market.AgentList[i].VirtualDealer.IVirtualDealer = this.IVirtualDealer;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        internal string UpdateDealerInfo()
        {
            string result = VirtualDealer.DBWAgentInstance.UpdateVirtualDealerInfo(this);
            int id;
            int.TryParse(result, out id);
            if (id > 0)
            {
                for (int i = Market.AgentList.Count - 1; i >= 0; i--)
                {
                    if (Market.AgentList[i].IsVirtualDealer)
                    {
                        if (Market.AgentList[i].VirtualDealer.ID == this.ID)
                        {
                            Market.AgentList[i].VirtualDealer.Name = this.Name;
                            Market.AgentList[i].VirtualDealer.StartVolume = this.StartVolume;
                            Market.AgentList[i].VirtualDealer.EndVolume = this.EndVolume;
                            Market.AgentList[i].VirtualDealer.ProfitMaxPip = this.ProfitMaxPip;
                            Market.AgentList[i].VirtualDealer.LossMaxPip = this.LossMaxPip;
                            Market.AgentList[i].VirtualDealer.AdditionalPip = this.AdditionalPip;
                            Market.AgentList[i].VirtualDealer.Delay = this.Delay;
                            Market.AgentList[i].VirtualDealer.IsEnable = this.IsEnable;
                            Market.AgentList[i].VirtualDealer.IsLimitAuto = this.IsLimitAuto;
                            Market.AgentList[i].VirtualDealer.IsStopAuto = this.IsStopAuto;
                            Market.AgentList[i].VirtualDealer.IsStopSlippage = this.IsStopSlippage;
                            Market.AgentList[i].VirtualDealer.Mode = this.Mode;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        internal string UpdateDealerSymbol()
        {
            string result = VirtualDealer.DBWAgentInstance.UpdateVirtualSymbol(this);
            int id;
            int.TryParse(result, out id);
            if (id > 0)
            {
                for (int i = Market.AgentList.Count - 1; i >= 0; i--)
                {
                    if (Market.AgentList[i].IsVirtualDealer)
                    {
                        if (Market.AgentList[i].VirtualDealer.ID == this.ID)
                        {
                            Market.AgentList[i].VirtualDealer.IVirtualDealer = this.IVirtualDealer;
                            Market.AgentList[i].VirtualDealer.GroupCondition = this.GroupCondition;
                            Market.AgentList[i].VirtualDealer.SymbolCondition = this.SymbolCondition;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        internal void CheckUpdateGroupVirtualDealerOnline()
        {
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer)
                {
                    Market.AgentList[i].VirtualDealer.UpdateGroupVirtualDealerOnline();
                }
            }
        }

        /// <summary>
        /// Add Group, Update Group, Add Symbol, Update Symbol
        /// </summary>
        internal void UpdateGroupVirtualDealerOnline()
        {
            List<IVirtualDealer> iVirtualDealers = new List<Business.IVirtualDealer>();
            List<int> itemGroup = TradingServer.Facade.FacadeMakeListIAgentGroupManager(this.GroupCondition);
            for (int i = 0; i < itemGroup.Count; i++)
            {
                IVirtualDealer iVirtualDealer = new IVirtualDealer();
                iVirtualDealer.SymbolID = -1;
                iVirtualDealer.InvestorGroupID = itemGroup[i];
                iVirtualDealers.Add(iVirtualDealer);
            }
            List<int> itemSymbol = TradingServer.Facade.FacadeMakeListSymbolIDManager(this.SymbolCondition);
            for (int i = 0; i < itemSymbol.Count; i++)
            {
                IVirtualDealer iVirtualDealer = new IVirtualDealer();
                iVirtualDealer.SymbolID = itemSymbol[i];
                iVirtualDealer.InvestorGroupID = -1;
                iVirtualDealers.Add(iVirtualDealer);
            }
            bool check = false;
            if (this.IVirtualDealer.Count != iVirtualDealers.Count)
            {
                check = true;
            }
            else
            {
                for (int i = this.IVirtualDealer.Count - 1; i >= 0; i--)
                {
                    if (iVirtualDealers[i].InvestorGroupID != this.IVirtualDealer[i].InvestorGroupID ||
                        iVirtualDealers[i].SymbolID != this.IVirtualDealer[i].SymbolID)
                    {
                        check = true;
                        break;
                    }
                }
            }
            if (check)
            {
                this.IVirtualDealer = iVirtualDealers;
                string result = VirtualDealer.DBWAgentInstance.UpdateVirtualSymbol(this);
                int id;
                int.TryParse(result, out id);
                if (id <= 0)
                {
                    TradingServer.Facade.FacadeAddNewSystemLog(1, "Robot Dealer update filter Group or Symbol failed", "Error", "123", "");
                }
            }

        }


        internal string DeleteDealer(int idVirtualDealer)
        {
            string result = VirtualDealer.DBWAgentInstance.DeleteVirtualDealer(idVirtualDealer);
            int id;
            int.TryParse(result, out id);
            if (id > 0)
            {
                for (int i = Market.AgentList.Count - 1; i >= 0; i--)
                {
                    if (Market.AgentList[i].IsVirtualDealer)
                    {
                        if (Market.AgentList[i].VirtualDealer.ID == this.ID)
                        {
                            this.Name = Market.AgentList[i].VirtualDealer.Name;
                            Market.AgentList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            return result;
        }

        internal List<Business.VirtualDealer> GetAllVirtualDealer()
        {
            return VirtualDealer.DBWAgentInstance.GetAllVirtualDealer();
        }

        /// <summary>
        /// map virtual dealer object from string
        /// </summary>
        /// <param name="para">ID}Name}Description}Delay}BeginVolume}EndVolume}ProfitMaxVolume}ProfitMaxPip}LossMaxVolume}LossMaxPip}groupID]syID1[syID2[syID3|groupID]syID1[syID2[syID3</param>
        /// <returns></returns>
        internal string MapDealer(string para)
        {
            int numInt;
            double numDouble;
            bool checkIs;
            string[] value = para.Split('}');
            if (value.Length != 15)
            {
                return "parameter wrong";
            }

            if (!int.TryParse(value[0], out numInt))
            {
                return "ID invalid";
            }
            this.ID = numInt;

            if (string.IsNullOrEmpty(value[1]))
            {
                return "Name invalid";
            }
            this.Name = value[1];

            if (!int.TryParse(value[2], out numInt))
            {
                return "Delay value invalid";
            }
            this.Delay = numInt;

            if (!double.TryParse(value[3], out numDouble))
            {
                return "Begin volume value invalid";
            }
            this.StartVolume = numDouble;

            if (!double.TryParse(value[4], out numDouble))
            {
                return "End volume value invalid";
            }
            this.EndVolume = numDouble;

            if (!int.TryParse(value[5], out numInt))
            {
                return "Mode invalid";
            }
            this.Mode = numInt;

            if (!double.TryParse(value[6], out numDouble))
            {
                return "Profit max pip valud invalid";
            }
            this.ProfitMaxPip = numDouble;

            if (!double.TryParse(value[7], out numDouble))
            {
                return "Loss max pip invalid";
            }
            this.LossMaxPip = numDouble;

            if (!double.TryParse(value[8], out numDouble))
            {
                return "Additional pip invalid";
            }
            this.AdditionalPip = numDouble;

            if (!bool.TryParse(value[9], out checkIs))
            {
                return "IsEnable invalid";
            }
            this.IsEnable = checkIs;

            if (!bool.TryParse(value[10], out checkIs))
            {
                return "IsStopSlippage invalid";
            }
            this.IsStopSlippage = checkIs;

            if (!bool.TryParse(value[11], out checkIs))
            {
                return "IsLimitAuto invalid";
            }
            this.IsLimitAuto = checkIs;

            if (!bool.TryParse(value[12], out checkIs))
            {
                return "IsStopAuto invalid";
            }
            this.IsStopAuto = checkIs;
            this.GroupCondition = value[13];
            List<int> itemGroup = TradingServer.Facade.FacadeMakeListIAgentGroupManager(this.GroupCondition);
            for (int i = 0; i < itemGroup.Count; i++)
            {
                IVirtualDealer iVirtualDealer = new IVirtualDealer();
                iVirtualDealer.SymbolID = -1;
                iVirtualDealer.InvestorGroupID = itemGroup[i];
                this.IVirtualDealer.Add(iVirtualDealer);
            }

            this.SymbolCondition = value[14];
            List<int> itemSymbol = TradingServer.Facade.FacadeMakeListSymbolIDManager(this.SymbolCondition);
            for (int i = 0; i < itemSymbol.Count; i++)
            {
                IVirtualDealer iVirtualDealer = new IVirtualDealer();
                iVirtualDealer.SymbolID = itemSymbol[i];
                iVirtualDealer.InvestorGroupID = -1;
                this.IVirtualDealer.Add(iVirtualDealer);
            }

            return "1";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>ID}Name}Delay}BeginVolume}EndVolume}ProfitMaxVolume}ProfitMaxPip}LossMaxVolume}LossMaxPip}groupID]syID1[syID2[syID3|groupID]syID1[syID2[syID3</returns>
        internal string ExtractDealer()
        {
            string result = this.ID + "}" + this.Name + "}" + this.Delay + "}" + this.StartVolume + "}" + this.EndVolume + "}" + this.Mode + "}" + this.ProfitMaxPip + "}" +
                            this.LossMaxPip + "}" + this.AdditionalPip + "}" + this.IsEnable + "}" + this.IsStopSlippage + "}" + this.IsLimitAuto + "}" + this.IsStopAuto
                            + "}" + this.GroupCondition + "}" + this.SymbolCondition;
            return result;
        }

        internal string MapChangeContentConfigVD()
        {
            string result = "";
            List<string> result1 = new List<string>();
            List<string> result2 = new List<string>();
            for (int i = Market.AgentList.Count - 1; i >= 0; i--)
            {
                if (Market.AgentList[i].IsVirtualDealer)
                {
                    VirtualDealer vd = Market.AgentList[i].VirtualDealer;
                    if (vd.ID == this.ID)
                    {
                        #region Config
                        if (vd.Name != this.Name)
                        {
                            result1.Add("name: " + vd.Name);
                            result2.Add("name: " + this.Name);
                        }
                        if (vd.IsEnable != this.IsEnable)
                        {
                            result1.Add("enable: " + vd.IsEnable);
                            result2.Add("enable: " + this.IsEnable);
                        }
                        if (vd.Delay != this.Delay)
                        {
                            result1.Add("delay: " + vd.Delay);
                            result2.Add("delay: " + this.Delay);
                        }
                        if (vd.StartVolume != this.StartVolume)
                        {
                            result1.Add("volume from: " + vd.StartVolume);
                            result2.Add("volume from: " + this.StartVolume);
                        }
                        if (vd.EndVolume != this.EndVolume)
                        {
                            result1.Add("volume to: " + vd.EndVolume);
                            result2.Add("volume to: " + this.EndVolume);
                        }
                        if (vd.ProfitMaxPip != this.ProfitMaxPip)
                        {
                            result1.Add("profit max: " + vd.ProfitMaxPip);
                            result2.Add("profit max: " + this.ProfitMaxPip);
                        }
                        if (vd.LossMaxPip != this.LossMaxPip)
                        {
                            result1.Add("loss max: " + vd.LossMaxPip);
                            result2.Add("loss max: " + this.LossMaxPip);
                        }
                        if (vd.AdditionalPip != this.AdditionalPip)
                        {
                            result1.Add("additional: " + vd.AdditionalPip);
                            result2.Add("additional: " + this.AdditionalPip);
                        }
                        if (vd.IsStopSlippage != this.IsStopSlippage)
                        {
                            result1.Add("stop order slippage: " + vd.IsStopSlippage);
                            result2.Add("stop order slippage: " + this.IsStopSlippage);
                        }
                        if (vd.IsLimitAuto != this.IsLimitAuto)
                        {
                            result1.Add("limit order automation: " + vd.IsLimitAuto);
                            result2.Add("limit order automation: " + this.IsLimitAuto);
                        }
                        if (vd.IsStopAuto != this.IsStopAuto)
                        {
                            result1.Add("stop order automation: " + vd.IsLimitAuto);
                            result2.Add("stop order automation: " + this.IsLimitAuto);
                        }
                        if (vd.Mode != this.Mode)
                        {
                            result1.Add("robot dealer: " + GetModeVD(vd.Mode));
                            result2.Add("robot dealer: " + GetModeVD(this.Mode));
                        }
                        if (vd.GroupCondition != this.GroupCondition)
                        {
                            result1.Add("filter group: " + vd.GroupCondition);
                            result2.Add("filter group: " + this.GroupCondition);
                        }
                        if (vd.SymbolCondition != this.SymbolCondition)
                        {
                            result1.Add("filter symbol: " + vd.SymbolCondition);
                            result2.Add("filter symbol: " + this.SymbolCondition);
                        }
                        #endregion
                        break;
                    }
                }
            }
            for (int i = 0; i < result1.Count; i++)
            {
                if (i == result1.Count - 1)
                {
                    result += result1[i];
                }
                else
                {
                    result += result1[i] + " - ";
                }
            }
            if (result != "")
            {
                result += " -> ";
            }
            for (int i = 0; i < result2.Count; i++)
            {
                if (i == result2.Count - 1)
                {
                    result += result2[i];
                }
                else
                {
                    result += result2[i] + " - ";
                }
            }
            return result;
        }

        internal string GetModeVD(int mode)
        {
            string result = "";
            switch (mode)
            {
                case 0:
                    {
                        result = "INSTANT MODE";
                        break;
                    }
                case 1:
                    {
                        result = "MARKET & REQUEST MODE";
                        break;
                    }
                case 3:
                    {
                        result = "PENDING MODE";
                        break;
                    }
                default:
                    return "UNKNOWN MODE";
            }
            return result;
        }
    }//end class code
}
