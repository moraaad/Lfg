using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using LFG.Collezioni;

namespace LFG.Pages.Vetrina.Collezioni;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IRepository<Collezione, Guid> _collezioneRepo;

    public IndexModel(IRepository<Collezione, Guid> collezioneRepo)
    {
        _collezioneRepo = collezioneRepo;
    }

    public record CollezioneVista(Guid Id, string Nome, string Stagione, int Anno, string Sezione);

    public List<CollezioneVista> Collezioni { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var collezioni = await _collezioneRepo.GetListAsync();

        Collezioni = collezioni
            .OrderBy(c => c.Sezione)
            .ThenByDescending(c => c.Anno)
            .ThenBy(c => c.Nome)
            .Select(c => new CollezioneVista(c.Id, c.Nome, c.Stagione, c.Anno.Year, c.Sezione))
            .ToList();

        return Page();
    }
}
