using Microsoft.AspNetCore.Mvc;
using POE_CloudDev.Models;
using System.Data.SqlClient;

namespace POE_CloudDev.Controllers
{
    public class ProductController : Controller
    {
        //-------------------------------------------------------------------------------------------------------------
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProductController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        //-------------------------------------------------------------------------------------------------------------
        public ProductsTable ProductsTable = new ProductsTable();
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult MyWork()
        {
            List<ProductsTable> products = ProductsTable.GetAllProducts();
            var userID = HttpContext.Session.GetInt32("CurrentUserId");
            // Pass products and userID to the view
            ViewData["Products"] = products;
            ViewData["UserID"] = userID;
            return View();
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public IActionResult AddProduct(ProductsTable product)
        {
            var result = ProductsTable.insert_product(product, _hostingEnvironment);
            return RedirectToAction("AddProduct", "Product");
            
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        //-------------------------------------------------------------------------------------------------------------
        public ActionResult PlaceOrder(int ProductId)
        {
            int temp = ProductId;
            int UserId = HttpContext.Session.GetInt32("CurrentUserId") ?? 0;
            if (UserId == 0)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }
            try
            {
                bool isOrderPlaced = ProductsTable.PlaceOrder(UserId, ProductId);
                return Json(new { success = isOrderPlaced });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
