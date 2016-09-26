using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Accounts;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Locations;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Java.Lang;
using SVBClient.Helpers;
using SVBClient.Sms;
using Exception = Java.Lang.Exception;
using Math = Java.Lang.Math;


namespace SVBClient
{
    [Activity(Label = "S V B", MainLauncher = true, Icon = "@drawable/SVBLogo64")]
    public class MainActivity : Activity, ILocationListener
    {

        public Button QibBtn { get; set; }

        public ImageButton ConfButn { get; set; }
        public ImageButton WhereBtn { get; set; }
        public ImageButton StopBtn { get; set; }
        public static MainActivity Main { get; set; }
        public  FileHelper FileHlp { get; set; }
        public  string PhoneVehicle { get; set; }

        private BroadcastReceiver _smsSentBroadcastReceiver, _smsDeliveredBroadcastReceiver;

        #region GPS Props

        private Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;
        private StringBuilder _deviceAddress;
        GeomagneticField geoField;

        
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        [Android.Runtime.Register("currentTimeMillis", "()J", "")]
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
        #endregion


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Main = this;
            Title = GetString(Resource.String.SVBWelcome);

            SetContentView(Resource.Layout.Main);
            InitializeLocationManager();
            ConfButn = FindViewById<ImageButton>(Resource.Id.ConfigBtn);
            WhereBtn = FindViewById<ImageButton>(Resource.Id.WhereBtn);
            StopBtn = FindViewById<ImageButton>(Resource.Id.StopBtn);
            QibBtn = FindViewById<Button>(Resource.Id.Qib);
            FileHlp=new FileHelper();
       
            PhoneVehicle= FileHlp.Read();
            
            ConfButn.Click += delegate
            {
                this.Beddel(typeof(ConfigActivity));
            };

            StopBtn.Click += delegate
            {
                this.Beddel(typeof(StopActivity));
            };
            WhereBtn.Click += delegate
            {
                this.SendSms(PhoneVehicle,"WHERE");
            };

            QibBtn.Click += delegate
            {

                
            };


        }

        

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);

                _smsSentBroadcastReceiver = new SmsSentReceiver();
                _smsDeliveredBroadcastReceiver = new SmsDeliveredReceiver();


                RegisterReceiver(_smsSentBroadcastReceiver, new IntentFilter("SMS_SENT"));
                RegisterReceiver(_smsDeliveredBroadcastReceiver, new IntentFilter("SMS_DELIVERED"));
            }
            catch (Exception)
            {
             
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
            catch (Exception)
            {
            
            }
        }

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
               
            }
        }
        public async void GetAdress()
        {
            try
            {
                if (_currentLocation == null)
                {
                    //Monitor.Text +=
                    //    "\r\n ----->Error on GetAdress location: Can't determine the current address. Try again in a few minutes.";
                    return;
                }

                var address = await ReverseGeocodeCurrentLocation();

                DisplayAddress(address);
            }
            catch (Exception e)
            {
               // Monitor.Text = e.Message;
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
               Log.Debug ("ReverseGeocodeCurrentLocation",e.Message);
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

                    //Monitor.Text += "\r\n --->Location changed new Lat:" + _currentLocation.Latitude + " Long:" +
                    //                _currentLocation.Longitude;
                    //MainDictionary["latitude"] = _currentLocation.Latitude.ToString();
                    //MainDictionary["longitude"] = _currentLocation.Longitude.ToString();
                    //Monitor.Text += "\r\n ------>Adress: " + _deviceAddress.ToString();
                    //MainDictionary["address"] = _deviceAddress.ToString();
                }
                else
                {
                    Log.Debug("Notif","------>Error on adress location: Unable to determine the address. Try again in a few minutes.");
                }
            }
            catch (Exception e)
            {
                
            }
        }
        
        private float heading;
        private Location _waypoint = new Location(LocationManager.GpsProvider);
        public void OnLocationChanged(Location location)
        {
            try
            {
                _currentLocation = location;
                //geoField = new GeomagneticField((float) location.Latitude, (float) location.Longitude,
                //    (float) location.Altitude, CurrentTimeMillis());
                //heading += geoField.Declination;
                //heading = location.Bearing - (location.Bearing + heading);
                //var x = -heading/360 + 18;
                 
                _waypoint.Latitude= 34.883777;
                _waypoint.Longitude= -1.310772;
                QibBtn.Text = "Br " + location.Bearing + "brTo " + location.BearingTo(_waypoint);
                    //"Br:"+ location.Bearing+"head:"+ heading+" round:" + x.ToString();
            }
            catch (Exception e)
            {

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


    }
}

