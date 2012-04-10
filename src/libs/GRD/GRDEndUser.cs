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
    public sealed partial class GRDEndUser : GRD_Base
    {
        public static readonly GRDEndUser Instance = new GRDEndUser();

        private const uint ProgIDCode = 1;
        private byte _modelId;

        private GRDEndUser()
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
                _keyId = findInfo.dwID;
                _keyType = findInfo.wType;
                _lanRes = findInfo.wRealNetRes;
                _modelId = (byte)findInfo.dwModel;
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
    }
}

#endif