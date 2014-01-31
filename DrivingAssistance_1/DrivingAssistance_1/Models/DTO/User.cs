using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class User
    {

       public int uId { get; set; }
       public String fName { get; set; }
       public String lName { get; set; }
       public String uName { get; set; }
       public String uPass { get; set; }
       public List<Search> searchList { get; set; }
    }
}