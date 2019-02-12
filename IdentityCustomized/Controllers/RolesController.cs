using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Protocols;
using IdentityCustomized.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityCustomized.Controllers
{
    [Authorize]
    [Title("گروه های کاربران")]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IdentityManager manager = new IdentityManager();

        public List<TreeViewItemModel> GetPermissionsList(string roleid = null)
        {
            var treeItems = new List<TreeViewItemModel>();
            List<string> rolePermissions = new List<string>();

            if (roleid != null)
            {
                rolePermissions = db.RolePermissions.Where(r => r.RoleID == roleid)
                    .Select(r => r.PermissionID).ToList();
            }

            var permissionGroups = db.PermissionGroups.Include("Permissions")
                .OrderBy(pg => pg.PermissionGroupTitle).ToList();

            foreach (var permissionGroup in permissionGroups)
            {
                TreeViewItemModel item = new TreeViewItemModel()
                {
                    Id = permissionGroup.PermissionGroupID,
                    Text = permissionGroup.PermissionGroupTitleLocalized ?? permissionGroup.PermissionGroupTitle
                };
                var permissions = permissionGroup.Permissions.OrderBy(p => p.PermissionTitle).ToList();

                foreach (var permission in permissions)
                {
                    item.Items.Add(new TreeViewItemModel()
                    {
                        Id = permission.PermissionID,
                        Text = permission.PermissionTitleLocalized ?? permission.PermissionTitle,
                        Checked = (roleid != null) && (rolePermissions.Contains(permission.PermissionID))
                    });
                }
                treeItems.Add(item);
            }
            return treeItems;
        }

        // GET: Roles
        [Title("مدیریت گروه های کاربران")]
        [Icon("fas fa-users")]
        public ActionResult Index()
        {
            var roles = (manager.GetAllRoles().Select(r => new RoleViewModel()
            {
                RoleName = r.Name,
                RoleNameLocalized = r.RoleNameLocalized,
                RoleID = r.Id
            })).OrderBy(r => r.RoleName).ToList();

            return View(roles);
        }        

        [Title("افزودن گروه کاربر")]
        public ActionResult Create()
        {
            ViewBag.PermissionItems = GetPermissionsList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleViewModel model, string[] SelectedPermissions)
        {
            if (ModelState.IsValid)
            {
                var manager = new IdentityManager();

                if (manager.GetRole(model.RoleName) != null)
                {
                    ModelState.AddModelError("", $"گروه کاربری '{model.RoleName}' از قبل موجود است. لطفا یک نام گروه دیگر انتخاب کنید.");
                    ViewBag.PermissionItems = GetPermissionsList();
                    return View(model);
                }


                manager.CreateRole(model.RoleName, model.RoleNameLocalized); // Save Role

                var role = manager.GetRole(model.RoleName);

                if (SelectedPermissions != null)
                {
                    var groups = db.PermissionGroups.Select(pg => pg.PermissionGroupID).ToList();

                    foreach (var selectedPermission in SelectedPermissions)
                    {
                        if (!groups.Contains(selectedPermission)) // Make sure current selectedPermission is not a PermissionGroup, Sine Permission Groups could be selected in TreeView
                        {
                            var permission = db.Permissions.Find(selectedPermission);
                            db.RolePermissions.Add(new RolePermission()
                            {
                                PermissionID = selectedPermission,
                                RoleID = role.Id
                            });
                        }
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception exp)
                    {

                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.PermissionItems = GetPermissionsList();
            return View(model);
        }

        [Title("ویرایش گروه کاربر")]
        public ActionResult Edit(string id = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationRole role = manager.GetRoleByID(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            ViewBag.PermissionItems = GetPermissionsList(id);
            return View(role);
        }

        [HttpPost]
        public ActionResult Edit(ApplicationRole role, string[] SelectedPermissions)
        {
            if (ModelState.IsValid)
            {
                var x = db.Roles.FirstOrDefault(r => r.Id == role.Id) as ApplicationRole;               
                

                if (x != null)
                {
                    var Application = HttpContext.Application;
                    

                    x.RoleNameLocalized = role.RoleNameLocalized;
                    x.Name = role.Name;

                    db.SaveChanges();

                    #region Get List of Users in current role, to reload their permissions
                    var AffectedUsers = x.Users.Select(u => u.UserId).ToList();

                    if (Application["AffectedUsers"] == null)
                    {
                        Application["AffectedUsers"] = AffectedUsers;
                    }
                    else
                    {
                        List<string> ExistingAffectedUsers = (List<string>)Application["AffectedUsers"];
                        foreach (string affectedUsername in AffectedUsers)
                        {
                            if (!ExistingAffectedUsers.Contains(affectedUsername))
                            {
                                ExistingAffectedUsers.Add(affectedUsername);
                            }
                        }
                        Application["AffectedUsers"] = ExistingAffectedUsers;                    }
                    #endregion

                    
                    var existingPermissions = db.RolePermissions.Where(rp => rp.RoleID == x.Id).ToList();
                    if (SelectedPermissions != null)
                    {
                        // Clear Existing RolePermissions:
                        db.RolePermissions.RemoveRange(existingPermissions);


                        var groups = db.PermissionGroups.Select(pg => pg.PermissionGroupID).ToList();

                        foreach (var selectedPermission in SelectedPermissions)
                        {
                            if (!groups.Contains(selectedPermission)) // Make sure current selectedPermission is not a PermissionGroup, Sine Permission Groups could be selected in TreeView
                            {
                                db.RolePermissions.Add(new RolePermission()
                                {
                                    PermissionID = selectedPermission,
                                    RoleID = role.Id
                                });
                            }
                        }
                        db.SaveChanges();
                    }
                }                
                return RedirectToAction("Index");
            }
            ViewBag.PermissionItems = GetPermissionsList(role.Id);
            return View(role);
        }

        [Title("حذف گروه کاربر")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole role = manager.GetRoleByID(id);
            if (role == null)
            {
                return HttpNotFound();
            }            
            return View(role);
        }

        [HttpPost]
        public ActionResult Delete(ApplicationRole role)
        {
            // Remove All Permissions For This Role:
            var rolePermissions = db.RolePermissions.Where(rp => rp.RoleID == role.Id).ToList();
            db.RolePermissions.RemoveRange(rolePermissions);
            db.SaveChanges();
            // Now delete the role itself:
            manager.DeleteRole(role);

            return RedirectToAction("Index");
        }
    }
}