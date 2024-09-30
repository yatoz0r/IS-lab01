
namespace CryptLogic
{
    public class CeaserCipher
    {
        public string Encode(string input, int shift)
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
    }
}
