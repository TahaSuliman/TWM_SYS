using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Curly_TWM.Models;
using Curly_TWM.Infrastructure.DbaseContext;
using Curly_TWM.Infrastructure.Repositories;
using Curly_TWM.Domain.Entities;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.Entity;

namespace Curly_TWM.Controllers
{
    //[Authorize(Roles = "SUPERADMIN")]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // GET: emp_health
        private UnitOfWork unitfw = null;

        //-------------------%%%%%%%%%%%%%%%%%%%%%%----------------
        public AccountController()
        {
            unitfw = new UnitOfWork();

        }

        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            ApplicationRoleManager roleManager,
            UnitOfWork _unitfw)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;

            this.unitfw = _unitfw;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        //
        public ActionResult Main()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            Session["user"] = user.user_fullname;
            var emp_id = user.emp_Id;
            var empcode = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["empcode"] = empcode.emp_code;
            var jobtitle = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["jobtitle"] = jobtitle.Jobs.JobName;
            var avatar = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => (c.empid == emp_id) && (c.docu_type.Contains("الصورة الشخصية")));
            if (avatar != null)
            {
                Session["Avatar"] = avatar.doc_url;
            }
            else
            {
                Session["Avatar"] = "noimage.png";
            }
            var users = db.Users.ToList();
            var empmain = unitfw.emp_main.Find(c => (c.emp_arnm != "super")).ToList();


            for (int x = 0; x < empmain.Count; x++)
            {

                empmain[x].remarks = "888";
                for (int y = 0; y < users.Count; y++)
                {

                    if (empmain[x].Id == users[y].emp_Id)
                    {
                        empmain[x].remarks = users[y].LockoutEnabled.ToString();
                    }

                }

            }
            return View(empmain);
        }
        //اضافة مستخدم
        public ActionResult Add_User(int id , String permission)
        {
            var userdata = unitfw.emp_main.SingleOrDefault(x => x.Id == id);
            if (userdata == null)
            {
                goto register;
            }

            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = userdata.emp_code.ToString(), Email = userdata.emp_code.ToString(), emp_Id = id, user_fullname = userdata.emp_arnm };
                string Password = userdata.emp_code.ToString();
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(Password);
                user.SecurityStamp = Guid.NewGuid().ToString();

                DateTime dt = new DateTime(9999, 9, 9, 9, 9, 9, DateTimeKind.Utc);
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                user.LockoutEndDateUtc = dt;
                user.EmailConfirmed = false;
                user.PhoneNumberConfirmed = false;

                var userExists = unitfw.GenericRepos<ApplicationUser>().SingleOrDefault(x => x.emp_Id == user.emp_Id);

                if (userExists == null)
                {
                    unitfw.GenericRepos<ApplicationUser>().InsertObj(user);
                    unitfw.Commit();
                    //UserManager.AddToRole(user.Id, permission);
                }
                else if (userExists != null)
                {

                    TempData["title"] = "المستخدم موجود مسبقاً ";
                    TempData["SuccessMsg"] = "لا يمكن اتمام العملية";
                    TempData["type"] = "warning";
                    return RedirectToAction("Main", "Account");
                }

                return RedirectToAction("Main", "Account");


            }

        register:

            return RedirectToAction("Main", "Account");
        }
        public ActionResult Index()
        {
            List<ApplicationUser> usersList = new List<ApplicationUser>();

            usersList = UserManager.Users.ToList();

            return View(usersList);
        }
        // GET: HR/Details/5
        public ActionResult Details(int id)
        {

            var maindata = UserManager.Users.Where(x => x.emp_Id == id).FirstOrDefault();
            //ViewBag.UserRole = UserManager.GetRoles(id).ToList();
            ViewBag.UserRole = UserManager.GetRoles(maindata.Id).ToList();


            ViewBag.RoleId = new SelectList(RoleManager.Roles.ToList(), "Id", "Name");

            if (maindata == null)
            {
                return HttpNotFound();
            }
            //// TODO: Add delete logic here
            return View(maindata);
        }
        [HttpPost]
        public ActionResult Details(FormCollection formCollection)
        {
            string UserId = formCollection["UserId"];
            string RoleId = formCollection["RoleId"];
            if (RoleId != "")
            {
                if (UserManager.IsInRole(UserId, RoleId))
                {
                    ViewBag.Message = "User already has this permission ...";
                }
                else
                {
                    var role = RoleManager.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
                    UserManager.AddToRole(UserId, role.Name);
                }
            }

            var maindata = UserManager.Users.Where(x => x.Id == UserId).FirstOrDefault();
            ViewBag.UserRole = UserManager.GetRoles(UserId).ToList();

            ViewBag.RoleId = new SelectList(RoleManager.Roles.ToList(), "Id", "Name");

            return View(maindata);

        }
        //[HttpPost]
        public ActionResult Delete(string RoleName, string UserId)
        {

            var maindata = UserManager.Users.Where(x => x.Id == UserId).FirstOrDefault();

            UserManager.RemoveFromRole(UserId, RoleName);

            //// TODO: Add delete logic here

            return RedirectToAction("Details", new { id = maindata.emp_Id });

        }
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private TWMDB db = new TWMDB();
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {


                        return RedirectToAction("checkk", "Account");


                    }

                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", " الحساب غير مفعل اتصل بمسؤول بالنظام ...");
                    return View("Login");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        public ActionResult checkk()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            Session["user"] = user.user_fullname;
            var emp_id = user.emp_Id;
            var empcode = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["empcode"] = empcode.emp_code;
            var jobtitle = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            //Session["jobtitle"] = jobtitle.Jobs.JobName;
            var avatar = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => (c.empid == emp_id) && (c.docu_type.Contains("الصورة الشخصية")));
            if (avatar != null)
            {
                Session["Avatar"] = avatar.doc_url;
            }
            else
            {
                Session["Avatar"] = "noimage.png";
            }

            //قائمة الصلاحيات
            var maindata = UserManager.Users.Where(x => x.Id == userId).FirstOrDefault();
            ViewBag.UserRole = UserManager.GetRoles(userId).ToList();
            //متعدد الصلاحيات
            if(UserManager.GetRoles(userId).ToList().Count() > 1)
            {
                return RedirectToAction("Land");
            }
            //احادي الصلاحيات
            else if (UserManager.GetRoles(userId).ToList().Count() == 1)
            {
            if (User.IsInRole("Team_Member"))
                return RedirectToAction("Main", "Team_Member");
           
            if (User.IsInRole("Initiative_Officer"))
                return RedirectToAction("Main", "Initiative_Officer");
           
            if (User.IsInRole("Organizational_Manager"))
                return RedirectToAction("Main", "Organizational_Manager");
           
            if (User.IsInRole("System_Manager"))
                return RedirectToAction("Main", "SysMan");
           
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home");
           

            }

            return RedirectToAction("Login", "Account");
        }

        public ActionResult Land()
        {
            ViewBag.ActionName = "صلاحيات المستخدم";

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            Session["user"] = user.user_fullname;
            var emp_id = user.emp_Id;
            var empcode = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["empcode"] = empcode.emp_code;
            var jobtitle = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            //Session["jobtitle"] = jobtitle.Jobs.JobName;
            var avatar = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => (c.empid == emp_id) && (c.docu_type.Contains("الصورة الشخصية")));
            if (avatar != null)
            {
                Session["Avatar"] = avatar.doc_url;
            }
            else
            {
                Session["Avatar"] = "noimage.png";
            }

            //قائمة الصلاحيات
            var maindata = UserManager.Users.Where(x => x.Id == userId).FirstOrDefault();
            ViewBag.UserRole = UserManager.GetRoles(userId).ToList();

            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl, string act, string crt)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(act, crt);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl, "Index", "Home");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //

        // POST: /Account/Register
        [HttpGet]
        //[Authorize(Roles = "SUPERADMIN")]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(int id)
        {
            var userdata = unitfw.emp_main.SingleOrDefault(x => x.Id == id);
            //var user_fullname = unitfw.emp_main.SingleOrDefault(x => x.Id == model.emp_Id);
            //var emp_class = userdata.emp_class;

            if (userdata == null)
            {
                ModelState.AddModelError("البريد الإلكتروني", "هذا المستخدم لا يمتلك بيانات إتصال !!");
                goto register;
            }

            if (ModelState.IsValid)
            {
                ViewBag.Email = "0";

                var user = new ApplicationUser { UserName = userdata.emp_mobile, Email = userdata.emp_mobile, emp_Id = userdata.Id, user_fullname = userdata.emp_arnm };
                //var t = UserManager.UpdateAsync(user);
                string Password = userdata.emp_code.ToString();
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(Password);
                user.SecurityStamp = Guid.NewGuid().ToString();

                DateTime dt = new DateTime(9999, 9, 9, 9, 9, 9, DateTimeKind.Utc);
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                user.LockoutEndDateUtc = dt;
                user.EmailConfirmed = false;
                user.PhoneNumberConfirmed = false;

                //var useremail = unitfw.GenericRepos<ApplicationUser>.SingleOrDefault(x => x.emp_Id == model.emp_Id);
                var userExists = UserManager.FindByNameAsync(user.Email);
                if (userExists.Result == null)
                {
                    unitfw.GenericRepos<ApplicationUser>().InsertObj(user);
                    unitfw.Commit();
                    //var role = RoleManager.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
                    UserManager.AddToRole(user.Id, userdata.emp_class);
                }
                else
                {
                    ViewBag.emp_Id = new SelectList(unitfw.emp_main.GetAll(), "Id", "emp_arnm").ToList();


                    TempData["title"] = "المستخدم معتمد مسبقاً ";
                    TempData["SuccessMsg"] = "لا يمكن اتمام العملية";
                    TempData["type"] = "warning";
                    return RedirectToAction("Index", "Admin");
                }

                //var result = await UserManager.CreateAsync(user, model.Password);
                TempData["title"] = "Success Message";
                TempData["SuccessMsg"] = "تم الاعتماد بنجاح";
                TempData["type"] = "success";

                return RedirectToAction("Index", "Admin");


            }

        register:

            //ViewBag.emp_Id = new SelectList(unitfw.emp_main.GetAll(), "Id", "emp_arnm").ToList();

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "Admin");
        }


        //Suspend
        public ActionResult Suspend(int id)
        {

            var datareturn = unitfw.GenericRepos<ApplicationUser>().SingleOrDefault(x => x.emp_Id == id);
            datareturn.LockoutEnabled = true;
            unitfw.GenericRepos<ApplicationUser>().UpdateObj(datareturn);
            unitfw.Commit();

            TempData["title"] = "Success Message ";
            TempData["SuccessMsg"] = "تم التحديث بنجاح";
            TempData["type"] = "success";
            return RedirectToAction("Main", "Account");
        }


        //UnSuspend
        public ActionResult UnSuspend(int id)
        {

            var datareturn = unitfw.GenericRepos<ApplicationUser>().SingleOrDefault(x => x.emp_Id == id);
            datareturn.LockoutEnabled = false;
            unitfw.GenericRepos<ApplicationUser>().UpdateObj(datareturn);
            unitfw.Commit();

            TempData["title"] = "Success Message ";
            TempData["SuccessMsg"] = "تم التحديث بنجاح";
            TempData["type"] = "success";
            return RedirectToAction("Main", "Account");
        }
      
        
        // GET: HR/Edit/5
        public ActionResult Edit(string id)
        {
            ApplicationUser user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: HR/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplicationUser userData)
        {
            var user = UserManager.FindById(userData.Id);

            user.LockoutEnabled = userData.LockoutEnabled;
            UserManager.Update(user);
            //unitfw.GenericRepos<ApplicationUser>().UpdateObj(user);

            //unitfw.Commit();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteAsync(string id)
        {
            ApplicationUser user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUser(ApplicationUser model)
        {
            string userId = model.Id;
            var userCount = await UserManager.Users.ToListAsync();
            var user = await UserManager.FindByIdAsync(userId);

            if (userCount.Count == 1 || user.emp_Id == 1)
            {
                return RedirectToAction("Index");
            }
            var userExists = await UserManager.FindByIdAsync(userId);
            if (userExists == null)
                return RedirectToAction("Index");

            await UserManager.DeleteAsync(userExists);

            return RedirectToAction("Index");
        }
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl, "Index", "Home");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl, "Index", "Home");
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }

        //

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        //[HttpPost] // GET EMAIL BY ID
        //public ActionResult GetEmail(int id)
        //{
        //    var user_data = unitfw.GenericRepos<empcontact>().GetObjByID(id);
        //    string email = user_data.emp_email;
        //    return Json(email);
        //}
        [HttpPost] // GET EMAIL BY ID
        public ActionResult GetEmail(int id)
        {
            string email = "taha is: " + id.ToString();
            return Json(email);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}