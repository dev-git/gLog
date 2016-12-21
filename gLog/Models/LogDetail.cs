using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gLog.Models
{
    public class LogDetail
    {
        public int ID { get; set; }
        public DateTime WhenCreated { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int BatteryLevel { get; set; }
        public string MACAddress { get; set; }
    }
}