using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cleanplanetapp.Svc
{
    public class HashGenerator
    {
        public static string ComputeSHA512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha512.ComputeHash(bytes);

                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

    }
}
