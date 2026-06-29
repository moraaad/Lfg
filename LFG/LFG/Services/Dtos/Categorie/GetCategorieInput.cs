using Volo.Abp.Application.Dtos;
using System;

namespace LFG.Categorie;

public abstract class GetCategorieInputBase : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? Nome { get; set; }

    public string? Descrizione { get; set; }

    public string? Sezione { get; set; }

    public GetCategorieInputBase()
    {
    }
}