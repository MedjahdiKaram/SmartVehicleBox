using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using SVBClient.Helpers;

namespace SVBClient
{
    [Activity(Label = "ConfigActivity")]
    public class ConfigActivity : Activity
    {
        public Button SendButton { get; set; }
        public EditText VehicleBox { get; set; }
        public EditText Phone1Box { get; set; }
        public EditText Phone2Box { get; set; }
        public EditText PasswordBox { get; set; }


        public string ConvertToConfig(string phone1, string phone2, string password)
        {
            return "#CONFIG#" + phone1 + "/" + phone2 + "/" + password;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConfigActivity);
            Title = GetString(Resource.String.ConfigSvb);


            SendButton = FindViewById<Button>(Resource.Id.sendconfigbtn);
            VehicleBox = FindViewById<EditText>(Resource.Id.numvehicle);
            Phone1Box  = FindViewById<EditText>(Resource.Id.mob1);
            Phone2Box  = FindViewById<EditText>(Resource.Id.mob2);
            PasswordBox = FindViewById<EditText>(Resource.Id.passwd);
            VehicleBox.Text = MainActivity.Main.PhoneVehicle; 
            SendButton.Click += delegate
            {
                this.SendSms(VehicleBox.Text,ConvertToConfig(Phone1Box.Text, Phone2Box.Text, PasswordBox.Text));
                MainActivity.Main.FileHlp.Write(VehicleBox.Text);
                
            };

        }
       
    }
  
}