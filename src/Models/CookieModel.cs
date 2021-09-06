using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GetFbAuth_LdPlayerGUI.Enums;

namespace GetFbAuth_LdPlayerGUI.Models
{
    public class CookieModel
    {
        public CookieModel(string rawTextFromfile, FbAuthEnum.FBAppTypes fBAppTypes)
        {
            rawText = rawTextFromfile;
            rawText = Regex.Replace(rawText, "[^\\u0020-\\u007E]", "|");
            FBAppType = fBAppTypes;
        }
        private FbAuthEnum.FBAppTypes FBAppType { get; set; }
        private string rawText { get; set; }
        public string xs { get; set; }
        public string fr {  get; set; }  
        public string datr {  get; set; }   
        public string uid {  get; set; }
        public string Token {  get; set; }   
        public string FullCookie
        {
            get
            {
                if (FBAppType == FbAuthEnum.FBAppTypes.Fb)
                {
                    initFb();
                }
                else
                {
                    InitFbLite();
                }
                return ($"datr={System.Net.WebUtility.UrlEncode(datr)}; c_user={uid}; xs={System.Net.WebUtility.UrlEncode(xs)}; fr={System.Net.WebUtility.UrlEncode(fr)};");
            }
        }
        private void initFb()
        {
            Token = "EAA" + Regex.Match(rawText, "EAA(.*?)\\|").Groups[1].Value;
            uid = Regex.Match(rawText, "uid\\|\\|(.*?)\\|").Groups[1].Value;
            xs = Utilities.GetNodeStringByName(rawText, "name\":\"xs\",\"value\":\"(.*?)\"", 1);
            fr = Utilities.GetNodeStringByName(rawText, "name\":\"fr\",\"value\":\"(.*?)\"", 1);
            datr = Utilities.GetNodeStringByName(rawText, "name\":\"datr\",\"value\":\"(.*?)\"", 1);
        }
        private void InitFbLite()
        {
            Token = "EAA" + Regex.Match(rawText, "EAA(.*?)\",").Groups[1].Value;
            uid = Regex.Match(rawText, "uid\":\"(\\d+)\",").Groups[1].Value;
            xs = Utilities.GetNodeStringByName(rawText, "name\":\"xs\"(.*?)\"value\":\"(.*?)\"", 2);
            fr = Utilities.GetNodeStringByName(rawText, "name\":\"fr\"(.*?)\"value\":\"(.*?)\"", 2);
            datr = Utilities.GetNodeStringByName(rawText, "name\":\"datr\"(.*?)\"value\":\"(.*?)\"", 2);
        }
    }
}
