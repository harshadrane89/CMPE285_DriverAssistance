using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DrivingAssistance_1.Models
{
    public class SignUp
    {

        public bool validateUser(String email)
        {
            SqlConnection conn = null;
            SqlCommand sqlCmd = null;
            SqlDataReader rs = null;
            bool result = true;
            try { 
            
             conn = new SqlConnection("Data Source=HARSHAD\\SQLEXPRESS;Initial Catalog=TRAVELLERS_ASSISTANCE;Integrated Security=True");
            String sql = string.Format("SELECT * FROM [USER_MASTER] WHERE Username= '{0}' ", email);
             sqlCmd = new SqlCommand(sql, conn);
            conn.Open();
             rs = sqlCmd.ExecuteReader();
           if(rs.HasRows)
           {
               result = false; 
           }
           conn.Close();
                sqlCmd.Dispose();
           rs.Close();
                }
            catch(Exception ex)
            {
                Console.WriteLine( ex.StackTrace);
            }
            finally
            {
                if(conn!=null)
                {
                    conn.Close();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
                if (rs != null)
                {
                    rs.Close();
                }
            }
           return result;
        }

        public String createUser(DTO.UserInfo userInfo)
        {
            SqlConnection conn = null;
            SqlCommand sqlCmd = null;
            SqlDataReader rs = null;
            String result = "";
            try
            {
                
                conn = new SqlConnection("Data Source=HARSHAD\\SQLEXPRESS;Initial Catalog=TRAVELLERS_ASSISTANCE;Integrated Security=True");
                String sql = string.Format("INSERT INTO [USER_MASTER] (Firstname,Lastname,Username,Password)VALUES('{0}','{1}','{2}','{3}')  ", userInfo.fName,userInfo.lName,userInfo.email,userInfo.password);
                sqlCmd = new SqlCommand(sql, conn);
                conn.Open();
                sqlCmd.ExecuteNonQuery();
                
                conn.Close();
                sqlCmd.Dispose();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
                if (rs != null)
                {
                    rs.Close();
                }
                result = userInfo.fName + userInfo.lName;
            }
            return result;

        }
    }
}