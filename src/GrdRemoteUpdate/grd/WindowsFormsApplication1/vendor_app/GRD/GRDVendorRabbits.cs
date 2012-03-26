using System;

namespace RabGRD
{
    public sealed partial class GRDVendorKey :GRD_Base, IDisposable
    {
        //Далее список адресов данных в ключе
        //        private const uint OffsDevMarker = 0;
        private const uint OffsCustomerID = 0;
        private const uint OffsOrganizationName = 32;
        private const uint OffsMaxBuildingsCount = 132;
        private const uint OffsFlagsMask = 136;
        private const uint OffsFarmStartDate = 144;
        private const uint OffsFarmStopDate = 156;
        private const uint OffsTempFlagsMask = 168;
        private const uint OffsTempFlagsEndDate = 176;

        private const uint LengthOrganizationName = 100;

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

        /*public int GetFarmsCnt()
        {
            int farmCnt = -1;
            uint addr = _uamOffset + OffsMaxBuildingsCount;

            LogIt("Reading farm cnt: ");
            farmCnt = ReadInt(addr);
            LogIt("Farms cnt is : " + farmCnt.ToString());

            return farmCnt;
        }*/



        /*public string GetOrgName()
        {
            uint addr = _uamOffset + OffsOrganizationName;
            LogIt("Reading Org Name: ");
            return ReadStringCp1251(addr, LengthOrganizationName);
        }*/

        /*public DateTime GetDateStart()
        {
            uint addr = _uamOffset + OffsFarmStartDate;
            LogIt("Reading Date start: ");
            return ReadDate(addr);
        }*/

        /*public DateTime GetDateEnd()
        {
            uint addr = _uamOffset + OffsFarmStopDate;
            LogIt("Reading Date end: ");
            return ReadDate(addr);
        }*/

        /*public bool GetFlag(FlagType ft)
        {
            FlagType flags = (FlagType)GetFlags(0);
            FlagType flagsTemp = (FlagType)GetTempFlags(0);

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
            uint addr = _uamOffset + OffsFlagsMask;

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
            uint dateAddr = _uamOffset + OffsTempFlagsEndDate;
            byte[] bts = new byte[12];

            uint res = 0;

            LogIt("Reading TempDateEnd : ");

            DateTime dt = ReadDate(dateAddr);
            LogIt("Date is: " + dt.ToString());

            if (dt > DateTime.Now)
            {
                uint flgsAddr = _uamOffset + OffsTempFlagsMask;
                bts = new byte[8];
                LogIt("Reading TempRole Flags : ");

                if (ReadBytes(out bts, flgsAddr, 8))
                {
                    UInt64 flgs = BitConverter.ToUInt64(bts, 0);
                    LogIt(string.Format("; Full TempFlags = {0:D}", flgs) +
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