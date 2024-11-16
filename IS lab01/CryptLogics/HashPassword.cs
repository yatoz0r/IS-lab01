using System.Security.Cryptography;

namespace IS_lab01.CryptLogics
{
    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private readonly BlockEncrypt _blockEncrypt;

        public PasswordHasher()
        {
            _blockEncrypt = new BlockEncrypt();
            _blockEncrypt.GenerateKey(); // Generate a new key for encryption
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty");

            // Create salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Encrypt the password
            string saltedPassword = Convert.ToBase64String(salt) + password; // Combine salt and password
            string encryptedPassword = _blockEncrypt.Encrypt(saltedPassword);

            // Combine salt and encrypted password for storage
            return Convert.ToBase64String(salt) + ":" + encryptedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                // Split the salt and encrypted password
                var parts = hashedPassword.Split(':');
                if (parts.Length != 2) return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                string encryptedPassword = parts[1];

                // Decrypt the stored password
                string decryptedPassword = _blockEncrypt.Decrypt(encryptedPassword);

                // Compare the original password with the decrypted password
                return decryptedPassword.Equals(Convert.ToBase64String(salt) + password);
            }
            catch
            {
                return false;
            }
        }
    }
}