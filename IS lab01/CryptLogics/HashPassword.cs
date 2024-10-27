using System.Security.Cryptography;

namespace IS_lab01.CryptLogics
{
    public class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty");

            // Создаем соль
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            // Создаем хеш
            byte[] hash = GetHash(password, salt);

            // Комбинируем соль и хеш
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                // Получаем байты из хешированного пароля
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Получаем соль
                byte[] salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                // Получаем оригинальный хеш
                byte[] originalHash = new byte[HashSize];
                Array.Copy(hashBytes, SaltSize, originalHash, 0, HashSize);

                // Вычисляем хеш для введенного пароля
                byte[] computedHash = GetHash(password, salt);

                // Сравниваем хеши
                for (int i = 0; i < HashSize; i++)
                {
                    if (computedHash[i] != originalHash[i]) return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private byte[] GetHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }
    }
}
