using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyManage
{
    public static class Common
    {
        public static void Show(this string message)
        {
            MessageBox.Show(message);
        }

        public static void Err(this string message)
        {
            throw  new Exception(message);
        }

        public static bool IsNullOrEmpty(this string message)
        {
            return string.IsNullOrEmpty(message);
        }

        public static string CreateHash(this string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return string.Join("", bytes.Select(a => a.ToString("x2")));
            }
        }


        public static int UserID;
    }
}
