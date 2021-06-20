using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Settings
{
    public class AzureWebPubSubSetting
    {
        public string Endpoint { get; set; }
        public string Hub { get; set; }
        public string AccessKey { get; set; }
        public string DummyUri { get; set; }
    }
}
