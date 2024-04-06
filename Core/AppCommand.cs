using Cashbox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;

namespace Cashbox.Core
{
    public class AppCommand
    {
        public static string GenCode(int length = 32)
        {
            Random random = new();
            char[] SymbolCodeChar =
            [
                'A','a','B','b','C','c',
                'D','d','F','f','X','x',
                '0','1','2','3','4','5',
                '6','7','8','9','G','g',
                'Z','z','S','s','N','n',
                'V','v','Q','q','E','e',
                'T','t','J','j','L','l',
                'H','h','U','u','I','i',
                'P','p','M','m','K','k'
            ];

            string ResCode = "";

            for (int i = 0; i <= length; i++)
            {
                int RandomIndex = random.Next(0, SymbolCodeChar.Length - 1);
                ResCode += SymbolCodeChar[RandomIndex];
            }
            return ResCode;
        }

        public static string HashCode(string plaintext)
        {
            string scode = "datacode";
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{plaintext}{scode}"));

            return Convert.ToBase64String(hash);
        }

        public static void ErrorMessage(string message, string caption = "Ошибка") => MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        

    }
}
