using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Telephony;
using Android.Util;
using DzBluetooth.Annotations;
using Java.Lang;
using SVBServer.Helpers;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;

namespace SVBServer.Sms
{
    [BroadcastReceiver(Enabled = true, Label = "SMS Receiver")]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" })]
    public class SmsReceiver : Android.Content.BroadcastReceiver
    {
        public static readonly string INTENT_ACTION = "android.provider.Telephony.SMS_RECEIVED";


        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action != INTENT_ACTION) return;
                var bundle = intent.Extras;
                if (bundle == null) return;
                var pdus = (Object[])bundle.Get("pdus");
                var msgs = new SmsMessage[pdus.Length];
                for (var i = 0; i < msgs.Length; i++)
                {
                    var format = bundle.GetString("format");
                    msgs[i] = SmsMessage.CreateFromPdu((byte[])pdus[i], format);                        
                    MainActivity.MainDictionary["sender"]= msgs[i].OriginatingAddress;
                    MainActivity.MainAct.Decide(msgs[i].MessageBody, MainActivity.MainDictionary);
                        
                     
                }
            }
            catch (Exception e)
            {
                Log.Debug("errrooooor on sms receiver", e.Message);
            }
        }

       
    }
}