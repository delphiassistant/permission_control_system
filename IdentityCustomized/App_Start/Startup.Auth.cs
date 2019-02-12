using IdentityCustomized.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace IdentityCustomized
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            PopulatePermissionsAndGroupsIntoDatabase();
            PopulateAllPermissionsIntoApplicationCollection();

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enables Roles Manager Feature:
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = "IdentityCustomized",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        private List<ControllerData> PopulateControllersAndActions()
        {
            List<ControllerData> data = new List<ControllerData>();

            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                // Ignore assemblies which their name start with following names:
                .Where(a => !a.FullName.StartsWith("mscorlib")
                            && !a.FullName.StartsWith("System")
                            && !a.FullName.StartsWith("Microsoft")
                            && !a.FullName.StartsWith("WebDev")
                            && !a.FullName.StartsWith("SMDiagnostics")
                            && !a.FullName.StartsWith("Anonymously")
                            && !a.FullName.StartsWith("Antlr3")
                            && !a.FullName.StartsWith("EntityFramework")
                            && !a.FullName.StartsWith("Newtonsoft")
                            && !a.FullName.StartsWith("Owin")
                            && !a.FullName.StartsWith("WebGrease")
                            && !a.FullName.StartsWith("App_")
                            && !a.FullName.StartsWith("Kendo"));

            foreach (Assembly assembly in assemblies)
            {
                // http://stackoverflow.com/questions/1423733/how-to-tell-if-a-net-assembly-is-dynamic
                if (!(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                    && assembly.ManifestModule.GetType().Namespace != "System.Reflection.Emit")
                {
                    Type[] types = assembly.GetTypes().Where(t => t.BaseType == typeof(Controller)).ToArray();

                    foreach (var type in types)
                    {
                        bool controllerHasAuthorizeAttribute = 
                            type.CustomAttributes.Any(c => c.AttributeType.Name == "AuthorizeAttribute");

                        bool classHasIgnorePermissionCheck = 
                            type.CustomAttributes.Any(c => c.AttributeType.Name == "IgnorePermissionCheckAttribute");
                        if (classHasIgnorePermissionCheck) // if the class has this attribute, that means it's marked to ignore permission check
                        {
                            continue;
                        }

                        string controllerNameLocalized = null;
                        var classTitle = type.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "TitleAttribute");
                        if (classTitle != null)
                        {
                            controllerNameLocalized = classTitle.ConstructorArguments[0].Value.ToString();
                        }

                        ControllerData controllerData = new ControllerData()
                        {
                            ControllerNamespace = type.Namespace,
                            ControllerName = type.Name.Replace("Controller", ""),
                            RequiresAuthorization = controllerHasAuthorizeAttribute,
                            ControllerNameLocalized = controllerNameLocalized
                        };

                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                        List<string> actions = new List<string>();
                        foreach (MethodInfo methodInfo in methods)
                        {
                            // Check if method has IgnorePermissionCheck attribute
                            bool methodHasIgnorePermissionCheck = methodInfo.CustomAttributes
                                    .Any(c => c.AttributeType.Name == "IgnorePermissionCheckAttribute");
                            if (methodHasIgnorePermissionCheck)
                            {
                                continue;
                            }

                            // Check if method has ChildActionOnlyAttribute
                            bool methodHasChildActionOnlyAttribute = methodInfo.CustomAttributes
                                .Any(c => c.AttributeType.Name == "ChildActionOnlyAttribute");
                            if (methodHasChildActionOnlyAttribute)
                            {
                                continue;
                            }

                            // Check for other attributes important to us
                            bool isAllowAnonymous = methodInfo.CustomAttributes.Any(c => c.AttributeType.Name == "AllowAnonymousAttribute");
                            var httpPostAttribute = methodInfo.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "HttpPostAttribute");
                            var actionNameAttribute = methodInfo.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "ActionNameAttribute");
                            bool actionRequiresAuthorization = methodInfo.CustomAttributes.Any(c => c.AttributeType.Name == "AuthorizeAttribute");
                            bool isHttpPost = httpPostAttribute != null;
                            

                            // ActionNameAttribute could rename an action, if it's used, then use new name of action
                            string actionName = null;
                            if (actionNameAttribute != null)
                            {
                                actionName = actionNameAttribute.ConstructorArguments[0].Value.ToString();
                            }
                            

                            string actionNameLocalized = null;
                            var actionTitle = methodInfo.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "TitleAttribute");
                            if (actionTitle != null) // if the class has this attribute, that means it's marked to ignore permission check
                            {
                                actionNameLocalized = actionTitle.ConstructorArguments[0].Value.ToString();
                            }

                            string actionIcon = null;
                            var displayIcon = methodInfo.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "IconAttribute");
                            if (displayIcon != null) // if the class has this attribute, that means it's marked to ignore permission check
                            {
                                actionIcon = displayIcon.ConstructorArguments[0].Value.ToString();
                            }

                            // Gather only methods with ActionResult return types:
                            if (methodInfo.ReturnType.Name == "ActionResult")
                            {
                                //actions.Add(methodInfo.Name);
                                if (!controllerData.ActionsList.Any(cd => cd.ActionName == (actionName ?? methodInfo.Name)))
                                {
                                    controllerData.ActionsList.Add(new ActionData()
                                    {
                                        ActionName = actionName ?? methodInfo.Name,
                                        ActionNameLocalized = actionNameLocalized,
                                        ActionIcon = actionIcon,
                                        AllowAnonymous = isAllowAnonymous,
                                        RequiresHttpPost = isHttpPost,
                                        RequiresAuthorization = actionRequiresAuthorization
                                    });
                                }
                            }
                        }
                        data.Add(controllerData);
                    }
                }
            }
            return data;
        }

        private void PopulateAllPermissionsIntoApplicationCollection()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var Application = HttpContext.Current.Application;
            Application["AllPermissions"] = db.Permissions.Include("PermissionGroup")
                            .Select(pr => new PermissionListItem()
                            {
                                PersmissionName = pr.PermissionTitle,
                                PermissiongRequiresAuthorization = pr.RequiresAuthorization,
                                PermissiongGroupName = pr.PermissionGroup.PermissionGroupTitle,
                                PermissiongGroupRequiresAuthorization = pr.PermissionGroup.RequiresAuthorization,
                                PermissionAllowAnonymous = pr.AllowAnonymous,
                                ActionFullName = pr.PermissionGroup.PermissionGroupTitle + pr.PermissionTitle
                            }).ToList();
        }

        private void PopulatePermissionsAndGroupsIntoDatabase()
        {
            var db = new ApplicationDbContext();
            var data = PopulateControllersAndActions();

            var exisitingGroups = db.PermissionGroups.Include("Permissions").ToList();

            foreach (var controllerData in data)
            {
                var group = exisitingGroups
                    .FirstOrDefault(g => 
                            g.PermissionGroupTitle == controllerData.ControllerName
                            && g.PermissionGroupNamespace == controllerData.ControllerNamespace);

                if (group != null)
                {
                    foreach (var action in controllerData.ActionsList)
                    {
                        string actionFullName = $"{controllerData.ControllerName}{action.ActionName}";

                        var currentPermission = 
                                    group.Permissions.FirstOrDefault(p => p.ActionFullName == actionFullName);

                        if (currentPermission == null)
                        {
                            var permission = new Permission()
                            {
                                PermissionGroupID = group.PermissionGroupID,
                                PermissionID = Guid.NewGuid().ToString(),
                                PermissionTitle = action.ActionName,
                                PermissionTitleLocalized = action.ActionNameLocalized,
                                PermissionIcon = action.ActionIcon,
                                RequiresAuthorization = action.RequiresAuthorization,
                                AllowAnonymous = action.AllowAnonymous,
                                ActionFullName = actionFullName
                            };
                            group.Permissions.Add(permission);
                        }
                        else
                        {
                            if (currentPermission.AllowAnonymous != action.AllowAnonymous || currentPermission.RequiresAuthorization != (action.RequiresAuthorization))
                            {
                                currentPermission.AllowAnonymous = action.AllowAnonymous;
                                currentPermission.RequiresAuthorization = action.RequiresAuthorization;
                            }
                            if (group.RequiresAuthorization != controllerData.RequiresAuthorization)
                            {
                                group.RequiresAuthorization = controllerData.RequiresAuthorization;
                            }

                            if (!string.IsNullOrEmpty(action.ActionNameLocalized) && currentPermission.PermissionTitleLocalized != action.ActionNameLocalized)
                            {
                                currentPermission.PermissionTitleLocalized = action.ActionNameLocalized;
                            }

                            if (!string.IsNullOrEmpty(action.ActionIcon) && currentPermission.PermissionIcon != action.ActionIcon)
                            {
                                currentPermission.PermissionIcon = action.ActionIcon;
                            }

                            if (!string.IsNullOrEmpty(controllerData.ControllerNameLocalized) && group.PermissionGroupTitleLocalized != controllerData.ControllerNameLocalized)
                            {
                                group.PermissionGroupTitleLocalized = controllerData.ControllerNameLocalized;
                            }

                            db.SaveChanges();

                        }
                    }
                }
                else
                {
                    var newGroup = new PermissionGroup()
                    {
                        PermissionGroupID = Guid.NewGuid().ToString(),
                        PermissionGroupTitle = controllerData.ControllerName,
                        PermissionGroupTitleLocalized = controllerData.ControllerNameLocalized,
                        PermissionGroupNamespace = controllerData.ControllerNamespace,
                        RequiresAuthorization = controllerData.RequiresAuthorization
                    };

                    foreach (var actionData in controllerData.ActionsList)
                    {
                        string actionName = $"{controllerData.ControllerName}{actionData.ActionName}";
                        var permission = new Permission()
                        {
                            PermissionGroupID = newGroup.PermissionGroupID,
                            PermissionID = Guid.NewGuid().ToString(),
                            PermissionTitle = actionData.ActionName,
                            PermissionTitleLocalized = actionData.ActionNameLocalized,
                            PermissionIcon = actionData.ActionIcon,
                            RequiresAuthorization = actionData.RequiresAuthorization,
                            AllowAnonymous = actionData.AllowAnonymous,
                            ActionFullName = actionName
                        };
                        if (permission.RequiresAuthorization == false && permission.AllowAnonymous == false && newGroup.RequiresAuthorization == true)
                        {
                            permission.RequiresAuthorization = true;
                        }
                        newGroup.Permissions.Add(permission);
                    }
                    db.PermissionGroups.Add(newGroup);
                }
            }
            db.SaveChanges();

        }

    }
}