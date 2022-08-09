using System;
namespace ParkingProject
{
    public class Hasher
    {
        public string GenerateHash(String password, String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            System.Security.Cryptography.SHA256Managed hashedpass = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = hashedpass.ComputeHash(bytes);

            return ByteArrayToString(hash);

        }

        public string GenerateSalt(int size)
        {
            var random = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var salt = new byte[size];
            random.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }

        public static string ByteArrayToString(byte[] bytearray)
        {
            return BitConverter.ToString(bytearray).Replace("-", "");
        }

    }
}
