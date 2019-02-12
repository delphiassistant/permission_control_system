using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentityCustomized.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.BuilderProperties;

namespace IdentityCustomized.Controllers
{
    [Authorize]
    [Title("کاربران")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        IdentityManager manager = new IdentityManager();
        private ApplicationUserManager _userManager;

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

        // GET: Users
        [Title("مدیریت کاربران")]
        [Icon("fas fa-user")]
        public ActionResult Index()
        {
            return View(manager.GetAllUsers());
        }

        // GET: Users/Details/5
        [Title("نمایش جزئیات کاربر")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = manager.GetUserByID(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        [Title("افزودن کاربر")]
        [Icon("fas fa-user-plus")]
        public ActionResult Create()
        {
            List<TreeViewItemModel> roles = manager.GetAllRoles().Select(r => new TreeViewItemModel()
            {
                Text = r.RoleNameLocalized ?? r.Name,
                Id = r.Name
            }).ToList();
            ViewBag.TreeItems = roles;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateUserViewModel model, string[] SelectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    NationalCode = model.NationalCode,
                    PhoneNumber = model.Phone,
                    LandLinePhoneNumber = model.LandLinePhoneNumber,
                    Address = model.Address,
                    Fullname = model.Fullname,
                    IsSuperAdmin = model.IsSuperAdmin
                };

                IdentityResult result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    if (SelectedRoles != null)
                    {
                        var addedUser = manager.GetUser(user.UserName);

                        foreach (var selectedRole in SelectedRoles)
                        {
                            manager.AddUserToRole(addedUser.Id, selectedRole);
                        }
                    }
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            ViewBag.TreeItems = manager.GetAllRoles()
                .Select(r => new TreeViewItemModel()
                {
                    Text = r.RoleNameLocalized ?? r.Name,
                    Id = r.Name
                }).ToList();


            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: Users/Edit/5
        [Title("ویرایش کاربر")]
        [Icon("fas fa-user-edit")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = manager.GetUserByID(id);
            
            if (user == null)
            {
                return HttpNotFound();
            }

            UpdateUserViewModel model = new UpdateUserViewModel()
            {
                Address = user.Address,
                Email = user.Email,
                Id = user.Id,
                Fullname = user.Fullname,
                LandLinePhoneNumber = user.LandLinePhoneNumber,
                NationalCode = user.NationalCode,
                Phone = user.PhoneNumber,
                Username = user.UserName,
                IsSuperAdmin = user.IsSuperAdmin
            };

            ViewBag.TreeItems = manager.GetAllRoles()
                .Select(r => new TreeViewItemModel()
                {
                    Text = r.RoleNameLocalized ?? r.Name,
                    Id = r.Name,
                    Checked = manager.UserHasRole(id, r.Name)
                }).ToList();

            return View(model);

        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateUserViewModel user, string[] SelectedRoles)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = manager.GetUserByID(user.Id);
                applicationUser.Address = user.Address;
                applicationUser.Email = user.Email;
                applicationUser.Fullname = user.Fullname;
                applicationUser.LandLinePhoneNumber = user.LandLinePhoneNumber;
                applicationUser.NationalCode = user.NationalCode;
                applicationUser.PhoneNumber = user.Phone;
                applicationUser.UserName = user.Username;
                applicationUser.IsSuperAdmin = user.IsSuperAdmin;

                manager.UpdateUser(applicationUser);

                manager.ClearUserRoles(applicationUser.Id);

                if (SelectedRoles != null)
                {
                    foreach (var selectedRole in SelectedRoles)
                    {
                        manager.AddUserToRole(applicationUser.Id, selectedRole);
                    }
                }
                if(HttpContext.Application["AffectedUsers"] != null)
                {
                    List<string> affectedUsers = HttpContext.Application["AffectedUsers"] as List<string>;
                    affectedUsers.Add(user.Id);
                    HttpContext.Application["AffectedUsers"] = affectedUsers;
                }
                else
                {
                    List<string> affectedUsers = new List<string> { user.Id };
                    HttpContext.Application["AffectedUsers"] = affectedUsers;
                }

                return RedirectToAction("Index");
            }

            ViewBag.TreeItems = manager.GetAllRoles()
                .Select(r => new TreeViewItemModel()
                {
                    Text = r.RoleNameLocalized ?? r.Name,
                    Id = r.Name,
                    Checked = manager.UserHasRole(user.Id, r.Name)
                }).ToList();

            return View(user);
        }

        // GET: Users/Delete/5
        [Title("حذف کاربر")]
        [Icon("fas fa-user-times")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = manager.GetUserByID(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            UpdateUserViewModel model = new UpdateUserViewModel()
            {
                Address = user.Address,
                Email = user.Email,
                Id = user.Id,
                Fullname = user.Fullname,
                LandLinePhoneNumber = user.LandLinePhoneNumber,
                NationalCode = user.NationalCode,
                Phone = user.PhoneNumber,
                Username = user.UserName,
                IsSuperAdmin = user.IsSuperAdmin
            };
            return View(model);
            
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            ApplicationUser user = manager.GetUserByID(id);
            manager.DeleteUser(id);
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
