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
    public sealed class GRD
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GRD));

        public static readonly GRD Instance = new GRD();

        private Handle _grdHandle = new Handle();    // Creates empty handle for Guardant protected container

        private string _keyId;

        /// <summary>
        /// Private constructor prevents instantiation from other classes
        /// </summary>
        private GRD()
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

        ~GRD()
        {
            Disconnect();
        }

        public string GetKeyID()
        {
            return _keyId;
        }

        public int GetFarmsCnt()
        {

            uint farms = 0;
            int farmCnt = -1;
            const uint addr = 1314;

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Farm cnt : ";
            retCode = GrdApi.GrdRead(_grdHandle, addr, out farms);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                logStr += string.Format("; CNT = {0:D}", farms);
                farmCnt = (int)farms;
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return farmCnt;
        }

        public string GetOrgName()
        {

            string nm = "";
            const uint addr = 1214;
            
            byte[] bts = new byte[100];
            
            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Organization Name : ";
            retCode = GrdApi.GrdRead(_grdHandle,addr,100,out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                nm = Cp1251BytesToString(bts, 0, 100);
                logStr += "; Name = " + nm;
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return nm;
        }

        public bool ValidKey()
        {


            byte[] bts = new byte[32];
            uint addr;
            addr = 1182;
            string marker = "";

            bool valid;

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Marker : ";
            retCode = GrdApi.GrdRead(_grdHandle, addr, 16, out bts);
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

        private uint GetFlags(uint byteNum)
        {
            const uint addr = 1318;

            byte[] bts = new byte[8];

            uint res = 0;

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Role Flags : ";
            retCode = GrdApi.GrdRead(_grdHandle, addr, 8, out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                UInt64 flgs = BitConverter.ToUInt64(bts, 0);
                logStr += string.Format("; Full Flags = {0:D}", flgs);
                logStr += string.Format("; Flags = {3} - {2} - {1} - {0}", Convert.ToString(bts[0], 2), Convert.ToString(bts[1], 2), Convert.ToString(bts[2], 2), Convert.ToString(bts[3], 2));
                res = bts[byteNum];
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return res;
        }

        public bool GetFlagZootech()
        {
            return ((GetFlags(0) & Convert.ToByte("00000001", 2)) > 0);
        }

        public bool GetFlagGenetics()
        {
            return ((GetFlags(0) & Convert.ToByte("00000010", 2)) > 0);
        }
        
        public bool GetFlagServer()
        {
            return ((GetFlags(0) & Convert.ToByte("00000100", 2)) > 0);
        }

        public DateTime GetDateStart()
        {
            const uint addr = 1326;

            DateTime dt = new DateTime();

            byte[] bts = new byte[12];

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading Start Date : ";
            retCode = GrdApi.GrdRead(_grdHandle, addr, 12, out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                string nm = Cp1251BytesToString(bts, 0, 12);

                DateTimeFormatInfo dtfi = new CultureInfo("en-US", false).DateTimeFormat;

                dt = Convert.ToDateTime(nm, dtfi);
                logStr += "; Date = " + dt.ToString();
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return dt;
        }

        
        public DateTime GetDateEnd()
        {
            const uint addr = 1338;

            DateTime dt = new DateTime();

            byte[] bts = new byte[12];

            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";

            logStr = "Reading End Date : ";
            retCode = GrdApi.GrdRead(_grdHandle, addr, 12, out bts);
            logStr += GrdApi.PrintResult((int)retCode);
            if (retCode == GrdE.OK)
            {
                string nm = Cp1251BytesToString(bts, 0, 12);

                DateTimeFormatInfo dtfi = new CultureInfo("en-US", false).DateTimeFormat;

                dt = Convert.ToDateTime(nm, dtfi);
                logStr += "; Date = " + dt.ToString();
            }
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            return dt;
        }


        private GrdE Connect()
        {
            GrdE retCode;                       // Error code for all Guardant API functions
            //string KeyId = "";

            const uint cryptPu = 0x8568683U;
            const uint cryptRd = 0x56547675U;

            uint PublicCode = 0x9BD54F75 - cryptPu;    // Must be encoded             
            uint ReadCode = 0xFE8392B2 - cryptRd;    // Must be encoded             

            // Variables to use in GrdSetFindMode()
            GrdFMR remoteMode;         // Operation mode flags                    
            GrdFM dongleFlags;        // Operation mode flags                    
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

            Int32 lms;

            string logStr = "";

            log.Debug("Guardant Stealth/Net III example for C# (MS .NET Environment)\n (C) 2006 Aktiv Co. All rights reserved");


            // Initialize this copy of GrdAPI. GrdStartup() must be called once before first GrdAPI call at application startup
            //            log.Debug("Initialize this copy of GrdAPI :");
            logStr = "Initialize this copy of GrdAPI : ";
            retCode = GrdApi.GrdStartup(GrdFMR.Local);	// + GrdFMR.Remote if you want to use network dongles
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
                PublicCode + cryptPu,   // Public code, should always be specified
                ReadCode + cryptRd);    // Private read code; you can omit this code and all following via using of overloaded function;
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
            remoteMode = GrdFMR.Local; 				            // Local dongles only
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
            if (retCode == GrdE.OK)		// Print table header if at least one dongle found
            {
                logStr += "; Found dongle with following ID : ";
            }
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
            }

            // -----------------------------------------------------------------
            // Search for the specified local or remote dongle and log in
            // -----------------------------------------------------------------
            logStr = "Searching for the specified local or remote dongle and log in : ";
            // If command line parameter is specified, License Management System functions are used
            lms = -1;
            // All following Guardant API calls before next GrdCloseHandle()/GrdLogin() will use this dongle
            retCode = GrdApi.GrdLogin(_grdHandle, lms, GrdLM.PerStation);
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
            uint addr;
            addr = 1182;
            string marker = "";

            logStr = "Reading Marker : ";
            retCode = GrdApi.GrdRead(_grdHandle,addr,16,out bts);
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

        private static string AsciiBytesToString(byte[] buffer, int offset, int maxLength)
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

        private static string Cp1251BytesToString(byte[] buffer, int offset, int maxLength)
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

        private GrdE Disconnect()
        {
            GrdE retCode;                       // Error code for all Guardant API functions
            string logStr = "";
            // -----------------------------------------------------------------
            // Close hGrd handle. Log out from dongle/server & free allocated memory
            // -----------------------------------------------------------------
            logStr = "Closing handle: ";
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
        //----------------------------------------------------------------------------------------------
        //  Handle errors
        //  Prints operation result, closes handle and forces program termination on error  
        //  Input:  Handle to Guardant protected Container
        //	Input:  error code
        //	return: error code
        //----------------------------------------------------------------------------------------------
        private static GrdE ErrorHandling(Handle hGrd, GrdE nRet)
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


}
#endif