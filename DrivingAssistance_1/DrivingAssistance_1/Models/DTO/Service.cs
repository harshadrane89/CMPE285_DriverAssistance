using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class Service
    {

        public Yelp.Business businessObj {get; set;}
        public GoogleMap googleMap { get; set; }
        public String directions { get; set; }
        
    }
}