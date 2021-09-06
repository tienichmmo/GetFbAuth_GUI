using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using GetFbAuth_LdPlayerGUI.Models;
using GetFbAuth_LdPlayerGUI.Enums;

namespace GetFbAuth_LdPlayerGUI
{
    public class AdbServices
    {
        public const string FbPath = "/data/data/com.facebook.katana/app_light_prefs/com.facebook.katana/authentication";
        public const string FbLitePath = "/data/data/com.facebook.lite/files/PropertiesStore_v02";
        public static List<string> GetListDevices()
        {
            List<string> list = new List<string>();
            string input = ExecuteCMD("adb devices");
            if (!string.IsNullOrEmpty(input))
            {
                string pattern = "(?<=List of devices attached)([^\\n]*\\n+)+";
                MatchCollection Listmatch = Regex.Matches(input, pattern, RegexOptions.Singleline);
                if (Listmatch.Count > 0)
                {
                    string value = Listmatch[0].Groups[0].Value;
                    string[] array = Regex.Split(value, "\r\n");
                    foreach (string text in array)
                    {
                        if (!string.IsNullOrEmpty(text) && text != " ")
                        {
                            string text2 = text.Trim().Replace("device", "");
                            list.Add(text2.Trim());
                        }
                    }
                }
            }
            return list;
        }
        public static string ExecuteCMD(string cmdCommand)
        {
            string result;
            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                process.Start();
                process.StandardInput.WriteLine(cmdCommand);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
                string text = process.StandardOutput.ReadToEnd();
                process.Dispose();
                result = text;
            }
            catch
            {
                result = null;
            }
            return result;
        }
        public static string CmdQuery(string DeviceId, FbAuthEnum.FBAppTypes fBAppTypes)
        {
            return fBAppTypes == FbAuthEnum.FBAppTypes.Fb ? $"adb -s {DeviceId} root && adb -s {DeviceId} pull {FbPath}"
                : $"adb -s {DeviceId} root && adb -s {DeviceId} pull {FbLitePath}";
        }
    }
}
