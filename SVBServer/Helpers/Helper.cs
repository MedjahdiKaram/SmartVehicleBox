using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SVBServer.Models;

namespace SVBServer.Helpers
{
    public static class Helper
    {
        public static void Decide(this MainActivity activity, string message, Dictionary<string, string> additional = null)
        {
            try
            {
                if (additional != null && (message.ToUpper().Contains("STOP")&& message.Contains(additional["password"])))
                {

                    activity.Bluetooth.WriteInSerial("stop");
                }
                else
                {
                    activity.GetAdress();
                    if (additional==null)
                    {
                        activity.Monitor.Text = "Empty dictionary";
                        return;
                    }
                    var mapsLink= @"http://maps.google.com/maps?q="+additional["latitude"]+","+additional["longitude"];
                    var messageToSend = additional["address"] + " Map: " + mapsLink;
                    if (message.ToUpper().Contains("SHOCK"))
                    {
                    
                          
                            activity.Monitor.Text += "\r\n ---> Sending SMS to "+additional["phone"]+" message "+ messageToSend;
                            activity.SendSms(additional["phone"],"Accident "+ messageToSend);
                            activity.SendSms(additional["phone2"],"Accident "+ messageToSend);
                            
                     
                    }
                    else if (message.ToUpper().StartsWith("#CONFIG#"))
                    {
                        
                        activity.ConfFile.Write(message);
                    }
                    else if (message.ToUpper().Contains("T OU") || message.ToUpper().Contains("T'ES OU") ||
                             message.ToUpper().Contains("WHERE") || message.ToUpper().Contains("FAYEN"))
                    {
                        activity.Monitor.Text += "\r\n ---> Sending SMS: " + additional["sender"] + " message:" +
                                                 messageToSend;
                        activity.SendSms(additional["sender"], messageToSend);
                    }
                }
                
            }
            catch (Exception e)
            {
                activity.Monitor.Text=("Can't make a decision: "+e.Message);
            }
        }



        public static ConfigSvb Parse(string message,char splitChar='/')
        {
            if (!message.ToUpper().StartsWith("#CONFIG#"))
                return new ConfigSvb {Password = "password", SosPhone1 = "+213552395895", SosPhone2 = "+213552395895"};
            var conf = message.Remove(0,8);
            var mainConf = new ConfigSvb
            {
                SosPhone1 = conf.Split(splitChar)[0],
                SosPhone2 = conf.Split(splitChar)[1],
                Password = conf.Split(splitChar)[2]
            };
            return mainConf;
        }
    }
}