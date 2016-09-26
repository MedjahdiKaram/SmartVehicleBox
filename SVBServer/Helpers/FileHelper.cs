using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;

using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Environment = Android.OS.Environment;

namespace SVBServer.Helpers
{
    public class FileHelper
    {
        public static string Path { get; set; }
        public static string Filename { get; set; }
     

        public void Write(string someString)
        {
       
            File.WriteAllText(Filename,someString);

        }

        public string Read()
        {
            string content;
            using (var streamReader = new StreamReader(Filename))
            {
                 content = streamReader.ReadToEnd();
                Log.Debug("--------> Read gallek: ",content);
            }
            return content;
        }

        public FileHelper(string fileName="config.txt")
        {
            Path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path;

            Filename = System.IO.Path.Combine(Path, fileName);

          
      
        }
            
    }
}