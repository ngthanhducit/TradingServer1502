using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace TradingServer.DBW
{
    internal class DBWInvestor
    {
        #region Function Table Investor Proflie
        /// <summary>
        /// Get All Investor Profile
        /// </summary>
        /// <returns>List<Business.InvestorProfile</returns>
        internal List<Business.Investor> GetAllInvestorProfile()
        {
            List<Business.Investor> Result = new List<Business.Investor>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorProfileTableAdapter adap = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                
                tbInvestorProfile = adap.GetData();
                if (tbInvestorProfile != null)
                {
                    int count = tbInvestorProfile.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Investor newInvestorProfile = new Business.Investor();                        
                        newInvestorProfile.InvestorID = tbInvestorProfile[i].InvestorID;
                        newInvestorProfile.Address = tbInvestorProfile[i].Address;
                        newInvestorProfile.Phone = tbInvestorProfile[i].Phone;
                        newInvestorProfile.City = tbInvestorProfile[i].City;
                        newInvestorProfile.Country = tbInvestorProfile[i].Country;
                        newInvestorProfile.Email = tbInvestorProfile[i].Email;
                        newInvestorProfile.ZipCode = tbInvestorProfile[i].ZipCode;
                        newInvestorProfile.RegisterDay = tbInvestorProfile[i].RegisterDay;
                        newInvestorProfile.InvestorComment = tbInvestorProfile[i].Comment;
                        newInvestorProfile.State = tbInvestorProfile[i].State;
                        newInvestorProfile.NickName = tbInvestorProfile[i].NickName;
                        newInvestorProfile.IDPassport = tbInvestorProfile[i].IDPassport;

                        Result.Add(newInvestorProfile);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// Get Investor Profile By Investor ID
        /// </summary>
        /// <param name="InvestorID">int InvestorID</param>
        /// <returns>Business.InvestorProfile</returns>
        internal Business.Investor GetInvestorProfileByInvestorID(int InvestorID)
        {
            Business.Investor Result = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorProfileTableAdapter adap = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;

                tbInvestorProfile = adap.GetInvestorProfileByInvestorID(InvestorID);

                if (tbInvestorProfile != null)
                {                    
                    Result.InvestorID = tbInvestorProfile[0].InvestorID;
                    Result.Address = tbInvestorProfile[0].Address;
                    Result.Phone = tbInvestorProfile[0].Phone;
                    Result.City = tbInvestorProfile[0].City;
                    Result.Country = tbInvestorProfile[0].Country;
                    Result.Email = tbInvestorProfile[0].Email;
                    Result.ZipCode = tbInvestorProfile[0].ZipCode;
                    Result.RegisterDay = tbInvestorProfile[0].RegisterDay;
                    Result.InvestorComment = tbInvestorProfile[0].Comment;
                    Result.State = tbInvestorProfile[0].State;                    
                    Result.NickName = tbInvestorProfile[0].NickName;
                    Result.IDPassport = tbInvestorProfile[0].IDPassport;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorProfileID"></param>
        /// <returns></returns>
        internal Business.Investor GetInvestorProfileByInvestorProfileID(int InvestorProfileID)
        {
            Business.Investor Result = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorProfileTableAdapter adap = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                
                tbInvestorProfile = adap.GetInvestorProfileByInvestorProfileID(InvestorProfileID);

                if (tbInvestorProfile != null)
                {
                    int count = tbInvestorProfile.Count;
                    Result.Address = tbInvestorProfile[0].Address;
                    Result.City = tbInvestorProfile[0].City;
                    Result.InvestorComment = tbInvestorProfile[0].Comment;
                    Result.Country = tbInvestorProfile[0].Country;
                    Result.Email = tbInvestorProfile[0].Email;
                    Result.InvestorID = tbInvestorProfile[0].InvestorID;                    
                    Result.NickName = tbInvestorProfile[0].NickName;
                    Result.Phone = tbInvestorProfile[0].Phone;
                    Result.RegisterDay = tbInvestorProfile[0].RegisterDay;
                    Result.State = tbInvestorProfile[0].State;
                    Result.ZipCode = tbInvestorProfile[0].ZipCode;
                    Result.IDPassport = tbInvestorProfile[0].IDPassport;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <returns></returns>
        internal int AddNewInvestorProfile(Business.Investor objInvestor)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorProfileTableAdapter adap = new DSTableAdapters.InvestorProfileTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = int.Parse(adap.AddNewInvestorProfile(objInvestor.InvestorID, objInvestor.Address, objInvestor.Phone, objInvestor.City,
                                            objInvestor.Country, objInvestor.Email, objInvestor.ZipCode, objInvestor.RegisterDay,
                                            objInvestor.InvestorComment, objInvestor.State, objInvestor.NickName, objInvestor.IDPassport).ToString());
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        internal void UpdateInvestorProfile(Business.Investor objInvestorProfile)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorProfileTableAdapter adap = new DSTableAdapters.InvestorProfileTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.UpdateInvestorProfile(objInvestorProfile.InvestorID, objInvestorProfile.Address, objInvestorProfile.Phone,
                    objInvestorProfile.City, objInvestorProfile.Country, objInvestorProfile.Email, objInvestorProfile.ZipCode,
                    objInvestorProfile.InvestorComment, objInvestorProfile.State, objInvestorProfile.NickName, objInvestorProfile.IDPassport, 
                    objInvestorProfile.InvestorProfileID);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal bool DeleteInvestorProfileByInvestorID(int InvestorID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorProfileTableAdapter adap = new DSTableAdapters.InvestorProfileTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adap.DeleteInvestorProfileByInvestorID(InvestorID);
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }
        #endregion

        #region Function Table Investor
        /// <summary>
        /// Get All Investor Account
        /// </summary>
        /// <returns>List<Business.Investor></returns>
        internal List<Business.Investor> GetAllInvestor()
        {
            List<Business.Investor> Result = new List<Business.Investor>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DSTableAdapters.InvestorProfileTableAdapter adapInvestorProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapInvestorProfile.Connection = conn;
                tbInvestor = adap.GetData();

                if (tbInvestor != null)
                {
                    int count = tbInvestor.Count;
                    for (int i = 0; i < count; i++)
                    {
                        tbInvestorProfile = adapInvestorProfile.GetInvestorProfileByInvestorID(tbInvestor[i].InvestorID);

                        Business.Investor newInvestor = new Business.Investor();
                        newInvestor.InvestorID = tbInvestor[i].InvestorID;
                        newInvestor.AgentID = tbInvestor[i].AgentID;

                        #region FILL INVESTOR GROUP INSTANCE
                        //Fill Investor Group Instance 
                        if (Business.Market.InvestorGroupList != null)
                        {
                            int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                            for (int j = 0; j < countInvestorGroup; j++)
                            {
                                if (Business.Market.InvestorGroupList[j].InvestorGroupID == tbInvestor[i].InvestorGroupID)
                                {
                                    newInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[j];
                                    break;
                                }
                            }
                        }
                        #endregion

                        //newInvestor.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(tbInvestor[i].InvestorGroupID);
                        newInvestor.InvestorStatusID = tbInvestor[i].InvestorStatusID;
                        newInvestor.Balance = tbInvestor[i].Balance;
                        newInvestor.Code = tbInvestor[i].Code;
                        newInvestor.Credit = tbInvestor[i].Credit;
                        newInvestor.IsDisable = tbInvestor[i].IsDisible;
                        newInvestor.TaxRate = tbInvestor[i].TaxRate;
                        newInvestor.Leverage = tbInvestor[i].Leverage;
                        newInvestor.PreviousLedgerBalance = tbInvestor[i].PreviousLedgerBalance;
                        newInvestor.PrimaryPwd = tbInvestor[i].Pwd;
                        newInvestor.ReadOnlyPwd = tbInvestor[i].ReadPwd;
                        newInvestor.PhonePwd = tbInvestor[i].PhonePwd;
                        newInvestor.ReadOnly = tbInvestor[i].ReadOnly;
                        newInvestor.AllowChangePwd = tbInvestor[i].AllowChangePwd;
                        newInvestor.SendReport = tbInvestor[i].SendReport;
                        newInvestor.UserConfig = tbInvestor[i].UserConfig;
                        newInvestor.UserConfigIpad = tbInvestor[i].UserConfigIpad;
                        newInvestor.UserConfigIphone = tbInvestor[i].UserConfigIphone;
                        newInvestor.RefInvestorID = tbInvestor[i].RefInvestorID;
                        newInvestor.AgentRefID = tbInvestor[i].AgentRefID;
                        
                        #region FILL INVESTOR PROFILE
                        //Add Data InvestorProfile To Result
                        if (tbInvestorProfile != null && tbInvestorProfile.Count > 0)
                        {
                            newInvestor.InvestorProfileID = tbInvestorProfile[0].InvestorProfileID;
                            newInvestor.Address = tbInvestorProfile[0].Address;
                            newInvestor.City = tbInvestorProfile[0].City;
                            newInvestor.InvestorComment = tbInvestorProfile[0].Comment;
                            newInvestor.Country = tbInvestorProfile[0].Country;
                            newInvestor.Email = tbInvestorProfile[0].Email;
                            newInvestor.NickName = tbInvestorProfile[0].NickName;
                            newInvestor.Phone = tbInvestorProfile[0].Phone;
                            newInvestor.RegisterDay = tbInvestorProfile[0].RegisterDay;
                            newInvestor.State = tbInvestorProfile[0].State;
                            newInvestor.ZipCode = tbInvestorProfile[0].ZipCode;
                            newInvestor.IDPassport = tbInvestorProfile[0].IDPassport;
                        }
                        #endregion                        

                        Result.Add(newInvestor);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// Get Investor By Investor ID
        /// </summary>
        /// <param name="InvestorID">int InvestorID</param>
        /// <returns>Business.Investor</returns>
        internal Business.Investor GetInvestorByInvestorID(int InvestorID)
        {
            Business.Investor Result = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DSTableAdapters.InvestorProfileTableAdapter adapInvestorProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapInvestorProfile.Connection = conn;
                tbInvestor = adap.GetInvestorByInvestorID(InvestorID);

                if (tbInvestor != null)
                {
                    tbInvestorProfile = adapInvestorProfile.GetInvestorProfileByInvestorID(tbInvestor[0].InvestorID);

                    if (tbInvestorProfile != null)
                    {
                        //Add Data Investor To Result
                        Result.InvestorID = tbInvestor[0].InvestorID;
                        Result.AgentID = tbInvestor[0].AgentID;
                        //Result.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(tbInvestor[0].InvestorGroupID);

                        if (Business.Market.InvestorGroupList != null)
                        {
                            int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                            for (int j = 0; j < countInvestorGroup; j++)
                            {
                                if (Business.Market.InvestorGroupList[j].InvestorGroupID == tbInvestor[0].InvestorGroupID)
                                {
                                    Result.InvestorGroupInstance = Business.Market.InvestorGroupList[j];

                                    break;
                                }
                            }
                        }
                        Result.InvestorStatusID = tbInvestor[0].InvestorStatusID;
                        Result.Balance = tbInvestor[0].Balance;
                        Result.Code = tbInvestor[0].Code;
                        Result.Credit = tbInvestor[0].Credit;
                        Result.IsDisable = tbInvestor[0].IsDisible;
                        Result.TaxRate = tbInvestor[0].TaxRate;
                        Result.Leverage = tbInvestor[0].Leverage;
                        Result.PreviousLedgerBalance = tbInvestor[0].PreviousLedgerBalance;
                        Result.UserConfig = tbInvestor[0].UserConfig;
                        Result.RefInvestorID = tbInvestor[0].RefInvestorID;

                        //Add Data InvestorProfile To Result
                        Result.Address = tbInvestorProfile[0].Address;
                        Result.City = tbInvestorProfile[0].City;
                        Result.InvestorComment = tbInvestorProfile[0].Comment;
                        Result.Country = tbInvestorProfile[0].Country;
                        Result.Email = tbInvestorProfile[0].Email;
                        Result.NickName = tbInvestorProfile[0].NickName;
                        Result.Phone = tbInvestorProfile[0].Phone;
                        Result.RegisterDay = tbInvestorProfile[0].RegisterDay;
                        Result.State = tbInvestorProfile[0].State;
                        Result.ZipCode = tbInvestorProfile[0].ZipCode;
                        Result.IDPassport = tbInvestorProfile[0].IDPassport;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RowNumber"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByStartEnd(int RowNumber,int Limit)
        {
            List<Business.Investor> Result = new List<Business.Investor>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DSTableAdapters.InvestorProfileTableAdapter adapInvestorProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapInvestorProfile.Connection = conn;
                tbInvestor = adap.GetInvestorByStartEnd(RowNumber, Limit);
                if (tbInvestor != null)
                {
                    int count = tbInvestor.Count;
                    for (int i = 0; i < count; i++)
                    {
                        tbInvestorProfile = adapInvestorProfile.GetInvestorProfileByInvestorID(tbInvestor[i].InvestorID);

                        Business.Investor newInvestor = new Business.Investor();
                        newInvestor.InvestorID = tbInvestor[i].InvestorID;
                        newInvestor.AgentID = tbInvestor[i].AgentID;
                        //Fill Investor Group Instance 
                        if (Business.Market.InvestorGroupList != null)
                        {
                            int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                            for (int j = 0; j < countInvestorGroup; j++)
                            {
                                if (Business.Market.InvestorGroupList[j].InvestorGroupID == tbInvestor[i].InvestorGroupID)
                                {
                                    newInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[j];
                                    break;
                                }
                            }
                        }

                        //newInvestor.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(tbInvestor[i].InvestorGroupID);
                        newInvestor.InvestorStatusID = tbInvestor[i].InvestorStatusID;
                        newInvestor.Balance = tbInvestor[i].Balance;
                        newInvestor.Code = tbInvestor[i].Code;
                        newInvestor.Credit = tbInvestor[i].Credit;
                        newInvestor.IsDisable = tbInvestor[i].IsDisible;
                        newInvestor.TaxRate = tbInvestor[i].TaxRate;
                        newInvestor.Leverage = tbInvestor[i].Leverage;
                        newInvestor.PreviousLedgerBalance = tbInvestor[i].PreviousLedgerBalance;
                        newInvestor.UserConfig = tbInvestor[i].UserConfig;
                        newInvestor.RefInvestorID = tbInvestor[i].RefInvestorID;

                        //Add Data InvestorProfile To Result
                        if (tbInvestorProfile != null && tbInvestorProfile.Count > 0)
                        {
                            newInvestor.InvestorProfileID = tbInvestorProfile[0].InvestorProfileID;
                            newInvestor.Address = tbInvestorProfile[0].Address;
                            newInvestor.City = tbInvestorProfile[0].City;
                            newInvestor.InvestorComment = tbInvestorProfile[0].Comment;
                            newInvestor.Country = tbInvestorProfile[0].Country;
                            newInvestor.Email = tbInvestorProfile[0].Email;
                            newInvestor.NickName = tbInvestorProfile[0].NickName;
                            newInvestor.Phone = tbInvestorProfile[0].Phone;
                            newInvestor.RegisterDay = tbInvestorProfile[0].RegisterDay;
                            newInvestor.State = tbInvestorProfile[0].State;
                            newInvestor.ZipCode = tbInvestorProfile[0].ZipCode;
                            newInvestor.IDPassport = tbInvestorProfile[0].IDPassport;
                        }

                        Result.Add(newInvestor);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                adapInvestorProfile.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// GET INVESTOR BY INVESTOR GROUP ID
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByInvestorGroupID(int InvestorGroupID)
        {
            List<Business.Investor> Result = new List<Business.Investor>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DSTableAdapters.InvestorProfileTableAdapter adapProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapProfile.Connection = conn;
                tbInvestor = adap.GetInvestorByInvestorGroupID(InvestorGroupID);

                if (tbInvestor != null)
                {
                    int count = tbInvestor.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Investor newInvestor = new Business.Investor();
                        tbInvestorProfile = adapProfile.GetInvestorProfileByInvestorID(tbInvestor[i].InvestorID);

                        if (tbInvestorProfile != null && tbInvestorProfile.Count > 0)
                        {
                            newInvestor.InvestorProfileID = tbInvestorProfile[0].InvestorProfileID;
                            newInvestor.Address = tbInvestorProfile[0].Address;
                            newInvestor.Phone = tbInvestorProfile[0].Phone;
                            newInvestor.City = tbInvestorProfile[0].City;
                            newInvestor.Country = tbInvestorProfile[0].Country;
                            newInvestor.Email = tbInvestorProfile[0].Email;
                            newInvestor.ZipCode = tbInvestorProfile[0].ZipCode;
                            newInvestor.RegisterDay = tbInvestorProfile[0].RegisterDay;
                            newInvestor.InvestorComment = tbInvestorProfile[0].Comment;
                            newInvestor.State = tbInvestorProfile[0].State;
                            newInvestor.NickName = tbInvestorProfile[0].NickName;
                            newInvestor.IDPassport = tbInvestorProfile[0].IDPassport;
                        }
                        else
                        {
                            newInvestor.InvestorProfileID = -1;
                            newInvestor.Address = "NaN";
                            newInvestor.Phone = "NaN";
                            newInvestor.City = "NaN";
                            newInvestor.Country = "NaN";
                            newInvestor.Email = "NaN";
                            newInvestor.ZipCode = "NaN";
                            newInvestor.RegisterDay = DateTime.Now;
                            newInvestor.InvestorComment = "NaN";
                            newInvestor.State = "NaN";
                            newInvestor.NickName = "NaN";
                            newInvestor.IDPassport = "";
                        }

                        newInvestor.InvestorID = tbInvestor[i].InvestorID;
                        newInvestor.InvestorStatusID = tbInvestor[i].InvestorStatusID;
                        //newInvestor.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(tbInvestor[i].InvestorGroupID);

                        if (Business.Market.InvestorGroupList != null)
                        {
                            int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                            for (int j = 0; j < countInvestorGroup; j++)
                            {
                                if (Business.Market.InvestorGroupList[j].InvestorGroupID == tbInvestor[i].InvestorGroupID)
                                {
                                    newInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[j];
                                    break;
                                }
                            }
                        }
                        newInvestor.AgentID = tbInvestor[i].AgentID;
                        newInvestor.Balance = tbInvestor[i].Balance;
                        newInvestor.Credit = tbInvestor[i].Credit;
                        newInvestor.Code = tbInvestor[i].Code;
                        newInvestor.IsDisable = tbInvestor[i].IsDisible;
                        newInvestor.TaxRate = tbInvestor[i].TaxRate;
                        newInvestor.Leverage = tbInvestor[i].Leverage;
                        newInvestor.AllowChangePwd = tbInvestor[i].AllowChangePwd;
                        newInvestor.ReadOnly = tbInvestor[i].ReadOnly;
                        newInvestor.SendReport = tbInvestor[i].SendReport;
                        newInvestor.PreviousLedgerBalance = tbInvestor[i].PreviousLedgerBalance;
                        newInvestor.UserConfig = tbInvestor[i].UserConfig;
                        newInvestor.RefInvestorID = tbInvestor[i].RefInvestorID;

                        Result.Add(newInvestor);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                adapProfile.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorGroupID"></param>
        /// <param name="Start"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        internal List<Business.Investor> GetInvestorByInvestorGroup(int InvestorGroupID, int Start, int Limit)
        {
            List<Business.Investor> Result = new List<Business.Investor>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DSTableAdapters.InvestorProfileTableAdapter adapProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapProfile.Connection = conn;

                tbInvestor = adap.GetInvestorByInvestorGroup(Start, InvestorGroupID, Limit);
                if (tbInvestor != null)
                {
                    int count = tbInvestor.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Business.Investor newInvestor = new Business.Investor();
                        tbInvestorProfile = adapProfile.GetInvestorProfileByInvestorID(tbInvestor[i].InvestorID);

                        if (tbInvestorProfile != null && tbInvestorProfile.Count > 0)
                        {
                            newInvestor.InvestorProfileID = tbInvestorProfile[0].InvestorProfileID;
                            newInvestor.Address = tbInvestorProfile[0].Address;
                            newInvestor.Phone = tbInvestorProfile[0].Phone;
                            newInvestor.City = tbInvestorProfile[0].City;
                            newInvestor.Country = tbInvestorProfile[0].Country;
                            newInvestor.Email = tbInvestorProfile[0].Email;
                            newInvestor.ZipCode = tbInvestorProfile[0].ZipCode;
                            newInvestor.RegisterDay = tbInvestorProfile[0].RegisterDay;
                            newInvestor.InvestorComment = tbInvestorProfile[0].Comment;
                            newInvestor.State = tbInvestorProfile[0].State;
                            newInvestor.NickName = tbInvestorProfile[0].NickName;
                            newInvestor.IDPassport = tbInvestorProfile[0].IDPassport;
                        }
                        else
                        {
                            newInvestor.InvestorProfileID = -1;
                            newInvestor.Address = "NaN";
                            newInvestor.Phone = "NaN";
                            newInvestor.City = "NaN";
                            newInvestor.Country = "NaN";
                            newInvestor.Email = "NaN";
                            newInvestor.ZipCode = "NaN";
                            newInvestor.RegisterDay = DateTime.Now;
                            newInvestor.InvestorComment = "NaN";
                            newInvestor.State = "NaN";
                            newInvestor.NickName = "NaN";
                            newInvestor.IDPassport = "";
                        }

                        newInvestor.InvestorID = tbInvestor[i].InvestorID;
                        newInvestor.InvestorStatusID = tbInvestor[i].InvestorStatusID;
                        //newInvestor.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(tbInvestor[i].InvestorGroupID);

                        if (Business.Market.InvestorGroupList != null)
                        {
                            int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                            for (int j = 0; j < countInvestorGroup; j++)
                            {
                                if (Business.Market.InvestorGroupList[j].InvestorGroupID == tbInvestor[i].InvestorGroupID)
                                {
                                    newInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[j];

                                    break;
                                }
                            }
                        }

                        newInvestor.AgentID = tbInvestor[i].AgentID;
                        newInvestor.Balance = tbInvestor[i].Balance;
                        newInvestor.Credit = tbInvestor[i].Credit;
                        newInvestor.Code = tbInvestor[i].Code;
                        newInvestor.IsDisable = tbInvestor[i].IsDisible;
                        newInvestor.TaxRate = tbInvestor[i].TaxRate;
                        newInvestor.Leverage = tbInvestor[i].Leverage;
                        newInvestor.AllowChangePwd = tbInvestor[i].AllowChangePwd;
                        newInvestor.ReadOnly = tbInvestor[i].ReadOnly;
                        newInvestor.SendReport = tbInvestor[i].SendReport;
                        newInvestor.PreviousLedgerBalance = tbInvestor[i].PreviousLedgerBalance;
                        newInvestor.UserConfig = tbInvestor[i].UserConfig;
                        newInvestor.RefInvestorID = tbInvestor[i].RefInvestorID;

                        Result.Add(newInvestor);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                adapProfile.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <returns></returns>
        internal string GetInvestorCodeByInvestorID(int InvestorID)
        {
            string Result = string.Empty;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestorData = new DS.InvestorDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestorData = adap.GetInvestorNameByInvestorID(InvestorID);
                if (tbInvestorData != null)
                {
                    Result = tbInvestorData[0].Code;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal double GetPreviousLedgerBalance(int investorID)
        {
            double result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                adap.Connection = conn;
                tbInvestor = adap.GetPreviousLedgerBalanceByInvestorID(investorID);
                if (tbInvestor != null)
                {
                    result = tbInvestor[0].PreviousLedgerBalance;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestorProfile"></param>
        /// <returns></returns>
        internal int AddNewInvestor(Business.Investor objInvestor)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DSTableAdapters.InvestorProfileTableAdapter adapInvestorProfile = new DSTableAdapters.InvestorProfileTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapInvestorProfile.Connection = conn;

                //string hashPwd = TradingServer.Model.ValidateCheck.GetEncodedString(objInvestor.PrimaryPwd);
                //string hashPwd = TradingServer.Model.ValidateCheck.Encrypt(objInvestor.PrimaryPwd);
                //string hashReadPwd = TradingServer.Model.ValidateCheck.GetEncodedString(objInvestor.ReadOnlyPwd);
                //string hashReadPwd = TradingServer.Model.ValidateCheck.Encrypt(objInvestor.ReadOnlyPwd);
                //string hashPhonePwd = TradingServer.Model.ValidateCheck.GetEncodedString(objInvestor.PhonePwd);
                //string hashPhonePwd = TradingServer.Model.ValidateCheck.Encrypt(objInvestor.PhonePwd);

                if ((objInvestor.InvestorStatusID == -1) && (objInvestor.InvestorGroupInstance.InvestorGroupID == -1) && (string.IsNullOrEmpty(objInvestor.AgentID)))
                {
                    Result = int.Parse(adap.AddNewInvestor(null, null, objInvestor.Balance, objInvestor.Credit, objInvestor.Code, objInvestor.PrimaryPwd, objInvestor.ReadOnlyPwd, objInvestor.PhonePwd,
                        true, objInvestor.TaxRate, objInvestor.Leverage, objInvestor.AllowChangePwd, objInvestor.ReadOnly, objInvestor.SendReport, "0", 0, "", objInvestor.RefInvestorID,
                        objInvestor.AgentRefID, "", "").ToString());
                }
                else
                {
                    int? resultFind = 0;
                    resultFind = adap.FindAgentWithAgentID(objInvestor.AgentID);
                    if (resultFind > 0)
                    {
                        Result = int.Parse(adap.AddNewInvestor(objInvestor.InvestorStatusID, objInvestor.InvestorGroupInstance.InvestorGroupID, objInvestor.Balance,
                            objInvestor.Credit, objInvestor.Code, objInvestor.PrimaryPwd, objInvestor.ReadOnlyPwd, objInvestor.PhonePwd, objInvestor.IsDisable, objInvestor.TaxRate, objInvestor.Leverage,
                            objInvestor.AllowChangePwd, objInvestor.ReadOnly, objInvestor.SendReport, objInvestor.AgentID, 0, "", objInvestor.RefInvestorID, objInvestor.AgentRefID, "", "").ToString());
                    }
                    else
                    {
                        Result = int.Parse(adap.AddNewInvestor(objInvestor.InvestorStatusID, objInvestor.InvestorGroupInstance.InvestorGroupID, objInvestor.Balance,
                            objInvestor.Credit, objInvestor.Code, objInvestor.PrimaryPwd, objInvestor.ReadOnlyPwd, objInvestor.PhonePwd, objInvestor.IsDisable, objInvestor.TaxRate, objInvestor.Leverage,
                            objInvestor.AllowChangePwd, objInvestor.ReadOnly, objInvestor.SendReport, "0", 0, "", objInvestor.RefInvestorID, objInvestor.AgentRefID, "", "").ToString());
                    }
                }

                //int.Parse(adapInvestorProfile.AddNewInvestorProfile(Result, objInvestor.Address, objInvestor.Phone, objInvestor.City, objInvestor.Country, objInvestor.Email,
                //                        objInvestor.ZipCode, objInvestor.RegisterDay, objInvestor.Comment, objInvestor.State, objInvestor.NickName).ToString());

            }
            catch (Exception ex)
            {
                return Result;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        /// <returns></returns>
        internal int CreateNewInvestorProfile(Business.Investor objInvestor)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);            
            DSTableAdapters.InvestorProfileTableAdapter adapInvestorProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            
            try
            {
                conn.Open();                
                adapInvestorProfile.Connection = conn;

                //if ((objInvestor.InvestorStatusID == -1) && (objInvestor.InvestorGroupInstance.InvestorGroupID == -1) && (string.IsNullOrEmpty(objInvestor.AgentID)))
                //{
                //    Result = int.Parse(adap.AddNewInvestor(null, null, objInvestor.Balance, objInvestor.Credit, objInvestor.Code, objInvestor.PrimaryPwd, objInvestor.ReadOnlyPwd, objInvestor.PhonePwd,
                //        true, objInvestor.TaxRate, objInvestor.Leverage, objInvestor.AllowChangePwd, objInvestor.ReadOnly, objInvestor.SendReport, "0", 0).ToString());
                //}
                //else
                //{
                //    int? resultFind = 0;
                //    resultFind = adap.FindAgentWithAgentID(objInvestor.AgentID);
                //    if (resultFind > 0)
                //    {
                //        Result = int.Parse(adap.AddNewInvestor(objInvestor.InvestorStatusID, objInvestor.InvestorGroupInstance.InvestorGroupID, objInvestor.Balance,
                //            objInvestor.Credit, objInvestor.Code, objInvestor.PrimaryPwd, objInvestor.ReadOnlyPwd, objInvestor.PhonePwd, objInvestor.IsDisable, objInvestor.TaxRate, objInvestor.Leverage,
                //            objInvestor.AllowChangePwd, objInvestor.ReadOnly, objInvestor.SendReport, objInvestor.AgentID, 0).ToString());
                //    }
                //    else
                //    {
                //        Result = int.Parse(adap.AddNewInvestor(objInvestor.InvestorStatusID, objInvestor.InvestorGroupInstance.InvestorGroupID, objInvestor.Balance,
                //            objInvestor.Credit, objInvestor.Code, objInvestor.PrimaryPwd, objInvestor.ReadOnlyPwd, objInvestor.PhonePwd, objInvestor.IsDisable, objInvestor.TaxRate, objInvestor.Leverage,
                //            objInvestor.AllowChangePwd, objInvestor.ReadOnly, objInvestor.SendReport, "0", 0).ToString());
                //    }
                //}

                Result = int.Parse(adapInvestorProfile.AddNewInvestorProfile(objInvestor.InvestorID, objInvestor.Address, objInvestor.Phone, objInvestor.City, objInvestor.Country, objInvestor.Email,
                                        objInvestor.ZipCode, objInvestor.RegisterDay, objInvestor.InvestorComment, objInvestor.State, objInvestor.NickName, objInvestor.IDPassport).ToString());

            }
            catch (Exception ex)
            {
                return Result;
            }
            finally
            {
                adapInvestorProfile.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        internal Business.Investor LoginSystem(string Code, string Password)
        {
            Business.Investor Result = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adapInvestor = new DSTableAdapters.InvestorTableAdapter();
            DSTableAdapters.InvestorProfileTableAdapter adapInvestorProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adapInvestor.Connection = conn;
                adapInvestorProfile.Connection = conn;

                //string hash = Model.ValidateCheck.GetEncodedString(Password);

                tbInvestor = adapInvestor.LoginSystem(Code, Password);
                if (tbInvestor != null && tbInvestor.Count > 0)
                {
                    //Add Data From Table Investor To Result
                    Result.InvestorID = tbInvestor[0].InvestorID;
                    Result.AgentID = tbInvestor[0].AgentID;
                    Result.Balance = tbInvestor[0].Balance;
                    Result.Code = tbInvestor[0].Code;
                    Result.Credit = tbInvestor[0].Credit;
                    //Result.IsOnline = true;
                    Result.IsDisable = tbInvestor[0].IsDisible;
                    Result.ReadOnly = tbInvestor[0].ReadOnly;
                    Result.IsReadOnly = false;

                    if (Business.Market.InvestorGroupList != null)
                    {
                        int count = Business.Market.InvestorGroupList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorGroupList[i].InvestorGroupID == tbInvestor[0].InvestorGroupID)
                            {
                                Result.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                                break;
                            }
                        }
                    }

                    Result.InvestorStatusID = tbInvestor[0].InvestorStatusID;
                    Result.IsDisable = tbInvestor[0].IsDisible;
                    Result.Leverage = tbInvestor[0].Leverage;
                    Result.PreviousLedgerBalance = tbInvestor[0].PreviousLedgerBalance;

                    tbInvestorProfile = adapInvestorProfile.GetInvestorProfileByInvestorID(tbInvestor[0].InvestorID);
                    if (tbInvestorProfile != null && tbInvestorProfile.Count > 0)
                    {
                        //Add Data From Table InvestorProfile To Result
                        Result.Address = tbInvestorProfile[0].Address;
                        Result.City = tbInvestorProfile[0].City;
                        Result.InvestorComment = tbInvestorProfile[0].Comment;
                        Result.Country = tbInvestorProfile[0].Country;
                        Result.Email = tbInvestorProfile[0].Email;
                        Result.NickName = tbInvestorProfile[0].NickName;
                        Result.Phone = tbInvestorProfile[0].Phone;
                        Result.RegisterDay = tbInvestorProfile[0].RegisterDay;
                        Result.State = tbInvestorProfile[0].State;
                        Result.ZipCode = tbInvestorProfile[0].ZipCode;
                        Result.InvestorProfileID = tbInvestorProfile[0].InvestorProfileID;
                        Result.IDPassport = tbInvestorProfile[0].IDPassport;
                    }
                }                
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adapInvestor.Connection.Close();
                adapInvestorProfile.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="readPwd"></param>
        /// <returns></returns>
        internal Business.Investor LoginWithReadPwd(string code, string readPwd)
        {
            Business.Investor result = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DSTableAdapters.InvestorProfileTableAdapter adapInvestorProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapInvestorProfile.Connection = conn;
                tbInvestor = adap.LoginWithReadPwd(code, readPwd);

                //string hash = Model.ValidateCheck.GetEncodedString(Password);
                if (tbInvestor != null && tbInvestor.Count > 0)
                {
                    //Add Data From Table Investor To Result
                    result.InvestorID = tbInvestor[0].InvestorID;
                    result.AgentID = tbInvestor[0].AgentID;
                    result.Balance = tbInvestor[0].Balance;
                    result.Code = tbInvestor[0].Code;
                    result.Credit = tbInvestor[0].Credit;
                    //Result.IsOnline = true;
                    result.IsDisable = tbInvestor[0].IsDisible;
                    result.ReadOnly = tbInvestor[0].ReadOnly;
                    result.IsReadOnly = true;

                    if (Business.Market.InvestorGroupList != null)
                    {
                        int count = Business.Market.InvestorGroupList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorGroupList[i].InvestorGroupID == tbInvestor[0].InvestorGroupID)
                            {
                                result.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                                break;
                            }
                        }
                    }

                    result.InvestorStatusID = tbInvestor[0].InvestorStatusID;
                    result.IsDisable = tbInvestor[0].IsDisible;
                    result.Leverage = tbInvestor[0].Leverage;
                    result.PreviousLedgerBalance = tbInvestor[0].PreviousLedgerBalance;

                    tbInvestorProfile = adapInvestorProfile.GetInvestorProfileByInvestorID(tbInvestor[0].InvestorID);
                    if (tbInvestorProfile != null && tbInvestorProfile.Count > 0)
                    {
                        //Add Data From Table InvestorProfile To Result
                        result.Address = tbInvestorProfile[0].Address;
                        result.City = tbInvestorProfile[0].City;
                        result.InvestorComment = tbInvestorProfile[0].Comment;
                        result.Country = tbInvestorProfile[0].Country;
                        result.Email = tbInvestorProfile[0].Email;
                        result.NickName = tbInvestorProfile[0].NickName;
                        result.Phone = tbInvestorProfile[0].Phone;
                        result.RegisterDay = tbInvestorProfile[0].RegisterDay;
                        result.State = tbInvestorProfile[0].State;
                        result.ZipCode = tbInvestorProfile[0].ZipCode;
                        result.InvestorProfileID = tbInvestorProfile[0].InvestorProfileID;
                        result.IDPassport = tbInvestorProfile[0].IDPassport;
                    }
                }    
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal Business.Investor CheckMasterPassword(int investorID, string password)
        {
            Business.Investor result = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();            
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();            

            try
            {
                conn.Open();
                adap.Connection = conn;                
                tbInvestor = adap.VerifyMasterPassword(investorID, password);

                //string hash = Model.ValidateCheck.GetEncodedString(Password);
                if (tbInvestor != null && tbInvestor.Count > 0)
                {
                    //Add Data From Table Investor To Result
                    result.InvestorID = tbInvestor[0].InvestorID;
                    result.AgentID = tbInvestor[0].AgentID;
                    result.Balance = tbInvestor[0].Balance;
                    result.Code = tbInvestor[0].Code;
                    result.Credit = tbInvestor[0].Credit;
                    //Result.IsOnline = true;
                    result.IsDisable = tbInvestor[0].IsDisible;
                    result.ReadOnly = true;

                    if (Business.Market.InvestorGroupList != null)
                    {
                        int count = Business.Market.InvestorGroupList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (Business.Market.InvestorGroupList[i].InvestorGroupID == tbInvestor[0].InvestorGroupID)
                            {
                                result.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                                break;
                            }
                        }
                    }

                    result.InvestorStatusID = tbInvestor[0].InvestorStatusID;
                    result.IsDisable = tbInvestor[0].IsDisible;
                    result.Leverage = tbInvestor[0].Leverage;
                    result.PreviousLedgerBalance = tbInvestor[0].PreviousLedgerBalance;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        internal Business.Investor LoginAgent(string Code, string Password)
        {
            Business.Investor Result = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adapInvestor = new DSTableAdapters.InvestorTableAdapter();            
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();            

            try
            {
                conn.Open();
                adapInvestor.Connection = conn;                

                //string hash = Model.ValidateCheck.GetEncodedString(Password);
                //string hash = Model.ValidateCheck.Encrypt(Password);

                tbInvestor = adapInvestor.LoginSystem(Code, Password);
                if (tbInvestor != null)
                {
                    //Add Data From Table Investor To Result
                    Result.InvestorID = tbInvestor[0].InvestorID;
                    //Result.AgentID = tbInvestor[0].AgentID;
                    //Result.Balance = tbInvestor[0].Balance;
                    //Result.Code = tbInvestor[0].Code;
                    //Result.Credit = tbInvestor[0].Credit;

                    //if (Business.Market.InvestorGroupList != null)
                    //{
                    //    int count = Business.Market.InvestorGroupList.Count;
                    //    for (int i = 0; i < count; i++)
                    //    {
                    //        if (Business.Market.InvestorGroupList[i].InvestorGroupID == tbInvestor[0].InvestorGroupID)
                    //        {
                    //            Result.InvestorGroupInstance = Business.Market.InvestorGroupList[i];
                    //            break;
                    //        }
                    //    }
                    //}

                    //Result.InvestorStatusID = tbInvestor[0].InvestorStatusID;
                    //Result.IsDisable = tbInvestor[0].IsDisible;
                    //Result.Leverage = tbInvestor[0].Leverage;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adapInvestor.Connection.Close();                
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        internal int DeleteInvestorByInvestorID(int InvestorID)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                Result = adap.DeleteInvestorByInvestorID(InvestorID);
            }
            catch (Exception ex)
            {
                Result = -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInvestor"></param>
        internal bool UpdateInvestor(Business.Investor objInvestor)
        {
            bool Result = false; 
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;

                int resultUpdate = adap.UpdateInvestor(objInvestor.InvestorStatusID, objInvestor.InvestorGroupInstance.InvestorGroupID, objInvestor.IsDisable, objInvestor.TaxRate,
                    objInvestor.Leverage, objInvestor.AllowChangePwd, objInvestor.ReadOnly, objInvestor.SendReport, objInvestor.AgentID, objInvestor.RefInvestorID, objInvestor.AgentRefID, objInvestor.InvestorID);
                if (resultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal Business.Investor SelectInvestorByCode(string Code)
        {
            Business.Investor newInvestor = new Business.Investor();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            DSTableAdapters.InvestorProfileTableAdapter adapProfile = new DSTableAdapters.InvestorProfileTableAdapter();
            DS.InvestorProfileDataTable tbInvestorProfile = new DS.InvestorProfileDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                adapProfile.Connection = conn;
                tbInvestor = adap.SelectInvestorByCode(Code);                

                if (tbInvestor != null)
                {
                    tbInvestorProfile = adapProfile.GetInvestorProfileByInvestorID(tbInvestor[0].InvestorID);

                    if (tbInvestorProfile != null && tbInvestorProfile.Count > 0)
                    {
                        newInvestor.InvestorProfileID = tbInvestorProfile[0].InvestorProfileID;
                        newInvestor.Address = tbInvestorProfile[0].Address;
                        newInvestor.Phone = tbInvestorProfile[0].Phone;
                        newInvestor.City = tbInvestorProfile[0].City;
                        newInvestor.Country = tbInvestorProfile[0].Country;
                        newInvestor.Email = tbInvestorProfile[0].Email;
                        newInvestor.ZipCode = tbInvestorProfile[0].ZipCode;
                        newInvestor.RegisterDay = tbInvestorProfile[0].RegisterDay;
                        newInvestor.InvestorComment = tbInvestorProfile[0].Comment;
                        newInvestor.State = tbInvestorProfile[0].State;
                        newInvestor.NickName = tbInvestorProfile[0].NickName;
                        newInvestor.IDPassport = tbInvestorProfile[0].IDPassport;
                    }
                    else
                    {
                        newInvestor.InvestorProfileID = -1;
                        newInvestor.Address = "NaN";
                        newInvestor.Phone = "NaN";
                        newInvestor.City = "NaN";
                        newInvestor.Country = "NaN";
                        newInvestor.Email = "NaN";
                        newInvestor.ZipCode = "NaN";
                        newInvestor.RegisterDay = DateTime.Now;
                        newInvestor.InvestorComment = "NaN";
                        newInvestor.State = "NaN";
                        newInvestor.NickName = "NaN";
                        newInvestor.IDPassport = "";
                    }
                    newInvestor.InvestorID = tbInvestor[0].InvestorID;
                    newInvestor.InvestorStatusID = tbInvestor[0].InvestorStatusID;
                    //newInvestor.InvestorGroupInstance = TradingServer.Facade.FacadeGetInvestorGroupByInvestorGroupID(tbInvestor[0].InvestorGroupID);

                    if (Business.Market.InvestorGroupList != null)
                    {
                        int countInvestorGroup = Business.Market.InvestorGroupList.Count;
                        for (int j = 0; j < countInvestorGroup; j++)
                        {
                            if (Business.Market.InvestorGroupList[j].InvestorGroupID == tbInvestor[0].InvestorGroupID)
                            {
                                newInvestor.InvestorGroupInstance = Business.Market.InvestorGroupList[j];
                                break;
                            }
                        }
                    }

                    newInvestor.AgentID = tbInvestor[0].AgentID;
                    newInvestor.Balance = tbInvestor[0].Balance;
                    newInvestor.Credit = tbInvestor[0].Credit;
                    newInvestor.Code = tbInvestor[0].Code;
                    newInvestor.IsDisable = tbInvestor[0].IsDisible;
                    newInvestor.TaxRate = tbInvestor[0].TaxRate;
                    newInvestor.Leverage = tbInvestor[0].Leverage;
                    newInvestor.AllowChangePwd = tbInvestor[0].AllowChangePwd;
                    newInvestor.ReadOnly = tbInvestor[0].ReadOnly;
                    newInvestor.SendReport = tbInvestor[0].SendReport;
                    newInvestor.PreviousLedgerBalance = tbInvestor[0].PreviousLedgerBalance;
                    newInvestor.PhonePwd = tbInvestor[0].PhonePwd;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                adapProfile.Connection.Close();
                conn.Close();
            }

            return newInvestor;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>
        internal bool UpdateBalanceAccount(int InvestorID, double Balance)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            //SqlTransaction tran;
            
            //tran = conn.BeginTransaction();
            //adap.Transaction = tran;

            try
            {
                conn.Open();
                adap.Connection = conn;

                int UpdateBalance = adap.UpdateBalanceAccount(Math.Round(Balance, 2), InvestorID);
                if (UpdateBalance > 0)
                {
                    Result = true;
                    //tran.Commit();
                }
                else
                {
                    TradingServer.Facade.FacadeAddNewSystemLog(1, "update balance investor id: " + InvestorID + " unsuccessfull", "[update balance]", "", "");
                }
            }
            catch (Exception ex)
            {
                //tran.Rollback();
                return false;
            }
            finally
            {                
                //tran.Dispose();
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Credit"></param>
        /// <returns></returns>
        internal bool UpdateCreditAccount(int InvestorID, double Credit)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();            

            try
            {                
                conn.Open();
                adap.Connection = conn;
                int UpdateCredit = adap.UpdateCredit(Math.Round(Credit, 2), InvestorID);
                if (UpdateCredit > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvestorID"></param>
        /// <param name="Balance"></param>
        /// <param name="Credit"></param>
        /// <returns></returns>
        internal bool UpdateBalanceAndCredit(int InvestorID, double Balance, double Credit)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int ResultUpdate = adap.UpdateBalanceAndCredit(Math.Round(Balance, 2), Math.Round(Credit, 2), InvestorID);
                if (ResultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// UPDATE PASSWORD BY CODE
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal bool UpdatePasswordByCode(string Code,string Pwd)
        {
            bool Result=false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                //string hashPwd = TradingServer.Model.ValidateCheck.GetEncodedString(Pwd);
                //string hashPwd = TradingServer.Model.ValidateCheck.Encrypt(Pwd);
                int ResultUpdate = adap.UpdatePasswordByCode(Pwd, Code);
                if (ResultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentRefID"></param>
        /// <param name="investorID"></param>
        /// <returns></returns>
        internal bool UpdateAgentRefID(int agentRefID, int investorID)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int ResultUpdate = adap.UpdateAgentRefID(agentRefID, investorID);
                if (ResultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal bool UpdateReadPasswordByCode(string Code, string Pwd)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                //string hashPwd = TradingServer.Model.ValidateCheck.GetEncodedString(Pwd);
                //string hashPwd = TradingServer.Model.ValidateCheck.Encrypt(Pwd);
                int ResultUpdate = adap.UpdateReadPasswordByCode(Pwd, Code);
                if (ResultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        internal bool UpdatePhonePasswordByCode(string Code, string Pwd)
        {
            bool Result=false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                //string hashPwd = TradingServer.Model.ValidateCheck.GetEncodedString(Pwd);
                int ResultUpdate = adap.UpdatePhonePasswordByCode(Pwd, Code);
                if (ResultUpdate > 0)
                    Result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="previousLedgerBalance"></param>
        /// <returns></returns>
        internal bool UpdatePreviousLedgerBalance(int investorID, double previousLedgerBalance)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdatePreviousLedgerBalance(previousLedgerBalance, investorID);
                if (resultUpdate > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal bool GetCodeInvestor(string Code)
        {
            bool Result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();

            try
            {   
                conn.Open();
                adap.Connection = conn;
                tbInvestor = adap.GetCodeInvestor(Code);

                if (tbInvestor.Count > 0)
                {
                    Result = false;
                }
                else
                {
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        internal int GetInvestorIDByCode(string Code)
        {
            int Result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();

            try
            {
                conn.Open();
                adap.Connection = conn;
                tbInvestor = adap.GetInvestorIDByCode(Code.Trim());

                if (tbInvestor != null)
                    Result = tbInvestor[0].InvestorID;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listInvestorID"></param>
        /// <returns></returns>
        internal Dictionary<int, string> GetCodeByListInvestor(List<int> listInvestorID)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);            
            DS.InvestorDataTable tbInvestor = new DS.InvestorDataTable();
            System.Data.SqlClient.SqlDataAdapter adap = new System.Data.SqlClient.SqlDataAdapter();

            try
            {
                conn.Open();
                string listInvestor = string.Empty;
                if (listInvestorID != null)
                {
                    int count = listInvestorID.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (i == count - 1)
                        {
                            listInvestor += listInvestorID[i];
                        }
                        else
                        {
                            listInvestor += listInvestorID[i] + ",";
                        }
                    }
                }

                string sql = "SELECT InvestorID,Code FROM Investor WHERE (InvestorID IN(" + listInvestor + "))";

                adap.SelectCommand = new System.Data.SqlClient.SqlCommand(sql, conn);
                
                adap.Fill(tbInvestor);

                if (tbInvestor != null)
                {
                    int count = tbInvestor.Count;
                    for (int i = 0; i < count; i++)
                    {
                        result.Add(tbInvestor[i].InvestorID, tbInvestor[i].Code);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        internal bool UpdatePrimaryPasword(int investorID, string newPassword)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdatePrimaryPassword(newPassword, investorID);
                if (resultUpdate > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        internal bool UpdateReadOnlyPassword(int investorID, string newPassword)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int updateReadPwd = adap.UpdateReadOnlyPassword(newPassword, investorID);

                if (updateReadPwd > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        internal bool UpdateAllUserConfig(string userConfig, string userConfigIpad, string userConfigIphone, int investorID)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateAllUserConfig(userConfig, userConfigIpad, userConfigIphone, investorID);
                if (resultUpdate > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        internal bool UpdateUserConfig(int investorID, string userConfig)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateUserConfig(userConfig, investorID);
                if (resultUpdate > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        internal bool UpdateUserConfigIpad(int investorID, string userConfig)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateUserConfigIpad(userConfig, investorID);
                if (resultUpdate > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="investorID"></param>
        /// <param name="userConfig"></param>
        /// <returns></returns>
        internal bool UpdateUserConfigIphone(int investorID, string userConfig)
        {
            bool result = false;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                int resultUpdate = adap.UpdateUserConfigIphone(userConfig, investorID);
                if (resultUpdate > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal int CountInvestor()
        {
            int? result = -1;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.InvestorTableAdapter adap = new DSTableAdapters.InvestorTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = adap.CountInvestor();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result.Value;
        }
    }
}
