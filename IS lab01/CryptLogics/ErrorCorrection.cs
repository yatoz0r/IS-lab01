namespace IS_lab01.CryptLogics
{
    public class ErrorCorrection
    {
        private const int GeneratorPolynomial = 0b1100011; // Generator polynomial g(x)
        public int CodeLength {get => 12;} // Total code length (6 data bits + 6 check bits)
        private static Random random = new Random();
        public Random Random {get => random;}
     
        // Method for encoding data
        public int Encode(int data)
        {
            int encodedData = data << 6; // Shift data 6 bits
            int remainder = encodedData;

            // Divide by the generator polynomial
            for (int i = 0; i < 6; i++)
            {
                if ((remainder & (1 << (CodeLength - 1 - i))) != 0) // Check the highest bit
                {
                    remainder ^= GeneratorPolynomial << (6 - 1 - i); // XOR with generator
                }
            }
            return encodedData | remainder; // Combine data and remainder
        }

        // Method for decoding data
        public DecodeResult Decode(int receivedData)
        {
            DecodeResult result = new DecodeResult();
            int syndrome = CalculateSyndrome(receivedData);

            if (syndrome != 0)
            {
                result.HasError = true;
                int errorPosition = FindErrorPosition(syndrome);
                if (errorPosition != -1)
                {
                    receivedData ^= (1 << errorPosition); // Correct the error
                    result.CorrectedData = receivedData >> 6; // Return original data
                    result.Message = $"Error detected and corrected at position {errorPosition}.";
                }
                else
                {
                    result.Message = "Error detected but cannot be corrected.";
                }
            }
            else
            {
                result.HasError = false;
                result.CorrectedData = receivedData >> 6; // Return original data
                result.Message = "Data decoded without errors.";
            }

            return result;
        }

        // Method to calculate syndrome
        public int CalculateSyndrome(int receivedData)
        {
            int remainder = receivedData;

            // Calculate syndrome
            for (int i = 0; i < 6; i++)
            {
                if ((remainder & (1 << (CodeLength - 1 - i))) != 0)
                {
                    remainder ^= GeneratorPolynomial << (6 - 1 - i);
                }
            }
            return remainder; // Return syndrome
        }

        // Method to find error position
        public int FindErrorPosition(int syndrome)
        {
            for (int i = 0; i < CodeLength; i++)
            {
                if (syndrome == (1 << i))
                {
                    return i; // Return error position
                }
            }
            return -1; // Error not found
        }

        public int GenerateRandomData(int length)
        {
            return random.Next(0, (1 << length));
        }

        public int IntroduceRandomError(int data)
        {
            int errorPosition = random.Next(0, CodeLength);
            return data ^ (1 << errorPosition);
        }
    }
    public class DecodeResult
    {
        public bool HasError { get; set; }
        public int CorrectedData { get; set; }
        public string Message { get; set; }
    }
}

