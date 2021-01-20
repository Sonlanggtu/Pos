using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Gateway.Securities.Common
{
    public static class PosEncryption
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }
        public static bool ValidatePassword(string password, string correctHash)
        {

            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(8);
        }
        public static string GetRandomPass()
        {
            Random random = new Random();
            string[] chars = new string[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "0123456789", "abcdefghijkmnopqrstuv", "@#&*" };
            return new string(Enumerable.Repeat(chars[0], 4).Select(s => s[random.Next(s.Length)]).ToArray())
            + new string(Enumerable.Repeat(chars[1], 2).Select(s => s[random.Next(s.Length)]).ToArray())
            + new string(Enumerable.Repeat(chars[2], 4).Select(s => s[random.Next(s.Length)]).ToArray())
            + new string(Enumerable.Repeat(chars[3], 1).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
