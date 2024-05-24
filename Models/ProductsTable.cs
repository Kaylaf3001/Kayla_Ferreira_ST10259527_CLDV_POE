using Microsoft.AspNetCore.Mvc;
using POE_CloudDev.ViewModels;
using System.Data.SqlClient;

namespace POE_CloudDev.Models
{
    //-------------------------------------------------------------------------------------------------------------
    public class ProductsTable
    {
        public static string con_string = "Server=tcp:st10259527-server.database.windows.net,1433;Initial Catalog=st10259527-DB;Persist Security Info=False;User ID=Kaylaf97;Password=@Bran@16;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static SqlConnection con = new SqlConnection(con_string);

        public int ProductID { get; set; }
        public string Name { get; set; }

        public string Price { get; set; }

        public string Category { get; set; }

        public bool Availability { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        public int insert_product(ProductsTable p, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                string sql = "INSERT INTO PRODUCTS (Name, Price, Category, Description, Quantity, Availability, ImagePath) VALUES (@Name, @Price, @Category, @Description, @Quantity, @Availability, @ImagePath)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.Parameters.AddWithValue("@Category", p.Category);
                cmd.Parameters.AddWithValue("@Description", p.Description);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);

                // Check if the quantity is greater than 0 so that availability can be set to true or false.
                if (p.Quantity > 0)
                {
                    p.Availability = true;
                }
                else
                {
                    p.Availability = false;
                }
                cmd.Parameters.AddWithValue("@Availability", p.Availability);
                p.ImageUrl = SaveImageToFile(p.Image, webHostEnvironment);
                cmd.Parameters.AddWithValue("@ImagePath", p.ImageUrl);
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
        //-------------------------------------------------------------------------------------------------------------
        public string SaveImageToFile(IFormFile image, IWebHostEnvironment webHostEnvironment)
        {
            string uniqueFileName = null;

            if (image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/ImgObj");
                uniqueFileName = Guid.NewGuid().ToString();
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            string FullFilePath = "/Images/ImgObj/" + uniqueFileName;
            return FullFilePath;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<ProductsTable> GetAllProducts()
        {
            List<ProductsTable> products = new List<ProductsTable>();

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT * FROM PRODUCTS";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProductsTable product = new ProductsTable();
                    product.ProductID = Convert.ToInt32(rdr["ProductId"]);
                    product.Name = rdr["Name"].ToString();
                    product.Price = rdr["Price"].ToString();
                    product.Category = rdr["Category"].ToString();
                    product.Description = rdr["Description"].ToString();
                    product.Quantity = Convert.ToInt32(rdr["Quantity"]);
                    product.Availability = Convert.ToBoolean(rdr["Availability"]);
                    product.ImageUrl = rdr["ImagePath"].ToString();

                    products.Add(product);
                }
            }

            return products;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<ProductCardViewModel> SelectProducts()
        {
            List<ProductCardViewModel> products = new List<ProductCardViewModel>();

            string con_string = "Server=tcp:st10259527-server.database.windows.net,1433;Initial Catalog=st10259527-DB;Persist Security Info=False;User ID=Kaylaf97;Password=@Bran@16;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT ProductId, Name, Price, Description, Category, Availability, ImagePath FROM ProductsTable";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ProductCardViewModel product = new ProductCardViewModel();
                    product.ProductID = Convert.ToInt32(reader["ProductID"]);
                    product.Name = Convert.ToString(reader["Name"]);
                    product.Price = Convert.ToDecimal(reader["Price"]);
                    product.Category = Convert.ToString(reader["Category"]);
                    product.Description = Convert.ToString(reader["Description"]);
                    product.Availability = Convert.ToBoolean(reader["Availability"]);
                    product.ImagePath = Convert.ToString(reader["ImagePath"]);
                    products.Add(product);
                }
                reader.Close();
            }
            return products;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool PlaceOrder(int UserId, int ProductId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    // Start a new transaction
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        // Insert the new transaction
                        string sql = "INSERT INTO TRANSACTIONS (UserId, ProductId, PurchaseDateTime) VALUES (@UserId, @ProductId, @PurchaseDateTime)";
                        using (SqlCommand cmd = new SqlCommand(sql, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserId", UserId);
                            cmd.Parameters.AddWithValue("@ProductId", ProductId);
                            cmd.Parameters.AddWithValue("@PurchaseDateTime", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }

                        // Decrease the quantity of the product
                        sql = "UPDATE PRODUCTS SET Quantity = Quantity - 1 WHERE ProductId = @ProductId";
                        using (SqlCommand cmd = new SqlCommand(sql, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", ProductId);
                            cmd.ExecuteNonQuery();
                        }

                        // Update the availability of the product if the quantity is 0
                        sql = "UPDATE PRODUCTS SET Availability = CASE WHEN Quantity = 0 THEN 0 ELSE 1 END WHERE ProductId = @ProductId";
                        using (SqlCommand cmd = new SqlCommand(sql, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", ProductId);
                            cmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        // Rollback the transaction if any operation fails
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
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


