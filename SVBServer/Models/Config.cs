using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DzBluetooth.Annotations;

namespace SVBServer.Models
{
    public class ConfigSvb
    {

        public string SosPhone1 { get; set; }
        public string SosPhone2 { get; set; }
        public string Password { get; set; }
        
    }
}