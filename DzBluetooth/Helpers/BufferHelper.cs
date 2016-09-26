using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DzBluetooth.Helpers
{
    public static class BufferHelper
    {
        public static bool IsBufferEmty(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return true;

            if (buffer.Any(byteentity => byteentity != 0))
            {
                return false;
            }
            return true;
        }
    }
}