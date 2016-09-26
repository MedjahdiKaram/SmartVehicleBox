using System;
using Android.App;
using Android.Content;
using Android.Telephony;
using Android.Util;
using Android.Widget;

namespace SVBServer.Sms
{
    [BroadcastReceiver(Exported = true)]
    public class SMSSentReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                switch ((int)ResultCode)
                {
                    case (int)Result.Ok:
                        Toast.MakeText(Application.Context, "SMS has been sent", ToastLength.Short).Show();
                        break;
                    case (int)SmsResultError.GenericFailure:
                        Toast.MakeText(Application.Context, "Generic Failure", ToastLength.Short).Show();
                        break;
                    case (int)SmsResultError.NoService:
                        Toast.MakeText(Application.Context, "No Service", ToastLength.Short).Show();
                        break;
                    case (int)SmsResultError.NullPdu:
                        Toast.MakeText(Application.Context, "Null PDU", ToastLength.Short).Show();
                        break;
                    case (int)SmsResultError.RadioOff:
                        Toast.MakeText(Application.Context, "Radio Off", ToastLength.Short).Show();
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Debug("Error on SMSSentReceiver",e.Message);
            }
        }
    }
}