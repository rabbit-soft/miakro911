using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using RabGRD;
using pEngine;
using gamlib;

namespace RabDongleFirmuper
{
    class VenReqSender : RequestSender
    {
        public VenReqSender()
        {
            _ServUriAppend = "";
            _RPCfile = "forrpc.php";
        }
    }

    class Program
    {
        private const string DEF_PWD = "user_with_old_key_10578vnr/* ekei";
        private static string _url = "http://trunk.rab_srv.wd2.9-bits.ru/";
        private static ILog _logger = null;
        private static GRDVendorKey _grd = null;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger("RabDongleFirmuper");
            int errCode = 0;
            try {
                if (args.Length == 0 || args[0] == "-h") {
                    _logger.Debug("print help");
                    printHelp();
                    Environment.Exit(0);
                }
                _logger.Debug("----------  START ----------");
                _grd = new GRDVendorKey();
                if (args[0] == "-s") {
                    _grd.WriteTRUHostMask();
                } else if (args[0] == "-c") {
                    if (args.Length > 1 && args[1] == "-u" && !String.IsNullOrEmpty(args[2])) {
                        _url = args[2];
                    }

                    _grd.SetTRUKey();
                    dongleUpdate();
                    Console.WriteLine("Success");
                }
            } catch (GrdException exc) {
                _logger.Error(exc);
                Console.WriteLine("Grd Error: " + exc.Message);
                errCode = 1;
            } catch (Exception exc) {
                _logger.Fatal(exc);
                if (exc.InnerException != null) {
                    exc = exc.InnerException;
                }
                Console.WriteLine("Error: " + exc.Message);
                errCode = 2;
            } finally {
                if (_grd != null) {
                    _grd.Dispose();
                }
                _logger.Debug("----------  END ----------");
                Environment.Exit(errCode);
            }
        }

        private static void dongleUpdate()
        {
            VenReqSender reqSend = new VenReqSender();
            reqSend.Url = Helper.UriNormalize(_url);
            reqSend.UserID = 0;
            reqSend.Key = new byte[GRD_Base.KEY_CODE_LENGTH];

            byte[] defPass = Encoding.UTF8.GetBytes(DEF_PWD);
            Array.Copy(defPass, reqSend.Key, defPass.Length);

            string q = _grd.GetTRUQuestion();
            ResponceItem ri = reqSend.ExecuteMethod(MethodName.ClientGetUpdate,
                MPN.question, q);
            if (String.IsNullOrEmpty(ri.Value as String)) {
                throw new Exception("Получен пустой ответ от сервера");
            }
            _grd.SetTRUAnswer(ri.Value as String);
        }

        private static void printHelp()
        {
            Console.WriteLine(@"-s Записать маску для сервиса обновления ключей
-c Записать маску для для клиента
-u Адрес сайта статистики RabServ (http://trunk.rab_srv.wd2.9-bits.ru/) обязательно должен заканчиваться прямым слешем(/)");
        }
    }
}
