using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class TrafficDTO
    {
        public String source { get; set; }
        public String destination { get; set; }
        public String trafficInfo { get; set; }
        public String route { get; set; }
        public String error { get; set; }
        public String dScript { get; set; }
        public List<Traffic.Resource> trafficList { get; set; }
    }
}