#define PROTECTED
#if PROTECTED
using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Guardant;
using log4net;

namespace RabGRD
{
    /// <summary>
    /// Thread-safe singleton example created at first call
    /// </summary>
    public partial class GRD_Base
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GRD_Base));

        //Далее список адресов данных в ключе
        protected const uint USER_DATA_BEGINING = 1182;
        protected const uint DEV_MARKER_OFFSET = 0;
        protected const uint ORGANIZATION_NAME_OFFSET = 32;
        protected const uint MAX_BUILDINGS_COUNT_OFFSET = 132;
        protected const uint FLAGS_MASK_OFFSET = 136;
        protected const uint FARM_START_DATE_OFFSET = 144;
        protected const uint FARM_STOP_DATE_OFFSET = 156;
        protected const uint TEMP_FLAGS_MASK_OFFSET = 168;
        protected const uint END_TEMP_FLAGS_OFFSET = 176;

        /// <summary>
        /// Достпные функции. Добавил тут - измени метод GetFlag
        /// </summary>
		[Flags]
        public enum FlagType
        {
			None = 0,
            RabNet = 1,             //0 bit 
            Genetics = 1 << 1,      //1 bit
            RabDump = 1 << 2,       //2 bit
            Butcher = 1 << 3,       //3 bit
            ReportPlugIns = 1 << 4,  //4 bit
            ServerDump = 1 << 5,    //5 bit
            WebReports = 1 << 6   	//6 bit
        }

        protected Handle _grdHandle = new Handle();    // Creates empty handle for Guardant protected container

        protected string _keyId;

        protected int _farmCntCache;

        protected int _cacheTicks;

        /// <summary>
        /// Private constructor prevents instantiation from other classes
        /// </summary>
        public GRD_Base()
        {
            try
            {
                Connect();
            }
            catch (Exception e)
            {
//                if (e.InnerException is System.DllNotFoundException)
//                {
                    MessageBox.Show(e.InnerException.Message, "Фатальная ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Debug(e.InnerException.Message);
                    Environment.Exit(100);
//                }

            }
        }

        ~GRD_Base()
        {
            Disconnect();
        }

        public string GetKeyID()
        {
            return _keyId;
        }

        /// <summary>
        /// Получает максимально-допустимое количество МИНИферм
        /// </summary>
        /// <returns></returns>
        public int GetFarmsCnt()
        {

            uint farms = 0;
            int farmCnt = -1;

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Farm cnt : ";
            retCode = GrdApi.GrdRead(_grdHandle, USER_DATA_BEGINING + MAX_BUILDINGS_COUNT_OFFSET, out farms);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                logStr += string.Format("; CNT = {0:D}", farms);
                farmCnt = (int)farms;
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            _farmCntCache = farmCnt;
            _cacheTicks = Environment.TickCount & Int32.MaxValue;

            return farmCnt;
        }

        public int GetFarmsCntCache()
        {

            if ((Environment.TickCount & Int32.MaxValue) > _cacheTicks+60*1000)
            {
                GetFarmsCnt();
            }

            return _farmCntCache;
        }

        /// <summary>
        /// Получить название организации,
        /// на которую выписан ключ
        /// </summary>
        public string GetOrgName()
        {
            uint addr = USER_DATA_BEGINING + ORGANIZATION_NAME_OFFSET;
            log.Debug("Reading Org Name: ");
            return ReadStringCp1251(addr, MAX_BUILDINGS_COUNT_OFFSET - ORGANIZATION_NAME_OFFSET);

            /*string nm = "";                     
            byte[] bts = new byte[100];          
            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Organization Name : ";
            retCode = GrdApi.GrdRead(_grdHandle, USER_DATA_BEGINING + ORGANIZATION_NAME_OFFSET, 100, out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                nm = Cp1251BytesToString(bts, 0, 100);
                logStr += "; Name = " + nm;
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return nm;*/
        }

        public bool ValidKey()
        {
            byte[] bts = new byte[32];
            string marker = "";
            bool valid;

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Marker : ";
            retCode = GrdApi.GrdRead(_grdHandle, USER_DATA_BEGINING + DEV_MARKER_OFFSET, 16, out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                marker = AsciiBytesToString(bts, 0, 32);
                logStr += "; \"" + marker + "\"";
            }
            if (marker != "9-bits RabSoft")
            {
                logStr += "; No correct marker";
                log.Debug(logStr);
                ErrorHandling(_grdHandle, GrdE.VerifyError);
                valid = false;
            }
            else
            {
                logStr += "; Correct marker";
                log.Debug(logStr);
                ErrorHandling(_grdHandle, retCode);
                valid = true;
            }
            return valid;
        }

        /// <summary>
        /// Возвращает число(битовую маску), содержащее битовые флаги включенных функций
        /// </summary>
        /// <param name="byteNum">Порядковый номер байта, из которого читать флаг</param>
        protected uint GetFlags(uint byteNum)
        {         
            byte[] bts = new byte[8];

            uint res = 0;

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Role Flags : ";
            retCode = GrdApi.GrdRead(_grdHandle, USER_DATA_BEGINING + FLAGS_MASK_OFFSET, 8, out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                UInt64 flgs = BitConverter.ToUInt64(bts, 0);
                logStr += string.Format("; Full Flags = {0:D}", flgs);
                logStr += string.Format("; Flags = {3} - {2} - {1} - {0}", 
                                    Convert.ToString(bts[0], 2), 
                                    Convert.ToString(bts[1], 2), 
                                    Convert.ToString(bts[2], 2), 
                                    Convert.ToString(bts[3], 2));
                res = bts[byteNum];
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return res;
        }

        protected uint GetTempFlags(uint byteNum)
        {
            byte[] bts = new byte[12];

            uint res = 0;

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading TempDateEnd : ";
            retCode = GrdApi.GrdRead(_grdHandle, USER_DATA_BEGINING + END_TEMP_FLAGS_OFFSET, 12, out bts);
            logStr += GrdApi.PrintResult((int)retCode);

            if (retCode == GrdE.OK)
            {
                string date = Cp1251BytesToString(bts, 0, 12);
                if (date != "")
                {
                    DateTimeFormatInfo dtfi = new CultureInfo("en-US", false).DateTimeFormat;
                    DateTime dt = Convert.ToDateTime(date, dtfi);
                    if (dt > DateTime.Now)
                    {
                        bts = new byte[8];
                        logStr = "Reading TempRole Flags : ";
                        retCode = GrdApi.GrdRead(_grdHandle, USER_DATA_BEGINING + TEMP_FLAGS_MASK_OFFSET, 8, out bts);
                        logStr += GrdApi.PrintResult((int)retCode);
                        if (retCode == GrdE.OK)
                        {
                            UInt64 flgs = BitConverter.ToUInt64(bts, 0);
                            logStr += string.Format("; Full TempFlags = {0:D}", flgs);
                            logStr += string.Format("; TempFlags = {3} - {2} - {1} - {0}", Convert.ToString(bts[0], 2), Convert.ToString(bts[1], 2), Convert.ToString(bts[2], 2), Convert.ToString(bts[3], 2));
                            res = bts[byteNum];
                        }
                    }
                }
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return res;
        }

        public bool GetFlag(FlagType ft)
        {
            /*uint bait = (uint)( (int)ft / 8 );
            int bit =(int)ft % 8;           
            string mask = "";
            for (int i = 0; i < 8; i++)
                mask = (i == bit ? '1' : '0') + mask;
            return (((GetFlags(bait) & Convert.ToByte(mask, 2)) > 0) || (GetTempFlags(bait) & Convert.ToByte(mask, 2)) > 0);*/
            
            FlagType flags = (FlagType)GetFlags(0);
            FlagType flagsTemp = (FlagType)GetTempFlags(0);

            log.Debug("========================> " + flags.ToString() + " " + ((int)flags).ToString());
            log.Debug("========================> " + flagsTemp.ToString() + " " + ((int)flagsTemp).ToString());

            return ((flags & ft) == ft) || ((flagsTemp & ft) == ft);
        }

        public DateTime GetDateStart()
        {
            uint addr = USER_DATA_BEGINING + FARM_START_DATE_OFFSET;
            log.Debug("Reading Date start: ");
            return ReadDate(addr);
        }

        public DateTime GetDateEnd()
        {
            uint addr = USER_DATA_BEGINING + FARM_STOP_DATE_OFFSET;
            log.Debug("Reading Date end: ");
            return ReadDate(addr);
        }

        public DateTime ReadDate(uint offset)
        {
            DateTime dt = new DateTime();
            byte[] bts = new byte[12];
            log.Debug("Reading date: ");
            if (ReadBytes(out bts, offset, 12))
            {
                string nm = Cp1251BytesToString(bts, 0, 12);
                log.Debug("Date  string = " + nm);
                DateTimeFormatInfo dtfi = new CultureInfo("en-US", false).DateTimeFormat;
                try
                {
                    dt = Convert.ToDateTime(nm, dtfi);
                    log.Debug("Date = " + dt.ToString());
                }
                catch (Exception e)
                {
                    log.Debug("Cannot convert date; " + e.Message);
                }
            }
            return dt;
        }

        protected bool ReadBytes(out byte[] buffer, uint offset, uint length)
        {
            string logStr = "Reading Bytes : ";
            GrdE retCode = GrdApi.GrdRead(_grdHandle, offset, (int)length, out buffer);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return (retCode == GrdE.OK);
        }

        protected string ReadString(uint offset, uint length)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, length))
            {
                string str = AsciiBytesToString(buffer, 0, (int)length);
                log.Debug("Got string : " + str);
                return str;
            }
            else
            {
                log.Debug("Got NO string");
                return "";
            }
        }

        protected string ReadStringCp1251(uint offset, uint length)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, length))
            {
                string str = Cp1251BytesToString(buffer, 0, (int)length);
                log.Debug("Got string : " + str);
                return str;
            }
            else
            {
                log.Debug("Got NO string");
                return "";
            }
        }

        protected uint ReadUInt(uint offset)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, 4))
            {
                log.Debug("Got UInt : " + BitConverter.ToUInt32(buffer, 0).ToString());
                return BitConverter.ToUInt32(buffer, 0);
            }
            else
            {
                log.Debug("Got No UInt");
                return 0;
            }
        }

        protected int ReadInt(uint offset)
        {
            byte[] buffer;
            if (ReadBytes(out buffer, offset, 4))
            {
                log.Debug("Got Int : " + BitConverter.ToInt32(buffer, 0).ToString());
                return BitConverter.ToInt32(buffer, 0);
            }
            else
            {
                log.Debug("Got No UInt");
                return 0;
            }
        }

        protected GrdE Connect()
        {
            GrdE retCode;                       // Error code for all Guardant API functions
            //string KeyId = "";

            /*const uint cryptPu = 0x8568683U;
            const uint cryptRd = 0x56547675U;
            uint PublicCode = 0x9BD54F75 - cryptPu;    // Must be encoded             
            uint ReadCode = 0xFE8392B2 - cryptRd;    // Must be encoded      */       

            // Variables to use in GrdSetFindMode()
            GrdFMR remoteMode;       // Operation mode flags                    
            GrdFM dongleFlags;       // Operation mode flags                    
            uint programNumber = 0;  // Program number                          
            uint version = 0;        // Version                                 
            uint dongleID = 0;       // Dongle ID
            uint modelID = 0;        // Dongle model
            //byte[] typeID = new byte[2];// Dogle type
            uint serialNumber;       // Serial number                           
            uint bitMask;            // Bit mask                                
            GrdDT dongleType;         // Dongle type                             
            GrdFMM dongleModel;        // Dongle model                             
            GrdFMI dongleInterface;    // Dongle interface                             
            FindInfo findInfo; //= new FindInfo();   // structure used in GrdFind()
            UInt32 tempData;           // Temporary data         
            //UInt32 lms;
            string logStr = "";

            // Initialize this copy of GrdAPI. GrdStartup() must be called once before first GrdAPI call at application startup
            logStr = "Initialize this copy of GrdAPI : ";
            retCode = GrdApi.GrdStartup(GrdFMR.Local | GrdFMR.Remote);	// + GrdFMR.Remote if you want to use network dongles
            //Console.WriteLine("Address of hGrd: " + hGrd.Address);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Creating Grd protected container & returning it's handle
            // -----------------------------------------------------------------
            logStr = "Create Guardant protected container : ";
            _grdHandle = GrdApi.GrdCreateHandle(_grdHandle, GrdCHM.MultiThread);
            if (_grdHandle.Address == 0)					// Some error found?
            {
                logStr += GrdApi.PrintResult((int)retCode);
                log.Debug(logStr);
                return ErrorHandling(new Handle(0), GrdE.MemoryAllocation);
            }
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, GrdE.OK);	        // Print success information
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Store dongle codes in Guardant protected container
            // -----------------------------------------------------------------
            logStr = "Storing dongle codes in Guardant protected container : ";
            retCode = GrdApi.GrdSetAccessCodes(_grdHandle,	// Handle to Guardant protected container
                                    PublicCode + CryptPu,   // Public code, should always be specified
                                    ReadCode + CryptRd);    // Private read code; you can omit this code and all following via using of overloaded function;
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Set dongle search criteria
            // -----------------------------------------------------------------
            logStr = "Setting dongle search conditions : ";
            remoteMode = GrdFMR.Local | GrdFMR.Remote; 				            // Search Local & Remote
            dongleFlags = GrdFM.NProg | GrdFM.Ver | GrdFM.Type;	// Check by bProg, bVer & dongle type flag
            programNumber = 1;     								// Check by specified program number                
            dongleID = 0;		     							// This search mode is not used                     
            serialNumber = 0;     							    // This search mode is not used                     
            version = 1;    									// Check by specified version                       
            bitMask = 0;     									// This search mode is not used                     
            dongleType = GrdDT.GSII64;				            // Dongle that supports GSII64 algorithm   
            dongleModel = GrdFMM.ALL;					        // Guardant Stealth III dongle
            dongleInterface = GrdFMI.ALL;		   		        // of any interface

            // All following GrdFind() & GrdLogin() calls before next
            // GrdSetFindMode() will use specified flag values. 
            // If dongle field values and specified values do not match, error code is
            // returned. Both access code and flags are required to call the dongle.
            retCode = GrdApi.GrdSetFindMode(_grdHandle,
                                            remoteMode,
                                            dongleFlags,
                                            programNumber,
                                            dongleID,
                                            serialNumber,
                                            version,
                                            bitMask,
                                            dongleType,
                                            dongleModel,
                                            dongleInterface);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Search for all specified dongles and print ID's
            // -----------------------------------------------------------------
            logStr = "Searching for all specified dongles and print info about it's : ";
            retCode = GrdApi.GrdFind(_grdHandle, GrdF.First, out dongleID, out findInfo);
            if (retCode != GrdE.OK && retCode!= GrdE.AllDonglesFound)		// Print table header if at least one dongle found
            {
                return retCode;
            }

            /*Если вставлен локальный ключ, то прога его находит. затем ищет удаленный ключ - не находит его и делает return. Так что ищем первый попавшийся
            while (retCode == GrdE.OK)
            {
                // Print info about dongles found
                logStr += string.Format(" {0,8:X}", dongleID);			    // Dongle's ID (unique)
                // Find next dongle
                retCode = GrdApi.GrdFind(_grdHandle, GrdF.Next, out dongleID, out findInfo);
            }
            log.Debug(logStr);
            if (retCode == GrdE.AllDonglesFound)						// Search has been completed?
            {
                log.Debug("Dongles search is complete with no errors");
            }
            else
            {
                ErrorHandling(_grdHandle, retCode);
                if (retCode != GrdE.OK)
                {
                    return retCode;
                }
            }*/

            // -----------------------------------------------------------------
            // Search for the specified local or remote dongle and log in
            // -----------------------------------------------------------------
            logStr = "Searching for the specified local or remote dongle and log in : ";
            // If command line parameter is specified, License Management System functions are used
            //lms = Int32.MaxValue;
            // All following Guardant API calls before next GrdCloseHandle()/GrdLogin() will use this dongle
            retCode = GrdApi.GrdLogin(_grdHandle, 0, GrdLM.PerStation);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Obtain model value of the dongle via hGrd handle
            // -----------------------------------------------------------------
            logStr = "Obtaining model value of the dongle via hGrd handle : ";
            retCode = GrdApi.GrdGetInfo(_grdHandle, GrdGIL.Model, out modelID);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                logStr += string.Format("; ModelID = {0:X}", modelID);
                if (modelID == (uint)GrdDM.GS3SU)
                {
                    logStr += "; We got right model";
                }
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Set System Address Mode (SAM) as default mode
            // -----------------------------------------------------------------
            logStr = "Setting System Address Mode as default mode : ";
            retCode = GrdApi.GrdSetWorkMode(_grdHandle, GrdWM.SAM, GrdWMFM.DriverAuto);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Read ID field value from the dongle. 
            // -----------------------------------------------------------------           
            logStr = "Reading ID field value : ";
            retCode = GrdApi.GrdRead(_grdHandle, GrdSAM.dwID, out tempData);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                logStr += string.Format("; ID = {0:X}", tempData);
                _keyId = string.Format("{0:X}", tempData);
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            logStr = "Setting User Address Mode as default mode : ";
            retCode = GrdApi.GrdSetWorkMode(_grdHandle, GrdWM.UAM, GrdWMFM.DriverAuto);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

            byte[] bts = new byte[32];
            string marker = "";

            logStr = "Reading Marker : ";
            retCode = GrdApi.GrdRead(_grdHandle, USER_DATA_BEGINING + DEV_MARKER_OFFSET, 16, out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {               
                marker = AsciiBytesToString(bts, 0, 32); 
                logStr += "; \"" + marker + "\"";
            }
            if (marker != "9-bits RabSoft")
            {
                
                logStr += "; No correct marker";
                log.Debug(logStr);
                ErrorHandling(_grdHandle, GrdE.VerifyError);
                return GrdE.VerifyError;
            }
            else
            {
                logStr += "; Correct marker";
                log.Debug(logStr);
                ErrorHandling(_grdHandle, retCode);
            }

            return GrdE.OK;
        }

        protected static string AsciiBytesToString(byte[] buffer, int offset, int maxLength)
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
        }

        protected static string Cp1251BytesToString(byte[] buffer, int offset, int maxLength)
        {
            int maxIndex = offset + maxLength;

            for (int i = offset; i < maxIndex; i++)
            {
                // Skip non-nulls.
                if (buffer[i] != 0) continue;
                // First null we find, return the string.
                //               return Encoding.ASCII.GetString(buffer, offset, i - offset);
                return Encoding.GetEncoding(1251).GetString(buffer, offset, i - offset);
            }
            // Terminating null not found. Convert the entire section from offset to maxLength.
            return Encoding.GetEncoding(1251).GetString(buffer, offset, maxLength);
        }

        protected GrdE Disconnect()
        {
            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            // -----------------------------------------------------------------
            // Close hGrd handle. Log out from dongle/server & free allocated memory
            // -----------------------------------------------------------------
            logStr = "Closing dongle handle: ";
            retCode = GrdApi.GrdCloseHandle(_grdHandle);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            // -----------------------------------------------------------------
            // Deinitialize this copy of GrdAPI. 
            // GrdCleanup() must be called after last GrdAPI call before program termination
            // -----------------------------------------------------------------
            logStr = "Deinitializing this copy of GrdAPI : ";
            retCode = GrdApi.GrdCleanup();
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            //Console.ReadLine();

            return GrdE.OK;
        }

        /// <summary>
        /// Handle errors
        /// Prints operation result, closes handle and forces program termination on error  
        /// </summary>
        /// <param name="hGrd">Handle to Guardant protected Container</param>
        /// <param name="nRet">error code</param>
        /// <returns>error code</returns>
        protected GrdE ErrorHandling(Handle hGrd, GrdE nRet)
        {
            // print the result of last executed function
            //log.Debug(GrdApi.PrintResult((int)nRet));
            string logStr = "";

            if (nRet != GrdE.OK)
            {
                if (hGrd.Address != 0)	// Perform some cleanup operations if hGrd handle exists
                {
                    // Close hGrd handle, log out from dongle/server, free allocated memory
                    logStr = ("Closing handle: ");
                    nRet = GrdApi.GrdCloseHandle(hGrd);
                    logStr += GrdApi.PrintResult((int)nRet);
                    log.Debug(logStr);
                }

                // Deinitialize this copy of GrdAPI. GrdCleanup() must be called after last GrdAPI call before program termination
                logStr = "Deinitializing this copy of GrdAPI : ";
                nRet = GrdApi.GrdCleanup();
                logStr += GrdApi.PrintResult((int)nRet);
                log.Debug(logStr);

                // Terminate application
                //Environment.Exit((int)nRet);
            }
            return nRet;
        }
    }

    public class GRD
    {       
        public static readonly GRD_Base Instance = new GRD_Base();
    }
}
#endif