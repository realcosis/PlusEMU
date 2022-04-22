using System.Text;

using Plus.Utilities;
using Plus.Communication.Encryption.Keys;
using Plus.Communication.Encryption.Crypto.RSA;
using Plus.Communication.Encryption.KeyExchange;

namespace Plus.Communication.Encryption
{
    public static class HabboEncryptionV2
    {
        private static RsaKey _rsa;
        private static DiffieHellman _diffieHellman;

        public static void Initialize(RsaKeys keys)
        {
            _rsa = RsaKey.ParsePrivateKey(keys.N, keys.E, keys.D);
            _diffieHellman = new DiffieHellman();
        }

        private static string GetRsaStringEncrypted(string message)
        {
            try
            {
                byte[] m = Encoding.Default.GetBytes(message);
                byte[] c = _rsa.Sign(m);

                return Converter.BytesToHexString(c);
            }
            catch
            {
                return "0";
            }
        }

        public static string GetRsaDiffieHellmanPrimeKey()
        {
            string key = _diffieHellman.Prime.ToString(10);
            return GetRsaStringEncrypted(key);
        }

        public static string GetRsaDiffieHellmanGeneratorKey()
        {
            string key = _diffieHellman.Generator.ToString(10);
            return GetRsaStringEncrypted(key);
        }

        public static string GetRsaDiffieHellmanPublicKey()
        {
            string key = _diffieHellman.PublicKey.ToString(10);
            return GetRsaStringEncrypted(key);
        }

        public static BigInteger CalculateDiffieHellmanSharedKey(string publicKey)
        {
            try
            {
                byte[] cbytes = Converter.HexStringToBytes(publicKey);
                byte[] publicKeyBytes = _rsa.Verify(cbytes);
                string publicKeyString = Encoding.Default.GetString(publicKeyBytes);
                return _diffieHellman.CalculateSharedKey(new BigInteger(publicKeyString, 10));
            }
            catch
            {
                return 0;
            }
        }
    }
}
