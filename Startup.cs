using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using AspNetIdentityDemo.Models;
using AspNetIdentityDemo.Services;
[assembly: OwinStartup(typeof(AspNetIdentityDemo.Startup))]

namespace AspNetIdentityDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string connectionString =
                @"Data Source=DESKTOP-86NATJ8;DataBase=AspNetIdentityToturialExtended;Integrated Security=True";
            app.CreatePerOwinContext(() => new ExtendedDbContext(connectionString));

            app.CreatePerOwinContext<UserStore<ExtendedUser>>(
                (opt, cont) => new UserStore<ExtendedUser>(cont.Get<ExtendedDbContext>()));

            app.CreatePerOwinContext<UserManager<ExtendedUser>>(
                (opt, cont) =>
                {
                    var userManager = new UserManager<ExtendedUser>(cont.Get<UserStore<ExtendedUser>>());
                    //configuring two factor authentification using one time password sent by SMS Twilio
                    userManager.RegisterTwoFactorProvider("SMS", new PhoneNumberTokenProvider<ExtendedUser> { MessageFormat = "Token: {0}" });
                    userManager.SmsService = new SmsService();
                    //configuring acount confirmation with email using sendGrid
                    userManager.UserTokenProvider = new DataProtectorTokenProvider<ExtendedUser>(opt.DataProtectionProvider.Create());
                    userManager.EmailService = new EmailService();
                    //configuring user policy
                    userManager.UserValidator = new UserValidator<ExtendedUser>(userManager)
                    {
                        RequireUniqueEmail = true
                    };
                    //configuring password policy
                    userManager.PasswordValidator = new PasswordValidator
                    {
                        RequireDigit = true,
                        RequiredLength = 8,
                        RequireLowercase = true,
                        RequireNonLetterOrDigit = true,
                        RequireUppercase = true

                    };

                    //Configuring Lockout 
                    userManager.UserLockoutEnabledByDefault = true;
                    userManager.MaxFailedAccessAttemptsBeforeLockout = 3;
                    userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(3);
                  
                    return userManager;
                });

            app.CreatePerOwinContext<SignInManager<ExtendedUser, string>>(
                (opt, cont) => new SignInManager<ExtendedUser, string>(
                    cont.Get<UserManager<ExtendedUser>>(), cont.Authentication));

            //uses cookies authentification to save user informations
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}
