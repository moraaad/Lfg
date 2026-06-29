using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity;

namespace LFG.Pages.Vetrina.Account;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly SignInManager<Volo.Abp.Identity.IdentityUser> _signInManager;

    public LogoutModel(SignInManager<Volo.Abp.Identity.IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/Vetrina");
    }
}
