using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    internal class DBWVirtualDealer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbVirtualDealer"></param>
        /// <returns></returns>
        private List<Business.VirtualDealer> MapVirtualDealer(DS.VirtualDealerDataTable tbVirtualDealer)
        {
            List<Business.VirtualDealer> result = new List<Business.VirtualDealer>();
            if (tbVirtualDealer != null)
            {
                int count = tbVirtualDealer.Count;
                for (int i = 0; i < count; i++)
                {
                    Business.VirtualDealer newVirtualDealer = new Business.VirtualDealer();
                    //newVirtualDealer.AgentID = tbVirtualDealer[i].AgentID;
                    //newVirtualDealer.VirtualDealerDescription = tbVirtualDealer[i].VirtualDealerDescription;
                    //newVirtualDealer.VirtualDealerID = tbVirtualDealer[i].VirtualDealerID;
                    //newVirtualDealer.VirtualDealerName = tbVirtualDealer[i].VirtualDealerName;
                    //newVirtualDealer.VirtualDealerConfig = TradingServer.Facade.FacadeGetVirtualConfigByVirtualDealerID(newVirtualDealer.VirtualDealerID);
                    //newVirtualDealer.AgentInstance = TradingServer.Facade.FacadeGetAgentByAgentID(newVirtualDealer.AgentID);

                    result.Add(newVirtualDealer);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal List<Business.VirtualDealer> GetAllVirtualDealer()
        {
            List<Business.VirtualDealer> result = new List<Business.VirtualDealer>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerTableAdapter virtualDealerAdap = new DSTableAdapters.VirtualDealerTableAdapter();
            DSTableAdapters.VirtualDealerConfigTableAdapter configAdap = new DSTableAdapters.VirtualDealerConfigTableAdapter();
            DSTableAdapters.IVirtualDealerTableAdapter iVirtualDealerAdap = new DSTableAdapters.IVirtualDealerTableAdapter();
            DS.VirtualDealerDataTable tbVirtualDealer = new DS.VirtualDealerDataTable();
            DS.VirtualDealerConfigDataTable tbVirtualDealerConfig = new DS.VirtualDealerConfigDataTable();
            DS.IVirtualDealerDataTable tbIVirtualDealer = new DS.IVirtualDealerDataTable();
            try
            {
                conn.Open();
                virtualDealerAdap.Connection = conn;
                configAdap.Connection = conn;
                iVirtualDealerAdap.Connection = conn;

                tbVirtualDealer = virtualDealerAdap.GetData();
                tbVirtualDealerConfig = configAdap.GetData();
                tbIVirtualDealer = iVirtualDealerAdap.GetData();

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                virtualDealerAdap.Connection.Close();
                configAdap.Connection.Close();
                iVirtualDealerAdap.Connection.Close();
                conn.Close();
            }

            for (int i = 0; i < tbVirtualDealer.Count; i++)
            {
                Business.VirtualDealer dealer = new Business.VirtualDealer();
                dealer.ID = tbVirtualDealer[i].VirtualDealerID;
                dealer.Name = tbVirtualDealer[i].VirtualDealerName;
                for (int j = 0; j < tbVirtualDealerConfig.Count; j++)
                {
                    #region 
                    if (tbVirtualDealerConfig[j].VirtualDealID == tbVirtualDealer[i].VirtualDealerID)
                    {
                        double num = 0;
                        switch (tbVirtualDealerConfig[j].Code)
                        {
                            case "VD01":
                                double.TryParse(tbVirtualDealerConfig[j].NumValue, out num);
                                dealer.ProfitMaxPip = num;
                                break;
                            case "VD02":
                                double.TryParse(tbVirtualDealerConfig[j].NumValue, out num);
                                dealer.LossMaxPip = num;
                                break;
                            case "VD03":
                                double.TryParse(tbVirtualDealerConfig[j].NumValue, out num);
                                dealer.StartVolume = num;
                                break;
                            case "VD04":
                                double.TryParse(tbVirtualDealerConfig[j].NumValue, out num);
                                dealer.EndVolume = num;
                                break;
                            case "VD05":
                                int delay;
                                int.TryParse(tbVirtualDealerConfig[j].NumValue, out delay);
                                dealer.Delay = delay;
                                break;
                            case "VD06":
                                double add;
                                double.TryParse(tbVirtualDealerConfig[j].NumValue, out add);
                                dealer.AdditionalPip = add;
                                break;
                            case "VD07":
                                int mode;
                                int.TryParse(tbVirtualDealerConfig[j].NumValue, out mode);
                                dealer.Mode = mode;
                                break;
                            case "VD08":
                                if (tbVirtualDealerConfig[j].BoolValue == 1) dealer.IsEnable = true;
                                else dealer.IsEnable = false;
                                break;
                            case "VD09":
                                if (tbVirtualDealerConfig[j].BoolValue == 1) dealer.IsLimitAuto = true;
                                else dealer.IsLimitAuto = false;
                                break;
                            case "VD10":
                                if (tbVirtualDealerConfig[j].BoolValue == 1) dealer.IsStopAuto = true;
                                else dealer.IsStopAuto = false;
                                break;
                            case "VD11":
                                if (tbVirtualDealerConfig[j].BoolValue == 1) dealer.IsStopSlippage = true;
                                else dealer.IsStopSlippage = false;
                                break;
                            case "VD12":
                                dealer.GroupCondition = tbVirtualDealerConfig[j].StringValue;
                                break;
                            case "VD13":
                                dealer.SymbolCondition = tbVirtualDealerConfig[j].StringValue;
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion                                      
                }

                
                for (int j = 0; j < tbIVirtualDealer.Count; j++)
                {
                    if (tbVirtualDealer[i].VirtualDealerID == tbIVirtualDealer[j].VirtualDealerID)
                    {
                        Business.IVirtualDealer iVirtualDealer = new Business.IVirtualDealer();
                        iVirtualDealer.InvestorGroupID = tbIVirtualDealer[j].InvestorGroupID;
                        iVirtualDealer.SymbolID = tbIVirtualDealer[j].SymbolID;
                        dealer.IVirtualDealer.Add(iVirtualDealer);
                    }                    
                }//end for
                result.Add(dealer);
            }//end for
            return result;
        }//end function

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVirtualDealer"></param>
        /// <returns></returns>
        internal string AddNewVirtualDealer(Business.VirtualDealer objVirtualDealer)
        {
            string result = "-1";
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            System.Data.SqlClient.SqlTransaction trans = null;
            DSTableAdapters.VirtualDealerTableAdapter virtualDealerAdap = new DSTableAdapters.VirtualDealerTableAdapter();
            DSTableAdapters.VirtualDealerConfigTableAdapter dealerConfigAdap = new DSTableAdapters.VirtualDealerConfigTableAdapter();
            DSTableAdapters.IVirtualDealerTableAdapter iVirtualDealerAdap = new DSTableAdapters.IVirtualDealerTableAdapter();
            try
            { 
                conn.Open();
                trans = conn.BeginTransaction();

                virtualDealerAdap.Connection = conn;
                virtualDealerAdap.Transaction = trans;
                int id = int.Parse(virtualDealerAdap.AddNewVirtualDealer(objVirtualDealer.Name, "").ToString());
                result = id.ToString();
                if (id == -1)
                {
                    throw new Exception("Data error");
                }
                
                dealerConfigAdap.Connection = conn;
                dealerConfigAdap.Transaction = trans;
                DateTime DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                dealerConfigAdap.Insert(null, "Profit max pip", id, "VD01", -1, "NaN", objVirtualDealer.ProfitMaxPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Loss max pip", id, "VD02", -1, "NaN", objVirtualDealer.LossMaxPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Min volume", id, "VD03", -1, "NaN", objVirtualDealer.StartVolume.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Max volume", id, "VD04", -1, "NaN", objVirtualDealer.EndVolume.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Delay", id, "VD05", -1, "NaN", objVirtualDealer.Delay.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Additional pip", id, "VD06", -1, "NaN", objVirtualDealer.AdditionalPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Mode", id, "VD07", -1, "NaN", objVirtualDealer.Mode.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "IsEnable", id, "VD08", objVirtualDealer.IsEnable ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsLimitAuto", id, "VD09", objVirtualDealer.IsLimitAuto ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsStopAuto", id, "VD10", objVirtualDealer.IsStopAuto ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsStopSlippage", id, "VD11", objVirtualDealer.IsStopSlippage ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "GroupCondition", id, "VD12", -1, objVirtualDealer.GroupCondition, "NaN", DateValue);
                dealerConfigAdap.Insert(null, "SymbolCodition", id, "VD13", -1, objVirtualDealer.SymbolCondition, "NaN", DateValue);
                iVirtualDealerAdap.Connection = conn;
                iVirtualDealerAdap.Transaction = trans;
                for (int i = 0; i < objVirtualDealer.IVirtualDealer.Count; i++)
                {
                    iVirtualDealerAdap.Insert(objVirtualDealer.IVirtualDealer[i].InvestorGroupID, objVirtualDealer.IVirtualDealer[i].SymbolID, id);
                }
                trans.Commit();
            }   
            catch (Exception ex)
            {
                trans.Rollback();
                return "Data error";
            }
            finally
            {
                virtualDealerAdap.Connection.Close();
                dealerConfigAdap.Connection.Close();
                iVirtualDealerAdap.Connection.Close();
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualDealerID"></param>
        /// <returns></returns>
        internal string DeleteVirtualDealer(int virtualDealerID)
        {
            string result = "-1";
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerTableAdapter virtualDealerAdap = new DSTableAdapters.VirtualDealerTableAdapter();
            DSTableAdapters.IVirtualDealerTableAdapter iVirtualDealerAdap = new DSTableAdapters.IVirtualDealerTableAdapter();
            DSTableAdapters.VirtualDealerConfigTableAdapter configDealerAdap = new DSTableAdapters.VirtualDealerConfigTableAdapter();
            System.Data.SqlClient.SqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();                

                configDealerAdap.Connection = conn;
                configDealerAdap.Transaction = trans;
                configDealerAdap.DeleteVirtualConfigByVirtualDealerID(virtualDealerID);

                iVirtualDealerAdap.Connection = conn;
                iVirtualDealerAdap.Transaction = trans;
                iVirtualDealerAdap.DeleteByVirtualDealerID(virtualDealerID);

                virtualDealerAdap.Connection = conn;
                virtualDealerAdap.Transaction = trans;
                int resultDelete = virtualDealerAdap.DeleteVirtualDealer(virtualDealerID);

                if (resultDelete == 0)
                {
                    throw new Exception("Data error");
                }
                result = resultDelete.ToString();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "Data error";
            }
            finally
            {
                virtualDealerAdap.Connection.Close();
                iVirtualDealerAdap.Connection.Close();
                configDealerAdap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVirtualDealer"></param>
        /// <returns></returns>
        internal string UpdateVirtualDealer(Business.VirtualDealer objVirtualDealer)
        {
            string result = "";
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerTableAdapter virtualDealerAdap = new DSTableAdapters.VirtualDealerTableAdapter();
            DSTableAdapters.VirtualDealerConfigTableAdapter dealerConfigAdap = new DSTableAdapters.VirtualDealerConfigTableAdapter();
            DSTableAdapters.IVirtualDealerTableAdapter iVirtualDealerAdap = new DSTableAdapters.IVirtualDealerTableAdapter();
            System.Data.SqlClient.SqlTransaction trans=null;
            try
            {                
                conn.Open();
                trans = conn.BeginTransaction();

                virtualDealerAdap.Connection = conn;
                virtualDealerAdap.Transaction = trans;
                int resultUpdate = virtualDealerAdap.UpdateVirtualDealer(objVirtualDealer.Name, "", objVirtualDealer.ID);
                result = resultUpdate.ToString();
                if (resultUpdate == 0)
                {
                    throw new Exception("Data error");
                }
                
                dealerConfigAdap.Connection = conn;
                dealerConfigAdap.Transaction = trans;
                resultUpdate = dealerConfigAdap.DeleteVirtualConfigByVirtualDealerID(objVirtualDealer.ID);
                if (resultUpdate == 0)
                {
                    throw new Exception("Data error");
                }
                DateTime DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                dealerConfigAdap.Insert(null, "Profit max pip", objVirtualDealer.ID, "VD01", -1, "NaN", objVirtualDealer.ProfitMaxPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Loss max pip", objVirtualDealer.ID, "VD02", -1, "NaN", objVirtualDealer.LossMaxPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Min volume", objVirtualDealer.ID, "VD03", -1, "NaN", objVirtualDealer.StartVolume.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Max volume", objVirtualDealer.ID, "VD04", -1, "NaN", objVirtualDealer.EndVolume.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Delay", objVirtualDealer.ID, "VD05", -1, "NaN", objVirtualDealer.Delay.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Additional pip", objVirtualDealer.ID, "VD06", -1, "NaN", objVirtualDealer.AdditionalPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Mode", objVirtualDealer.ID, "VD07", -1, "NaN", objVirtualDealer.Mode.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "IsEnable", objVirtualDealer.ID, "VD08", objVirtualDealer.IsEnable ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsLimitAuto", objVirtualDealer.ID, "VD09", objVirtualDealer.IsLimitAuto ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsStopAuto", objVirtualDealer.ID, "VD10", objVirtualDealer.IsStopAuto ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsStopSlippage", objVirtualDealer.ID, "VD11", objVirtualDealer.IsStopSlippage ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "GroupCondition", objVirtualDealer.ID, "VD12", -1, objVirtualDealer.GroupCondition, "NaN", DateValue);
                dealerConfigAdap.Insert(null, "SymbolCodition", objVirtualDealer.ID, "VD13", -1, objVirtualDealer.SymbolCondition, "NaN", DateValue);

                iVirtualDealerAdap.Connection = conn;
                iVirtualDealerAdap.Transaction = trans;
                int rowEffect = iVirtualDealerAdap.DeleteByVirtualDealerID(objVirtualDealer.ID);
                for (int i = 0; i < objVirtualDealer.IVirtualDealer.Count; i++)
                {
                    rowEffect = iVirtualDealerAdap.Insert(objVirtualDealer.IVirtualDealer[i].InvestorGroupID, objVirtualDealer.IVirtualDealer[i].SymbolID, objVirtualDealer.ID);
                    if (rowEffect == 0)
                    {
                        throw new Exception("Can't insert IDealer");
                    }
                }
                result = "1";
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                result= "Data error";
            }
            finally
            {
                virtualDealerAdap.Connection.Close();
                dealerConfigAdap.Connection.Close();
                iVirtualDealerAdap.Connection.Close();
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVirtualDealer"></param>
        /// <returns></returns>
        internal string UpdateVirtualDealerInfo(Business.VirtualDealer objVirtualDealer)
        {
            string result = "";
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.VirtualDealerTableAdapter virtualDealerAdap = new DSTableAdapters.VirtualDealerTableAdapter();
            DSTableAdapters.VirtualDealerConfigTableAdapter dealerConfigAdap = new DSTableAdapters.VirtualDealerConfigTableAdapter();            
            System.Data.SqlClient.SqlTransaction trans = null;
            try
            {                
                conn.Open();
                trans = conn.BeginTransaction();

                virtualDealerAdap.Connection = conn;
                virtualDealerAdap.Transaction = trans;
                int resultUpdate = virtualDealerAdap.UpdateVirtualDealer(objVirtualDealer.Name, "", objVirtualDealer.ID);
                result = resultUpdate.ToString();
                if (resultUpdate == 0)
                {
                    throw new Exception("Data error");
                }

                dealerConfigAdap.Connection = conn;
                dealerConfigAdap.Transaction = trans;
                resultUpdate = dealerConfigAdap.DeleteVirtualConfigByVirtualDealerID(objVirtualDealer.ID);
                if (resultUpdate == 0)
                {
                    throw new Exception("Data error");
                }
                DateTime DateValue = DateTime.Parse("1753-01-01 00:00:00.000");
                dealerConfigAdap.Insert(null, "Profit max pip", objVirtualDealer.ID, "VD01", -1, "NaN", objVirtualDealer.ProfitMaxPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Loss max pip", objVirtualDealer.ID, "VD02", -1, "NaN", objVirtualDealer.LossMaxPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Min volume", objVirtualDealer.ID, "VD03", -1, "NaN", objVirtualDealer.StartVolume.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Max volume", objVirtualDealer.ID, "VD04", -1, "NaN", objVirtualDealer.EndVolume.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Delay", objVirtualDealer.ID, "VD05", -1, "NaN", objVirtualDealer.Delay.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Additional pip", objVirtualDealer.ID, "VD06", -1, "NaN", objVirtualDealer.AdditionalPip.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "Mode", objVirtualDealer.ID, "VD07", -1, "NaN", objVirtualDealer.Mode.ToString(), DateValue);
                dealerConfigAdap.Insert(null, "IsEnable", objVirtualDealer.ID, "VD08", objVirtualDealer.IsEnable ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsLimitAuto", objVirtualDealer.ID, "VD09", objVirtualDealer.IsLimitAuto ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsStopAuto", objVirtualDealer.ID, "VD10", objVirtualDealer.IsStopAuto ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "IsStopSlippage", objVirtualDealer.ID, "VD11", objVirtualDealer.IsStopSlippage ? 1 : 0, "NaN", "NaN", DateValue);
                dealerConfigAdap.Insert(null, "GroupCondition", objVirtualDealer.ID, "VD12", -1, objVirtualDealer.GroupCondition, "NaN", DateValue);
                dealerConfigAdap.Insert(null, "SymbolCodition", objVirtualDealer.ID, "VD13", -1, objVirtualDealer.SymbolCondition, "NaN", DateValue);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                result = "Data error";
            }
            finally
            {
                virtualDealerAdap.Connection.Close();
                dealerConfigAdap.Connection.Close();
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVirtualDealer"></param>
        /// <returns></returns>
        internal string UpdateVirtualSymbol(Business.VirtualDealer objVirtualDealer)
        {
            string result = "";
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.IVirtualDealerTableAdapter iVirtualDealerAdap = new DSTableAdapters.IVirtualDealerTableAdapter();
            System.Data.SqlClient.SqlTransaction trans = null;
            try
            {                
                conn.Open();
                trans = conn.BeginTransaction();

                iVirtualDealerAdap.Connection = conn;
                iVirtualDealerAdap.Transaction = trans;
                int rowEffect = iVirtualDealerAdap.DeleteByVirtualDealerID(objVirtualDealer.ID);
                for (int i = 0; i < objVirtualDealer.IVirtualDealer.Count; i++)
                {
                    rowEffect = iVirtualDealerAdap.Insert(objVirtualDealer.IVirtualDealer[i].InvestorGroupID, objVirtualDealer.IVirtualDealer[i].SymbolID, objVirtualDealer.ID);
                    if (rowEffect == 0)
                    {
                        throw new Exception("Can't insert IDealer");
                    }                    
                }
                result = "1";
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                result = "Data error";
            }
            finally
            {
                iVirtualDealerAdap.Connection.Close();
                conn.Close();
            }
            return result;
        }
    }//end class
}
