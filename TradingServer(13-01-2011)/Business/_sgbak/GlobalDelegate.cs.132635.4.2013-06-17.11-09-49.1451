using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Reflection;
namespace TradingServer.Business
{
    internal class GlobalDelegate
    {
        [DllImport("LibraryAPI.dll")]
        private static extern void SetGlobalDelegate(Callback cb);
        [DllImport("LibraryAPI.dll")]
        private static extern void IniNotify();

        [DllImport("LibraryAPI.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string CommandAPI([MarshalAs(UnmanagedType.LPStr)]string value);

        public Func<string, string> convertMethod;

        private delegate void Callback(string text);
        private Callback delegateInstance;

        /// <summary>
        /// 
        /// </summary>
        public void EnableDelegate()
        {
            delegateInstance = new Callback(Handler);
            SetGlobalDelegate(delegateInstance);
            IniNotify();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void Handler(string text)
        {
            if (convertMethod != null)
            {
                convertMethod(text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SendCommand(string value)
        {
            string result = CommandAPI(value);
            return result.ToString();
        }
    }
}
