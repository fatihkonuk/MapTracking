using System.Security.Cryptography;
using System.Text;

namespace WebApi.Services.Helpers
{
    public class PasswordHasher
    {
        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            // Salt oluşturma
            using var rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[16];
            rng.GetBytes(salt); // Rastgele salt oluştur

            // Şifreyi byte dizisine çevir
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Salt ve şifreyi birleştir
            byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
            Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

            // Saltlı şifreyi hashle
            byte[] hashedBytes = SHA256.HashData(saltedPassword);

            // Salt ve hashlenmiş şifreyi döndür
            return (Convert.ToBase64String(hashedBytes), Convert.ToBase64String(salt));
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            if (enteredPassword == null) throw new ArgumentNullException(nameof(enteredPassword));
            if (storedHash == null) throw new ArgumentNullException(nameof(storedHash));
            if (storedSalt == null) throw new ArgumentNullException(nameof(storedSalt));

            // Salt'ı base64 formatından byte dizisine çevir
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            // Kullanıcının girdiği şifreyi byte dizisine çevir
            byte[] passwordBytes = Encoding.UTF8.GetBytes(enteredPassword);

            // Salt ve şifreyi birleştir
            byte[] saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, saltedPassword, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, saltBytes.Length, passwordBytes.Length);

            // Saltlı şifreyi hashle
            byte[] hashedBytes = SHA256.HashData(saltedPassword);

            // Saklanan hash'i base64'ten çöz
            byte[] storedHashBytes = Convert.FromBase64String(storedHash);

            // Hash'leri karşılaştır
            return hashedBytes.SequenceEqual(storedHashBytes);
        }
    }
}
