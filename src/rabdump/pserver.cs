//#define PROTECTED
#if PROTECTED
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace rabdump
{
    class pserver
    {
        [DllImport("key.dll", EntryPoint = "#2")]
        public static extern int VerifyKey(uint pc, uint rc);

        private const uint PUB_CODE = 0x9BD54F75;
        private const uint RD_CODE = 0xFE8392B2;
        private bool stop = true;
        private static bool keyinserted = true;
        private Thread t = null;
        public pserver()
        {
            stop = false;
            t = new Thread(new ThreadStart(Process));
            t.Start();
        }
        ~pserver()
        {
            release();
        }

        public void release()
        {
            stop = true;
            t.Join(30000);
            t.Abort();
        }

        private void Process()
        {
            UdpClient uc = new UdpClient(31749);
            uc.EnableBroadcast = true;
            while (!stop)
            {
                if (uc.Available > 0)
                {
                    IPEndPoint ep = new IPEndPoint(0, 0);
                    byte[] data = uc.Receive(ref ep);
                    if (data.Length > 0 && ep.Port != 0)
                    {
                        String pck = new String(Encoding.Unicode.GetChars(data));
                        if (pck == "TBRB")
                        {
                            data = Encoding.Unicode.GetBytes(String.Format("TBRB={0:d}", verify()));
                            uc.Send(data, data.Length, ep);
                        }
                    }
                }
                else
                    Thread.Sleep(300);
            }
            uc.Close();
        }

        public static bool haskey()
        {
            return (verify() > 0);
        }
        public static int verify()
        {
            int res=VerifyKey(PUB_CODE, RD_CODE);
            keyinserted=(res>0);
            return res;
        }
        public static bool canwork()
        {
            return keyinserted;
        }
    }
}
#endif
