using Microsoft.AspNetCore.Mvc;
using POE_CloudDev.Models;
using POE_CloudDev.ViewModels;

namespace POE_CloudDev.Controllers
{
    //-------------------------------------------------------------------------------------------------------------
    public class UserController : Controller
    {
        public userTable usrtbl = new userTable();
        public LoginModel loginModel = new LoginModel();

        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult SignUp(userTable Users)
        {
            var result = usrtbl.insert_User(Users);
            return RedirectToAction("MyWork", "Product");
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult SignUp()
        {
            return View(usrtbl);
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult login(string email, string password)
        {       
            int userID = loginModel.SelectUser(email, password);
            if (userID != -1)
            {
                HttpContext.Session.SetInt32("CurrentUserId", userID);
                return RedirectToAction("MyWork", "Product", new { userID = userID });
            }
            else
            {
                // User not found, handle accordingly (e.g., show error message)
                return View("LoginFailed");
                
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult login()
        {
            return View(loginModel) ;
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult Profile(TransactionsModel transactions)
        {
            return View();
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Profile()
        {
            var userID = HttpContext.Session.GetInt32("CurrentUserId");
            if (userID == null)
            {
                return RedirectToAction("Login", "User");
            }
            
            List<TransactionRecordViewModel> transactions = TransactionsModel.UserTransactions((int)userID);
            UserProfileViewModel ProfileDetails = usrtbl.getUserDetails((int)userID);
            ViewData["ProfileDetails"] = ProfileDetails;
            ViewData["Transactions"] = transactions;
            
            return View();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}

