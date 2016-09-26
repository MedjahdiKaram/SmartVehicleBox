using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SVBClient.Helpers;

namespace SVBClient
{
    [Activity(Label = "StopActivity")]
    public class StopActivity : Activity
    {
        public Button SendButton { get; set; }
        public EditText VehicleBox { get; set; }
        public EditText Password { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title =GetString(Resource.String.StopTitle);
            SetContentView(Resource.Layout.StopActivity);
            VehicleBox = FindViewById<EditText>(Resource.Id.CarPhoneEdit);
            Password= FindViewById<EditText>(Resource.Id.PasswordEdit);
            SendButton = FindViewById<Button>(Resource.Id.sendconfigbtn);
            VehicleBox.Text = MainActivity.Main.PhoneVehicle;
            SendButton.Click += delegate
            {
                this.SendSms(VehicleBox.Text, Password.Text+"#stop");
             
             
             
            };
        }

      
    }
}