using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LFG.Recensioni;
using LFG.Shared;

namespace LFG.Web.Pages.Recensioni;

public class IndexModel : IndexModelBase
{
    public IndexModel(IRecensioniAppService recensioniAppService) : base(recensioniAppService)
    {
    }
}