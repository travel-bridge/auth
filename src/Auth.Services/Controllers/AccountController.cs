using Auth.Services.Data;
using Auth.Services.Infrastructure;
using Auth.Services.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Services.Controllers;

[SecurityHeaders]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;

    public AccountController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IIdentityServerInteractionService interactionService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _interactionService = interactionService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return View("Error");
        
        return View(new LoginModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var context = await _interactionService.GetAuthorizationContextAsync(model.ReturnUrl);
        if (context is null)
            return View("Error");

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberLogin,
            true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }
        
        return Redirect(model.ReturnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        if (string.IsNullOrWhiteSpace(logoutId))
            return View("Error");
        
        var context = await _interactionService.GetLogoutContextAsync(logoutId);
        if (context is null)
            return View("Error");

        if (User.Identity?.IsAuthenticated == true)
            await _signInManager.SignOutAsync();

        return Redirect(context.PostLogoutRedirectUri);
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return View("Error");
        
        return View(new RegisterModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User { Email = model.Email, UserName = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.Action(
            "ConfirmEmail",
            "Account",
            new { returnUrl = model.ReturnUrl, userId = user.Id, code },
            Request.Scheme);
        // TODO: Send email

        return View("Login", new LoginModel { ReturnUrl = model.ReturnUrl });
    }
    
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string returnUrl, string userId, string code)
    {
        if (string.IsNullOrWhiteSpace(returnUrl)
            || string.IsNullOrWhiteSpace(userId)
            || string.IsNullOrWhiteSpace(code))
            return View("Error");
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return View("Error");
        
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
            return View("Error");

        return View("EmailConfirmed");
    }
    
    [HttpGet]
    public IActionResult ForgotPassword(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return View("Error");
        
        return View(new ForgotPasswordModel { ReturnUrl = returnUrl });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user is null)
            return View("Error");
        
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action(
            "ResetPassword",
            "Account",
            new { returnUrl = model.ReturnUrl, userId = user.Id, code },
            Request.Scheme);
        // TODO: Send email

        return View("ResetPasswordCodeSent");
    }
    
    [HttpGet]
    public IActionResult ResetPassword(string returnUrl, string userId, string code)
    {
        if (string.IsNullOrWhiteSpace(returnUrl)
            || string.IsNullOrWhiteSpace(userId)
            || string.IsNullOrWhiteSpace(code))
            return View("Error");

        return View(new ResetPasswordModel { ReturnUrl = returnUrl, UserId = userId, Code = code });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user is null)
            return View("Error");
        
        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        return View("Login", new LoginModel { ReturnUrl = model.ReturnUrl });
    }
}