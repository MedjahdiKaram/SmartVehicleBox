using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using Android.Text;
using Android.Util;
using DzBluetooth;
using Java.Lang;
using SVBServer.Helpers;
using SVBServer.Models;
using SVBServer.Sms;
using Exception = Java.Lang.Exception;
using StringBuilder = Java.Lang.StringBuilder;

namespace SVBServer
{
    [Activity(Label = "SVB Monitor", MainLauncher = true, Icon = "@drawable/SVBLogoServer64")]
    public class MainActivity : Activity, ILocationListener
    {
        private Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;
        private SmsManager _smsManager;
        private StringBuilder _deviceAddress;

        private BroadcastReceiver _smsSentBroadcastReceiver, _smsDeliveredBroadcastReceiver;
        public MyBluetooth Bluetooth { get; set; }

        public TextView Monitor { get; private set; }
        public Button ButtonRun { get; private set; }

        public static MainActivity MainAct { get; set; }
        public static Dictionary<string,string> MainDictionary { get; set; }
        public NoSpamPlease NoSpam { get; set; }
        public FileHelper ConfFile { get; set; }
        public ConfigSvb ConfigEntity { get; set; } 
        protected override void OnCreate(Bundle bundle)
        {

            #region Android stuff

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Title = "SVB Console";
            Monitor = FindViewById<TextView>(Resource.Id.MonitorView);
            ButtonRun = FindViewById<Button>(Resource.Id.MyButton);
            MainAct = this;
            ConfFile = new FileHelper();
            ConfigEntity=Helper.Parse(ConfFile.Read());
            MainDictionary = new Dictionary<string, string>
            {
                {"phone", ConfigEntity.SosPhone1},{"phone2", ConfigEntity.SosPhone2},
                {"sender", "0552395895"},
                {"longitude", "0.000000"},
                {"latitude", "0.000000"},
                {"password", ConfigEntity.Password},
                {"address", "NULL Address ! GPS ma rahch ykhdem"}
            };

            #endregion


            #region Bluetooth stuff

            Bluetooth = new MyBluetooth("HC-05", 100);
            Bluetooth.PropertyChanged += DoOnBluetoothPropChanged;
            if (Bluetooth.Socket.IsConnected)
                Monitor.Text += "\r\n Bluetooth HC-05 Connected !!! \r\n";
            else
                Monitor.Text += "\r\n Ooops Bluetooth not Connected !!! \r\n";

            #endregion
            
            Monitor.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
            Monitor.TextChanged += ClearMonitor;
     



            ButtonRun.Click += delegate
            {
                GetAdress();               
            };
            
            InitializeLocationManager();
            _smsManager = SmsManager.Default;

        }       

        private void DoOnBluetoothPropChanged(object sender, PropertyChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {

                if (e.PropertyName == "BufferReaderString")
                {
                    Monitor.Text ="From Bluetooth> "+ Bluetooth.BufferReaderString.Replace("\r","").Replace("\n","");

                    
                    
                    this.Decide(Bluetooth.BufferReaderString, MainDictionary);
                }
            });

        }


        #region UI & Activity Methods

       

        public void ClearMonitor(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            try
            {
                if (Monitor.Text.Length > 1500)
                    Monitor.Text = "";
            }
            catch (Exception e)
            {
                Monitor.Text = e.Message;
            }    
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
           
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
                _smsSentBroadcastReceiver = new SMSSentReceiver();
                _smsDeliveredBroadcastReceiver = new SMSDeliveredReceiver();
               

                RegisterReceiver(_smsSentBroadcastReceiver, new IntentFilter("SMS_SENT"));
                RegisterReceiver(_smsDeliveredBroadcastReceiver, new IntentFilter("SMS_DELIVERED"));
            }
            catch (Exception e)
            {
                Monitor.Text = e.Message;
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
                _locationManager.RemoveUpdates(this);

                UnregisterReceiver(_smsSentBroadcastReceiver);
                UnregisterReceiver(_smsDeliveredBroadcastReceiver);
            }
            catch (Exception e)
            {
                Monitor.Text=e.Message;
            }
        }

        #endregion


        #region GPS Functions
        void InitializeLocationManager()
        {
            try
            {
                _locationManager = (LocationManager)GetSystemService(LocationService);
                var criteriaForLocationService = new Criteria
                {
                    Accuracy = Accuracy.Fine
                };
                IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

                if (acceptableLocationProviders.Any())
                {
                    _locationProvider = acceptableLocationProviders.First();
                }
                else
                {
                    _locationProvider = string.Empty;
                }
                
            }
            catch (Exception e)
            {
                Monitor.Text=e.Message;
            }
        }
        public async void GetAdress()
        {
            try
            {
                if (_currentLocation == null)
                {
                    Monitor.Text +=
                        "\r\n ----->Error on GetAdress location: Can't determine the current address. Try again in a few minutes.";
                    return;
                }

                var address = await ReverseGeocodeCurrentLocation();

                DisplayAddress(address);
            }
            catch (Exception e)
            {
                Monitor.Text = e.Message;
            }
        }

        private async Task<Address> ReverseGeocodeCurrentLocation()
        {
            try
            {
                var geocoder = new Geocoder(this);
                var addressList =
                    await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

                var address = addressList.FirstOrDefault();
                return address;
            }
            catch (Exception e)
            {
                Monitor.Text = e.Message;
                return null;
            }
        }

        private void DisplayAddress(Address address)
        {
            try
            {
                if (address != null)
                {
                    _deviceAddress = new StringBuilder();
                    for (int i = 0; i < address.MaxAddressLineIndex; i++)
                    {
                        _deviceAddress.Append(address.GetAddressLine(i));
                    }
                
                    Monitor.Text += "\r\n --->Location changed new Lat:" + _currentLocation.Latitude + " Long:" +
                                    _currentLocation.Longitude;
                    MainDictionary["latitude"] = _currentLocation.Latitude.ToString();
                    MainDictionary["longitude"] = _currentLocation.Longitude.ToString();
                    Monitor.Text += "\r\n ------>Adress: " + _deviceAddress.ToString();
                    MainDictionary["address"] = _deviceAddress.ToString();
                }
                else
                {
                    Monitor.Text +=
                        "------>Error on adress location: Unable to determine the address. Try again in a few minutes.";
                }
            }
            catch (Exception e)
            {
                Monitor.Text = e.Message;
            }
        }

        public void OnLocationChanged(Location location)
        {
            try
            {
                _currentLocation = location;
            }
            catch (Exception e)
            {
                Monitor.Text=e.Message;
            }
        }        
        #region Useless functions

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #endregion

        public void SendSms(string phone, string message)
        {
            if (NoSpam==null)
                NoSpam=new NoSpamPlease(4);
            try
            {
                if (!NoSpam.CanSend())
                {
                    Monitor.Text += "\r\n!!!!! SMS Spam controller doesn't allow to send more than 4 sms per minute";
               
                    Monitor.Text="";
                    return;
                }
                var piSent = PendingIntent.GetBroadcast(this, 0, new Intent("SMS_SENT"), 0);
                var piDelivered = PendingIntent.GetBroadcast(this, 0, new Intent("SMS_DELIVERED"), 0);
                _smsManager.SendTextMessage(phone, null, message, piSent, piDelivered);
                
            }
            catch (Exception e)
            {
                Monitor.Text=e.Message;
            }
        }

    }
}

