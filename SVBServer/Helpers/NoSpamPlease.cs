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

namespace SVBServer.Helpers
{
    public class NoSpamPlease
    {
        public DateTime Moment { get; set; }
        public int MaxSend { get; set; }
        public int SendCount { get; set; }

        public bool CanSend()
        {
            var moment = DateTime.Now;
            if (SendCount >= MaxSend)
            {
                if (Moment.Minute == DateTime.Now.Minute)
                    return false;
                
                    SendCount = 0;

            }
            Moment = moment;
            SendCount++;
            return true;

        }

        public NoSpamPlease(int maxSend)
        {
            SendCount = 0;
            MaxSend = maxSend;
        }
    }
}