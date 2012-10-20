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
            bool batch = false;
            if (args.Length > 0)
            {
                batch = args[0] == "batch";
            }
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

            /*bool update = hasRabNet || hasRabDump;
            if (hasRabNet && !hasRabDump)           
                if (Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\..\RabDump"))               
                    update = false;*/                         
            int res = 0;
            if (_rnc.HaveDataSources())
            {
                _logger.Info("have datasources");
                UpdateForm uf = new UpdateForm(/*flRabDump, flRabNet, batch*/);
                Application.Run(uf);
                res = uf.Result;
            }
            else
            {
                _logger.Info("not have datasources");
                InstallForm ifr = new InstallForm();
                Application.Run(ifr);
                res = ifr.Result;
            }
            Environment.ExitCode = res;
            //            }
            //            catch (Exception e)
            //            {
            //                MessageBox.Show(e.Message + e.StackTrace);
            //            }
        }
    }
}