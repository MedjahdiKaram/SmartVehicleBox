using System;
using System.IO;
using Android.Util;
using Environment = Android.OS.Environment;

namespace SVBClient.Helpers
{
    public class FileHelper
    {
        public static string Path { get; set; }
        public static string Filename { get; set; }
     

        public void Write(string someString)
        {

            try
            {
                File.WriteAllText(Filename,someString);
            }
            catch (Exception e)
            {
                Log.Debug("Write error: ", e.Message);
            }

        }

        public string Read()
        {
            try
            {
                string content;
                using (var streamReader = new StreamReader(Filename))
                {
                    content = streamReader.ReadToEnd();
                    Log.Debug("--------> Read gallek: ",content);
                }
                return content;
            }
            catch (Exception e)
            {
                Log.Debug("Read erro: ", e.Message);
                return "0549597224";
            }
        }

        public FileHelper(string fileName="ConfigClient.txt")
        {
            try
            {
                Path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).Path;

                Filename = System.IO.Path.Combine(Path, fileName);

            }
            catch (Exception e)
            {
                Log.Debug("FH ctor:", e.Message);
            }
      
        }
            
    }
}