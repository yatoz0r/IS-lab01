using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace IS_lab01.CryptLogics
{
    public class GammaEncryption
    {
        private const int BLOCK_SIZE = 64;
        private RandomNumberGenerator rng;

        public void EncryptFile(string inputFile, string outputFile)
        {
            rng = RandomNumberGenerator.Create();
            // Читаем исходный текст
            string text = File.ReadAllText(inputFile);
            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            // Создаем гамму той же длины
            byte[] gamma = new byte[textBytes.Length];
            rng.GetBytes(gamma);

            // Шифруем текст
            byte[] encryptedBytes = new byte[textBytes.Length];
            for (int i = 0; i < textBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(textBytes[i] ^ gamma[i]);
            }

            // Сохраняем зашифрованные данные
            File.WriteAllBytes(outputFile, encryptedBytes);
            // Сохраняем гамму отдельно
            File.WriteAllBytes(outputFile + ".gamma", gamma);
        }

        public void DecryptFile(string inputFile, string outputFile, string gammaFile)
        {
            // Читаем зашифрованные данные и гамму
            byte[] encryptedBytes = File.ReadAllBytes(inputFile);
            byte[] gamma = File.ReadAllBytes(gammaFile);

            // Расшифровываем
            byte[] decryptedBytes = new byte[encryptedBytes.Length];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ gamma[i]);
            }

            // Преобразуем байты обратно в текст и сохраняем
            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
            File.WriteAllText(outputFile, decryptedText);
        }
        private ulong ModularMultiplicativeInverse(ulong value, ulong modulus)
        {
            ulong a = value;
            ulong b = modulus;
            ulong x = 1;
            ulong y = 0;

            while (b > 0)
            {
                ulong quotient = a / b;
                (a, b) = (b, a % b);
                (x, y) = (y, x - quotient * y);
            }

            return (x % modulus + modulus) % modulus;
        }
    }
}
