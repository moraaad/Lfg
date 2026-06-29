using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using LFG.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LFG.Categorie;

namespace LFG.Web.Pages.Categorie;

public class CreateModalModel : CreateModalModelBase
{
    public CreateModalModel(ICategorieAppService categorieAppService) : base(categorieAppService)
    {
    }
}