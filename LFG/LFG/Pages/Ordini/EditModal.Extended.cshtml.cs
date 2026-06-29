using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using LFG.Ordini;

namespace LFG.Web.Pages.Ordini;

public class EditModalModel : EditModalModelBase
{
    public EditModalModel(IOrdiniAppService ordiniAppService) : base(ordiniAppService)
    {
    }
}