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
    public class TrafficInformation
    {
        public GoogleMap getGeoCode(String location)
        {
            GoogleMap pointer = new GoogleMap();
            try
            {

                String searchURL = "http://dev.virtualearth.net/REST/v1/Locations/" + location + "?o=json&key=AoAtLFi1vWDNZpOvpHF-2-S71qWRTg1s_vu5LHGFGrw_v0jA4b0rqmHuuj0cF-8Y";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(searchURL);

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();

                Geocoding.Rootobject referenceObbj = JsonConvert.DeserializeObject<Geocoding.Rootobject>(response);

                if (referenceObbj.resourceSets[0].estimatedTotal != 0)
                {
                    pointer.latitude = referenceObbj.resourceSets[0].resources[0].point.coordinates[0] + "";
                    pointer.longitude = referenceObbj.resourceSets[0].resources[0].point.coordinates[1] + "";
                    pointer.bbox = referenceObbj.resourceSets[0].resources[0].bbox;
                }
            }
            catch (Exception ex)
            {
                trafficInfo.error += location;
                trafficInfo.error += ex.StackTrace.ToString();

            }
            finally
            {


            }
            return pointer;
        }
        public String getRoute(String startLocation, String endLocation)
        {
            GoogleMap google = getGeoCode(startLocation);
            String marker = "";
            marker += " var myLatlng = new google.maps.LatLng(" + google.latitude + "," + google.longitude + ");";
            marker += "var mapOptions = { zoom:7 ,center: myLatlng }; var map= new google.maps.Map(document.getElementById(\"map\"),mapOptions);";
            marker += "directionsDisplay.setMap(map);";

            return marker;
        }

        public List<Traffic.Resource> getTrafficInfo(String location)
        {

            
            List<Traffic.Resource> traffic=new List<Traffic.Resource>();
            GoogleMap gMap = getGeoCode(location);
            String url = "http://dev.virtualearth.net/REST/v1/Traffic/Incidents/" + gMap.bbox[0] + "," + gMap.bbox[1] + "," + gMap.bbox[2] + "," + gMap.bbox[3] + "?o=json&key=AoAtLFi1vWDNZpOvpHF-2-S71qWRTg1s_vu5LHGFGrw_v0jA4b0rqmHuuj0cF-8Y";
            Traffic.Rootobject referenceObbj = null;
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();

                referenceObbj = JsonConvert.DeserializeObject<Traffic.Rootobject>(response);

                GoogleMap google = null;
                foreach (Traffic.Resource resource in referenceObbj.resourceSets[0].resources)
                {
                    traffic.Add(resource);

                    google = new GoogleMap();
                    google.latitude = resource.point.coordinates[0] + "";
                    google.longitude = resource.point.coordinates[1] + "";
                    google.description = resource.description.Replace("'", ""); ;
                    if (resource.severity == 1)
                    {
                        google.description += "\\n Severity: Low Impact";
                    }
                    else if (resource.severity == 2)
                    {
                        google.description += "\\n Severity:Minor";
                    }
                    else if (resource.severity == 3)
                    {
                        google.description += "\\n Severity: Moderate";
                    }
                    else if (resource.severity == 4)
                    {
                        google.description += "\\n Severity: Serious";
                    }

                    if (resource.type == 1)
                    {
                        google.description += "\\n Type: Accident";
                    }
                    else if (resource.type == 2)
                    {
                        google.description += "\\n Type: Congestion";
                    }
                    else if (resource.type == 3)
                    {
                        google.description += "\\n Type: Disabled Vehicle";
                    }
                    else if (resource.type == 4)
                    {
                        google.description += "\\n Type: Mass Transit";
                    }
                    else if (resource.type == 5)
                    {
                        google.description += "\\n Type: Miscellaneous";
                    }
                    else if (resource.type == 6)
                    {
                        google.description += "\\n Type: Other News";
                    }
                    else if (resource.type == 7)
                    {
                        google.description += "\\n Type: Planned Event";
                    }
                    else if (resource.type == 8)
                    {
                        google.description += "\\n Type: Road Hazard";
                    }
                    else if (resource.type == 9)
                    {
                        google.description += "\\n Type: Construction";
                    }
                    else if (resource.type == 10)
                    {
                        google.description += "\\n Type: Alert";
                    }
                    else if (resource.type == 11)
                    {
                        google.description += "\\n Type: Weather";
                    }


                    if (resource.roadClosed)
                    {
                        google.description += "\\n Road Closed";
                    }
                    gPointers.Add(google);
                }
            }
            catch (Exception ex)
            {
                //url= ex.Source.ToString();
                //url += ex.StackTrace.ToString();
                // output.error += location;
                //  output.error += ex.Message.ToString();
                //output.error += ex.Data.ToString();
                // output.error += ex.StackTrace.ToString();
            }


            return traffic;
        }

        public String getTrafficMap()
        {
            String markers = "";
            int counter = 1;


            foreach (var google in gPointers)
            {

                markers += " var traffic" + counter + " = new google.maps.LatLng(" + google.latitude + "," + google.longitude + ");";
                markers += "var marker" + counter + " = new google.maps.Marker({ position: traffic" + counter + ", map: map,  icon : '/Images/Construction.png' , title : '" + google.description + "'});";
                //    markers += "var infoWindow"+counter+" =new google.map.InfoWindow({ content : '" + google.description + "' });";
                //  markers += "google.maps.event.addListner(marker" + counter + ",'click',function(){infoWindow" + counter + ".open(map,marker" + counter + ")});";
                counter++;
            }
            //  output.error += markers;
            return markers;
        }

        public String getDirectionsScript(String startLocation, String endLocation)
        {
            String script = "";
            script += "function calcRoute() { var start = '" + startLocation + "';var end = '" + endLocation + "'; var request = { origin:start,  destination:end,  travelMode: google.maps.TravelMode.DRIVING };";
            script += "directionsService.route(request, function(response, status) {  if (status == google.maps.DirectionsStatus.OK) {     directionsDisplay.setDirections(response);   }  });}";
            return script;
        }


        TrafficDTO trafficInfo;
        List<GoogleMap> gPointers;
        public TrafficDTO getInfo(String startLocation,String endLocation)
        {
            trafficInfo = new TrafficDTO();
            gPointers = new List<GoogleMap>();
            trafficInfo.source = startLocation;
            trafficInfo.destination = endLocation;
            trafficInfo.trafficList = new List<Traffic.Resource>();
            List<Traffic.Resource> temp = getTrafficInfo(startLocation);
            if (temp.Count > 0)
            {
                foreach (Traffic.Resource resource in temp)
                {
                    trafficInfo.trafficList.Add(resource);
                }
            }
            temp = getTrafficInfo(endLocation);
            if (temp.Count > 0)
            {
                foreach (Traffic.Resource resource in temp)
                {
                    trafficInfo.trafficList.Add(resource);
                }
            }
             trafficInfo.trafficInfo = getTrafficMap();
             trafficInfo.route = getRoute(startLocation, endLocation);
             trafficInfo.dScript = getDirectionsScript(startLocation, endLocation);
            return trafficInfo;
        }
    }
}