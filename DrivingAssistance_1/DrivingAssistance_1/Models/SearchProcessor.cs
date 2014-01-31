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
    public class SearchProcessor
    {
        public String markerLink;
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
                output.error += location+"LOCATION LOCATION LOCATION";
               output.error += ex.StackTrace.ToString();

            }
            finally
            {

                
            }
            return pointer;
        }

        public String getRoute(String startLocation,String endLocation)
        {
            GoogleMap google = getGeoCode(startLocation);
            String marker="";
            marker += " var myLatlng = new google.maps.LatLng(" + google.latitude + "," + google.longitude + ");";
            marker += "var mapOptions = { zoom:7 ,center: myLatlng }; var map= new google.maps.Map(document.getElementById(\"map\"),mapOptions);";
            marker += "directionsDisplay.setMap(map);";

            return marker;
        }

        public String getServiceMap(List<GoogleMap> refList)
        {
            String markers="";

            int counter=1;
            char mark = 'A';
            foreach(var google in refList)
            {
                if (google.latitude != "" || google.longitude != "")
                {
                    markers += " var myLatlng" + counter + " = new google.maps.LatLng(" + google.latitude + "," + google.longitude + ");";
                    markers += "var marker" + counter + " = new google.maps.Marker({ position: myLatlng" + counter + ", map: map,   icon :'http://www.google.com/mapfiles/marker" + mark + ".png'});";
                    mark++;
                    counter++;
                }
            }
            return markers;
        }

        public String getServiceInfoWindow(List<GoogleMap> refList)
        {
            String markers = "";

            int counter = 1;
            
            foreach (var google in refList)
            {
                if (google.latitude != "" || google.longitude != "")
                {
                    markers += "google.maps.event.addListener(marker" + counter + ", 'click', function() { new google.maps.InfoWindow({content:'" + google.description + "'}).open(map,marker" + counter + ");});";
                    counter++;
                }
            }
            return markers;
        }




        public string getTrafficInfo(String location)
        {
                        
            List<GoogleMap> trafficList = new List<GoogleMap>();
            GoogleMap gMap= getGeoCode(location);
            String url = "http://dev.virtualearth.net/REST/v1/Traffic/Incidents/" +gMap.bbox[0]+","+gMap.bbox[1]+","+gMap.bbox[2]+","+gMap.bbox[3]+ "?o=json&key=AoAtLFi1vWDNZpOvpHF-2-S71qWRTg1s_vu5LHGFGrw_v0jA4b0rqmHuuj0cF-8Y";
             Traffic.Rootobject referenceObbj= null;
            try
            {

                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
              

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();

                 referenceObbj = JsonConvert.DeserializeObject<Traffic.Rootobject>(response);
               
                GoogleMap google = null;
                foreach(Traffic.Resource resource in referenceObbj.resourceSets[0].resources)
                {
                    google = new GoogleMap();
                    google.latitude = resource.point.coordinates[0]+"";
                    google.longitude = resource.point.coordinates[1] +"";
                    google.description = resource.description.Replace("'", "");;
                    if(resource.severity==1)
                    {
                        google.description += "\\n Severity: Low Impact";
                    }
                    else if(resource.severity==2)
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
                    else if (resource.type ==8)
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


                    if(resource.roadClosed)
                    {
                        google.description += "\\n Road Closed";
                    }
                    trafficList.Add(google);
                }
            }
            catch(Exception ex)
            {
                //url= ex.Source.ToString();
                //url += ex.StackTrace.ToString();
               // output.error += location;
              //  output.error += ex.Message.ToString();
               //output.error += ex.Data.ToString();
              // output.error += ex.StackTrace.ToString();
            }

          
            return getTrafficMap(trafficList);
        }

        public String searchString(String search)
        {
            String result=""    ;
            if(search.Equals("Gas Station"))
            {
                result = "servicestations";
            }
             else if(search.Equals("Towing"))
            {
                result = "towing";
            }
              else if(search.Equals("Car Wash"))
            {
                result = "carwash";
            }
              else if(search.Equals("Wheel & Rim Repair"))
            {
                result = "wheelrimrepair";
            }
              else if(search.Equals("Car Servicing"))
            {
                result = "autorepair";

            }
            return result;
        }

        public String getTrafficMap(List<GoogleMap> googleList)
        {
            String markers = "";
            int counter = 1;
            
            
            foreach (var google in googleList)
            {
                
                markers += " var traffic" + counter + " = new google.maps.LatLng(" + google.latitude + "," + google.longitude + ");";
                markers += "var trafficmarker" + counter + " = new google.maps.Marker({ position: traffic" + counter + ", map: map,  icon : '/Images/Construction.png' , title : '"+google.description+"'});";
            //    markers += "var infoWindow"+counter+" =new google.map.InfoWindow({ content : '" + google.description + "' });";
              //  markers += "google.maps.event.addListner(marker" + counter + ",'click',function(){infoWindow" + counter + ".open(map,marker" + counter + ")});";
                counter++;
            }
          //  output.error += markers;
            return markers;
        }


        CompositeModel output;


        public CompositeModel searchInfo(Search searchObj)
        {
            output = new CompositeModel();
                
            String searchCriteria=searchObj.searchCriteria;
            String startLocation=searchObj.startLocation.Replace(" ","%20");
            String endLocation = searchObj.endLocation.Replace(" ", "%20");
            String radius=searchObj.readius;
            output.searchObj = searchObj;
            output.trafficInfo = getTrafficInfo(startLocation);
            output.trafficInfo += getTrafficInfo(endLocation);
            GoogleDirections.TDirections directionsObj= getDirectionsToDestination(startLocation, endLocation);

            
            List<GoogleMap> pointerList = getServicePointsAlongRoute(directionsObj);
            List<Yelp.Business> businessList = getServices(pointerList,searchCriteria,radius);
           
            //output.error += directionsObj.routes[0].legs[0].steps.Length + "  " + pointerList.Count + "  " + businessList.Count;
            List<GoogleMap> serviceList = getGoogleMapServicePointers(businessList);
            output.serviceList = serviceList;
            output.bList = businessList;
            output.basePointer = getRoute(startLocation,endLocation);
            output.directionsScript = getDirectionsScript(searchObj.startLocation,searchObj.endLocation );
            output.servicePointers = getServiceMap(serviceList);
            output.infoWindow = getServiceInfoWindow(serviceList);
           
            return output;
        }

       public String getDirectionsScript(String startLocation,String endLocation)
        {
            String script = "";
           script+="function calcRoute() { var start = '"+startLocation+"';var end = '"+endLocation+"'; var request = { origin:start,  destination:end,  travelMode: google.maps.TravelMode.DRIVING };";
           script += "directionsService.route(request, function(response, status) {  if (status == google.maps.DirectionsStatus.OK) {     directionsDisplay.setDirections(response);   }  });}";
            return script;
        }

       List<GoogleMap> getGoogleMapServicePointers(List<Yelp.Business> bList)
       {
           String description = "";
           List<GoogleMap> gPlotList = new List<GoogleMap>();
           GoogleMap google = null;
            foreach(Yelp.Business business in bList)    
               {
                    google = new GoogleMap();
                    description = "";
                    description += "" + business.name.Replace("'", "") + "";
                    description += "\\n " + business.address1.Replace("'", "");
                    description += "\\n " + business.address2.Replace("'", ""); ;
                    description += "\\n " + business.city.Replace("'", ""); ;
                    description += "\\n Rating: " + business.avg_rating;
                    description += "\\n Phone " + business.phone.Replace("'", ""); ;
                    
                String locationInfo="";
                
                if((business.address1!=""||business.address1!=null))
                {
                    locationInfo = business.address1.Replace(" ", "%20");
                    locationInfo+=",";
                }

                if (business.address2 != "" || business.address2 != null)
                {
                    locationInfo += business.address2.Replace(" ", "%20");
                    locationInfo += ",";
                }

                if (business.address3 != "" || business.address3 != null)
                {
                    locationInfo += business.address3.Replace(" ", "%20");
                    locationInfo += ",";
                }

                if (business.city != "" || business.city != null)
                {
                    locationInfo += business.city.Replace(" ", "%20");
                    locationInfo += ",";
                }

                if (business.state != "" || business.state != null)
                {
                    locationInfo += business.state.Replace(" ", "%20");
                    locationInfo += ",";
                }

                if (business.country != "" || business.country != null)
                {
                    locationInfo += business.country.Replace(" ", "%20");
                }
                locationInfo = locationInfo.Replace(" ", "%20");
                locationInfo = locationInfo.Replace(",,", ",");
                
                     google = getGeoCode(locationInfo);
                     google.description = description;
                     google.id = business.id;
                     gPlotList.Add(google);

                    
                }
           return gPlotList;
       }

        public List<Yelp.Business> getServices(List<GoogleMap> pointerList,String searchCriteria,String radius)
        {

            String url = "";
            HttpWebRequest request = null;
            WebResponse webResponse=null;
            GoogleDirections.TDirections directionsObject=new GoogleDirections.TDirections();
            Yelp.Rootobject rootObj=null;
            List<Yelp.Business> bList=new  List<Yelp.Business> ();
            try
            {

                foreach(GoogleMap pointer in pointerList)
                {
                    url = "http://api.yelp.com/business_review_search?term=" + searchCriteria.Replace(" ","%20") + "&tl_lat=" + pointer.latitude + "&tl_long=" + pointer.longitude + "&br_lat=" + pointer.elatitude + "&br_long=" + pointer.elongitude + "&radius="+radius+"&limit=5&ywsid=8YyJlTMtiQw7Jkm99c5pcw";
              //  output.error += url;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                rootObj = JsonConvert.DeserializeObject<Yelp.Rootobject>(response);
                    foreach(Yelp.Business bObj in rootObj.businesses)
                    {
                        char[] address=bObj.address1.ToCharArray();
                        if (Char.IsDigit(address[0]))
                        {
                            bList.Add(bObj);
                        }
                    }
                        
                responseReader.Close();
                }
            }
            catch(Exception ex)
            {
                //output.error = ex.StackTrace.ToString();
            }

            return bList;
        }



        public List<GoogleMap> getServicePointsAlongRoute(GoogleDirections.TDirections directionsObj)
        {
            bool flag = false;
            int size = directionsObj.routes[0].legs[0].steps.Length;
            GoogleDirections.Step[] tLegs=directionsObj.routes[0].legs[0].steps;
            GoogleDirections.Step pointer = null;
            int incrementSize = size / 5;
            incrementSize = incrementSize-1;
            List<GoogleMap> pointerList = new List<GoogleMap>();
            GoogleMap gMap = null;
            for(int i=0;i<tLegs.Length;i++)
            {
                if(i==tLegs.Length-1)
                {
                    flag = true;
                }
                pointer = tLegs[i];
                gMap = new GoogleMap();
                gMap.latitude=pointer.start_location.lat+"";
                gMap.longitude=pointer.start_location.lng+"";

                gMap.elatitude = pointer.end_location.lat + "";
                gMap.elongitude = pointer.end_location.lng + "";


            
                pointerList.Add(gMap);
            }
            if(!flag)
            {
                gMap = new GoogleMap();
                gMap.latitude = tLegs[tLegs.Length-1].end_location.lat + "";
                gMap.longitude = tLegs[tLegs.Length - 1].end_location.lng + "";
                
                pointerList.Add(gMap);
            }
            return pointerList;

        }


        public GoogleDirections.TDirections getDirectionsToDestination(String startLocation, String endLocation)
        {
            String url = "";
            HttpWebRequest request = null;
            WebResponse webResponse=null;
            GoogleDirections.TDirections directionsObject=new GoogleDirections.TDirections();
            try
            {
                url = "http://maps.googleapis.com/maps/api/directions/json?origin=" + startLocation + "&destination=" + endLocation + "&sensor=false";
                request = (HttpWebRequest)WebRequest.Create(url);
                webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                directionsObject = JsonConvert.DeserializeObject<GoogleDirections.TDirections>(response);
                responseReader.Close();
               // output.error += directionsObject.routes[0].legs[0].start_location.lat;
               // output.error += directionsObject.routes[0].legs[0].start_location.lng;
                //output.error += directionsObject.routes[0].legs[0].end_location.lat;
                //output.error += directionsObject.routes[0].legs[0].end_location.lng;
            }
            catch(Exception ex)
            {
               //    output.error = ex.StackTrace.ToString();
            }

            return directionsObject;
            
            
        }
      
    }
}