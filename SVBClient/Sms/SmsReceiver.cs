using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Telephony;
using Android.Util;
using SVBClient.Helpers;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;

namespace SVBClient.Sms
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
                    //SVBServer.MainActivity.MainDictionary["sender"]= msgs[i].OriginatingAddress;
                    //SVBServer.MainActivity.MainAct.Decide(msgs[i].MessageBody, SVBServer.MainActivity.MainDictionary);
                    var msg = msgs[i].MessageBody;

                    MainActivity.Main.Beddel(typeof(MapViewActivity),HelpMe.GetUrl(msg));
                    //var launchIntent = MainActivity.Main.PackageManager.GetLaunchIntentForPackage("uz.efir.muazzin");
                  
                    //MainActivity.Main.StartActivity(launchIntent);

                }
            }
            catch (Exception e)
            {
                Log.Debug("errrooooor on sms receiver", e.Message);
            }
        }

       
    }
}