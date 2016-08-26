using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Guardant;

namespace RabGRD
{
    /// <summary>
    /// Константы API не указанные в GuardantDotNetApi
    /// </summary>
    public struct GRDConst
    {
        public const int GrdWmUAMOffset = 14;
        public const int GrdWmSAMOffset = 44;
        public const uint GrdSAMToUAM = GrdWmSAMOffset - GrdWmUAMOffset;// 1Eh  start address of UAM memory


        /// <summary>
        /// NS Algorithm Flags Low
        /// </summary>
        public static class nsafl
        {
            /// <summary>
            /// Уникальность алгоритма по ID ключа. При одинаковых определителях алгоритмы в разных ключах кодируют данные по-разному
            /// </summary>
            public static byte ID = 1;

            /// <summary>
            /// Уменьшать счетчик GP при каждой обращении к алгоритму. По достижении счетчиком GP 0, алгоритм автоматически деактивируется и при дальнейших обращениях возвращается код ошибки GrdE_InactiveItem
            /// </summary>
            public static byte GP_dec = 2;

            /// <summary>
            /// В современных ключах не используется
            /// </summary>
            public static byte GP = 4;

            /// <summary>
            /// Для Guardant Sign/Time/Code/Stealth III/Net III флаг должен быть установлен
            /// </summary>
            public static byte ST_III = 8;

            /// <summary>
            /// Сервис активации доступен
            /// </summary>

            public static byte ActivationSrv = 16;

            /// <summary>
            /// Сервис деактивации доступен
            /// </summary>
            public static byte DeactivationSrv = 32;

            /// <summary>
            ///  Сервис изменения ячейки rs_K[] по паролю доступен (функция GrdPI_Update поддерживается)
            /// </summary>
            public static byte UpdateSrv = 64;

            /// <summary>
            /// Признак, что в данный момент алгоритм деактивирован. Операции GrdTransform, GrdPI_Read, GrdPI_Update недоступны
            /// </summary>
            public static byte InactiveFlag = 128;
        }

        /// <summary>
        /// NS Algorithm Flags High
        /// </summary>
        public static class nsafh
        {
            /// <summary>
            /// Сервис чтения данных ячейки rs_K[] доступен (функция GrdPI_Read поддерживается)
            /// </summary>
            public static byte ReadSrv = 1;

            /// <summary>
            /// Чтение осуществляется по паролю rs_ReadPwd
            /// </summary>
            public static byte ReadPwd = 2;

            /// <summary>
            /// Включен режим активации в указанное время (хранится в поле rs_BirthTime)
            /// </summary>
            public static byte BirthTime = 4;

            /// <summary>
            /// Включен режим деактивации в указанное время (хранится в поле rs_DeadTime)
            /// </summary>
            public static byte DeadTime = 8;

            /// <summary>
            /// Включен режим деактивации через указанное время после первого обращения к ячейке (оставшееся до деактивации время хранится в ячейке rs_LifeTime) Одновременное использование с флагами nsafh_DeadTime и nsafh_BirthTime не допускается!
            /// </summary>
            public static byte LifeTime = 16;

            /// <summary>
            /// Включен режим автоматического изменения определителя каждые rs_DaysGap-дней, начиная с даты rs_ChangeFlipTimeStart. Можно комбинировать с флагами nsafh_DeadTime, nsafh_BirthTime и nsafh_LifeTime. 
            /// </summary>
            public static byte FlipTime = 32;
        }

        /// <summary>
        /// Поле rs_algo содержит код типа защищенной ячейки.
        /// </summary>
        public static class RsAlgo
        {
            public static byte GSII64 = 5;
            public static byte HASH64 = 6;
            public static byte RND64 = 7;
            public static byte PI = 8;
            public static byte GSII64_ENCRYPT = 10;
            public static byte GSII64_DECRYPT = 11;
            public static byte ECC160 = 12;
            public static byte AES128 = 13;
            public static byte LoadableCode = 14;
            public static byte SHA256 = 15;
            public static byte AES128Encode = 16;
            public static byte AES128Decode = 17;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TRUQuestionStruct
    {
        public ushort type;
        public ushort lanRes;
        public byte model;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] question;
        public uint id;
        public uint pubKey;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] hash;
        public ulong dongleTime;
        public uint deadTimesNumber;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] deadTimes;
    }

    public struct KeyInfo
    {
        public uint ID;
        public ushort Type;
        public string TypeString;
        public uint UAM;
        public uint Model;
        public uint UAMOffset;
    }
    
    public static class GRDUtils
    {
        public static string ModelName(byte model)
        {
            switch ((GrdDM)model)
            {
                case GrdDM.GS1L:
                    return "Guardant Stealth LPT";
                case GrdDM.GS1U:
                    return "Guardant Stealth USB";
                case GrdDM.GF1L:
                    return "Guardant Fidus LPT";
                case GrdDM.GS2L:
                    return "Guardant Stealth II LPT";
                case GrdDM.GS2U:
                    return "Guardant Stealth II USB";
                case GrdDM.GS3U:
                    return "Guardant Stealth III USB";
                case GrdDM.GF1U:
                    return "Guardant Fidus USB";
                case GrdDM.GS3SU:
                    return "Guardant StealthIII Sign/Time USB";
                default:
                    return "";
            }
        }

        public static byte[] RawSerialize(object anything, int size)
        {
            int rawSize = Marshal.SizeOf(anything);
            int rawDestSize = Marshal.SizeOf(anything);

            if (size != -1)
            {
                rawDestSize = size;

            }
            int copySize = Math.Min(rawDestSize, rawSize);

            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawdata = new byte[rawDestSize];
            Marshal.Copy(buffer, rawdata, 0, copySize);
            Marshal.FreeHGlobal(buffer);
            return rawdata;
        }

        public static byte[] RawSerialize(object anything)
        {
            return RawSerialize(anything, -1);
        }

        public static object RawDeserialize(byte[] rawdatas, Type anytype)
        {
            int rawsize = Marshal.SizeOf(anytype);
            if (rawsize > rawdatas.Length)           
                return null;            
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawdatas, 0, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anytype);
            Marshal.FreeHGlobal(buffer);
            return retobj;
        }

    }

    public sealed class GRDEnumerator
    {
        private readonly List<KeyInfo> _keyList;

        public GRDEnumerator()
        {
            _keyList = new List<KeyInfo>();

            Handle _grdHandle = new Handle();

            GrdE retCode; // Error code for all Guardant API functions

            FindInfo findInfo; //= new FindInfo();   // structure used in GrdFind()
            uint dongleID;
            string logStr = "";

            // Initialize this copy of GrdAPI. GrdStartup() must be called once before first GrdAPI call at application startup
            logStr = "Initialize this copy of GrdAPI : ";
            retCode = GrdApi.GrdStartup(GrdFMR.Local); // + GrdFMR.Remote if you want to use network dongles
            logStr += GrdApi.PrintResult(retCode);
            ErrorHandling(new Handle(IntPtr.Zero), retCode);
            if (retCode != GrdE.OK && retCode != GrdE.AlreadyInitialized)
            {
                return;
            }

            logStr = "Create Guardant protected container : ";
            _grdHandle = GrdApi.GrdCreateHandle(GrdCHM.MultiThread);
            if (_grdHandle.Address == IntPtr.Zero) // Some error found?
            {
                logStr += GrdApi.PrintResult(GrdE.MemoryAllocation);

                ErrorHandling(new Handle(IntPtr.Zero), GrdE.MemoryAllocation);
                return;
            }
            logStr += GrdApi.PrintResult((int)GrdE.OK);

            ErrorHandling(_grdHandle, GrdE.OK); // Print success information

            logStr = "Storing dongle codes in Guardant protected container : ";
            retCode = GrdApi.GrdSetAccessCodes(_grdHandle,	// Handle to Guardant protected container
                                    0);    // Private read code; you can omit this code and all following via using of overloaded function;
            logStr += GrdApi.PrintResult(retCode);

            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return;
            }

            logStr = "Setting dongle search conditions : ";

            // All following GrdFind() & GrdLogin() calls before next
            // GrdSetFindMode() will use specified flag values. 
            // If dongle field values and specified values do not match, error code is
            // returned. Both access code and flags are required to call the dongle.
            retCode = GrdApi.GrdSetFindMode(_grdHandle,
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
            logStr += GrdApi.PrintResult(retCode);

            ErrorHandling(_grdHandle, retCode);
            if (retCode != GrdE.OK)
            {
                return;
            }

            logStr = "Searching for all specified dongles and print info about it's : ";



            retCode = GrdApi.GrdFind(_grdHandle, GrdF.First, out dongleID, out findInfo);
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

                KeyInfo key = new KeyInfo();

                key.ID = findInfo.dwID;
                key.Type = findInfo.wType;
                key.UAM = findInfo.dwRkmUserAddr;
                key.Model = findInfo.dwModel;

                key.UAMOffset = findInfo.wWriteProtectS3 - GRDConst.GrdSAMToUAM;

                _keyList.Add(key);

                // Find next dongle
                retCode = GrdApi.GrdFind(_grdHandle, GrdF.Next, out dongleID, out findInfo);
            }
            if (retCode == GrdE.AllDonglesFound)
            {
                //
            }
            else
            {
                ErrorHandling(_grdHandle, retCode);
                if (retCode != GrdE.OK)
                {
                    return;
                }
            }

            GrdApi.GrdCloseHandle(_grdHandle);
            GrdApi.GrdCleanup();

            return;
        }

        public List<KeyInfo> KeyList
        {
            get { return _keyList; }
        }

        private GrdE ErrorHandling(Handle hGrd, GrdE nRet)
        {
            // print the result of last executed function
            //log.Debug(GrdApi.PrintResult((int)nRet));
            string logStr = "";

            if (nRet != GrdE.OK && nRet != GrdE.AlreadyInitialized)
            {
                if (hGrd.Address != IntPtr.Zero) // Perform some cleanup operations if hGrd handle exists
                {
                    // Close hGrd handle, log out from dongle/server, free allocated memory
                    logStr = ("Closing handle: ");
                    nRet = GrdApi.GrdCloseHandle(hGrd);
                    logStr += GrdApi.PrintResult(nRet);

                }


                // Deinitialize this copy of GrdAPI. GrdCleanup() must be called after last GrdAPI call before program termination
                //                logStr = "Deinitializing this copy of GrdAPI : ";
                //                nRet = GrdApi.GrdCleanup();
                //                logStr += GrdApi.PrintResult((int)nRet);
                //                log.Debug(logStr);

                // Terminate application
                //Environment.Exit((int)nRet);

            }
            //_isActive = false;
            return nRet;
        }
    }
}