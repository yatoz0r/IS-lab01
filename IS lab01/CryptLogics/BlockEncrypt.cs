using System.Security.Cryptography;
using System.Text;

namespace IS_lab01.CryptLogics
{
    public class BlockEncrypt
    {
        private const int BLOCK_SIZE = 4; // 32 bits = 4 bytes
        private const int KEY_SIZE = 4; // 32 bits = 4 bytes
        public byte[] Key { get => _key;}
        public byte[] Encrypted { get; set; }
        public byte[] Decrypted { get; set; }
        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        private readonly byte[] _key = new byte[KEY_SIZE];

        public string GetKey()
        {
            string keyString = Convert.ToBase64String(_key);
            return keyString;
        }
        public string ByteArrayToBinaryString(byte[] bytes)
        {
            return string.Join("", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }
        public void GenerateKey()
        {
            rng.GetBytes(_key);
        }

        public string Encrypt(string input)
        {
            if (_key.Length != KEY_SIZE)
                throw new ArgumentException($"Key must be {KEY_SIZE} bytes long.");

            // Convert input string to byte array
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            int padding = BLOCK_SIZE - (inputBytes.Length % BLOCK_SIZE);
            byte[] paddedInputBytes = new byte[inputBytes.Length + padding];
            Array.Copy(inputBytes, paddedInputBytes, inputBytes.Length);

            // Padding with zeros
            for (int i = inputBytes.Length; i < paddedInputBytes.Length; i++)
            {
                paddedInputBytes[i] = 0;
            }

            byte[] encryptedBytes = new byte[paddedInputBytes.Length];

            for (int i = 0; i < paddedInputBytes.Length; i += BLOCK_SIZE)
            {
                for (int j = 0; j < BLOCK_SIZE; j++)
                {
                    encryptedBytes[i + j] = (byte)(paddedInputBytes[i + j] ^ _key[j]);
                }
            }
            Encrypted = encryptedBytes;

            // Convert encrypted bytes to a base64 string
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encryptedInput)
        {
            if (_key.Length != KEY_SIZE)
                throw new ArgumentException($"Key must be {KEY_SIZE} bytes long.");

            // Convert encrypted base64 string to byte array
            byte[] encryptedBytes = Convert.FromBase64String(encryptedInput);

            byte[] decryptedBytes = new byte[encryptedBytes.Length];

            for (int i = 0; i < encryptedBytes.Length; i += BLOCK_SIZE)
            {
                for (int j = 0; j < BLOCK_SIZE; j++)
                {
                    decryptedBytes[i + j] = (byte)(encryptedBytes[i + j] ^ _key[j]);
                }
            }
            Decrypted = decryptedBytes;

            return Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\0');
        }
    }
}
