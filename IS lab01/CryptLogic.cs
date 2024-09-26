using System.Numerics;
using System.Security.Cryptography;


namespace IS_lab01
{
    public class CryptLogic
    {
        private BigInteger p, q, n, phi, e, d, y;
        private readonly int minBits = 12;
        public BigInteger P { get => p; }
        public BigInteger Q { get => q; }
        public BigInteger N { get => n; }
        public BigInteger E { get => e; }
        public BigInteger D { get => d; }
        
        public BigInteger Y { get => y; }

        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        public CryptLogic()
        {
            GeneratePrimes();
            CalculateKeys();
        }
        public string CaesarCipher(string input, int shift)
        {
            char[] buffer = input.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                char letter = buffer[i];
                if (char.IsLetter(letter))
                {
                    char d = char.IsUpper(letter) ? 'A' : 'a';
                    letter = (char)((((letter + shift) - d + 26) % 26) + d);
                    buffer[i] = letter;
                }
            }
            return new string(buffer);
        }
        
        private static bool IsPrime(BigInteger number)
        {
            if (number % 2 == 0) return false;

            for (long i = 3; i < number; i++)
                if (number % i == 0)
                    return false;

            return true;
        }
        public BigInteger GenerateRandomNumber()
        {
            BigInteger prime;

            do
            {
                byte[] bytes = new byte[minBits / 8 + 1];
                rng.GetBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;

                prime = new BigInteger(bytes);
            }
            while (prime < BigInteger.Pow(2, minBits - 1) || !IsPrime(prime));

            return prime;
        }
        private void GeneratePrimes()
        {
            p = GenerateRandomNumber();

            q = GenerateRandomNumber();
        }
        private BigInteger CalculateD()
        {
            BigInteger d, x;
            ExtendedEuclideanAlgorithm(e, phi, out d, out x, out y);
            return (d % phi + phi) % phi;
        }
        private void CalculateKeys()
        {
            n = p * q;
            phi = (p - 1) * (q - 1);
            e = 2;
            while (BigInteger.GreatestCommonDivisor(e, phi) != 1)
            {
                e = GenerateRandomNumber();
            }
            d = CalculateD();
        }
        private void ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y, out BigInteger gcd)
        {
            if (a == 0)
            {
                gcd = b;
                x = 0;
                y = 1;
                return;
            }

            BigInteger x1, y1;
            ExtendedEuclideanAlgorithm(b % a, a, out x1, out y1, out gcd);

            x = y1 - (b / a) * x1;
            y = x1;
        }

        public string Encrypt(string message)
        {
            var encryptedChars = new List<string>();

            foreach (var character in message)
            {
                // Преобразуем символ в BigInteger
                var charBigInt = new BigInteger(character);

                // Шифруем BigInteger
                var encryptedCharBigInt = BigInteger.ModPow(charBigInt, e, n);

                // Сохраняем зашифрованный BigInteger как строку десятичного числа
                encryptedChars.Add(encryptedCharBigInt.ToString());
            }

            // Соединяем зашифрованные значения через разделитель
            return string.Join(" ", encryptedChars);
        }

        public string Decrypt(string encryptedMessage)
        {
            var encryptedChars = encryptedMessage.Split(' ');
            var decryptedChars = new List<char>();

            foreach (var encryptedChar in encryptedChars)
            {
                // Преобразуем строку в BigInteger
                var encryptedCharBigInt = BigInteger.Parse(encryptedChar);
                //var encryptedCharBytes = Convert.FromBase64String(encryptedChar);

                //var encryptedCharBigInt = new BigInteger(encryptedCharBytes);

                // Расшифровываем BigInteger
                var decryptedCharBigInt = BigInteger.ModPow(encryptedCharBigInt, d, n);

                // Преобразуем расшифрованный BigInteger обратно в символ
                var decryptedChar = (char)decryptedCharBigInt;

                // Добавляем расшифрованный символ в список
                decryptedChars.Add(decryptedChar);
            }

            // Преобразуем список символов обратно в строку
            return new string(decryptedChars.ToArray());
        }
    }
}
