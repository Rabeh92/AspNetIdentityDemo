using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using AspNetIdentityDemo.ViewModel;
using System.Threading.Tasks;
using System.Configuration;
using AspNetIdentityDemo.Models;
namespace AspNetIdentityDemo.Controllers
{
    public class AccountController : Controller
    {

        public UserManager<ExtendedUser> UserManager
            => HttpContext.GetOwinContext().Get<UserManager<ExtendedUser>>();
        public SignInManager<ExtendedUser, string> SignInManager
            => HttpContext.GetOwinContext().Get<SignInManager<ExtendedUser, string>>();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var idenityUser = UserManager.FindByName(viewModel.UserName);
            if (idenityUser != null)
                return RedirectToAction("Index", "Home");

            var user = new ExtendedUser
            {
                UserName = viewModel.UserName,
                FullName = viewModel.FullName,
                Email = viewModel.UserName
            };
            user.Adresses.Add(new Adresse { AdresseLine = viewModel.AdresseLine, Country = viewModel.Country, UserId = user.Id });
            var creationResult = await UserManager.CreateAsync(user, viewModel.Password);
            if (creationResult.Succeeded)
            {
                var token = UserManager.GenerateEmailConfirmationToken(user.Id);
                var confirmUrl = Url.Action("EmailConfirmation", "Account", new { userId = user.Id, token = token }, Request.Url.Scheme);

                await UserManager.SendEmailAsync(user.Id, "Email confirmation", $"Use link to confirm email: {confirmUrl}");
                return RedirectToAction("Index", "Home");
            }


            ModelState.AddModelError("", creationResult.Errors.FirstOrDefault());
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            var signInStatus = await SignInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, true, true);
            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("ChooseProvider");
                default:
                    {
                        ModelState.AddModelError("", "Invalid credentials");
                        return View(loginViewModel);
                    }
            }
        }
        [HttpGet]
        public async Task<ActionResult> ChooseProvider()
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            var providers = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            return View(new ChooseProviderViewModel { Providers = providers.ToList() });

        }

        [HttpPost]
        public async Task<ActionResult> ChooseProvider(ChooseProviderViewModel viewModel)
        {
            var result = await SignInManager.SendTwoFactorCodeAsync(viewModel.ChosenProvider);
            if (result)
                return RedirectToAction("TwoFactor", new { provider = viewModel.ChosenProvider });
            else
                return Content("an error occured");
        }

        [HttpGet]
        public ActionResult TwoFactor(string provider)
        {
            return View(new TwoFactorViewModel { Provider = provider });
        }

        [HttpPost]
        public async Task<ActionResult> TwoFactor(TwoFactorViewModel viewModel)
        {
            var signInStatus = await SignInManager.TwoFactorSignInAsync(viewModel.Provider, viewModel.Code, true, viewModel.RememberBrowser);
            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");

                default:
                    ModelState.AddModelError("", "Invalid credentials");
                    return View(viewModel);
            }

        }
        public async Task<ActionResult> EmailConfirmation(string userId, string token)
        {
            var identityResult = await UserManager.ConfirmEmailAsync(userId, token);
            if (identityResult.Succeeded)
            {
                var user = UserManager.FindById(userId);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, true, false);
                }

                return RedirectToAction("Index", "Home");
            }


            return Content("An error occured");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            var user = await UserManager.FindByNameAsync(viewModel.UserName);
            if (user != null)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var resetUrl = Url.Action("PasswordReset", "Account", new { UserId = user.Id, Token = token }, Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Password reset", $"Use link to reset your password: {resetUrl}");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult PasswordReset(string userId, string token)
        {
            var passwordResetViewModel = new PasswordResetViewModel
            {
                UserId = userId,
                Token = token
            };
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PasswordReset(PasswordResetViewModel viewModel)
        {
            var identityResut = await UserManager.ResetPasswordAsync(viewModel.UserId, viewModel.Token, viewModel.Password);
            if (identityResut.Succeeded)
                return RedirectToAction("Index", "Home");

            return Content("an error occured");
        }

    }
}