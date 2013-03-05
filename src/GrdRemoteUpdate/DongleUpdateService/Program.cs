using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace DongleUpdateService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new GRDDUService() 
            //};
            //ServiceBase.Run(ServicesToRun);
            new GRDDUService(); //test
        }
    }
}
