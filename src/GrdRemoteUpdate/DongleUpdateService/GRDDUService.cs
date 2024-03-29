﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using CookComputing.XmlRpc;
using System.Net;
using System.Net.Sockets;
using log4net;
using System.Threading;

namespace DongleUpdateService
{
    public partial class GRDDUService 
#if !DEBUG
        : ServiceBase
#endif
    {
        protected ILog _logger = LogManager.GetLogger(typeof(GRDDUService));
        private string _srv = "http://*:11000/";//rpc2/";
        HttpListener _listener;

        public GRDDUService()
        {
            InitializeComponent();
#if DEBUG
            startListen();
#endif
        }

#if !DEBUG
        protected override void OnStart(string[] args)
        {
            addSysLog("starting");

            Thread t = new Thread(startListen);
            t.Start();
        }

        protected override void OnStop()
        {
            try
            {
                addSysLog("stoping");
                if (_listener != null && _listener.IsListening)
                    _listener.Stop();
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
                addSysLog(exc.ToString(),EventLogEntryType.Error);
            }

        }
#endif

        private void addSysLog(string log,EventLogEntryType type)
        {            
            try
            {                
                if (!EventLog.SourceExists(this.eventLog1.Source))
                {
                    _logger.Debug("creating event source");
                    EventLog.CreateEventSource(this.eventLog1.Source, this.eventLog1.Log);                    
                }
                
                eventLog1.WriteEntry(log, type);
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
                //addSysLog(exc.StackTrace, EventLogEntryType.Error);
            }
        }
        private void addSysLog(string log)
        {
            addSysLog(log,EventLogEntryType.Information);
        }

        private void startListen()
        {
            try
            {
                if (_listener == null)
                {
                    _listener = new HttpListener();
                    _listener.Prefixes.Add(_srv);
                    _listener.Start();
                }
               
                while (_listener.IsListening)
                {
                    _logger.Info("-- waiting a new connect");
                    HttpListenerContext context = _listener.GetContext();
                    System.Diagnostics.Debug.WriteLine("HAVE CONNECT");
                    //if (context.Request.UserAgent != "XML-RPC")
                    //{
                    //    context.Response.Abort();                      
                    //    continue;
                    //}
                    XmlRpcListenerService svc = new GrdUpdateHost();                
                    svc.ProcessRequest(context);
                }
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
                eventLog1.WriteEntry(exc.StackTrace, EventLogEntryType.Error);
            }
        }

        private void InitializeComponent()
        {
            this.eventLog1 = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            // 
            // eventLog1
            // 
            this.eventLog1.Log = "Application";
            this.eventLog1.Source = "DongleUpdateService";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();

        }


        private EventLog eventLog1;
    }
}
