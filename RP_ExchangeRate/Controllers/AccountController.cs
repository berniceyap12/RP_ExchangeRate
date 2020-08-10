using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_ExchangeRate.Models;
using FYPExchangeRateModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FYP_ExchangeRate.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        public FYPExchangeRateContext dbContext;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
            dbContext = new FYPExchangeRateContext();
        }

        public IActionResult ManageAccount()
        {
            ViewData["Title"] = "Manage All Accounts";
            List<UserModel> AccountResult = new List<UserModel>();
            try
            {
                var result = dbContext.SelectAllUsers();
                if (result != null && result.Count() > 0)
                {
                    foreach (var obj in result)
                    {
                        AccountResult.Add(new UserModel
                        {
                            userid = obj.UserID,
                            username = obj.Username,
                            password = obj.Passwords,
                            Remarks = obj.Remarks,
                            usertype = (UserTypes)obj.UserType,
                            emailaddress = obj.EmailAddress,
                            CreatedDate = obj.CreatedDate.Value,
                            ModifiedDate = obj.ModifiedDate.Value,
                            IsActive = obj.IsActive.Value,
                            IsActiveRemark = obj.IsActive.Value ? "Active" : "Not Active"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ManageAccountErrors = "There is an  error occurred" + ex.Message;
            }
            return View("ManageAccount",AccountResult);
        }
//Remove User Accounts - jz
        [Route("DeleteAccount")]
        public IActionResult DeleteAccount(UserModel model)
        {
            List<UserModel> AccountResult = new List<UserModel>();
            try
            {
                bool updateResult = dbContext.DeleteUser(new User { UserID = model.userid });
                var result = dbContext.SelectAllUsers();
                if (result != null && result.Count() > 0)
                {
                    foreach (var obj in result)
                    {
                        AccountResult.Add(new UserModel
                        {
                            userid = obj.UserID,
                            username = obj.Username,
                            password = obj.Passwords,
                            Remarks = obj.Remarks,
                            usertype = (UserTypes)obj.UserType,
                            emailaddress = obj.EmailAddress,
                            CreatedDate = obj.CreatedDate.Value,
                            ModifiedDate = obj.ModifiedDate.Value,
                            IsActive = obj.IsActive.Value,
                            IsActiveRemark = obj.IsActive.Value ? "Active" : "Not Active"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = "There is an  error occurred" + ex.Message;
            }
            return View("ManageAccount", AccountResult);
        }
//Change Account Password - Jinhzhi
        public IActionResult UpdateAccount(UserModel model)
        {
            ViewData["Title"] = "Manage Account";
            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password) || string.IsNullOrEmpty(model.emailaddress) || string.IsNullOrEmpty(model.Remarks) ||
                (int)model.usertype <0)
            {
                return View("ManageAccount");
            }
            List<UserModel> AccountResult = new List<UserModel>();
            try
            {
                bool updateResult = dbContext.UpdateUser(new User { UserID = model.userid, Username = model.username, Passwords = model.password, EmailAddress = model.emailaddress, UserType = (int)model.usertype });
                var result = dbContext.SelectAllUsers();
                if (result != null && result.Count() > 0)
                {
                    foreach (var obj in result)
                    {
                        AccountResult.Add(new UserModel
                        {
                            userid = obj.UserID,
                            username = obj.Username,
                            password = obj.Passwords,
                            Remarks = obj.Remarks,
                            usertype = (UserTypes)obj.UserType,
                            emailaddress = obj.EmailAddress,
                            CreatedDate = obj.CreatedDate.Value,
                            ModifiedDate = obj.ModifiedDate.Value,
                            IsActive = obj.IsActive.Value,
                            IsActiveRemark = obj.IsActive.Value ? "Active" : "Not Active"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = "There is an  error occurred" + ex.Message;
            }
            return View("ManageAccount", AccountResult);
        }
//Create Staff and Admin Accounts - Bernice
        public IActionResult AddingAccount(UserModel model)
        {
            ViewData["Title"] = "Adding New Account";
            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password) || string.IsNullOrEmpty(model.emailaddress))            
            {
                ViewBag.IsSignUpAccountSuccess = false;
                ViewBag.SignUpAccountError = "Please fill in all the forms";
                return View("AddingAccount");
            }
            try
            {
                bool result = dbContext.InsertNewUser(new User { Username = model.username, Passwords = model.password, EmailAddress = model.emailaddress, UserType = (int)model.usertype});
                ViewBag.IsSignUpAccountSuccess = result ? true : false;
            }
            catch (Exception ex)
            {
                ViewBag.IsSignUpAccountSuccess = false;
                ViewBag.SignUpAccountError = "There is an  error occurred" + ex.Message;
            }
            return View("AddingAccount");
        }

        public IActionResult SignInHome()
        {
            ViewData["Title"] = "Sign-In";
            return View("SignIn");
        }

        public IActionResult SignUpHome()
        {
            ViewData["Title"] = "Sign-Up";
            return View("SignUp");
        }
//Create Customer Account - Ashitha
        public IActionResult SignUpSubmit(UserModel model)
        {
            ViewData["Title"] = "Sign-Up";
            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password) || string.IsNullOrEmpty(model.emailaddress))
            {
                ViewBag.IsSignUpSuccess = false;
                ViewBag.SignUpError = "Please fill in all the blanks";
                return View("SignUp");
            }
            try
            {
                if (dbContext.SelectAllUsers().Where(t => t.Username == model.username).Count() > 0)
                {
                    ViewBag.IsSignUpSuccess = false;
                    ViewBag.SignUpError = "The username already exists";
                    return View("SignUp");
                }
                bool result = dbContext.InsertNewUser(new User { Username = model.username, Passwords = model.password, EmailAddress = model.emailaddress, UserType = 3 });
                ViewBag.IsSignUpSuccess = result ? true : false;
            }
            catch (Exception ex)
            {
                ViewBag.IsSignUpSuccess = false;
                ViewBag.SignUpError = "An error occurred" + ex.Message;
            }
            return View("SignIn");
        }

        public IActionResult SignOutSubmit()
        {
            ViewData["Title"] = "Sign-Up";
            HttpContext.Session.Clear();
            ViewBag.ExchangeRates = new ExchangeRateController().GetExchangeRateList();
            ViewBag.Countries = new ExchangeRateController().GetCountrySelectList();
            return View("ManageExchangeRates");
        }
//Log into system - Ashitha
        public IActionResult SignInSubmit(UserModel model)
        {
            ViewData["Title"] = "Sign-In";
            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password))
            {
                ViewBag.IsSignSuccess = false;
                ViewBag.SignInError = "Please fill in all the forms";
                return View("SignIn");
            }
            try
            {
                List<User> result = dbContext.SignIn(new User { Username = model.username, Passwords = model.password, EmailAddress = model.emailaddress });
                if (result != null && result.Count() > 0)
                {
                    HttpContext.Session.SetString("Username", result.First().Username);
                    HttpContext.Session.SetString("UserType", result.First().UserType.Value.ToString());
                    HttpContext.Session.SetString("UserID",result.First().UserID.ToString());
                    if (result.First().UserType.Value == 1)
                    {
                        HttpContext.Session.SetString("Month", DateTime.Now.Month.ToString());
                        HttpContext.Session.SetString("Year", DateTime.Now.Year.ToString());
                    }
                    ViewBag.IsSignInSuccess = true;
                }
                else
                {
                    ViewBag.IsSignSuccess = false;
                    ViewBag.SignInError = "The password/username is invalid";
                }
            }
            catch (Exception ex)
            {
                ViewBag.IsSignSuccess = false;
                ViewBag.SignInError = "There is an  error occurred" + ex.Message;
                return View("SignIn");
            }
            ViewBag.ExchangeRates = new ExchangeRateController().GetExchangeRateList();
            ViewBag.Countries = new ExchangeRateController().GetCountrySelectList();
            return View("ManageExchangeRates");
        }


    }
}