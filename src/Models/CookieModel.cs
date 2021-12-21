using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GetFbAuth_LdPlayerGUI.Enums;
using Newtonsoft.Json.Linq;
using static System.Net.WebUtility;

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
                return ($"datr={UrlEncode(datr)}; c_user={uid}; xs={UrlEncode(xs)}; fr={UrlEncode(fr)};");
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
            #region OldMethod
            //Token = "EAA" + Regex.Match(rawText, "EAA(.*?)\",").Groups[1].Value;
            //uid = Regex.Match(rawText, "uid\":\"(\\d+)\",").Groups[1].Value;
            //xs = Utilities.GetNodeStringByName(rawText, "name\":\"xs\"(.*?)\"value\":\"(.*?)\"", 2);
            //fr = Utilities.GetNodeStringByName(rawText, "name\":\"fr\"(.*?)\"value\":\"(.*?)\"", 2);
            //datr = Utilities.GetNodeStringByName(rawText, "name\":\"datr\"(.*?)\"value\":\"(.*?)\"", 2);
            //Token = "EAA" + Regex.Match(rawText, "EAA(.*?)\",").Groups[1].Value;
            #endregion
            string center_json = Regex.Match(rawText, "\\[{\"expires\":(.*?}\\])").Groups[1].Value;
            string first_json = "[{\"expires\":" + center_json;
            JArray jArray = JArray.Parse(first_json);
            List<FbAuthJson> auth = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FbAuthJson>>(jArray.ToString());
            Token = "EAA" + Regex.Match(rawText, "EAA(.*?)\\|").Groups[1].Value;
            xs = auth.Find(x => x.name == "xs").value;
            fr = auth.Find(x => x.name == "fr").value;
            datr = auth.Find(x => x.name == "datr").value;
            uid = auth.Find(x => x.name == "c_user").value;
        }
        private class FbAuthJson
        {
            public string expires { get; set; }
            public string path { get; set; }
            public string is_secure { get; set; }
            public string domain { get; set; }
            public string value { get; set; }
            public string name { get; set; }
            public string expires_timestamp { get; set; }
        }
    }
}
