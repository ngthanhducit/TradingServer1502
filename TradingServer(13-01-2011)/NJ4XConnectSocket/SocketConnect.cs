using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TradingServer.NJ4XConnectSocket
{
    internal class SocketConnect
    {
        private static Socket senderClient;
        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        string loginKey = string.Empty;
        bool isCompleteConnect = false;
        private static String response = String.Empty;
        static Thread threadStartClient;

        private string feedName = string.Empty;

        #region CREATE INSTANCE NJ4XCONNECT SOCKET
        private static NJ4XConnectSocket.SocketConnect _instance;
        public static NJ4XConnectSocket.SocketConnect Instance
        {
            get
            {
                if (NJ4XConnectSocket.SocketConnect._instance == null)
                    NJ4XConnectSocket.SocketConnect._instance = new SocketConnect();

                return NJ4XConnectSocket.SocketConnect._instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        internal void StartClient(string ipAddress, int port)
        {
            // Connect to a remote device.
            try
            {
                this.feedName = feedName;

                // Establish the remote endpoint for the socket.
                // The name of the 
                // remote device is "host.contoso.com".
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());

                IPAddress ipAdd = System.Net.IPAddress.Parse(ipAddress);
                IPEndPoint remoteEP = new IPEndPoint(ipAdd, port);

                SocketConnect.senderClient = new Socket(AddressFamily.InterNetwork,
                   SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                SocketConnect.senderClient.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), SocketConnect.senderClient);
                connectDone.WaitOne();

                // Receive the response from the remote device.
                Receive();
                receiveDone.WaitOne();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                // Signal that the connection has been made.
                connectDone.Set();

                // Send test data to the remote device.
                this.Send("80737871~" + this.feedName);//PING
                sendDone.WaitOne();
            }
            catch (Exception e)
            {
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        internal void Send(String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                Socket hanler = SocketConnect.senderClient;
                // Begin sending the data to the remote device.
                hanler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), SocketConnect.senderClient);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        internal void Receive()
        {
            try
            {
                // Create the state object.
                NJ4XConnectSocket.StateObject state = new NJ4XConnectSocket.StateObject();
                state.workSocket = NJ4XConnectSocket.SocketConnect.senderClient;

                // Begin receiving the data from the remote device.
                NJ4XConnectSocket.SocketConnect.senderClient.BeginReceive(state.buffer, 0, NJ4XConnectSocket.StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                NJ4XConnectSocket.StateObject state = (NJ4XConnectSocket.StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb = new StringBuilder();
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    string[] subValue = state.sb.ToString().Split('~');

                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, NJ4XConnectSocket.StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
