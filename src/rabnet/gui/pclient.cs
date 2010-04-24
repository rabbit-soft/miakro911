//#define PROTECTED
#if PROTECTED
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace rabnet
{
    class PClient 
    {
        public static PClient obj=null;
        private int frms=-1;
        private bool haskey = false;
        private bool hasserver = false;
        private DateTime updtime = DateTime.MinValue;
        public static PClient get()
        {
            if (obj==null)
                obj=new PClient();
            return obj;
        }
        public PClient()
        {
            update();
        }
        public void update()
        {
            updtime = DateTime.Now;
            UdpClient uc=new UdpClient();
            uc.EnableBroadcast=true;
            byte[] data=Encoding.Unicode.GetBytes("TBRB");
            uc.Send(data,data.Length,"255.255.255.255",31749);
            int i=0;
            while (uc.Available == 0 && i < 30)
            {
                Thread.Sleep(100);
                i++;
            }
            if (uc.Available > 0)
            {
                hasserver = true;
                IPEndPoint ep = new IPEndPoint(0, 0);
                data = uc.Receive(ref ep);
                if (ep.Port != 0 && data.Length > 0)
                {
                    String s = new String(Encoding.Unicode.GetChars(data));
                    if (s.Substring(0, 5) == "TBRB=")
                    {
                        int f = -1;
                        int.TryParse(s.Substring(5), out f);
                        if (f > 0)
                        {
                            haskey = true;
                            frms = f;
                        }
                        else
                        {
                            frms = -1;
                            haskey = false;
                        }
                    }
                }
                else
                {
                    haskey = false;
                    frms = -1;
                }
            }
            else
            {
                hasserver = haskey = false;
                frms = -1;
            }
        }

        public int farms()
        {
            if (!canwork())
                return -1;
            return frms;
        }
        public bool canwork()
        {
            if ((DateTime.Now - updtime).Minutes > 5 || !hasserver || !haskey || frms==-1)
                update();
            return (haskey && hasserver);
        }
    }
}

#endif