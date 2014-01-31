using DrivingAssistance_1.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace DrivingAssistance_1.Models
{
    public class ServiceProcessor
    {

        

        public Service getServiceInfo(String id,CompositeModel refObject)
        {
            Service service = new Service();
            foreach(Yelp.Business business in refObject.bList)
            {
                if(id==business.id)
                {
                    service.businessObj = business;
                    break;
                }
            }

            foreach(GoogleMap gMap in refObject.serviceList)
            {
                if(id==gMap.id)
                {
                    service.googleMap = gMap;
                    break;
                }
            }

            String location="";
            
            location=service.businessObj.address1;
            location+=","+service.businessObj.address2;
            location += "," + service.businessObj.address3;
            location += "," + service.businessObj.city;
            location += "," + service.businessObj.country;
            location = location.Replace(",,", ",");
            service.directions=getDirectionsScript( refObject.searchObj.startLocation,location);
            return service;
        }

        public String getDirectionsScript(String startLocation, String endLocation)
        {
            String script = "";
            script += "function calcRoute() { var start = '" + startLocation + "';var end = '" + endLocation + "'; var request = { origin:start,  destination:end,  travelMode: google.maps.TravelMode.DRIVING };";
            script += "directionsService.route(request, function(response, status) {  if (status == google.maps.DirectionsStatus.OK) {     directionsDisplay.setDirections(response);   }  });}";
            return script;
        }

    }
}