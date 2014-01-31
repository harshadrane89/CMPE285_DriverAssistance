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
    public class WeatherProcessor
    {
        Weather weather;

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
                weather.error += location + "LOCATION LOCATION LOCATION";
                weather.error += ex.StackTrace.ToString();

            }
            finally
            {


            }
            return pointer;
        }

        public String getJScript(GoogleMap marker)
        {
            
            String result = "";
            if(marker.latitude!=""||marker.latitude!=null)
            {
                result += "var myLatlng = new google.maps.LatLng(" + marker.latitude + "," + marker.longitude + ");";
                
            }
            return result;
        }


        public Weather getInfo(String location)
        {
            weather = new Weather();
            GoogleMap marker = getGeoCode(location);
           weather.jScript=getJScript(marker);
           weather.location = location;
           return weather;
        }

    }
}