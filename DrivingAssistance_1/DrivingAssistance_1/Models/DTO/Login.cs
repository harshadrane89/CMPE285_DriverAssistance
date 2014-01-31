using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class Login
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String error { get; set; }
    }
}