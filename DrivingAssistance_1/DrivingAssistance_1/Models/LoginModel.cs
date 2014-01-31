using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.SqlClient;
using DrivingAssistance_1.Models.DTO;
namespace DrivingAssistance_1.Models
{
    public class LoginModel
    {
       private static List<Search> getSearchInfo(String uId)
        {
            List<Search> searchList = new List<Search>();
            Search tempObj = null;
            SqlConnection conn = new SqlConnection("Data Source=HARSHAD\\SQLEXPRESS;Initial Catalog=TRAVELLERS_ASSISTANCE;Integrated Security=True");
            String sql = string.Format("SELECT * FROM [SEARCH_HISTORY] WHERE Userid= {0}", uId);
            SqlCommand sqlCmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader rs = sqlCmd.ExecuteReader();
            while (rs.Read())
            {
                tempObj = new Search();
                
                tempObj.searchCriteria = (string)rs.GetValue(1);
                tempObj.startLocation= (string)rs.GetValue(2);
                tempObj.readius = (string)rs.GetValue(3);
                searchList.Add(tempObj);
            }


            return searchList;
        }


          public static User loginUser(string uName,string uPass)
        {
            User userObj=new User();
            SqlConnection conn= new SqlConnection("Data Source=HARSHAD\\SQLEXPRESS;Initial Catalog=TRAVELLERS_ASSISTANCE;Integrated Security=True");
            String sql=string.Format("SELECT * FROM [USER_MASTER] WHERE Username= '{0}' AND PASSWORD = '{1}'",uName,uPass);
            SqlCommand sqlCmd=new SqlCommand(sql,conn);
            conn.Open();
            SqlDataReader rs=sqlCmd.ExecuteReader();
            while(rs.Read())
            {
                userObj.uId= (int)rs.GetValue(0);
                userObj.uName =(string) rs.GetValue(1);
                userObj.uPass = (string)rs.GetValue(2);
                userObj.fName = (string)rs.GetValue(3);
                userObj.lName = (string)rs.GetValue(4);
            }



            userObj.searchList = getSearchInfo(userObj.uId+"");
            return userObj;

        }

         
    }

   
}
