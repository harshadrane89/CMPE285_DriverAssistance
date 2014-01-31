using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models.DTO
{
    public class Traffic
    {

        
public class Rootobject
{
public string authenticationResultCode { get; set; }
public string brandLogoUri { get; set; }
public string copyright { get; set; }
public Resourceset[] resourceSets { get; set; }
public int statusCode { get; set; }
public string statusDescription { get; set; }
public string traceId { get; set; }
}

public class Resourceset
{
public int estimatedTotal { get; set; }
public Resource[] resources { get; set; }
}

public class Resource
{
public string __type { get; set; }
public Point point { get; set; }
public string description { get; set; }
public DateTime end { get; set; }
public int incidentId { get; set; }
public DateTime lastModified { get; set; }
public bool roadClosed { get; set; }
public String mySeverity { get; set; }
public int severity { get; set; }
public DateTime start { get; set; }
public Topoint toPoint { get; set; }
public int type { get; set; }
public bool verified { get; set; }
public string lane { get; set; }
}

public class Point
{
public string type { get; set; }
public float[] coordinates { get; set; }
}

public class Topoint
{
public string type { get; set; }
public float[] coordinates { get; set; }
}

    }
}