namespace RabGRD
{
    public partial class GRD_Base
    {
        protected const uint CryptPu = 0x8568683U;
        protected const uint CryptRd = 0x56547675U;
        protected const uint CryptWr = 0x38298893U; // Random value for access codes security                              
        protected const uint CryptMs = 0x3882822U; // Random value for access codes security                              

        protected const uint PublicCode = 0x9BD54F75 - CryptPu;   // Must be encoded             
        protected const uint ReadCode = 0xFE8392B2 - CryptRd;     // Must be encoded             
        protected const uint WriteCode = 0x5d97743c - CryptWr;    // Must be encoded             
        protected const uint MasterCode = 0xe8e1e08 - CryptMs;    // Must be encoded   

    }
}
