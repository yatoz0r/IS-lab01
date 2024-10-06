using System.IO;
using System.Security.Cryptography;


namespace IS_lab01.CryptLogics
{
    public class BlockEncrypt
    {
        private const int BLOCK_SIZE = 4; // 32 bits = 4 bytes
        private const int KEY_SIZE = 4; // 32 bits = 4 bytes
        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        public byte[] GenerateKey()
        {
             byte[] key = new byte[KEY_SIZE];
             rng.GetBytes(key);
             return key;
        }
        public byte[] Encrypt(byte[] fileBytes, byte[] key)
        {
            if (key.Length != KEY_SIZE)
                throw new ArgumentException("Key must be 32 bits (4 bytes) long.");

            byte[] paddedInput = PadInput(fileBytes);
            byte[] output = new byte[paddedInput.Length];

            for (int i = 0; i < paddedInput.Length; i += BLOCK_SIZE)
            {
                for (int j = 0; j < BLOCK_SIZE; j++)
                {
                    output[i + j] = (byte)(paddedInput[i + j] ^ key[j]);
                }
            }

            return output;
        }

        public byte[] Decrypt(byte[] encryptedBytes, byte[] key)
        {
            if (key.Length != KEY_SIZE)
                throw new ArgumentException("Key must be 32 bits (4 bytes) long.");

            return Encrypt(encryptedBytes, key); // XOR is symmetric, so encryption is the same as decryption
        }

        private byte[] PadInput(byte[] input)
        {
            int paddingLength = BLOCK_SIZE - (input.Length % BLOCK_SIZE);
            if (paddingLength == 0) paddingLength = BLOCK_SIZE;

            byte[] paddedInput = new byte[input.Length + paddingLength];
            Array.Copy(input, paddedInput, input.Length);

            // PKCS7 padding
            for (int i = input.Length; i < paddedInput.Length; i++)
            {
                paddedInput[i] = (byte)paddingLength;
            }

            return paddedInput;
        }

        private byte[] RemovePadding(byte[] input)
        {
            int paddingLength = input[input.Length - 1];
            return input.Take(input.Length - paddingLength).ToArray();
        }

        public void EncryptFile(string inputFile, string outputFile, byte[] key)
        {
            byte[] fileBytes = File.ReadAllBytes(inputFile);
            byte[] encryptedBytes = Encrypt(fileBytes, key);
            File.WriteAllBytes(outputFile, encryptedBytes);
        }

        public void DecryptFile(string inputFile, string outputFile, byte[] key)
        {
            byte[] encryptedBytes = File.ReadAllBytes(inputFile);
            byte[] decryptedBytes = Decrypt(encryptedBytes, key);
            File.WriteAllBytes(outputFile, RemovePadding(decryptedBytes));
        }
    }
}
