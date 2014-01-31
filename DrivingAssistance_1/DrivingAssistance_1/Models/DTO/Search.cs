using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class Search
    {
        public String searchCriteria { get; set; }

        [Required(ErrorMessage="Please Provide a Location")]
        public String startLocation { get; set; }
        public String endLocation { get; set; }
        public String readius { get; set; }
        public List<String> searchCriteriaList { get; set; }
    }
}