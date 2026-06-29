using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Sconti;

namespace LFG.Web.Pages.Sconti;

public class CreateModalModel : CreateModalModelBase
{
    public CreateModalModel(IScontiAppService scontiAppService) : base(scontiAppService)
    {
    }
}