using System.Security.Cryptography;
using UI;

namespace Services
{
    public class CryptoManager
    {
        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public int GenerateRandom(int range)
        {
            if (range <= 0)
            {
                ConsoleStyler.PrintError("Error: Range must be greater than 0.");
                return -1;
            }
            byte[] randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            uint randomValue = BitConverter.ToUInt32(randomBytes, 0);
            return (int)(randomValue % (uint)range);
        }

        public (string hmac, byte[] key, int value) GenerateHmacRandom(int range)
        {
            if (range <= 0)
            {
                ConsoleStyler.PrintError("Error: Range must be greater than 0.");
                return (string.Empty, Array.Empty<byte>(), -1);
            }
            int randomValue = GenerateRandom(range);
            if (randomValue == -1)
            {
                return (string.Empty, Array.Empty<byte>(), -1);
            }
            byte[] key = new byte[32];
            rng.GetBytes(key);
            using var hmacSha256 = new HMACSHA256(key);
            byte[] hmacBytes = hmacSha256.ComputeHash(BitConverter.GetBytes(randomValue));
            string hmac = BitConverter.ToString(hmacBytes).Replace("-", "");
            return (hmac, key, randomValue);
        }
    }
}