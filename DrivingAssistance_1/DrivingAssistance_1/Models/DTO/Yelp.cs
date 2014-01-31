using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class Yelp
    {
        

        public class Rootobject
        {
            public Message message { get; set; }
            public Business[] businesses { get; set; }
        }

        public class Message
        {
            public string text { get; set; }
            public int code { get; set; }
            public string version { get; set; }
        }

        public class Business
        {
            
            public string rating_img_url { get; set; }
            public string country_code { get; set; }
            public string id { get; set; }
            public bool is_closed { get; set; }
            public string city { get; set; }
            public string mobile_url { get; set; }
            public int review_count { get; set; }
            public string zip { get; set; }
            public string state { get; set; }
            public string rating_img_url_small { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string address3 { get; set; }
            public string phone { get; set; }
            public string state_code { get; set; }
            public Category[] categories { get; set; }
            public string photo_url { get; set; }
            public float distance { get; set; }
            public string name { get; set; }
            public Neighborhood[] neighborhoods { get; set; }
            public string url { get; set; }
            public string country { get; set; }
            public float avg_rating { get; set; }
            public string nearby_url { get; set; }
            public Review[] reviews { get; set; }
            public string photo_url_small { get; set; }
        }

        public class Category
        {
            public string category_filter { get; set; }
            public string search_url { get; set; }
            public string name { get; set; }
        }

        public class Neighborhood
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class Review
        {
            public string rating_img_url_small { get; set; }
            public string user_photo_url_small { get; set; }
            public string rating_img_url { get; set; }
            public int rating { get; set; }
            public string user_url { get; set; }
            public string url { get; set; }
            public string mobile_uri { get; set; }
            public string text_excerpt { get; set; }
            public string user_photo_url { get; set; }
            public string date { get; set; }
            public string user_name { get; set; }
            public string id { get; set; }
        }
    }
}