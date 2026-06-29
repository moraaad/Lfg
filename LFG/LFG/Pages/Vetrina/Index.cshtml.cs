using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LFG.Pages.Vetrina
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
