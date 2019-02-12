using System.Collections.Generic;
using System.Linq;
using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

public class IdentityManager
{
    // Swap ApplicationRole for IdentityRole:
    RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
        new RoleStore<ApplicationRole>(new ApplicationDbContext()));

    UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
        new UserStore<ApplicationUser>(new ApplicationDbContext()));

    ApplicationDbContext _db = new ApplicationDbContext();

    public List<ApplicationRole> GetAllRoles()
    {        
        return _roleManager.Roles.ToList();
    }

    public ApplicationRole GetRole(string rolename)
    {
        return _roleManager.FindByName(rolename);
    }

    public ApplicationRole GetRoleByID(string roleID)
    {
        return _roleManager.FindById(roleID);
    }

    public List<string> GetUserRoles(string userid)
    {
       return _userManager.GetRoles(userid).ToList();
    }

    public ApplicationUser GetUser(string username)
    {
        return _userManager.FindByName(username);
    }

    public ApplicationUser GetUserByID(string userid)
    {
        return _userManager.FindById(userid);
    }

    public List<ApplicationUser> GetAllUsers()
    {
        return _userManager.Users.ToList();
    } 

    public IdentityResult UpdateRole(ApplicationRole role)
    {
        return _roleManager.Update(role);
    }

    public IdentityResult UpdateUser(ApplicationUser user)
    {
        return _userManager.Update(user);
    }

    public bool RoleExists(string name)
    {
        return _roleManager.RoleExists(name);
    }

    public bool UserHasRole(string userid, string rolename)
    {
        return _userManager.IsInRole(userid, rolename);
    }


    public bool CreateRole(string name, string roleNameLocalized = "")
    {
        // Swap ApplicationRole for IdentityRole:
        var idResult = _roleManager.Create(new ApplicationRole(name, roleNameLocalized));
        return idResult.Succeeded;
    }


    public bool CreateUser(ApplicationUser user, string password)
    {
        var idResult = _userManager.Create(user, password);
        return idResult.Succeeded;
    }


    public bool AddUserToRole(string userId, string roleName)
    {
        var idResult = _userManager.AddToRole(userId, roleName);
        return idResult.Succeeded;
    }


    public void ClearUserRoles(string userId)
    {
        var user = _userManager.FindById(userId);
        var currentRoles = new List<IdentityUserRole>();

        currentRoles.AddRange(user.Roles);
        foreach (var role in currentRoles)
        {
            var dbrole = GetRoleByID(role.RoleId);
            string rolename = dbrole.Name;
            _userManager.RemoveFromRole(userId, rolename);
        }
    }

    public void DeleteRole(ApplicationRole role)
    {
        var x = GetRoleByID(role.Id);
        _roleManager.Delete(x);
    }

    public void DeleteUser(string userid)
    {
        ApplicationUser user = GetUserByID(userid);
        _userManager.Delete(user);
    }

    
}


