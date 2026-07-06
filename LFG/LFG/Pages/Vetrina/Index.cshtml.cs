using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using LFG.VarianteProdotti;

namespace LFG.Pages.Vetrina
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;
        private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;

        public IndexModel(
            IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo,
            IRepository<VarianteProdotto, Guid> varianteRepo)
        {
            _prodottoRepo = prodottoRepo;
            _varianteRepo = varianteRepo;
        }

        public record ProdottoVista(Guid Id, string Nome, string Prezzo, string UrlImmagine);

        public List<ProdottoVista> InEvidenzaGlf { get; set; } = new();
        public List<ProdottoVista> InEvidenzaLfg { get; set; } = new();

        public async Task OnGetAsync()
        {
            var prodotti = await _prodottoRepo.GetListAsync();
            var varianti = await _varianteRepo.GetListAsync();

            ProdottoVista MappaProdotto(LFG.Prodotti.Prodotto prod)
            {
                var img = varianti
                    .Where(v => v.ProdottoId == prod.Id && !string.IsNullOrEmpty(v.UrlImmagine))
                    .Select(v => v.UrlImmagine!)
                    .FirstOrDefault() ?? "/images/placeholder.png";

                return new ProdottoVista(prod.Id, prod.Nome, prod.Prezzo, img);
            }

            InEvidenzaGlf = prodotti
                .Where(p => string.Equals(p.Sezione, "GLF", StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.CreationTime)
                .Take(3)
                .Select(MappaProdotto)
                .ToList();

            InEvidenzaLfg = prodotti
                .Where(p => string.Equals(p.Sezione, "LFG", StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.CreationTime)
                .Take(3)
                .Select(MappaProdotto)
                .ToList();
        }
    }
}
