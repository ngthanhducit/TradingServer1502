using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TradingServer.NJ4XConnectSocket
{
    public class NJ4XConnectSocketAsync
    {
        private static Socket SocketClient;

        private static List<NJ4XConnectSocket.SocketTicket> SocketTickets;
        private static Thread ThreadSendSocket;
        private static bool IsProcess;

        private static NJ4XConnectSocket.NJ4XConnectSocketAsync _instance;
        public static NJ4XConnectSocket.NJ4XConnectSocketAsync Instance
        {
            get
            {
                if (NJ4XConnectSocket.NJ4XConnectSocketAsync._instance == null)
                    NJ4XConnectSocket.NJ4XConnectSocketAsync._instance = new NJ4XConnectSocketAsync();

                return NJ4XConnectSocket.NJ4XConnectSocketAsync._instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NJ4XConnectSocketAsync()
        {
            NJ4XConnectSocketAsync.SocketTickets = new List<SocketTicket>();
            NJ4XConnectSocketAsync.ThreadSendSocket = new Thread(new ThreadStart(this.ProcessSendNJ4X));
            //NJ4XConnectSocketAsync.ThreadSendSocket.Start();
            NJ4XConnectSocketAsync.IsProcess = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessSendNJ4X()
        {
            NJ4XConnectSocket.SocketTicket data = null;
            while (NJ4XConnectSocketAsync.IsProcess)
            {
                data = this.GetNJ4XTicket();
                while (data != null)
                {
                    string result = string.Empty;
                    result = this.SendNJ4X(data.Cmd);

                    if (!string.IsNullOrEmpty(result))
                    {
                        string[] subValue = result.Split('$');
                        switch (subValue[0])
                        {
                            case "OrderSend":
                                {
                                    data.CmdResult = result;
                                    data.Ticket = int.Parse(subValue[1]);
                                    data.IsDisable = true;
                                    data.IsSuccess = true;
                                }
                                break;

                            case "OrderClose":
                                {
                                    string[] subParameter = subValue[1].Split('{');
                                    data.CmdResult = result;
                                    data.Ticket = int.Parse(subParameter[1]);
                                    data.IsDisable = true;
                                    data.IsSuccess = true;
                                }
                                break;
                        }
                    }
                    else
                    {
                        string[] subValue = data.Cmd.Split('$');
                        switch (subValue[0])
                        {
                            case "OrderSend":
                                data.CmdResult = "OrderSend$" + 9999;
                                data.Ticket = -1;
                                data.IsDisable = true;
                                data.IsSuccess = true;
                                break;

                            case "OrderClose":
                                data.CmdResult = "OrderClose$False{" + 9999;
                                data.IsDisable = true;
                                data.IsSuccess = true;
                                break;
                        }
                    }

                    data = this.GetNJ4XTicket();
                    System.Threading.Thread.Sleep(10);
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private NJ4XConnectSocket.SocketTicket GetNJ4XTicket()
        {
            NJ4XConnectSocket.SocketTicket data = null;

            if (NJ4XConnectSocketAsync.SocketTickets != null && NJ4XConnectSocketAsync.SocketTickets.Count > 0)
            {
                if (NJ4XConnectSocketAsync.SocketTickets[0] != null)
                {
                    data = NJ4XConnectSocketAsync.SocketTickets[0];
                    NJ4XConnectSocketAsync.SocketTickets.Remove(data);
                }
                else
                    NJ4XConnectSocketAsync.SocketTickets.RemoveAt(0);
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void StartClient(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP  socket.
            NJ4XConnectSocketAsync.SocketClient = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            NJ4XConnectSocketAsync.SocketClient.ReceiveTimeout = 60000;

            NJ4XConnectSocketAsync.SocketClient.Connect(remoteEP);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        public void SendNJ4X(NJ4XConnectSocket.SocketTicket ticket)
        {
            NJ4XConnectSocketAsync.SocketTickets.Add(ticket);
        }

        /// <summary>
        /// 
        /// </summary>
        public string SendNJ4X(string data)
        {
            string result = string.Empty;

            IPAddress ipAddress = IPAddress.Parse(Business.Market.DEFAULT_IPADDRESS_LOCAL);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Business.Market.DEFAULT_NJ4X);
            // Create a TCP/IP  socket.
            Socket sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            //sender.ReceiveTimeout = 90000;

            sender.Connect(remoteEP);

            byte[] bytes = new byte[65507];

            byte[] msg = Encoding.ASCII.GetBytes(data);
            int bytesSend = sender.Send(msg);
            int bytesRec = sender.Receive(bytes);
            result = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            // Release the socket.
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

            return result;
        }
    }
}
