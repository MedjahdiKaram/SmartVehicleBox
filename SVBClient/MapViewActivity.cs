using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

using Exception = Java.Lang.Exception;

namespace SVBClient
{
    [Activity(Label = "Localisation du véhicule")]
    public class MapViewActivity : Activity
    {

        public WebView WebBrowser { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MapViewActivity);
            var mapUrl = Intent.GetStringExtra("Amana");
            WebBrowser = FindViewById<WebView>(Resource.Id.SvbBrowser);
            WebBrowser.SetWebViewClient(new WebViewClient());
            WebBrowser.Settings.JavaScriptEnabled = true;
            WebBrowser.LoadUrl(mapUrl);
        }

        
    }
}