using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LFG.Web.Controllers.Prodotti;

[Route("[controller]/[action]")]
public class ProdottiController : AbpController
{
    [HttpGet]
    public virtual async Task<PartialViewResult> ChildDataGrid(Guid prodottoId)
    {
        return PartialView("~/Pages/Shared/Prodotti/_ChildDataGrids.cshtml", prodottoId);
    }
}