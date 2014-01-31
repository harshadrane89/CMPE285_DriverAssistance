using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class CompositeModel
    {
        public Search searchObj { get; set; }
        public List<Yelp.Business> bList { get; set; }
        public List<GoogleMap> serviceList { get; set; }
        public Yelp.Business refBusiness { get; set; }
        public String directionsScript { get; set; }
        public String servicePointers { get; set; }
        public String basePointer { get; set; }
        public String trafficInfo { get; set; }
        public String error { get; set; }
        public String infoWindow { get; set; }
    }
}