using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Clienti;
using LFG.Shared;

namespace LFG.Web.Pages.Clienti;

public class IndexModel : IndexModelBase
{
    public IndexModel(IClientiAppService clientiAppService) : base(clientiAppService)
    {
    }
}