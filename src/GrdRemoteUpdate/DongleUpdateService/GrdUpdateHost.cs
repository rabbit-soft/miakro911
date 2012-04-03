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
        public string DongleUpdate(string base64_question,string orgId, int farms, int flags, string startDate, string endDate)
        {
#if A
            _log.Debug("DongleUpdate");
            GRDVendorKey key = new GRDVendorKey();
            string ans;
            key.GetTRUAnswer(out ans,base64_question, orgId, farms, flags, 
                DateTime.Parse(startDate),
                DateTime.Parse(endDate));
            key.Dispose();
            if(ans=="")
                throw new XmlRpcException("Ошибка генерации числа-ответа на сервере");
            return ans;
#else
            GRDVendorKey key = new GRDVendorKey();
            key.WriteMask(orgId, farms, flags, DateTime.Parse(startDate), DateTime.Parse(endDate));
            return "";
#endif
        }
    }
}
