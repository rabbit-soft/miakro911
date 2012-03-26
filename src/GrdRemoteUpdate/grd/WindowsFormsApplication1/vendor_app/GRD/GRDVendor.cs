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
    public sealed class GRDEnumerator
    {
        private readonly List<KeyInfo> _keyList;

        public GRDEnumerator()
        {
            _keyList = new List<KeyInfo>();
            
            Handle grdH=new Handle();

            GrdE retCode; // Error code for all Guardant API functions

            FindInfo findInfo; //= new FindInfo();   // structure used in GrdFind()

            string logStr = "";

            // Initialize this copy of GrdAPI. GrdStartup() must be called once before first GrdAPI call at application startup
            logStr = "Initialize this copy of GrdAPI : ";
            retCode = GrdApi.GrdStartup(GrdFMR.Local); // + GrdFMR.Remote if you want to use network dongles
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(new Handle(0), retCode);
            if (retCode != GrdE.OK && retCode != GrdE.AlreadyInitialized)
            {
                return;
            }

            logStr = "Create Guardant protected container : ";
            grdH = GrdApi.GrdCreateHandle(grdH, GrdCHM.MultiThread);
            if (grdH.Address == 0) // Some error found?
            {
                logStr += GrdApi.PrintResult((int)GrdE.MemoryAllocation);
                LogIt(logStr);
                ErrorHandling(new Handle(0), GrdE.MemoryAllocation);
                return;
            }
            logStr += GrdApi.PrintResult((int)GrdE.OK);
            LogIt(logStr);
            ErrorHandling(grdH, GrdE.OK); // Print success information

            logStr = "Storing dongle codes in Guardant protected container : ";
            retCode = GrdApi.GrdSetAccessCodes(grdH, // Handle to Guardant protected container
                                               0); // Public code, should always be specified

            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(grdH, retCode);
            if (retCode != GrdE.OK)
            {
                return;
            }

            logStr = "Setting dongle search conditions : ";

            // All following GrdFind() & GrdLogin() calls before next
            // GrdSetFindMode() will use specified flag values. 
            // If dongle field values and specified values do not match, error code is
            // returned. Both access code and flags are required to call the dongle.
            retCode = GrdApi.GrdSetFindMode(grdH,
                                            GrdFMR.Local,
                                            GrdFM.Type,
                                            0,
                                            0,
                                            0,
                                            0,
                                            0,
                                            GrdDT.TRU,
                                            GrdFMM.ALL,
                                            GrdFMI.ALL);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(grdH, retCode);
            if (retCode != GrdE.OK)
            {
                return;
            }

            logStr = "Searching for all specified dongles and print info about it's : ";
            
            uint dongleID;

            retCode = GrdApi.GrdFind(grdH, GrdF.First, out dongleID, out findInfo);
            if (retCode == GrdE.OK) // Print table header if at least one dongle found
            {
                logStr += "; Found dongle with following ID : ";
            }
            while (retCode == GrdE.OK)
            {
                // Print info about dongles found
                logStr += string.Format(" {0,8:X}", dongleID); // Dongle's ID (unique)
                logStr += " type:" + findInfo.wType.ToString();
                //_type = findInfo.wType;
                //_lanRes = findInfo.wRealNetRes;

                KeyInfo key=new KeyInfo();

                key.ID = findInfo.dwID;
                key.Type = findInfo.wType;
                key.UAM = findInfo.dwRkmUserAddr;
                key.Model = findInfo.dwModel;

                key.UAMOffset = findInfo.wWriteProtectS3 - GRDConst.GrdSAMToUAM;

                _keyList.Add(key);

                // Find next dongle
                retCode = GrdApi.GrdFind(grdH, GrdF.Next, out dongleID, out findInfo);
            }
            LogIt(logStr);
            if (retCode == GrdE.AllDonglesFound) // Search has been completed?
            {
                LogIt("Dongles search is complete with no errors");
            }
            else
            {
                ErrorHandling(grdH, retCode);
                if (retCode != GrdE.OK)
                {
                    return;
                }
            }

            GrdApi.GrdCloseHandle(grdH);

            GrdApi.GrdCleanup();

            return;
        }

        public List<KeyInfo> KeyList
        {
            get { return _keyList; }
        }

        private static void LogIt(string txt)
        {
#if !LOCALDEBUG
            log.Debug(txt);
#else
            /*System.Diagnostics.Debug.WriteLine(txt);
            TextWriter logFile = new StreamWriter(".\\log.txt", true);
            logFile.WriteLine(txt);
            logFile.Close();*/
#endif
        }

        private GrdE ErrorHandling(Handle hGrd, GrdE nRet)
        {
            // print the result of last executed function
            //logIt(GrdApi.PrintResult((int)nRet));
            string logStr = "";

            if (nRet != GrdE.OK && nRet != GrdE.AlreadyInitialized)
            {
                if (hGrd.Address != 0) // Perform some cleanup operations if hGrd handle exists
                {
                    // Close hGrd handle, log out from dongle/server, free allocated memory
                    logStr = ("Closing handle: ");
                    nRet = GrdApi.GrdCloseHandle(hGrd);
                    logStr += GrdApi.PrintResult((int)nRet);
                    LogIt(logStr);
                }


                // Deinitialize this copy of GrdAPI. GrdCleanup() must be called after last GrdAPI call before program termination
//                logStr = "Deinitializing this copy of GrdAPI : ";
//                nRet = GrdApi.GrdCleanup();
//                logStr += GrdApi.PrintResult((int)nRet);
//                LogIt(logStr);

                // Terminate application
                //Environment.Exit((int)nRet);

            }
            //_isActive = false;
            return nRet;
        }
    }

    public sealed partial class GRDVendorKey : GRD_Base, IDisposable
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(GRDVendorKey));

        //private Handle _grdHandle = new Handle(); // Creates empty handle for Guardant protected container

        private bool _isActive = false;

        private uint _id;
        private byte _model;
        private ushort _keyType;
        private uint _prog;
        private ushort _lanRes;
        private uint _uamOffset;

        private byte[] _userBuf=new byte[4096];
        private ushort _userBufSize = 0;
        private byte[] _truKey = new byte[16];


        public void CleanUserBuf()
        {
            _userBuf = new byte[4096];
            _userBufSize = 0;

        }

        public bool WriteToUserBuf(byte[] buf, ushort offset, ushort length)
        {
            ushort len = length;
            if (buf.Length<len)
            {
                len = (ushort)buf.Length;
            }
            Array.Copy(buf, 0, _userBuf, 0, len);
            if (_userBufSize < offset + len)
            {
                _userBufSize = (ushort)(offset + len);
            }
            return true;
        }

        public bool WriteToUserBuf(string txt, ushort offset, ushort length)
        {
            return WriteToUserBuf(Encoding.GetEncoding(1251).GetBytes(txt), offset, length);
        }

        public bool WriteToUserBuf(int i, ushort offset)
        {
            return WriteToUserBuf(BitConverter.GetBytes(i), offset, 4);
        }

        public bool WriteToUserBuf(uint i, ushort offset)
        {
            return WriteToUserBuf(BitConverter.GetBytes(i), offset, 4);
        }

        public bool WriteToUserBuf(DateTime dt, ushort offset)
        {
            string sdt = dt.Year.ToString("0000") + "-" + dt.Month.ToString("00") + "-" + dt.Day.ToString("00");
            return WriteToUserBuf(sdt, offset, 12);
        }

        /*public bool ReadBytes(out byte[] buffer, uint offset, uint length)
        {
            string logStr = "Reading Bytes : ";
            GrdE retCode = GrdApi.GrdRead(_grdHandle, offset, (int)length, out buffer);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            return (retCode == GrdE.OK);
        }

        public string ReadString(uint offset, uint length)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, length))
            {
                string str = AsciiBytesToString(buffer, 0, (int)length);
                LogIt("Got string : " + str);
                return str;
            }
            else
            {
                LogIt("Got NO string");
                return "";
            }
        }

        public string ReadStringCp1251(uint offset, uint length)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, length))
            {
                string str = Cp1251BytesToString(buffer, 0, (int)length);
                LogIt("Got string : " + str);
                return str;
            }
            else
            {
                LogIt("Got NO string");
                return "";
            }
        }*/

        /*public uint ReadUInt(uint offset)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, 4))
            {
                LogIt("Got UInt : " + BitConverter.ToUInt32(buffer, 0).ToString());
                return BitConverter.ToUInt32(buffer, 0);
            }
            else
            {
                LogIt("Got No UInt");
                return 0;
            }
        }*/

        /*public int ReadInt(uint offset)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, 4))
            {
                LogIt("Got Int : " + BitConverter.ToInt32(buffer, 0).ToString());
                return BitConverter.ToInt32(buffer, 0);
            }
            else
            {
                LogIt("Got No UInt");
                return 0;
            }
        }*/

        /*private static string AsciiBytesToString(byte[] buffer, int offset, int maxLength)
        {
            int maxIndex = offset + maxLength;
            for (int i = offset; i < maxIndex; i++)
            {
                // Skip non-nulls.
                if (buffer[i] != 0) continue;
                // First null we find, return the string.
                return Encoding.ASCII.GetString(buffer, offset, i - offset);
            }
            // Terminating null not found. Convert the entire section from offset to maxLength.
            return Encoding.ASCII.GetString(buffer, offset, maxLength);
        }*/

        /*private static string Cp1251BytesToString(byte[] buffer, int offset, int maxLength)
        {
            int maxIndex = offset + maxLength;
            for (int i = offset; i < maxIndex; i++)
            {
                // Skip non-nulls.
                if (buffer[i] != 0) continue;
                // First null we find, return the string.
                return Encoding.GetEncoding(1251).GetString(buffer, offset, i - offset);
            }
            // Terminating null not found. Convert the entire section from offset to maxLength.
            return Encoding.GetEncoding(1251).GetString(buffer, offset, maxLength);
        }*/

        /*public DateTime ReadDate(uint offset)
        {
            DateTime dt = new DateTime();
            byte[] bts = new byte[12];
            LogIt("Reading date: ");
            if (ReadBytes(out bts, offset, 12))
            {
                string nm = Cp1251BytesToString(bts, 0, 12);
                LogIt("Date  string = " + nm);
                DateTimeFormatInfo dtfi = new CultureInfo("en-US", false).DateTimeFormat;
                try
                {
                    dt = Convert.ToDateTime(nm, dtfi);
                    LogIt("Date = " + dt.ToString());
                }
                catch (Exception e)
                {
                    LogIt("Cannot convert date; " + e.Message);
                }
            }
            return dt;
        }*/

        public void SetTRUAnswer(string base64_question)
        {
            byte[] buf = Convert.FromBase64String(base64_question);
            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Apply encrypted answer data: ";

            retCode = GrdApi.GrdTRU_ApplyAnswer(_grdHandle, // handle to Guardant protected container of dongle with 
                // corresponding pre-generated question 
                                                buf);       // answer data update buffer prepared and encrypted by GrdTRU_EncryptAnswer 

            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            byte[] buffer = new byte[16];
            byte[] initVector = new byte[16];

            logStr = "Testing new mask by GrdTransform test:";
            retCode = GrdApi.GrdTransform(_grdHandle, 0, 8, buffer, 0, initVector);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);
        }

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

        [DllImport("WriteMask.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AddAlgorithm")]
        private static extern void AddAlgorithm([MarshalAs(UnmanagedType.LPArray)] byte[] pbyAlgos,
                                                [MarshalAs(UnmanagedType.LPArray)] byte[] pbyAST,
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



        public GRDVendorKey(uint keyID,byte[] truKey)
        {
            uint len=16;
            if (truKey.Length<16)
            {
                len = (uint)truKey.Length;
            }
            Array.Copy(truKey, _truKey, len);
            Connect(keyID);
        }

        public uint UAMOffset
        {
            get { return _uamOffset; }
        }

        public uint ProgID
        {
            get { return _prog; }
        }

        private GrdE Connect(uint keyID)
        {
            GrdE retCode; // Error code for all Guardant API functions

            FindInfo findInfo; //= new FindInfo();   // structure used in GrdFind()

            string logStr = "";

            // Initialize this copy of GrdAPI. GrdStartup() must be called once before first GrdAPI call at application startup
            logStr = "Initialize this copy of GrdAPI : ";
            retCode = GrdApi.GrdStartup(GrdFMR.Local); // + GrdFMR.Remote if you want to use network dongles
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(new Handle(0), retCode);
            if (retCode != GrdE.OK && retCode != GrdE.AlreadyInitialized)
            {
                return retCode;
            }

            logStr = "Create Guardant protected container : ";
            _grdHandle = GrdApi.GrdCreateHandle(_grdHandle, GrdCHM.MultiThread);
            if (_grdHandle.Address == 0) // Some error found?
            {
                logStr += GrdApi.PrintResult((int)GrdE.MemoryAllocation);
                LogIt(logStr);
                return ErrorHandling(new Handle(0), GrdE.MemoryAllocation);
            }
            logStr += GrdApi.PrintResult((int)GrdE.OK);
            LogIt(logStr);
            ErrorHandling(_grdHandle, GrdE.OK); // Print success information

            logStr = "Storing dongle codes in Guardant protected container : ";
            retCode = GrdApi.GrdSetAccessCodes(_grdHandle,              // Handle to Guardant protected container
                                               PublicCode + CryptPu,    // Public code, should always be specified
                                               ReadCode + CryptRd,      // Private read code; you can omit this code and all following via using of overloaded function;
                                               WriteCode + CryptWr, MasterCode + CryptMs);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            logStr = "Setting dongle search conditions (" + keyID.ToString("X") + ") : ";
            // All following GrdFind() & GrdLogin() calls before next
            // GrdSetFindMode() will use specified flag values. 
            // If dongle field values and specified values do not match, error code is
            // returned. Both access code and flags are required to call the dongle.
            retCode = GrdApi.GrdSetFindMode(_grdHandle,
                                            GrdFMR.Local,
                                            GrdFM.ID,
                                            0,
                                            keyID,
                                            0,
                                            0,
                                            0,
                                            GrdDT.TRU,
                                            GrdFMM.ALL,
                                            GrdFMI.ALL);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            logStr = "Searching for all specified dongles and print info about it's : ";

            uint dongleID;

            retCode = GrdApi.GrdFind(_grdHandle, GrdF.First, out dongleID, out findInfo);
            if (retCode == GrdE.OK) // Print table header if at least one dongle found
            {
                logStr += "; Found dongle with following ID : ";
            }
            else
            {
                ErrorHandling(_grdHandle, retCode);
                return retCode;
            }
            while (retCode == GrdE.OK)
            {
                // Print info about dongles found
                logStr += string.Format(" {0,8:X}", dongleID); // Dongle's ID (unique)
                logStr += " type:" + findInfo.wType.ToString();
                //_type = findInfo.wType;
                //_lanRes = findInfo.wRealNetRes;


                _id = findInfo.dwID;
                _keyType = findInfo.wType;
                _model = (byte)findInfo.dwModel;
                _prog = findInfo.byNProg;
                _lanRes = findInfo.wRealNetRes;
                _uamOffset = findInfo.wWriteProtectS3 - GRDConst.GrdSAMToUAM;


                // Find next dongle
                retCode = GrdApi.GrdFind(_grdHandle, GrdF.Next, out dongleID, out findInfo);
            }
            LogIt(logStr);
            if (retCode == GrdE.AllDonglesFound) // Search has been completed?
            {
                LogIt("Dongles search is complete with no errors");
            }
            else
            {
                ErrorHandling(_grdHandle, retCode);
                if (retCode != GrdE.OK)
                {
                    return retCode;
                }
            }

            logStr = "Searching for the specified local or remote dongle and log in : ";
            // If command line parameter is specified, License Management System functions are used
            // lms = -1;
            // All following Guardant API calls before next GrdCloseHandle()/GrdLogin() will use this dongle
            retCode = GrdApi.GrdLogin(_grdHandle, GrdLM.PerStation);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            _isActive = true;
            return GrdE.OK;
        }

        ~GRDVendorKey()
        {
            Disconnect();
        }

        public void Dispose()
        {
            Disconnect();
        }

        public bool Active
        {
            get { return _isActive; }
        }

        private GrdE Disconnect()
        {
            GrdE retCode; // Error code for all Guardant API functions
            string logStr;
            // -----------------------------------------------------------------
            // Close hGrd handle. Log out from dongle/server & free allocated memory
            // -----------------------------------------------------------------
            logStr = "Closing handle: ";

            if (GrdApi.GrdIsValidHandle(_grdHandle))
            {
                retCode = GrdApi.GrdCloseHandle(_grdHandle);
                logStr += GrdApi.PrintResult((int)retCode);
                LogIt(logStr);
            }
            else
            {
                LogIt("..Already closed!");
            }

            _isActive = false;

            return GrdE.OK;
        }

        private static void LogIt(string txt)
        {
            _logger.Debug(txt);
#if LOCALDEBUG
            /*System.Diagnostics.Debug.WriteLine(txt);
            TextWriter logFile = new StreamWriter(".\\log.txt", true);
            logFile.WriteLine(txt);
            logFile.Close();*/
#endif
        }

        //----------------------------------------------------------------------------------------------
        //  Handle errors
        //  Prints operation result, closes handle and forces program termination on error  
        //  Input:  Handle to Guardant protected Container
        //	Input:  error code
        //	return: error code
        //----------------------------------------------------------------------------------------------
        private GrdE ErrorHandling(Handle hGrd, GrdE nRet)
        {
            if (nRet != GrdE.OK && nRet != GrdE.AlreadyInitialized)
            {
                Disconnect();
            }
            _isActive = false;
            return nRet;
        }

        const ushort AlgoNumGSII64 = 0;
        const byte nsafl_ST_III = 8;
        const byte nsafl_ActivationSrv = 16;
        const byte nsafl_DeactivationSrv = 32;
        const byte nsafl_UpdateSrv = 64;
        const ushort nsafh_ReadSrv = 128;
        const ushort nsafh_ReadPwd = 2;
        const byte rs_algo_GSII64 = 5;
        const byte GrdAdsGSII64Demo = 16;
        const byte GrdArsGSII64Demo = 8;
        const UInt32 GrdApGSII64DemoActivation = 0xAAAAAAAA;
        const UInt32 GrdApGSII64DemoDeactivation = 0xDDDDDDDD;
        const UInt32 GrdApGSII64DemoRead = 0xBBBBBBBB;
        const UInt32 GrdApGSII64DemoUpdate = 0xCCCCCCCC;

        const ushort AlgoNumHash64 = 1;
        const byte RsAlgoHash64 = 6;
        const byte GrdAdsHash64Demo = 16;
        const byte GrdArsHash64Demo = 8;
        const UInt32 GrdApHash64DemoActivation = 0xAAAAAAAA;
        const UInt32 GrdApHash64DemoDeactivation = 0xDDDDDDDD;
        const UInt32 GrdApHash64DemoRead = 0xBBBBBBBB;
        const UInt32 GrdApHash64DemoUpdate = 0xCCCCCCCC;


        public bool WriteMask()
        {
            if (!GrdApi.GrdIsValidHandle(_grdHandle))
            {
                return false;
            }

            byte[] abyMask = new byte[4096];
            byte[] abyMaskHeader = new byte[4096];

            DongleHeaderStruct donglHeader;
            donglHeader.ProgID = 55;
            donglHeader.Version = 10;
            donglHeader.SerialNumber = 123;
            donglHeader.Mask = 12121;

            byte[] abyDongleHeader = GRDUtils.RawSerialize(donglHeader, 14);

            WriteMaskInit(_keyType, _model);

            ushort wMaskSize = 0;
            ushort wASTSize = 0;
            ushort wNumberOfItems = 0;



            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         AlgoNumGSII64,
                         (byte)(nsafl_ST_III + nsafl_ActivationSrv + nsafl_DeactivationSrv + nsafl_UpdateSrv),
                         (ushort)(nsafh_ReadSrv + nsafh_ReadPwd),
                         rs_algo_GSII64,
                         GrdAdsGSII64Demo,
                         GrdArsGSII64Demo,
                         GrdApGSII64DemoActivation,
                         GrdApGSII64DemoDeactivation,
                         GrdApGSII64DemoRead,
                         GrdApGSII64DemoUpdate,
                         null,
                         null,
                         null,
                         null,
                         0xFFFF,
                         0xFFFF,
                         _truKey,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);

            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         AlgoNumHash64,
                         (byte)(nsafl_ST_III + nsafl_ActivationSrv + nsafl_DeactivationSrv + nsafl_UpdateSrv),
                         (ushort)(nsafh_ReadSrv + nsafh_ReadPwd),
                         RsAlgoHash64,
                         GrdAdsHash64Demo,
                         GrdArsHash64Demo,
                         GrdApHash64DemoActivation,
                         GrdApHash64DemoDeactivation,
                         GrdApHash64DemoRead,
                         GrdApHash64DemoUpdate,
                         null,
                         null,
                         null,
                         null,
                         0xFFFF,
                         0xFFFF,
                         _truKey,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);

            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Setting TruKey ";

            retCode = GrdApi.GrdTRU_SetKey(_grdHandle, _truKey);
            logStr += retCode.ToString() + " " + GrdApi.PrintResult((byte)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            logStr = "Writing dongle header : ";
            retCode = GrdApi.GrdWrite(_grdHandle,
                                      (uint)0,
                                      GRDConst.GrdWmUAMOffset,
                                      abyDongleHeader);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            logStr = "Writing Mask header : ";
            retCode = GrdApi.GrdWrite(_grdHandle,
                                      (uint)GRDConst.GrdWmUAMOffset,
                                      wASTSize,
                                      abyMaskHeader);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            logStr = "Writing mask : ";
            retCode = GrdApi.GrdWrite(_grdHandle,
                                      (uint)(GRDConst.GrdWmUAMOffset + wASTSize),
                                      wMaskSize,
                                      abyMask);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            logStr = "Writing test user buffer : ";
            retCode = GrdApi.GrdWrite(_grdHandle,
                                      (uint)(GRDConst.GrdWmUAMOffset + wASTSize + wMaskSize),
                                      _userBufSize,
                                      _userBuf);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            logStr = "Protecting : ";
            retCode = GrdApi.GrdProtect(_grdHandle,
                                        (uint)(GRDConst.GrdWmSAMOffset + wASTSize + wMaskSize),
                                        (uint)(GRDConst.GrdWmSAMOffset + wASTSize + wMaskSize),
                                        wNumberOfItems,
                                        0);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);
            LogIt(" ====> " + (GRDConst.GrdWmUAMOffset + wASTSize + wMaskSize).ToString());

            return true;

        }

        public string GetAnswer(string base64_question)
        {
            //MessageBox.Show(st);

            byte[] buf = Convert.FromBase64String(base64_question);

            TRUQuestionStruct qq = new TRUQuestionStruct();

            qq = (TRUQuestionStruct)GRDUtils.RawDeserialize(buf, qq.GetType());

            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Decrypt and validate question: ";

            retCode = GrdApi.GrdTRU_DecryptQuestion(_grdHandle,     // handle to Guardant protected container of dongle that contains 
                // GSII64 algorithm with the same key as in remote dongle 
                                                    AlgoNumGSII64,  // dongle GSII64 algorithm number with same key as in remote dongle 
                                                    AlgoNumHash64,  // dongle HASH64 algorithm number with same key as in remote dongle 
                                                    qq.question,    // pointer to Question					8 bytes (64 bit) 
                                                    qq.id,          // ID									4 bytes 
                                                    qq.pubKey,      // Public Code							4 bytes 
                                                    qq.hash);       // pointer to Hash of previous 16 bytes	8 bytes 

            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            byte[] abyMask = new byte[4096];
            byte[] abyMaskHeader = new byte[4096];

            DongleHeaderStruct dongleHeader;
            dongleHeader.ProgID = 50;
            dongleHeader.Version = 11;
            dongleHeader.SerialNumber = 543;
            dongleHeader.Mask = 21212;

            byte[] abyDongleHeader = GRDUtils.RawSerialize(dongleHeader, 14);

            WriteMaskInit(_keyType, _model);

            ushort wMaskSize = 0;
            ushort wASTSize = 0;
            ushort wNumberOfItems = 0;

            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         AlgoNumGSII64,
                         (byte)(nsafl_ST_III + nsafl_ActivationSrv + nsafl_DeactivationSrv + nsafl_UpdateSrv),
                         (ushort)(nsafh_ReadSrv + nsafh_ReadPwd),
                         rs_algo_GSII64,
                         GrdAdsGSII64Demo,
                         GrdArsGSII64Demo,
                         GrdApGSII64DemoActivation,
                         GrdApGSII64DemoDeactivation,
                         GrdApGSII64DemoRead,
                         GrdApGSII64DemoUpdate,
                         null,
                         null,
                         null,
                         null,
                         0xFFFF,
                         0xFFFF,
                         _truKey,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);

            AddAlgorithm(abyMask,
                         abyMaskHeader,
                         AlgoNumHash64,
                         (byte)(nsafl_ST_III + nsafl_ActivationSrv + nsafl_DeactivationSrv + nsafl_UpdateSrv),
                         (ushort)(nsafh_ReadSrv + nsafh_ReadPwd),
                         RsAlgoHash64,
                         GrdAdsHash64Demo,
                         GrdArsHash64Demo,
                         GrdApHash64DemoActivation,
                         GrdApHash64DemoDeactivation,
                         GrdApHash64DemoRead,
                         GrdApHash64DemoUpdate,
                         null,
                         null,
                         null,
                         null,
                         0xFFFF,
                         0xFFFF,
                         _truKey,
                         ref wMaskSize,
                         ref wASTSize,
                         ref wNumberOfItems);

            byte[] pbyWholeMask = new byte[GRDConst.GrdWmUAMOffset + wASTSize + wMaskSize + 32 + 100];


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
            Array.Copy(_userBuf,
                       0,
                       pbyWholeMask,
                       GRDConst.GrdWmUAMOffset + wASTSize + wMaskSize,
                       _userBufSize);

            logStr = "Set Init & Protect parameters for Trusted Remote Update: ";
            retCode = GrdApi.GrdTRU_SetAnswerProperties(_grdHandle,                                             // handle to Guardant protected container 
                                                        GrdTRU.Flags_Init | GrdTRU.Flags_Protect,               // use Init & Protect 
                                                        (uint)(GRDConst.GrdWmSAMOffset + wASTSize + wMaskSize), // SAM address of the first byte available for writing in bytes 
                                                        (uint)(GRDConst.GrdWmSAMOffset + wASTSize + wMaskSize), // SAM address of the first byte available for reading in bytes 
                                                        wNumberOfItems,                                         // number of hardware-implemented algorithms in the dongle including all protected items and LMS table of Net III 
                                                        0,                                                      // LMS Item number 
                                                        0);                                                     // Global Flags 


            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            int ansSize;
            byte[] answer;

            logStr = "Encrypt answer for Trusted Remote Update: ";
            retCode = GrdApi.GrdTRU_EncryptAnswer(_grdHandle,                                                         // handle to Guardant protected container 
                // GSII64 algorithm with the same key as in remote dongle 
                // and pre-stored GrdTRU_SetAnswerProperties data if needed 
                                                  GRDConst.GrdSAMToUAM,                                                 // starting address for writing in dongle 
                                                  4 + GRDConst.GrdWmUAMOffset + wASTSize + wMaskSize + _userBufSize,    // size of data to be written 
                                                  pbyWholeMask,                                                         // buffer for data to be written 
                                                  qq.question,                                                          // pointer to decrypted Question 
                                                  AlgoNumGSII64,                                                        // dongle GSII64 algorithm number with the same key as in remote dongle 
                                                  AlgoNumHash64,                                                        // dongle HASH64 algorithm number with the same key as in remote dongle 
                                                  out answer,                                                           // pointer to the buffer for Answer data 
                                                  out ansSize);                                                         // IN: Maximum buffer size for Answer data, OUT: Size of pAnswer buffer 
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            //string base64data = 
            return Convert.ToBase64String(answer, 0, ansSize, Base64FormattingOptions.InsertLineBreaks);
        }

        public string get_question()
        {
            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Generate encrypted question and initialize remote update procedure: ";

            TRUQuestionStruct quest=new TRUQuestionStruct();

            if (((GrdDT)_keyType & GrdDT.RTC) == GrdDT.RTC) // End User dongle supports RTC 
            {
                retCode = GrdApi.GrdTRU_GenerateQuestionTime(
                                                            _grdHandle,                 // handle to Guardant protected container 
                                                            out quest.question,         // pointer to question					8 bytes (64 bit) 
                                                            out quest.id,               // pointer to dongle ID					4 bytes 
                                                            out quest.pubKey,           // pointer to dongle Public Code		4 bytes 
                                                            out quest.dongleTime,       // pointer to dongle time (encrypted)	8 bytes 
                                                            128 * sizeof(byte),         // size of DeadTimes array in bytes 
                                                            out quest.deadTimes,        // pointer to array of DeadTimes 
                                                            out quest.deadTimesNumber,  // number of returned DeadTimes 
                                                            out quest.hash);            // pointer to Hash of previous data		8 bytes 
                                                            
            }
            else
            {
                retCode = GrdApi.GrdTRU_GenerateQuestion(
                                                            _grdHandle,         // handle to Guardant protected container 
                                                            out quest.question, // pointer to question					8 bytes (64 bit) 
                                                            out quest.id,       // pointer to dongle ID					4 bytes 
                                                            out quest.pubKey,   // pointer to dongle Public Code		4 bytes 
                                                            out quest.hash);    // pointer to Hash of previous data		8 bytes 
            }
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            quest.type = _keyType;
            quest.lanRes = _lanRes;
            quest.model = _model;

            byte[] buf = GRDUtils.RawSerialize(quest);

            return Convert.ToBase64String(buf, 0, buf.Length,Base64FormattingOptions.None);


        }

       

    }
}
