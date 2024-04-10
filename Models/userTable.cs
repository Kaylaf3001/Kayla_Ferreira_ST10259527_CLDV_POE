using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace POE_CloudDev.Models
{
        public class userTable
        {

            //public static string con_string = "Server=tcp:clouddev-sql-server.database.windows.net,1433;Initial Catalog=CLDVDatabase;Persist Security Info=False;User ID=Byron;Password=RockeyM12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            public static string con_string = "Server=tcp:st10259527-server.database.windows.net,1433;Initial Catalog=st10259527-DB;Persist Security Info=False;User ID=Kaylaf97;Password=@Bran@16;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            public static SqlConnection con = new SqlConnection(con_string);

            public string Name { get; set; }

            public string Surname { get; set; }

            public string Email { get; set; }
            public string Password { get; set; }

            public int insert_User(userTable m)
            {

                try
                {
                    string sql = "INSERT INTO USERS (UserFirstName, UserLastName, UserEmail,UserPassword) VALUES (@Name, @Surname, @Email, @Password)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Name", m.Name);
                    cmd.Parameters.AddWithValue("@Surname", m.Surname);
                    cmd.Parameters.AddWithValue("@Email", m.Email);
                cmd.Parameters.AddWithValue("@Password", m.Password);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    // For now, rethrow the exception
                    throw ex;
                }


            }

        }
    }

