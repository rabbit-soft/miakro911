using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using CookComputing.XmlRpc;
using System.Net;
using System.Net.Sockets;

namespace DongleUpdateService
{
    public partial class GRDDUService //: ServiceBase
    {
        

        public GRDDUService()
        {
            //InitializeComponent();
            //TcpListener lst = new TcpListener();
            //lst.Start
            

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://192.168.0.110:11000/rpc2/");
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                XmlRpcListenerService svc = new GrdUpdateHost();
                svc.ProcessRequest(context);
            }
        }

        /*protected override void OnStart(string[] args)
        {
            AddLog("start");
        }

        protected override void OnStop()
        {
            AddLog("stop");
        }

        public void AddLog(string log)
        {

            try
            {
                if (!EventLog.SourceExists("GRDDUService"))
                {
                    EventLog.CreateEventSource("GRDDUService", "GRDDUService");
                }

                eventLog1.Source = "GRDDUService";
                eventLog1.WriteEntry(log);
            }
            catch { }
        }*/
    }
}
