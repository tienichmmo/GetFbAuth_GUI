using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GetFbAuth_LdPlayerGUI
{
    public static class Utilities
    {
        private static Random random = new Random();
        public static string RandomStr(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetNodeStringByName(string Source, string pattern, int group)
        {
            return Regex.Match(Source, pattern).Groups[group].Value;
        }
    }
}
