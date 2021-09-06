using System;
using System.Collections.Generic;
using System.Text;

namespace GetFbAuth_LdPlayerGUI.Models
{
    public class DatagridModel
    {
        public int NumberRow { get; set; }
        public string DeviceId { get; set; }
        public string Fbid {  get; set; }
        public string FbToken { get; set; }
        public string FbCookie { get; set; }
    }
}
