using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingAssistance_1.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.IO.Ports;
using DrivingAssistance_1.Models.DTO;

namespace DrivingAssistance_1.Controllers
{
    public class HomeController : Controller, JSM.IJavaScriptModelAware 
    {
        //
        // GET: /Home/

        bool authenticated = false;
        public ActionResult Login()
        {
            if(authenticated)
            {
                return View("Dashboard", (CompositeModel)Session["compositeModel"]);
            }
            else
            {
                return View("Login", new Login());
            }
            
        }

        [HttpPost]
        public ActionResult Login(Login login )
        {
            login.UserName = Request["uName"].ToString();
            login.Password = Request["uPasswd"].ToString();
            User newUser = LoginModel.loginUser(login.UserName, login.Password);

            if (newUser.uId != 0)
            {
                authenticated = true; 
                Session.Add("userInfo", newUser);
                Session.Add("compositeModel", new CompositeModel());
                return View("Dashboard", new CompositeModel());
                
            }
            else
            {
                login.error = "Invalid Login Credentials";
                return View("Login",login);

            }
        }

        [HttpPost]
        public ActionResult Dashboard(CompositeModel model)
        {
            if (model.searchObj.startLocation == "" || model.searchObj.startLocation == null)
            {
                model.error = "Please Provide Start Location";
                return View("Dashboard", model);
            }
            else
            {
                model.error = "";
            }
            if (model.searchObj.endLocation == "" || model.searchObj.endLocation == null)
            {
                model.error = "Please Provide End Location";
                return View("Dashboard", model);
            }
            else
            {
                model.error = "";
            }
            SearchProcessor refObj = new SearchProcessor();
            model = refObj.searchInfo(model.searchObj);
            
            Session["compositeModel"]=model;
            return View("Dashboard",model);
            
        }
 
        [HttpGet]
        public ActionResult SignUp()
        {
            return View("SignUp",new UserInfo());
        }

        [HttpPost]
        public ActionResult SignUp(UserInfo userInfo)
        {
            SignUp signUp=new SignUp();
            userInfo = new UserInfo();
            userInfo.fName = Request["fName"].ToString();
            userInfo.lName = Request["lName"].ToString();
            userInfo.email = Request["email"].ToString();

            userInfo.password = Request["passwd"].ToString();
            if( !signUp.validateUser(userInfo.email))
            {
                userInfo.error="The Email is already under use";
                userInfo.email = "";
                return View("SignUp",userInfo);
            }
            else
            {
                 userInfo.error = "";
                   signUp.createUser(userInfo);
                   Session.Add("compositeModel", new CompositeModel());
                   return View("Dashboard",new CompositeModel());
            }
         
        }

        [HttpGet]
        public ActionResult Weather()
        {

            return View("Weather", new Weather());
        }

        [HttpGet]
        public ActionResult TrafficInfo()
        {

            return View("TrafficInformation", new TrafficDTO());
        }

        [HttpPost]
        public ActionResult Weather(String name)
        {
            Weather weather=new Weather();
            weather.location=Request["Location"].ToString();
            WeatherProcessor weatherProcessor = new WeatherProcessor();
            weather=weatherProcessor.getInfo(weather.location);
            
            return View("Weather", weather);
        }

        [HttpGet]
        public ActionResult Dashboard()
        {

            return View("Dashboard", (CompositeModel)Session["compositeModel"]);

        }


        [HttpPost]
        public ActionResult SearchInfo()
        {
            CompositeModel model1 = (CompositeModel)Session["compositeModel"];
            ServiceProcessor serviceProcess = new ServiceProcessor();
            Service service=serviceProcess.getServiceInfo(Request["name"].ToString(), model1);
            return View("Service", service);

        }

        [HttpPost]
        public ActionResult TrafficInformation()
        {
            TrafficInformation tInfo = new TrafficInformation();
            String sLocation=Request["tSource"].ToString();
            String eLocation=Request["tDestination"].ToString();
            TrafficDTO traffic = tInfo.getInfo(sLocation, eLocation);
            return View("TrafficInformation", traffic);

        }
        
    }
}
