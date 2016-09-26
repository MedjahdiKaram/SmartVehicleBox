using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DzBluetooth.Annotations;
using DzBluetooth.Helpers;
using Java.Lang;

namespace DzBluetooth
{
    public class MyBluetooth:INotifyPropertyChanged
    {
        private string _bufferReaderString;
        private byte[] _bufferReader;
        public BluetoothConnection MyConnection { get; set; }
        public BluetoothSocket Socket { get; set; }

        public Java.Lang.Thread ListnerThread { get; set; }
        public string DeviceName { get; set; }
        public int BufferReaderSize { get; set; }
        public int ListnerSleepDuration { get; set; }

        public byte[] BufferReader
        {
            get { return _bufferReader; }
            set
            {
       
                _bufferReader = value;
                BufferReaderString=Encoding.UTF8.GetString(value);
                OnPropertyChanged();
            }
        }

        public string BufferReaderString
        {
            get { return _bufferReaderString; }
            set
            {
        
                _bufferReaderString = value;
                OnPropertyChanged();
            }
        }

  
        public void Listener()
        {
            BufferReader = new byte[BufferReaderSize]; ;




            while (true)
            {
                try
                {
                    if (MyConnection.BSocket==null)
                        continue;
                    MyConnection.BSocket.InputStream.Read(BufferReader, 0, BufferReader.Length - 1);
                    if (!BufferHelper.IsBufferEmty(BufferReader))
                    {
           
                        BufferReaderString = Encoding.UTF8.GetString(BufferReader);
                    }
                    MyConnection.BSocket.InputStream.Flush();
                    MyConnection.BSocket.InputStream.Flush();
                    MyConnection.BSocket.InputStream.Close();
                    Thread.Sleep(ListnerSleepDuration);          
                }
                catch 
                {
                    continue;
                }
            }
        }

        public void Connect()
        {


            ////////////////////////////////////////////////
            ListnerThread.Start();

            MyConnection = new BluetoothConnection();


            MyConnection.GetAdapter();

            MyConnection.BAdapter.StartDiscovery();

            try
            {

                MyConnection.GetDevice(DeviceName);
                MyConnection.BDevice.SetPairingConfirmation(false);
                //   myConnection.thisDevice.Dispose();
                MyConnection.BDevice.SetPairingConfirmation(true);
                MyConnection.BDevice.CreateBond();


            }
            catch 
            {
                //test
            }

            MyConnection.BAdapter.CancelDiscovery();

            Socket = MyConnection.BDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

            MyConnection.BSocket = Socket;

            System.Threading.Thread.Sleep(500);
            try
            {



                MyConnection.BSocket.Connect();


                if (ListnerThread.IsAlive == false)
                {
                    ListnerThread.Start();
                }
                //else
                //{
                //    ListnerThread.Abort();
                //}

            }
            catch 
            {

            }
        }
        public void Disconnect()
        {
            try
            {


                ListnerThread.Dispose();//stop

                MyConnection.BDevice.Dispose();

                MyConnection.BSocket.OutputStream.WriteByte(187);
                MyConnection.BSocket.OutputStream.Close();

                MyConnection.BSocket.Close();

                MyConnection = new BluetoothConnection();
                Socket = null;


            }
            catch { }
        }
        public void WriteInSerial(string message)
        {
            var msg = System.Text.Encoding.UTF8.GetBytes(message);
            MyConnection.BSocket.OutputStream.Write(msg, 0, msg.Length);
            MyConnection.BSocket.OutputStream.Close();
        }

        public MyBluetooth(string deviceName,int listnerSleepDuration=10, int bufferReaderSize=20)
        {
            ListnerSleepDuration = listnerSleepDuration;
            BufferReaderSize = bufferReaderSize;
            DeviceName = deviceName;
            ListnerThread = new Thread(Listener);
            Connect();
        
        }

   
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}