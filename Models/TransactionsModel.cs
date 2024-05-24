using Microsoft.AspNetCore.Mvc;
using POE_CloudDev.ViewModels;
using System.Data.SqlClient;
using System.Transactions;


namespace POE_CloudDev.Models
{
    //-------------------------------------------------------------------------------------------------------------
    public class TransactionsModel
    {
        public  static string con_string = "Server=tcp:st10259527-server.database.windows.net,1433;Initial Catalog=st10259527-DB;Persist Security Info=False;User ID=Kaylaf97;Password=@Bran@16;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public int UserId { get; set; }
        public int ProductID { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        public static List<TransactionRecordViewModel> UserTransactions(int userId)
        {
            List<TransactionRecordViewModel> transactions = new List<TransactionRecordViewModel>();

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = @"
                    SELECT T.UserId, T.ProductID, U.UserFirstName, U.UserLastName, U.UserEmail, P.Name, P.ImagePath, P.Price
                    FROM TRANSACTIONS T
                    INNER JOIN PRODUCTS P ON T.ProductID = P.ProductID
                    INNER JOIN USERS U ON T.UserId = U.UserId
                    WHERE T.UserId = @userId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@userId", userId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TransactionRecordViewModel transaction = new TransactionRecordViewModel();
                    transaction.ProductID = Convert.ToInt32(reader["ProductID"]);
                    transaction.Name = Convert.ToString(reader["Name"]);
                    transaction.Price = Convert.ToDecimal(reader["Price"]);
                    transaction.ImagePath = Convert.ToString(reader["ImagePath"]);
                    transactions.Add(transaction);
                }
                reader.Close();
            }
            return transactions;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
          
 

