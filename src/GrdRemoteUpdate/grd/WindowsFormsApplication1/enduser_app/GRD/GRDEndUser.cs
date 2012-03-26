#define LOCALDEBUG
#if PROTECTED
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Guardant;

using log4net;


namespace RabGRD
{
    
    /// <summary>
    /// Thread-safe singleton example created at first call
    /// </summary>
    public sealed partial class GRDEndUser:GRD_Base
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(GRDEndUser));
        
        //public static GRDEndUser Instance = new GRDEndUser();                         
        //public static readonly GRDEndUser Instance = new GRDEndUser();
        //private Handle _grdHandle = new Handle(); // Creates empty handle for Guardant protected container
        //private string _keyId;

        private byte _modelid;
        private ushort _type;
        private ushort _lanRes;
        private uint _id;
        private uint _uamOffset;

        public void ResetSearchConditions()
        {
            _findPropRemoteMode = GrdFMR.Local;                 // Local dongles only
            _findPropDongleFlags = GrdFM.Type | GrdFM.NProg;    // Check by bProg, bVer & dongle type flag
            _findPropProgramNumber = ProgIDCode;                // Check by specified program number                
            _findPropDongleID = 0;                              // This search mode is not used                     
            _findPropSerialNumber = 0;                          // This search mode is not used                     
            _findPropProgramVersion = 0;                        // Check by specified FindProp_program_version                       
            _findPropBitMask = 0;                               // This search mode is not used                     
            _findPropDongleType = GrdDT.TRU;                    // Dongle that supports GSII64 algorithm   
            _findPropDongleModel = GrdFMM.ALL;                  // Guardant Stealth III dongle
            _findPropDongleInterface = GrdFMI.ALL;              // of any interface
        }

        public void SetTRUAnswer()
        {
            TextReader tr = new StreamReader("answer.txt");

            string st = tr.ReadToEnd();

            Regex test = new Regex(@"^##.*$", RegexOptions.Multiline);

            st = test.Replace(st, string.Empty);

            byte[] buf = Convert.FromBase64String(st);

            tr.Close();

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

            logStr = "Testing new mask by GrdTransform test:";
            retCode = GrdApi.GrdTransform(_grdHandle, 0, 8, buffer, 0, initVector);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
        }

        public void GetTRUQuestion()
        {
            GrdE retCode; // Error code for all Guardant API functions
            string logStr = "Generate encrypted question and initialize remote update procedure: ";

            TRUQuestionStruct quest = new TRUQuestionStruct();

            if (((GrdDT)_type & GrdDT.RTC) == GrdDT.RTC) // End User dongle supports RTC 
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
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);

            quest.type = _type;
            quest.lanRes = _lanRes;
            quest.model = _modelid;

            byte[] buf = GRDUtils.RawSerialize(quest);

            string base64data;

            base64data = Convert.ToBase64String(buf, 0, buf.Length, Base64FormattingOptions.None);

            TextWriter tw = new StreamWriter("question.txt");

            tw.WriteLine("############################################################################");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("##     Key update question data                                           ##");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("############################################################################");

            tw.WriteLine(base64data);

            tw.Close();
        }

        #region rudiments
        /// <summary>
        /// Private constructor prevents instantiation from other classes
        /// </summary>
        /* GRDEndUser()
        {
            try
            {
                Connect();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException.Message, "Фатальная ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogIt(e.InnerException.Message);
                Environment.Exit(100);
            }
        }

        ~GRDEndUser()
        {
            Disconnect();
        }*/

        /*private static void LogIt(string txt)
        {
            Log.Debug(txt);
#if LOCALDEBUG
            System.Diagnostics.Debug.WriteLine(txt);
            TextWriter logFile = new StreamWriter(".\\log.txt",true);
            logFile.WriteLine(txt);
            logFile.Close();
#endif
        }*/

        /*public uint GetKeyID()
        {
            return _id;
        }*/

        //partial void EditSearchConditions();


        /*private GrdE Connect()
        {
            GrdE retCode; // Error code for all Guardant API functions

            // Variables to use in GrdSetFindMode()
            //GrdFMR remoteMode;      // Operation mode flags                    
            //GrdFM FindProp_dongleFlags;      // Operation mode flags                    
            //uint FindProp_programNumber = 0; // Program number                          
            //uint FindProp_program_version = 0;       // Version                                 
            //uint FindProp_dongleID = 0;      // Dongle ID
            //uint FindProp_modelID = 0;       // Dongle model
            //byte[] typeID = new byte[2];// Dogle type
            //uint FindProp_serialNumber;      // Serial number                           
            //uint FindProp_bitMask;           // Bit mask                                
            //GrdDT FindProp_dongleType;       // Dongle type                             
            //GrdFMM FindProp_dongleModel;     // Dongle model                             
            //GrdFMI FindProp_dongleInterface; // Dongle interface                             


            FindInfo findInfo; //= new FindInfo();   // structure used in GrdFind()

            string logStr = "";

            // Initialize this copy of GrdAPI. GrdStartup() must be called once before first GrdAPI call at application startup
            logStr = "Initialize this copy of GrdAPI : ";
            retCode = GrdApi.GrdStartup(GrdFMR.Local); // + GrdFMR.Remote if you want to use network dongles
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK && retCode != GrdE.AlreadyInitialized)
            {
                return retCode;
            }

            // -----------------------------------------------------------------
            // Creating Grd protected container & returning it's handle
            // -----------------------------------------------------------------
            logStr = "Create Guardant protected container : ";
            _grdHandle = GrdApi.GrdCreateHandle(_grdHandle, GrdCHM.MultiThread);
            if (_grdHandle.Address == 0)	// Some error found?
            {
                logStr += GrdApi.PrintResult((int)retCode);
                log.Debug(logStr);
                return ErrorHandling(new Handle(0), GrdE.MemoryAllocation);
            }
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, GrdE.OK);	// Print success information
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
            _findPropRemoteMode = GrdFMR.Local;      // Local dongles only
            _findPropDongleFlags = GrdFM.Type;       // Check by bProg, bVer & dongle type flag
            //            FindProp_dongleFlags = GrdFM.NProg | GrdFM.Ver | GrdFM.Type; // Check by bProg, bVer & dongle type flag
            //            FindProp_programNumber = 1; // Check by specified program number                
            _findPropProgramNumber = 0;              // Check by specified program number                
            _findPropDongleID = 0;                   // This search mode is not used                     
            _findPropSerialNumber = 0;               // This search mode is not used                     
//            FindProp_program_version = 1; // Check by specified FindProp_program_version                       
            _findPropProgramVersion = 0;                    // Check by specified FindProp_program_version                       
            _findPropBitMask = 0;                    // This search mode is not used                     
//            FindProp_dongleType = GrdDT.GSII64; // Dongle that supports GSII64 algorithm   
            _findPropDongleType = GrdDT.TRU;         // Dongle that supports GSII64 algorithm   
            _findPropDongleModel = GrdFMM.ALL;       // Guardant Stealth III dongle
            _findPropDongleInterface = GrdFMI.ALL;   // of any interface

            EditSearchConditions();

            // All following GrdFind() & GrdLogin() calls before next
            // GrdSetFindMode() will use specified flag values. 
            // If dongle field values and specified values do not match, error code is
            // returned. Both access code and flags are required to call the dongle.
            retCode = GrdApi.GrdSetFindMode(_grdHandle,
                                            _findPropRemoteMode,
                                            _findPropDongleFlags,
                                            _findPropProgramNumber,
                                            _findPropDongleID,
                                            _findPropSerialNumber,
                                            _findPropProgramVersion,
                                            _findPropBitMask,
                                            _findPropDongleType,
                                            _findPropDongleModel,
                                            _findPropDongleInterface);
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
            retCode = GrdApi.GrdFind(_grdHandle, GrdF.First, out _findPropDongleID, out findInfo);
            if (retCode == GrdE.OK) // Print table header if at least one dongle found
            {
                logStr += "; Found dongle with following ID : ";
            }
            while (retCode == GrdE.OK)
            {
                // Print info about dongles found
                logStr += string.Format(" {0,8:X}", _findPropDongleID); // Dongle's ID (unique)
                logStr += " type:"+findInfo.wType.ToString();
                _id = findInfo.dwID;
                _type = findInfo.wType;
                _lanRes = findInfo.wRealNetRes;
                _modelid = (byte)findInfo.dwModel;

                _uamOffset = findInfo.wWriteProtectS3 - GRDConst.GrdSAMToUAM;

                // Find next dongle
                retCode = GrdApi.GrdFind(_grdHandle, GrdF.Next, out _findPropDongleID, out findInfo);
            }
            log.Debug(logStr);
            if (retCode == GrdE.AllDonglesFound)	// Search has been completed?
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
            // lms = -1;
            // All following Guardant API calls before next GrdCloseHandle()/GrdLogin() will use this dongle
            retCode = GrdApi.GrdLogin(_grdHandle, 0, GrdLM.PerStation);
            logStr += GrdApi.PrintResult((int)retCode);
            log.Debug(logStr);
            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return retCode;
            }

			return GrdE.OK;
        }
*/

        /*public bool ValidKey()
        {
            return GrdApi.GrdIsValidHandle(_grdHandle);
        }*/

        /*public bool ReadBytes(out byte[] buffer, uint offset, uint length)
        {
            string logStr = "Reading Bytes : ";
            GrdE retCode = GrdApi.GrdRead(_grdHandle, offset, (int)length, out buffer);
            logStr += GrdApi.PrintResult((int)retCode);
            LogIt(logStr);
            ErrorHandling(_grdHandle, retCode);

            return (retCode==GrdE.OK);
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
        }

        public uint ReadUInt(uint offset)
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
        }

        public int ReadInt(uint offset)
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

        /*private GrdE Disconnect()
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

//            _isActive = false;

            return GrdE.OK;
        }*/

        //----------------------------------------------------------------------------------------------
        //  Handle errors
        //  Prints operation result, closes handle and forces program termination on error  
        //  Input:  Handle to Guardant protected Container
        //	Input:  error code
        //	return: error code
        //----------------------------------------------------------------------------------------------
        /*private GrdE ErrorHandling(Handle hGrd, GrdE nRet)
        {
            if (nRet != GrdE.OK && nRet != GrdE.AlreadyInitialized)
            {
                Disconnect();
            }
//            _isActive = false;
            return nRet;
        }*/
        #endregion rudiments
    }
}

#endif