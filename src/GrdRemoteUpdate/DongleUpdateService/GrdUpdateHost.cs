#define A
using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;
using RabGRD;
using log4net;


namespace DongleUpdateService
{
    public class GrdUpdateHost : XmlRpcListenerService
    {
        private ILog _log = LogManager.GetLogger(typeof(GrdUpdateHost));


        [XmlRpcMethod("dongle.update")]
        public string DongleUpdate(string base64_question,int orgId, string orgName, int farms, int flags, string startDate, string endDate,string base64key)
        {
#if A
            byte[] keyCode = Convert.FromBase64String(base64key);
            _log.Info(String.Format("Request For UpdateDongle {5:d} {0:s} {1:d} {2:d} {3:s} {4:s}",orgName,farms,flags,startDate,endDate,orgId));
            GRDVendorKey key = new GRDVendorKey();
            string ans;
            key.GetTRUAnswer(out ans, base64_question, orgId,orgName, farms, flags, 
                DateTime.Parse(startDate),
                DateTime.Parse(endDate),
                keyCode);
            key.Dispose();
            if(ans=="")
                throw new XmlRpcException("Ошибка генерации числа-ответа на сервере");
            return ans;
#else
            _log.Debug("DongleUpdate");
            GRDVendorKey key = new GRDVendorKey();
            string ans;
            string q = key.GetTRUQuestion();
            key.GetTRUAnswer(out ans, q, orgId, farms, flags,
                DateTime.Parse(startDate),
                DateTime.Parse(endDate));
            key.Dispose();
            if (ans == "")
                throw new XmlRpcException("Ошибка генерации числа-ответа на сервере");
            return ans;

            //GRDVendorKey key = new GRDVendorKey();
            key.WriteMask(orgId, farms, flags, DateTime.Parse(startDate), DateTime.Parse(endDate));
            return "";
#endif
        }
    }
}
