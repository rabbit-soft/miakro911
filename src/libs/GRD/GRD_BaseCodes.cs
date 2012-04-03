#if PROTECTED
namespace RabGRD
{
    public partial class GRD_Base
    {
        protected const uint CryptPu = 0x8568683U;
        protected const uint CryptRd = 0x56547675U;

        protected const uint PublicCode = 0x9BD54F75 - CryptPu; // Must be encoded             
        protected const uint ReadCode = 0xFE8392B2 - CryptRd; // Must be encoded             
    }
}
#endif