using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LFG.Web.Pages.Shared;

public abstract class LookupModalBase : AbpPageModel
{
    public string CurrentId { get; set; }

    public string CurrentDisplayName { get; set; }

    public LookupModalBase()
    {
        CurrentId = string.Empty;
        CurrentDisplayName = string.Empty;
    }

    public virtual Task OnGetAsync(string currentId, string currentDisplayName)
    {
        CurrentId = currentId;
        CurrentDisplayName = currentDisplayName;
        return Task.CompletedTask;
    }
}