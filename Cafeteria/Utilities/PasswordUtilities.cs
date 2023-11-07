using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace Cafeteria.Utilities
{
    public class PasswordUtilities
    {
        public static string GeneratePassword()
        {
            string password = "";
            Random random = new Random();
            int length = random.Next(8, 16);
            for (int i = 0; i < length; i++)
            {
                int type = random.Next(0, 3);
                if (type == 0)
                {
                    password += (char)random.Next(48, 58);
                }
                else if (type == 1)
                {
                    password += (char)random.Next(65, 91);
                }
                else
                {
                    password += (char)random.Next(97, 123);
                }
            }
            return password;
        }

        public static string PasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            }
        }
        public static bool PasswordVerify(string password, string checkPassword)
        {
            string senhaCriptografadaInput = PasswordHash(password);
            return senhaCriptografadaInput == checkPassword;
        }
    }
}
