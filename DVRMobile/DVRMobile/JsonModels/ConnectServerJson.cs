using System;
using System.Collections.Generic;
using System.Text;

namespace DVRMobile.JsonModels
{
    public class ConnectServerJson : JsonResult
    {
        public string token { get; set; }
        public List<Models.Channel> listChannel { get; set; }
    }
}
