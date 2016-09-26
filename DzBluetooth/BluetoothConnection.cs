using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DzBluetooth
{
    public class BluetoothConnection
    {
        #region Properties

        public BluetoothAdapter BAdapter { get; set; }
        public BluetoothDevice BDevice { get; set; }
        public ICollection<BluetoothDevice> BDevices { get; set; }

        public BluetoothSocket BSocket { get; set; }

        #endregion



        #region Functions

        public void GetAdapter()
        {
            BAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public void GetDevice(string deviceName)
        {
            BDevice = (from bd in BAdapter.BondedDevices where bd.Name == deviceName select bd).FirstOrDefault();
        }

        public void GetDevices()
        {
            BDevices = (from bd in BAdapter.BondedDevices select bd).ToList();
        }

        #endregion



        #region Initializers

        public BluetoothConnection()
        {

        }

        public BluetoothConnection(string deviceName)
        {
            GetDevice(deviceName);
        }

        #endregion

    }
}