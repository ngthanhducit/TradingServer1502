using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.Model
{
    public class Helper
    {
        #region create instance class helper
        private static Model.Helper _instance;
        public static Model.Helper Instance
        {
            get
            {
                if (Model.Helper._instance == null)
                    Model.Helper._instance = new Helper();

                return Model.Helper._instance;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public string convertCommandTypeIDToString(int typeID)
        {
            string result = string.Empty;
            if (typeID == 1 || typeID == 2)
            {
                result = "Open";
            }
            else
            {
                if (typeID == 7)
                {
                    result = "BuyLimit";
                }

                if (typeID == 8)
                {
                    result = "SellLimit";
                }

                if (typeID == 9)
                {
                    result = "BuyStop";
                }

                if (typeID == 10)
                {
                    result = "SellStop";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public int ConvertTypeToMT4Type(int typeID)
        {
            int result = -1;
            switch (typeID)
            {
                case 1:
                    result = 0;
                    break;
                case 2:
                    result = 1;
                    break;
                case 7:
                    result = 2;
                    break;
                case 8:
                    result = 3;
                    break;
                case 9:
                    result = 4;
                    break;
                case 10:
                    result = 5;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public bool IsBuy(int typeID)
        {
            bool result = false;

            if (typeID == 1 || typeID == 7 || typeID == 9)
                result = true;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="commandName"></param>
        /// <param name="message"></param>
        /// <param name="isSucess"></param>
        public void SendCommandToClient(Business.OpenTrade Command, EnumET5.CommandName commandName, EnumET5.ET5Message message, bool isSucess, int result, int typeMessage)
        {
            string msg = string.Empty;

            switch (typeMessage) //0: Add Command - 1: Close Command - 2: Update Command
            {
                case 0:
                    msg = commandName.ToString() + "$" + isSucess + "," + this.GetMessage(message) + "," + result + "," + Command.Investor.InvestorID + "," +
                               Command.Symbol.Name + "," + Command.Size + "," + Model.Helper.Instance.IsBuy(Command.Type.ID) + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," +
                                   Command.TakeProfit + "," + Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + Command.Comment + "," + Command.ID + "," +
                                   Model.Helper.Instance.convertCommandTypeIDToString(Command.Type.ID) + "," + 1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," +
                                   Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin + ",Open";
                    break;

                case 1:
                    msg = commandName.ToString() + "$" + isSucess + "," + this.GetMessage(message) + "," + result + "," + Command.Investor.InvestorID + "," + Command.Symbol.Name + "," +
                                   Command.Size + "," + true + "," + Command.OpenTime + "," + Command.OpenPrice + "," + Command.StopLoss + "," + Command.TakeProfit + "," +
                                   Command.ClosePrice + "," + Command.Commission + "," + Command.Swap + "," + Command.Profit + "," + Command.Comment + "," + Command.ID + "," + Command.Type.Name + "," +
                                   1 + "," + Command.ExpTime + "," + Command.ClientCode + "," + Command.CommandCode + "," + Command.IsHedged + "," + Command.Type.ID + "," + Command.Margin +
                                   ",Close," + Command.CloseTime;
                    break;

                case 2:
                    msg = commandName.ToString() + "$" + isSucess + "," + this.GetMessage(message) + "," + Command.ID + "," +
                                                                                Command.Investor.InvestorID + "," +
                                                                                Command.Symbol.Name + "," +
                                                                                Command.Size + "," + Model.Helper.Instance.IsBuy(Command.Type.ID) + "," +
                                                                                Command.OpenTime + "," +
                                                                                Command.OpenPrice + "," +
                                                                                Command.StopLoss + "," +
                                                                                Command.TakeProfit + "," +
                                                                                Command.ClosePrice + "," +
                                                                                Command.Commission + "," +
                                                                                Command.Swap + "," +
                                                                                Command.Profit + "," +
                                                                                Command.Comment + "," +
                                                                                Command.ID + "," +
                                                                                Command.Type.Name + "," +
                                                                                1 + "," + Command.ExpTime + "," +
                                                                                Command.ClientCode + "," +
                                                                                Command.CommandCode + "," +
                                                                                Command.IsHedged + "," +
                                                                                Command.Type.ID + "," +
                                                                                Command.Margin + ",Update";
                    break;
            }
            
            if (Command.Investor.ClientCommandQueue == null)
                Command.Investor.ClientCommandQueue = new List<string>();

            Command.Investor.ClientCommandQueue.Add(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal string GetMessage(EnumET5.ET5Message message)
        {
            string result = string.Empty;
            switch (message)
            {
                case EnumET5.ET5Message.DISCONNECT_FROM_MT4:
                    result = "DISCONNECT FROM MT4";
                    break;

                case EnumET5.ET5Message.MT4_CAN_NOT_MAKE_COMMAND:
                    result = "MT4 CAN'T MAKE COMMAND";
                    break;

                case EnumET5.ET5Message.NO_RESULT:
                    result = "no result.";
                    break;

                case EnumET5.ET5Message.COMMON_ERROR:
                    result = "common error.";
                    break;

                case EnumET5.ET5Message.INVALID_TRADE_PARAMETERS:
                    result = "Invalid trade parameters.";
                    break;

                case EnumET5.ET5Message.TRADE_SERVER_IS_BUSY:
                    result = "Trade server is busy.";
                    break;

                case EnumET5.ET5Message.OLD_VERSION_OF_THE_CLIENT_TERMINAL:
                    result = "Old version of the client terminal.";
                    break;

                case EnumET5.ET5Message.NO_CONNECTION_WITH_TRADE_SERVER:
                    result = "No connection with trade server.";
                    break;

                case EnumET5.ET5Message.TOO_FREQUENT_REQUESTS:
                    result = "Too frequent requests.";
                    break;

                case EnumET5.ET5Message.ACCOUNT_DISABLED:
                    result = "Account disabled.";
                    break;

                case EnumET5.ET5Message.INVALID_ACCOUNT:
                    result = "Invalid account.";
                    break;

                case EnumET5.ET5Message.TRADE_TIMEOUT:
                    result = "Trade timeout.";
                    break;

                case EnumET5.ET5Message.INVALID_PRICE:
                    result = "Invalid price.";
                    break;

                case EnumET5.ET5Message.INVALID_TICKET:
                    result = "Invalid ticket.";
                    break;

                case EnumET5.ET5Message.INVALID_TRADE_VOLUME:
                    result = "Invalid trade volume.";
                    break;

                case EnumET5.ET5Message.NOT_ENOUGH_MONEY:
                    result = "Not enough money.";
                    break;

                case EnumET5.ET5Message.OFF_QUOTES:
                    result = "Off quotes.";
                    break;

                case EnumET5.ET5Message.INVALID_FUNCTION_PARAMETER_VALUE:
                    result = "Invalid function parameter value.";
                    break;

                case EnumET5.ET5Message.CUSTOM_INDICATOR_ERROR:
                    result = "Custom indicator error.";
                    break;

                case EnumET5.ET5Message.STRING_PARAMETER_EXPECTED:
                    result = "String parameter expected.";
                    break;

                case EnumET5.ET5Message.INTEGER_PARAMETER_EXPECTED:
                    result = "Integer parameter expected.";
                    break;

                case EnumET5.ET5Message.UNKNOWN_SYMBOL:
                    result = "Unknown symbol.";
                    break;

                case EnumET5.ET5Message.TRADE_IS_NOT_ALLOWED_ENABLE_CHECKBOX_ALLOW_LIVE_TRADING_IN_THE_EXPERT_PROPERTIES:
                    result = "Trade is not allowed. Enable checkbox \"Allow live trading\" in the expert properties.";
                    break;

                case EnumET5.ET5Message.LONGS_ARE_NOT_ALLOWED_CHECK_THE_EXPERT_PROPERTIES:
                    result = "Longs are not allowed. Check the expert properties.";
                    break;

                case EnumET5.ET5Message.SHORTS_ARE_NOT_ALLOWED_CHECK_THE_EXPERT_PROPERTIES:
                    result = "Shorts are not allowed. Check the expert properties.";
                    break;

                case EnumET5.ET5Message.INVALID_STOPS:
                    result = "invalid stops.";
                    break;

                case EnumET5.ET5Message.TRADE_IS_DISABLED:
                    result = "trade is disabled.";
                    break;

                case EnumET5.ET5Message.UNKNOWN_ERROR:
                    result = "unknown error.";
                    break;

                default:
                    result = "UNKNOWN ERROR";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public bool IsMT4ErrorCode(int ticket)
        {
            bool result = false;

            if (ticket == 138 || ticket == 4051 || ticket == 4055 || ticket == 4062 || ticket == 4063 ||
                        ticket == 4106 || ticket == 4107 || ticket == 4109 || ticket == 4110 || ticket == 4111 ||
                        ticket == 2 || ticket == 3 || ticket == 4 || ticket == 5 || ticket == 6 || ticket == 8 ||
                        ticket == 64 || ticket == 65 || ticket == 128 || ticket == 129 || ticket == 134 || ticket == 136 ||
                        ticket == 135 || ticket == 2004 || ticket == 9999 || ticket == 130 || ticket == 133 || ticket == 1
                        || ticket == 131)
                result = true;

            return result;
        }
    }
}
