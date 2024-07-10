using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Areas.Identity.Data;
using TasksEvaluation.Core.DTOs;

namespace TasksEvaluation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(model);
                user.UserName = model.Email;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Redirect to login page instead of signing in the user
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            return RedirectToAction("ForgotPasswordConfirmation");
        //        }

        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);

        //        var placeholders = new Dictionary<string, string>
        //        {
        //            { "header", "Reset Your Password" },
        //            { "body", "Click the link below to reset your password." },
        //            { "url", callbackUrl },
        //            { "linkTitle", "Reset Password" }
        //        };

        //        var emailContent = _emailBodyBuilder.GetEmailBody("reset_password", placeholders);
        //        await _emailSenders.SendEmailAsync(model.Email, "Reset Password", emailContent);

        //        return RedirectToAction("ForgotPasswordConfirmation");
        //    }
        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ResetPassword(string token = null)
        //{
        //    if (token == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    var model = new ResetPasswordDTO { Token = token };
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation");
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation");
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}
    }
}

