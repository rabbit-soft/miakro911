using System.Runtime.InteropServices;
using Guardant;

namespace GRD
{

    public struct GRDConst
    {
        public const int GrdWmUAMOffset = 14;
        public const int GrdWmSAMOffset = 44;
        public const uint GrdSAMToUAM = GrdWmSAMOffset - GrdWmUAMOffset;// 1Eh  start address of UAM memory
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


    }
}