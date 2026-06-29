using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;     // SignInManager
using Volo.Abp.Identity;                 // IdentityUser

namespace LFG.Pages.Vetrina.Account;

// LOGIN — form proprio nella vetrina, ma autenticazione tramite ABP (SignInManager).
// NON verificare la password a mano: usare SEMPRE il motore ABP.
[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SignInManager<Volo.Abp.Identity.IdentityUser>
    _signInManager;

    public LoginModel(SignInManager<Volo.Abp.Identity.IdentityUser>
    signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    // Dove tornare dopo il login (es. la pagina da cui si � cliccato "Accedi")
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }
    }

    public void OnGet() { }

    public async Task<IActionResult>
    OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // email = username. Autenticazione via ABP/Identity.
        var result = await _signInManager.PasswordSignInAsync(
        Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            // torna alla pagina di partenza, o alla home vetrina
            return LocalRedirect(string.IsNullOrEmpty(ReturnUrl) ? "/Vetrina" : ReturnUrl);
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Account temporaneamente bloccato. Riprova pi� tardi.");
            return Page();
        }

        ModelState.AddModelError(string.Empty, "Email o password non corretti.");
        return Page();
    }
}