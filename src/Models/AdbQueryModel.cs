using System;
using System.Collections.Generic;
using System.Text;
using GetFbAuth_LdPlayerGUI.Enums;

namespace GetFbAuth_LdPlayerGUI.Models
{
    public static class AdbQueryModel
    {
        public static FbAuthEnum.FBAppTypes FBAppType { get; set; }
        public static FbAuthEnum.CmdQuerys Cmd { get; set; }
    }
}
