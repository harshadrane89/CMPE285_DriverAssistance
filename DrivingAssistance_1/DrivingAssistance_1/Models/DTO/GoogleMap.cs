using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class GoogleMap
    {
        public String id { get; set; }
        public String elatitude { get; set; }
        public String elongitude { get; set; }
        public String latitude { get; set; }
        public String longitude { get; set; }
        public float[] bbox { get; set; }
        public String description;
    }
}