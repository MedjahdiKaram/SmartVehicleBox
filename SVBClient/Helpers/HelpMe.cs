using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace SVBClient.Helpers
{
    public static class HelpMe
    {
        public static void Beddel(this Activity currentActivity, Type target)
        {
            var targetActivity = new Intent(currentActivity, target);

            currentActivity.StartActivity(targetActivity);
        }


        public static void Beddel(this Activity currentActivity, Type target, string amana)
        {
            try
            {
                var targetActivity = new Intent(currentActivity, target);


                targetActivity.PutExtra("Amana", amana);
                currentActivity.StartActivity(targetActivity);
            }
            catch (Java.Lang.Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void SendSms(this Activity activity, string phone, string message)
        {

            try
            {
                var smsManager = SmsManager.Default;

                var piSent = PendingIntent.GetBroadcast(activity, 0, new Intent("SMS_SENT"), 0);
                var piDelivered = PendingIntent.GetBroadcast(activity, 0, new Intent("SMS_DELIVERED"), 0);
                smsManager.SendTextMessage(phone, null, message, piSent, piDelivered);

            }
            catch
            {

            }
        }

        public static string GetUrl(string txt)
        {
            var res = "";
            foreach (Match item in Regex.Matches(txt, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
            {
                res= item.Value;
                break;
            }
            return res;
        }
    }
}