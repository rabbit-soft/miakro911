#if PROTECTED
using System;
using Guardant;

namespace RabGRD
{
    public partial class GRD_Base
    {
        /// <summary>
        /// ���������� ����� ������ ��� ���������� ���������� �����(TRU)
        /// </summary>
        /// <returns>����� ������ (base64string)</returns>
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

        public GrdE SetTRUAnswer(string base64_answer)
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
            if (retCode != GrdE.OK)
                return retCode;

            byte[] buffer = new byte[16];
            byte[] initVector = new byte[16];

            logStr = "Testing new mask by GrdTransform test: ";
            retCode = GrdApi.GrdTransform(_grdHandle, 0, 8, buffer, 0, initVector);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            return ErrorHandling(_grdHandle, retCode);          
        }
    }
}
#endif