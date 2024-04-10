using Microsoft.AspNetCore.Mvc;
using POE_CloudDev.Models;

namespace POE_CloudDev.Controllers
{
    public class UserController : Controller
    {
        public userTable usrtbl = new userTable();
        public LoginModel loginModel = new LoginModel();


        [HttpPost]
        public ActionResult SignUp(userTable Users)
        {
            var result = usrtbl.insert_User(Users);
            return RedirectToAction("MyWork", "Home");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View(usrtbl);
        }


        [HttpPost]
        public ActionResult login(string email, string password)
        {       
            int userID = loginModel.SelectUser(email, password);
            if (userID != -1)
            {
                // User found, proceed with login logic (e.g., set authentication cookie)
                // For demonstration, redirecting to a dummy page
                return RedirectToAction("Index", "Home", new { userID = userID });
            }
            else
            {
                // User not found, handle accordingly (e.g., show error message)
                return View("LoginFailed");
                
            }
        }
        [HttpGet]
        public ActionResult login()
        {
            return View(loginModel) ;
        }
    }


}

