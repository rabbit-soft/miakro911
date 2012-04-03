#if PROTECTED
using System;
using System.Windows.Forms;
using log4net;
using Guardant;

namespace RabGRD
{
    
    /// <summary>
    /// Thread-safe singleton example created at first call
    /// </summary>
    public sealed partial class GRDEndUser:GRD_Base
    {
        //public static readonly GRDEndUser Instance = new GRDEndUser();

        private const uint ProgIDCode = 1;
        private ushort _keyType;
        private ushort _lanRes;
        private byte _modelId;

        public uint ID { get { return _id;} } 

        public GRDEndUser()
        {
            try
            {
                log = LogManager.GetLogger(typeof(GRDEndUser));
                Connect();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException.Message, "Фатальная ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Debug(e.InnerException.Message);
                Environment.Exit(100);
            }
        }
        ~GRDEndUser()
        {
            disconnect();
        }

        public void Disconnect()
        {
            disconnect();
        }

        public GrdE Connect()
        {
            GrdE retCode;
            string logStr;
            FindInfo findInfo;

            _findPropRemoteMode = GrdFMR.Local;
            _findPropDongleType = GrdDT.TRU;            

            prepareHandle();

            // -----------------------------------------------------------------
            // Search for all specified dongles and print ID's
            // -----------------------------------------------------------------
            logStr = "Searching for all specified dongles and print info about it's : ";
            retCode = GrdApi.GrdFind(_grdHandle, GrdF.First, out _findPropDongleID, out findInfo);
            if (retCode == GrdE.OK) // Print table header if at least one dongle found
            {
                logStr += string.Format("; Found dongle with following ID : {0,8:X}", _findPropDongleID);
                _id = findInfo.dwID;
                _keyType = findInfo.wType;
                _lanRes = findInfo.wRealNetRes;
                _model = (byte)findInfo.dwModel;
            }
            else
            {
                return ErrorHandling(_grdHandle, retCode);
            }

            // -----------------------------------------------------------------
            // Search for the specified local or remote dongle and log in
            // -----------------------------------------------------------------
            logStr = "Searching for the specified local or remote dongle and log in : ";
            retCode = GrdApi.GrdLogin(_grdHandle, 0, GrdLM.PerStation);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            return ErrorHandling(_grdHandle, retCode);
        }

        public void SetTRUAnswer(string base64_answer)
        {
            byte[] buf = Convert.FromBase64String(base64_answer);
            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Apply encrypted answer data: ";

            retCode = GrdApi.GrdTRU_ApplyAnswer(_grdHandle, // handle to Guardant protected container of dongle with 
                                                            // corresponding pre-generated question 
                                                buf);       // answer data update buffer prepared and encrypted by GrdTRU_EncryptAnswer 

            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            byte[] buffer = new byte[16];
            byte[] initVector = new byte[16];

            logStr = "Testing new mask by GrdTransform test: ";
            retCode = GrdApi.GrdTransform(_grdHandle, 0, 8, buffer, 0, initVector);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
        }

        /// <summary>
        /// Генерирует число вопрос для Удаленного Обновления Ключа(TRU)
        /// </summary>
        /// <returns>Число вопрос (base64string)</returns>
        public string GetTRUQuestion()
        {
            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Generate encrypted question and initialize remote update procedure: ";

            TRUQuestionStruct quest = new TRUQuestionStruct();


            retCode = GrdApi.GrdTRU_GenerateQuestion(
                            _grdHandle,         // handle to Guardant protected container 
                            out quest.question, // pointer to question					8 bytes (64 bit) 
                            out quest.id,       // pointer to dongle ID					4 bytes 
                            out quest.pubKey,   // pointer to dongle Public Code		4 bytes 
                            out quest.hash);    // pointer to Hash of previous data		8 bytes 

            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            quest.type = _keyType;
            quest.lanRes = _lanRes;
            quest.model = _model;

            byte[] buf = GRDUtils.RawSerialize(quest);

            return Convert.ToBase64String(buf, 0, buf.Length, Base64FormattingOptions.None);
        }
    }
}

#endif