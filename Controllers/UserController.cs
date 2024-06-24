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
                ViewData["LoginResult"] = 2;
                HttpContext.Session.SetInt32("CurrentUserId", userID);
                return RedirectToAction("MyWork", "Product", new { userID = userID });
            }
            else
            {
                // User not found, handle accordingly (e.g., show error message)
                ViewData["LoginResult"] = 1;
                return View();
                
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult login()
        {
            ViewData["LoginResult"] = 0;
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

            return View(userID); // Pass userID as the model
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult UpdateProfile(int UserId, string Name, string Surname, string Email)
        {
            userTable user = new userTable
            {
                Name = Name,
                Surname = Surname,
                Email = Email
            };
            var result = usrtbl.update_UserDetails(UserId, user);
            if (result > 0)
            {
                // Update successful
                return RedirectToAction("Profile");
            }
            else
            {
                // Handle update failure
                return View("Error");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Index", "Home"); // Redirect to the home page or login page as appropriate
        }
    }
}

