using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace POE_CloudDev.Models
{
    public class LoginModel
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string con_string = "Server=tcp:st10259527-server.database.windows.net,1433;Initial Catalog=st10259527-DB;Persist Security Info=False;User ID=Kaylaf97;Password=@Bran@16;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //-------------------------------------------------------------------------------------------------------------
        public int SelectUser(string email, string password)
        {
            int userId = -1; // Default value if user is not found
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT UserID FROM USERS WHERE UserEmail = @Email AND UserPassword = @Password";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                
                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    // For now, rethrow the exception
                    throw ex;
                }
            }
            return userId;
        }
        //-------------------------------------------------------------------------------------------------------------

    }
}

