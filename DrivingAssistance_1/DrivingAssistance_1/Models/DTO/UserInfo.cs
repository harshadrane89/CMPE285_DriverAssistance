using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class UserInfo
    {
        public String fName { get; set; }
        public String lName { get; set; }
        public String email { get; set; }
        public String password { get; set; }
        public String error { get; set; }

    }
}