using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace SVBServer.Sms
{
    [BroadcastReceiver(Exported = true)]
    public class SMSDeliveredReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                switch ((int)ResultCode)
                {
                    case (int)Result.Ok:
                        Toast.MakeText(Application.Context, "SMS Delivered", ToastLength.Short).Show();
                        break;
                    case (int)Result.Canceled:
                        Toast.MakeText(Application.Context, "SMS not delivered", ToastLength.Short).Show();
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Debug("Error on SMSDeliveredReceiver", e.Message);
            }
        }
    }
}