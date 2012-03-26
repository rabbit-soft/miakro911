#if PROTECTED
using System;
using log4net;
using Guardant;

namespace RabGRD
{
    public sealed partial class GRDEndUser : GRD_Base
    {
        public static readonly GRDEndUser Instance = new GRDEndUser();

        private ILog log = LogManager.GetLogger(typeof(GRDEndUser));
        
        //Далее список адресов данных в ключе
        /*const uint USER_DATA_BEGINING = 1182;
        const uint DEV_MARKER_OFFSET = 0;
        const uint ORGANIZATION_NAME_OFFSET = 32;
        const uint MAX_BUILDINGS_COUNT_OFFSET = 132;
        const uint FLAGS_MASK_OFFSET = 136;
        const uint FARM_START_DATE_OFFSET = 144;
        const uint FARM_STOP_DATE_OFFSET = 156;
        const uint TEMP_FLAGS_MASK_OFFSET = 168;
        const uint END_TEMP_FLAGS_OFFSET = 176;*/

        //const uint LengthOrganizationName = 100;

        private const uint ProgIDCode = 1;

        /// <summary>
        /// Достпные флаги
        /// </summary>
        /*[Flags]
        public enum FlagType
        {
            None = 0,
            RabNet = 1,             //0 bit 
            Genetics = 1 << 1,      //1 bit
            RabDump = 1 << 2,       //2 bit
            Butcher = 1 << 3,       //3 bit
            ReportPlugIns = 1 << 4  //4 bit
        }*/       
        //private int _farmCntCache;
        //private int _cacheTicks;



        /*public int GetFarmsCnt()
        {
            int farmCnt = -1;
            uint addr = _uamOffset + MAX_BUILDINGS_COUNT_OFFSET;

            LogIt("Reading farm cnt: ");
            farmCnt = ReadInt(addr);
            LogIt("Farms cnt is : "+farmCnt.ToString());

            _farmCntCache = farmCnt;
            _cacheTicks = Environment.TickCount & Int32.MaxValue;

            return farmCnt;
        }*/

        /*public uint GetCustomerID()
        {
            uint addr = _uamOffset + DEV_MARKER_OFFSET;

            LogIt("Reading customer id: ");
            return ReadUInt(addr);
        }*/

        /*public int GetFarmsCntCache()
        {
            if ((Environment.TickCount & Int32.MaxValue) > _cacheTicks + 60 * 1000)
            {
                GetFarmsCnt();
            }
            return _farmCntCache;
        }*/

        /*public string GetOrgName()
        {
            uint addr = _uamOffset + ORGANIZATION_NAME_OFFSET;
            LogIt("Reading Org Name: ");
            return ReadStringCp1251(addr, LengthOrganizationName);
        }*/

        /*public DateTime GetDateStart()
        {
            uint addr = _uamOffset + FARM_START_DATE_OFFSET;
            LogIt("Reading Date start: ");
            return ReadDate(addr);
        }

        public DateTime GetDateEnd()
        {
            uint addr = _uamOffset + FARM_STOP_DATE_OFFSET;
            LogIt("Reading Date end: ");
            return ReadDate(addr);
        }

        public DateTime ReadDate(uint offset)
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


        /*public bool GetFlag(FlagType ft)
        {
            FlagType flags = (FlagType) GetFlags(0);
            FlagType flagsTemp = (FlagType) GetTempFlags(0);

            LogIt("========================> " + flags.ToString() + " " + ((int)flags).ToString());
            LogIt("========================> " + flagsTemp.ToString() + " " + ((int)flagsTemp).ToString());

            return ((flags & ft) == ft) || ((flagsTemp & ft) == ft);
        }*/

        /// <summary>
        /// Возвращает число(битовую маску), содержащее битовые флаги включенных функций
        /// </summary>
        /// <param name="byteNum">Порядковый номер байта, из которого читать флаг</param>
        /*private uint GetFlags(uint byteNum)
        {
            uint addr = _uamOffset + FLAGS_MASK_OFFSET;

            byte[] bts = new byte[8];

            uint res = 0;

            LogIt("Reading Role Flags : ");
            if (ReadBytes(out bts, addr, 8))
            {
                UInt64 flgs = BitConverter.ToUInt64(bts, 0);
                res = bts[byteNum];
                LogIt(string.Format("; Full Flags = {0:D}", flgs) +
                      string.Format("; Flags = {3} - {2} - {1} - {0}", 
                                    Convert.ToString(bts[0], 2),
                                    Convert.ToString(bts[1], 2), 
                                    Convert.ToString(bts[2], 2),
                                    Convert.ToString(bts[3], 2)));
            }
            return res;
        }*/

        /*private uint GetTempFlags(uint byteNum)
        {
            uint dateAddr = _uamOffset + END_TEMP_FLAGS_OFFSET;
            byte[] bts = new byte[12];

            uint res = 0;

            LogIt("Reading TempDateEnd : ");

            DateTime dt = ReadDate(dateAddr);
            LogIt("Date is: "+dt.ToString());

            if (dt > DateTime.Now)
            {
                uint flgsAddr = _uamOffset + TEMP_FLAGS_MASK_OFFSET;
                bts = new byte[8];
                LogIt("Reading TempRole Flags : ");

                if (ReadBytes(out bts,flgsAddr,8)) {
                    UInt64 flgs = BitConverter.ToUInt64(bts, 0);
                    LogIt(string.Format("; Full TempFlags = {0:D}", flgs)+
                          string.Format("; TempFlags = {3} - {2} - {1} - {0}", 
                                        Convert.ToString(bts[0], 2),
                                        Convert.ToString(bts[1], 2), 
                                        Convert.ToString(bts[2], 2),
                                        Convert.ToString(bts[3], 2)));
                    res = bts[byteNum];
                }
            }

            return res;
        }*/

    }
}
#endif