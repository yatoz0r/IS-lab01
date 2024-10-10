
using System.Numerics;
using System.Security.Cryptography;

namespace IS_lab01.CryptLogics
{
    public class DiffieHellman
    {
        public BigInteger X { get; private set; }
        public BigInteger Y { get; private set; }
        public BigInteger ClientPrivateA { get; private set; }
        public BigInteger ClientPrivateB { get; private set; }

        public BigInteger PublicKeyA { get; private set; }
        public BigInteger PublicKeyB { get; private set; }

        private BigInteger _privateKey;
        public BigInteger PrivateKey { get => _privateKey;  }
        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        private int _minBits = 12;

        public DiffieHellman()
        {
            X = GenerateRandomNumber();

            Y = GenerateRandomNumber();

            ClientPrivateA = GenerateRandomNumber();

            ClientPrivateB = GenerateRandomNumber();
            CalculateKeys();
        }
        
        private void CalculateKeys()
        {
            PublicKeyA = BigInteger.ModPow(Y, ClientPrivateA, X);
            PublicKeyB = BigInteger.ModPow(Y, ClientPrivateB, X);
            _privateKey = BigInteger.ModPow(PublicKeyB, PublicKeyB, X);
        }
        private bool IsPrime(BigInteger number)
        {
            if (number % 2 == 0) return false;

            for (long i = 3; i < number; i++)
                if (number % i == 0)
                    return false;

            return true;
        }
        private BigInteger GenerateRandomNumber()
        {
            BigInteger prime;

            do
            {
                byte[] bytes = new byte[_minBits / 8 + 1];
                rng.GetBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;

                prime = new BigInteger(bytes);
            }
            while (prime < BigInteger.Pow(2, _minBits - 1) || !IsPrime(prime));

            return prime;
        }
        public string Encrypt(string plaintext)
        {
            string ciphertext = "";
            double tangent = Math.Tan((double)_privateKey);

            foreach (char c in plaintext)
            {
                BigInteger encryptedChar = BigInteger.Multiply((BigInteger)c, _privateKey)/* * (BigInteger)tangent*/;
                ciphertext += encryptedChar.ToString() + " ";
            }

            return ciphertext;
        }

        public string Decrypt(string ciphertext)
        {
            string plaintext = "";
            string[] encryptedChars = ciphertext.Trim().Split(' ');
            double tangent = Math.Tan((double)_privateKey); 
            foreach (string encryptedChar in encryptedChars)
            {
                BigInteger bigInt = BigInteger.Parse(encryptedChar);
                char c = (char)(/*(double)*/BigInteger.Divide(bigInt, _privateKey) /*/ tangent*/);
                plaintext += c;
            }

            return plaintext;
        }
    }
}
