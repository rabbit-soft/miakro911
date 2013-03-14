/**
 * Build this only on x86 platform. Because WriteMask builded on 32-bits
 */
#define LOCALDEBUG
#define WriteEnable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Guardant;
using log4net;

namespace RabGRD
{

    public sealed partial class GRDVendorKey : GRD_Base, IDisposable
    {
        const ushort TRUAlgoNumGSII64 = 0;
        const ushort TRUAlgoNumHash64 = 1;

        private bool _isActive = false;
        private uint _prog;

        /// <summary>
        /// Этот ключ-обновления записан во все проданые ключи
        /// </summary>
        private byte[] _keyTRU = new byte[]{    0xC6, 0x3B, 0x04, 0xF0, 0xB0, 0x37, 0x56, 0x88, 
                                                0x76, 0x89, 0x8A, 0x2F, 0xAE, 0xD1, 0x8E, 0x07 };

        /// <summary>
        /// Этот ключ авто-шифрования записан во все проданые ключи
        /// </summary>
        private byte[] _keyGSII64 = new byte[]{ 0x97, 0xA1, 0x16, 0xD8, 0xCF, 0xE2, 0x42, 0xE1,
                                                0xD2, 0x73, 0x2A, 0xBE, 0x39, 0x6F, 0x43, 0xEF };

        private byte[] _keyHash = new byte[]   {0x34, 0x3D, 0xBC, 0x24, 0xA5, 0x91, 0x87, 0x62,
                                                0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20 };
        /// <summary>
        /// Этот секретный ключ ЭЦП записан во все проданные ключи
        /// </summary>
        private byte[] _keyEcc160 = new byte[] {0xF5, 0xC1, 0x4B, 0x1F, 0x18, 0x8D, 0x24, 0x54, 0xF2, 0x43, 
                                                0xDE, 0xB9, 0x39, 0x7C, 0x2F, 0x97, 0x41, 0xCB, 0x9B, 0xAB };
        public GRDVendorKey()
        {
            _logger = LogManager.GetLogger(typeof(GRDVendorKey));
            _findPropDongleType = GrdDT.TRU;
            connect();
        }

        protected override GrdE setAccessCodes()
        {
            return GrdApi.GrdSetAccessCodes(_grdHandle,             // Handle to Guardant protected container
                                           PublicCode + CryptPu,    // Public code, should always be specified
                                           ReadCode + CryptRd,      // Private read code; you can omit this code and all following via using of overloaded function;
                                           WriteCode + CryptWr,
                                           MasterCode + CryptMs);
        }

        #region delete
        /*public void CleanUserBuf()
        {
            _userBuf = new byte[4096];
            _userBufSize = 0;
        }*/

        /*public GRDVendorKey(uint keyID, byte[] truKey)
        {
            uint len = 16;
            if (truKey.Length < 16)
            {
                len = (uint)truKey.Length;
            }
            Array.Copy(truKey, _keyTRU, len);
            _findPropDongleFlags = GrdFM.ID;
            _findPropDongleID = keyID;
            _findPropDongleType = GrdDT.TRU;
            connect();
        }*/

        /*public uint UAMOffset
        {
            get { return _uamOffset; }
        }*/

        /*public uint ProgID
        {
            get { return _prog; }
        }*/
        #endregion delete

        /// <summary>
        /// Подключается к первому найденному ключу
        /// </summary>
        /// <returns></returns>
        private GrdE connect()
        {
            GrdE retCode; // Error code for all Guardant API functions

            FindInfo findInfo; //= new FindInfo();   // structure used in GrdFind()
            uint dongleID;
            string logStr = "";

            prepareHandle();

            logStr = "Searching for all specified dongles and print info about it's : ";
            retCode = GrdApi.GrdFind(_grdHandle, GrdF.First, out dongleID, out findInfo);
            if (retCode == GrdE.OK) // Print table header if at least one dongle found
            {
                logStr += String.Format("; Found dongle with following ID : {0,8:X}", _findPropDongleID);
                _keyId = findInfo.dwID;
                _keyType = findInfo.wType;
                _model = (byte)findInfo.dwModel;
                _prog = findInfo.byNProg;
                _lanRes = findInfo.wRealNetRes;
            }
            else
            {
                ErrorHandling(_grdHandle, retCode);
                return retCode;
            }

            logStr = "Searching for the specified local or remote dongle and log in : ";
            retCode = GrdApi.GrdLogin(_grdHandle, GrdLM.PerStation);
            logStr += GrdApi.PrintResult((int)retCode);
            _logger.Debug(logStr);
            if (retCode != GrdE.OK)
            {
                return ErrorHandling(_grdHandle, retCode); ;
            }
            _isActive = true;
            return GrdE.OK;
        }

        ~GRDVendorKey()
        {
            _isActive = false;
            disconnect();
        }

        public void Dispose()
        {
            _isActive = false;
            disconnect();
        }

        public bool Active
        {
            get { return _isActive; }
        }

        /// <summary>
        /// Обнуляет ключ. Устанавливает пароль для Удаленного Обновления
        /// </summary>
        public int SetTRUKey()
        {
            GrdE retCode = GrdApi.GrdTRU_SetKey(_grdHandle, _keyTRU);
            _logger.Debug("Setting TruKey " + retCode.ToString() + " " + GrdApi.PrintResult((byte)retCode));
            ErrorHandling(_grdHandle, retCode);
            return (int)retCode;
        }

        /// <summary>
        /// Прошивает ключ для сервиса удаленного обновления ключей
        /// </summary>
        /// <returns></returns>
        public int WriteTRUHostMask()
        {
            uint protectLength;
            ushort wNumberOfItems;
            byte[] pbyWholeMask = makeNewMask(new byte[0], out protectLength, out wNumberOfItems, false);
            return writeMask(pbyWholeMask, protectLength, wNumberOfItems);
        }

        /// <summary>
        /// Записывает пользовательские данные в ключ-Guardant
        /// </summary>
        public int WriteMask(int orgId,string orgName, int farms, int flags, DateTime startDate, DateTime endDate)
        {
            if (!GrdApi.GrdIsValidHandle(_grdHandle))            
                return (int)GrdE.InvalidHandle;
            
            byte[] userBuff = makeUserBuff(orgId,orgName, farms, flags, startDate, endDate);                                 
            uint protectLength;
            ushort wNumberOfItems;
            byte[] pbyWholeMask = makeNewMask(userBuff, out protectLength, out wNumberOfItems);

            return writeMask(pbyWholeMask, protectLength, wNumberOfItems);
        }


        public GrdE GetTRUAnswer(out string base64_answer, string base64_question, int orgId,string orgName, int farms, int flags, DateTime startDate, DateTime endDate,byte[] keyCode, DateTime endSuppors)
        {            
            byte[] buf = Convert.FromBase64String(base64_question);

            TRUQuestionStruct qq = (TRUQuestionStruct)GRDUtils.RawDeserialize(buf, typeof(TRUQuestionStruct));

            base64_answer = "";
            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Decrypt and validate question: ";

            retCode = GrdApi.GrdTRU_DecryptQuestion(_grdHandle,      // handle to Guardant protected container of dongle that contains 
                                                                     // GSII64 algorithm with the same key as in remote dongle 
                                                    TRUAlgoNumGSII64,// dongle GSII64 algorithm number with same TRUkey as in remote dongle 
                                                    TRUAlgoNumHash64,// dongle HASH64 algorithm number with same TRUkey as in remote dongle 
                                                    qq.question,     // pointer to Question					8 bytes (64 bit) 
                                                    qq.id,           // ID									4 bytes 
                                                    qq.pubKey,       // Public Code							4 bytes 
                                                    qq.hash);        // pointer to Hash of previous 16 bytes	8 bytes 

            logStr += GrdApi.PrintResult((int)retCode);
            _logger.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
                return retCode;

            uint protectLength;
            ushort wNumberOfItems;
            byte[] userBuff = makeUserBuff(orgId,orgName, farms, flags, startDate, endDate,keyCode, endSuppors);
            byte[] pbyWholeMask = makeNewMask(userBuff, out protectLength, out wNumberOfItems);

            logStr = "Set Init & Protect parameters for Trusted Remote Update: ";
            retCode = GrdApi.GrdTRU_SetAnswerProperties(_grdHandle,                                         // handle to Guardant protected container 
                                                        GrdTRU.Flags_Init | GrdTRU.Flags_Protect,           // use Init & Protect 
                                                        protectLength,                                      // SAM address of the first byte available for writing in bytes 
                                                        protectLength,                                      // SAM address of the first byte available for reading in bytes 
                                                        wNumberOfItems,                                     // number of hardware-implemented algorithms in the dongle including all protected items and LMS table of Net III 
                                                        0,                                                  // LMS Item number 
                                                        GrdGF.HID);                                         // Global Flags 


            logStr += GrdApi.PrintResult((int)retCode);
            _logger.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
                return retCode;

            byte[] answer = new byte[pbyWholeMask.Length * 3 + 128];
            int ansSize = answer.Length;

            logStr = "Encrypt answer for Trusted Remote Update: ";
            retCode = GrdApi.GrdTRU_EncryptAnswer(_grdHandle,                   // handle to Guardant protected container 
                                                                                // GSII64 algorithm with the same key as in remote dongle 
                                                                                // and pre-stored GrdTRU_SetAnswerProperties data if needed 
                                                  GRDConst.GrdSAMToUAM,         // starting address for writing in dongle 
                                                  (int)pbyWholeMask.Length,     // size of data to be written 
                                                  pbyWholeMask,                 // buffer for data to be written 
                                                  qq.question,                  // pointer to decrypted Question 
                                                  TRUAlgoNumGSII64,             // dongle GSII64 algorithm number with the same key as in remote dongle 
                                                  TRUAlgoNumHash64,             // dongle HASH64 algorithm number with the same key as in remote dongle 
                                                  out answer,                   // pointer to the buffer for Answer data 
                                                  out ansSize);                 // IN: Maximum buffer size for Answer data, OUT: Size of pAnswer buffer 
            logStr += GrdApi.PrintResult((int)retCode);
            _logger.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
                return retCode;

            //string base64data = 
            base64_answer = Convert.ToBase64String(answer, 0, ansSize, Base64FormattingOptions.InsertLineBreaks);
            return GrdE.OK;
        }

        private byte[] makeUserBuff(int orgId,string orgName, int farms, int flags, DateTime startDate, DateTime endDate)
        {
            byte[] userBuff = new byte[USER_DATA_LENGTH]; //TODO говнокод!
            byte[] tmp = BitConverter.GetBytes((ushort)orgId);
            Array.Copy(tmp, userBuff, tmp.Length);
                
            tmp = Encoding.GetEncoding(1251).GetBytes(DEV_MARKER);
            Array.Copy(tmp, 0,userBuff,DEV_MARKER_OFFSET, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(orgName);
            Array.Copy(tmp, 0, userBuff, ORGANIZATION_NAME_OFFSET, tmp.Length);

            tmp = BitConverter.GetBytes(farms);
            Array.Copy(tmp, 0, userBuff, MAX_BUILDINGS_COUNT_OFFSET, tmp.Length);

            tmp = BitConverter.GetBytes(flags);
            Array.Copy(tmp, 0, userBuff, FLAGS_MASK_OFFSET, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(startDate.ToString("yyyy-MM-dd"));
            Array.Copy(tmp, 0, userBuff, FARM_START_DATE_OFFSET, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(endDate.ToString("yyyy-MM-dd"));
            Array.Copy(tmp, 0, userBuff, FARM_STOP_DATE_OFFSET, tmp.Length);

            tmp = BitConverter.GetBytes(15);
            Array.Copy(tmp, 0, userBuff, TEMP_FLAGS_MASK_OFFSET, tmp.Length);

            tmp = Encoding.GetEncoding(1251).GetBytes(endDate.AddMonths(1).ToString("yyyy-MM-dd"));
            Array.Copy(tmp, 0, userBuff, TEMP_FLAGS_END_OFFSET, tmp.Length);         

            return userBuff;
        }
        private byte[] makeUserBuff(int orgId,string orgName, int farms, int flags, DateTime startDate, DateTime endDate, byte[] keyCode,DateTime endSupport)
        {
            byte[] res = makeUserBuff(orgId,orgName,farms,flags,startDate,endDate);
            Array.Copy(keyCode, 0, res, KEY_CODE_OFFSET, keyCode.Length);
            byte [] tmp = Encoding.GetEncoding(1251).GetBytes(startDate.ToString("yyyy-MM-dd"));
            Array.Copy(tmp, 0, res, SUPPORT_END_DATE_OFFSET, tmp.Length);
            return res;
        }
        
        /// <summary>
        /// Создает окончательную битовую маску, которая будет записана в ключ Клиента.
        /// </summary>
        /// <param name="userBuff">Битовая маска с данными конкретного клиента</param>
        /// <param name="forUser">Прошивается ли для конечного пользователя. Если False, то для сервиса обновления ключей</param>
        private byte[] makeNewMask(byte[] userBuff, out uint protectLength, out ushort wNumberOfItems, bool forUser)
        {
            DongleHeaderStruct dongleHeader;
            dongleHeader.ProgID = 1;
            dongleHeader.Version = 1;
            dongleHeader.SerialNumber = 1;
            dongleHeader.Mask = 21;

            byte[] abyDongleHeader = GRDUtils.RawSerialize(dongleHeader, 14);
            byte[] abyMask = new byte[4096];
            byte[] abyMaskHeader = new byte[4096];

            ushort wMaskSize = 0;
            ushort wASTSize = 0;
            wNumberOfItems = 0;

            WriteMaskInit(_keyType, _model);

            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         (ushort)GrdAN.GSII64, //номер алгоритма по умолчанию
                         (byte)GRDConst.nsafl.ST_III,
                         (ushort)0,
                         GRDConst.RsAlgo.GSII64,
                         (ushort)GrdADS.GSII64,
                         (ushort)GrdARS.GSII64,
                         0, 0, 0, 0,
                         null, null, null, null,
                         0, 0,
                         forUser ? _keyGSII64 : _keyTRU,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);

            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         (ushort)GrdAN.HASH64,
                         (byte)GRDConst.nsafl.ST_III,
                         (ushort)0,
                         GRDConst.RsAlgo.HASH64,
                         (ushort)GrdADS.HASH64,
                         (ushort)GrdARS.HASH64,
                         0, 0, 0, 0,
                         null, null, null, null,
                         0, 0,
                         forUser ? _keyHash : _keyTRU,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);
            if (forUser)
                AddAlgorithm(abyMask,
                             abyMaskHeader,
                             (ushort)GrdAN.ECC160,
                             (byte)(GRDConst.nsafl.ST_III + GRDConst.nsafl.ActivationSrv + GRDConst.nsafl.DeactivationSrv),
                             (ushort)(GRDConst.nsafh.ReadSrv + GRDConst.nsafh.ReadPwd),
                             GRDConst.RsAlgo.ECC160,
                             (ushort)GrdADS.ECC160,
                             (ushort)GrdARS.ECC160,
                             2863311530,
                             3722304989,
                             3149642683,
                             0,
                             null, null, null, null,
                             0,
                             10,
                             _keyEcc160,
                             ref wMaskSize,
                             ref wASTSize,
                             ref wNumberOfItems);

            byte[] pbyWholeMask = new byte[WHOLE_MASK_LENGTH];

            Array.Copy(abyDongleHeader,
                       0,
                       pbyWholeMask,
                       0,
                       GRDConst.GrdWmUAMOffset);
            Array.Copy(abyMaskHeader,
                       0,
                       pbyWholeMask,
                       GRDConst.GrdWmUAMOffset,
                       wASTSize);
            Array.Copy(abyMask,
                       0,
                       pbyWholeMask,
                       GRDConst.GrdWmUAMOffset + wASTSize,
                       wMaskSize);
            Array.Copy(userBuff,
                       0,
                       pbyWholeMask,
                       USER_DATA_BEGINING,
                       userBuff.Length);

            protectLength = (uint)(GRDConst.GrdWmSAMOffset + wASTSize + wMaskSize);
            return pbyWholeMask;
        }
        private byte[] makeNewMask(byte[] userBuff, out uint protectLength, out ushort wNumberOfItems)
        {
            return makeNewMask(userBuff, out protectLength, out wNumberOfItems, true);
        }

        private int writeMask(byte[] pbyWholeMask, uint protectLength, ushort wNumberOfItems)
        {
            string logStr;
            GrdE retCode; // Error code for all Guardant API functions

            //retCode = (GrdE)SetTRUKey();
            //if (retCode != GrdE.OK) return (int)retCode;

            logStr = "Writing user buffer : ";
            retCode = GrdApi.GrdWrite(_grdHandle,
                                      (uint)0,
                                      pbyWholeMask.Length,
                                      pbyWholeMask);

            logStr += GrdApi.PrintResult((int)retCode);
            _logger.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK) return (int)retCode;

            logStr = "Protecting : ";
            retCode = GrdApi.GrdProtect(new IntPtr(_grdHandle.Address),
                                        protectLength,
                                        protectLength,
                                        wNumberOfItems,
                                        0,
                                        (uint)GrdGF.HID, IntPtr.Zero);
            logStr += GrdApi.PrintResult((int)retCode);
            _logger.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK) return (int)retCode;

            _logger.Debug(" ====> " + (protectLength).ToString());
            return (int)retCode;
        }

        #region writemask.dll

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WriteMaskInit")]
        private static extern void WriteMaskInit(ushort wType,
                                                 byte byDongleModel);

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LMS_DET_Prepare")]
        private static extern void LMS_DET_Prepare([MarshalAs(UnmanagedType.LPArray)] byte[] pMem,
                                                   ushort wFlags,
                                                   byte byLMSSize,
                                                   ushort wLANResource,
                                                   [MarshalAs(UnmanagedType.LPArray)] ushort[] wLMSModules,
                                                   ref ushort wLMSDetSize);

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CODE_DET_Prepare")]
        private static extern void CODE_DET_Prepare(Handle hGrd,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pMem,
                                                           byte byNumberOfCode,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] szCodeMaskImage,
                                                           uint dwRAMStart,
                                                           uint dwRAMSize,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pPubECCKey4Sign,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pPrivECCKey4Key,
                                                           ref ushort pwCodeDetSize);

        /// <summary>
        /// Записывает дискриптор алгоритма в маску, которая будет зашита  в ключ
        /// </summary>
        /// <param name="pbyAlgos">pointer to a buffer to return generated mask</param>
        /// <param name="pbyAST">pointer to a buffer to return generated mask header</param>
        /// <param name="wNumericName">numeric name (algorithm number) of added algorithm/protected item (PI)</param>
        /// <param name="byLoFlags">services flags</param>
        /// <param name="wHiFlags">services flags</param>
        /// <param name="byAlgorithmCode">algorithm code</param>
        /// <param name="wKeyLength">key length</param>
        /// <param name="wBlockLength">block length</param>
        /// <param name="dwActivatePwd">activation service password</param>
        /// <param name="dwDeactivatePwd">deactivation service password</param>
        /// <param name="dwReadPwd">read service password</param>
        /// <param name="dwUpdatePwd">update service password</param>
        /// <param name="pBirthTime">pointer to structure, contains birth time of algorithm/PI (only for Time dongles)</param>
        /// <param name="pDeadTime">pointer to structure, contains dead time of algorithm/PI (only for Time dongles)</param>
        /// <param name="pLifeTime">pointer to structure, contains life time of algorithm/PI (only for Time dongles)</param>
        /// <param name="pFlipTime">pointer to structure, contains flip time of algorithm/PI (only for Time dongles)</param>
        /// <param name="wGpCounter">GP counter of algorithm/PI</param>
        /// <param name="wErrorCounter">error counter of algorithm/PI</param>
        /// <param name="pbyDet"> algorithm determinant</param>
        /// <param name="pwMaskSize">pointer to variables, contains mask size</param>
        /// <param name="pwNewASTSize"> pointer to variables, contains AST size</param>
        /// <param name="pwNumberOfItems">pointer to variables, contains number of items in mask</param>
        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AddAlgorithm")]
        private static extern void AddAlgorithm([MarshalAs(UnmanagedType.LPArray)] 
                                                byte[] pbyAlgos,
                                                [MarshalAs(UnmanagedType.LPArray)] 
                                                byte[] pbyAST,
                                                ushort wNumericName,
                                                byte byLoFlags,
                                                ushort wHiFlags,
                                                byte byAlgorithmCode,
                                                ushort wKeyLength,
                                                ushort wBlockLength,
                                                uint dwActivatePwd,
                                                uint dwDeactivatePwd,
                                                uint dwReadPwd,
                                                uint dwUpdatePwd,
            /*GrdTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pBirthTime,
            /*GrdTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pDeadTime,
            /*GrdLifeTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pLifeTime,
            /*GrdLifeTime*/ [MarshalAs(UnmanagedType.LPArray)] byte[] pFlipTime,
                            ushort wGpCounter,
                            ushort wErrorCounter,
                            [MarshalAs(UnmanagedType.LPArray)] byte[] pbyDet,
                            ref ushort pwMaskSize,
                            ref ushort pwNewASTSize,
                            ref ushort pwNumberOfItems);



        [StructLayout(LayoutKind.Explicit)]
        struct DongleHeaderStruct
        {
            [FieldOffset(0)]
            public byte ProgID;
            [FieldOffset(1)]
            public byte Version;
            [FieldOffset(2)]
            public UInt16 SerialNumber;
            [FieldOffset(4)]
            public UInt16 Mask;
        }

        #endregion writemask.dll
    }
}
