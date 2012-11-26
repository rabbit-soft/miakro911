#if DEBUG
    #define TFPF
#endif
using System;
using System.Windows.Forms;
using System.IO;
using log4net;
using rabnet.RNC;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace updater
{
    static class Program
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Program));
        private static RabnetConfig _rnc;
        public static RabnetConfig RNC { get { return _rnc; } }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            _rnc = new RabnetConfig();

            _logger.Info("STARTING");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ///Определяет пути к файлам конфигураций     
            String flRabDump = rabnet.Run.SerachConfig("rabdump");
            String flRabNet = rabnet.Run.SerachConfig("rabdump");
            
            ///Извлекаем настройки из файлов конфигурации
            if ( flRabNet!="" && InstallForm.TestRabNetConfig(flRabNet))
                _rnc.ExtractConfig(flRabNet);
            if (flRabNet != "" && InstallForm.TestRabDumpConfig(flRabDump))
                _rnc.ExtractConfig(flRabDump);
            _rnc.RelocateRegOptions();
                        
            int res = 0;
            if (_rnc.HaveDataSources())
            {
                _logger.Info("have datasources");
                UpdateForm uf = new UpdateForm();
                Application.Run(uf);
                res = uf.Result;
            }
            else
            {
                try
                {
                    _logger.Info("not have datasources");
                    if (args.Length > 0 && args[0] == "/d")
                    {
                        _logger.Info("default config");
                        rabnet.Run.DBCreate("nudb", "localhost", "kroliki", "kroliki", "krol", "root", "");
                        Program.RNC.LoadDataSources();
                        Program.RNC.SaveDataSource(System.Guid.NewGuid().ToString(), "Новая ферма", "localhost", "kroliki", "kroliki", "krol");
                        Program.RNC.SaveDataSources();
                    }
                    else
                    {
                        InstallForm ifr = new InstallForm();
                        Application.Run(ifr);
                        res = ifr.Result;
                    }
                }
                catch (Exception exc)
                {
                    _logger.Error(exc);
                    MessageBox.Show(exc.Message);
                    Environment.ExitCode = 1;
                }
            }
            Environment.ExitCode = res;
        }
    }
}