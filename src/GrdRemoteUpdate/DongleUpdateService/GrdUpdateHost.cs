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
        public string DongleUpdate(string base64_question, int orgId, string orgName, int farms, int flags, string startDate, string endDate, string base64key, string endSupport)
        {
            DateTime dtStart, dtEnd, dtSupport;
            if (!DateTime.TryParse(startDate, out dtStart) || !DateTime.TryParse(endDate, out dtEnd))
                throw new Exception("Не верный форматы даты начала или конца.");
            if (!DateTime.TryParse(endSupport, out dtSupport))
                throw new Exception("Не верный форматы даты окончания поддержки.");
            byte[] keyCode = Convert.FromBase64String(base64key);
            _log.Info(String.Format("Request For UpdateDongle {5:d} {0:s} {1:d} {2:d} {3:s} {4:s}", orgName, farms, flags, startDate, endDate, orgId));
            GRDVendorKey key = new GRDVendorKey();           
            //try
            //{
                string ans = key.GetTRUAnswer(base64_question, orgId, orgName, farms, flags,
                    dtStart,
                    dtEnd,
                    keyCode,
                    dtSupport);
                key.Dispose();
            //}
            //catch (Exception exc)
            //{ 
            //    _log.Error(exc);
            //}
            key.Dispose();
            if (ans == "")
                throw new Exception("Сервис обновления ключей не смог создать число-ответ");
            return ans;
        }

        [XmlRpcMethod("dongle.encrypt.id")]
        public int EncryptDongleId(string base64_question)
        {
            //GRDVendorKey key = new GRDVendorKey();
            byte[] buf = Convert.FromBase64String(base64_question);

            TRUQuestionStruct qq = (TRUQuestionStruct)GRDUtils.RawDeserialize(buf, typeof(TRUQuestionStruct));
            //key.Dispose();
            return (int)qq.id;          
        }
    }
}
