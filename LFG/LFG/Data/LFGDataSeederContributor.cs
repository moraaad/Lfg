using LFG.Categorie;
using LFG.Clienti;
using LFG.Collezioni;
using LFG.ElementoListe;
using LFG.Indirizzi;
using LFG.ListeDesideri;
using LFG.Ordini;
using LFG.Pagamenti;
using LFG.Prodotti;
using LFG.Recensioni;
using LFG.RigaOrdini;
using LFG.Sconti;
using LFG.VarianteProdotti;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace LFG.Data;

// Adatta i 'using' delle entità sotto al namespace reale generato da ABP Suite
// (di solito LFG.Clientes, LFG.Prodottos, ecc. oppure LFG.Entities).
// Idem per i nomi dei costruttori se Suite li ha generati diversi.

public class LFGDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;

    // Sostituisci i tipi dei repository con quelli reali delle tue entità
    private readonly IRepository<Categoria, Guid> _categoriaRepo;
    private readonly IRepository<Collezione, Guid> _collezioneRepo;
    private readonly IRepository<Sconto, Guid> _scontoRepo;
    private readonly IRepository<Cliente, Guid> _clienteRepo;
    private readonly IRepository<Prodotto, Guid> _prodottoRepo;
    private readonly IRepository<VarianteProdotto, Guid> _varianteRepo;
    private readonly IRepository<Indirizzo, Guid> _indirizzoRepo;
    private readonly IRepository<ListaDesideri, Guid> _listaRepo;
    private readonly IRepository<Ordine, Guid> _ordineRepo;
    private readonly IRepository<Pagamento, Guid> _pagamentoRepo;
    private readonly IRepository<RigaOrdine, Guid> _rigaRepo;
    private readonly IRepository<ElementoLista, Guid> _elementoRepo;
    private readonly IRepository<Recensione, Guid> _recensioneRepo;

    public LFGDataSeedContributor(
        IGuidGenerator guidGenerator,
        IRepository<Categoria, Guid> categoriaRepo,
        IRepository<Collezione, Guid> collezioneRepo,
        IRepository<Sconto, Guid> scontoRepo,
        IRepository<Cliente, Guid> clienteRepo,
        IRepository<Prodotto, Guid> prodottoRepo,
        IRepository<VarianteProdotto, Guid> varianteRepo,
        IRepository<Indirizzo, Guid> indirizzoRepo,
        IRepository<ListaDesideri, Guid> listaRepo,
        IRepository<Ordine, Guid> ordineRepo,
        IRepository<Pagamento, Guid> pagamentoRepo,
        IRepository<RigaOrdine, Guid> rigaRepo,
        IRepository<ElementoLista, Guid> elementoRepo,
        IRepository<Recensione, Guid> recensioneRepo)
    {
        _guidGenerator = guidGenerator;
        _categoriaRepo = categoriaRepo;
        _collezioneRepo = collezioneRepo;
        _scontoRepo = scontoRepo;
        _clienteRepo = clienteRepo;
        _prodottoRepo = prodottoRepo;
        _varianteRepo = varianteRepo;
        _indirizzoRepo = indirizzoRepo;
        _listaRepo = listaRepo;
        _ordineRepo = ordineRepo;
        _pagamentoRepo = pagamentoRepo;
        _rigaRepo = rigaRepo;
        _elementoRepo = elementoRepo;
        _recensioneRepo = recensioneRepo;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Evita doppi inserimenti se il seed gira più volte
        if (await _categoriaRepo.GetCountAsync() > 0)
        {
            return;
        }

        // 1. CATEGORIA (no FK)
        var catTshirt = await _categoriaRepo.InsertAsync(
            new Categoria(_guidGenerator.Create(),
                "T-Shirt",
                "LFG",
                "Magliette in cotone"

            ), autoSave: true);

        // 2. COLLEZIONE (no FK)
        var coll = await _collezioneRepo.InsertAsync(
            new Collezione(_guidGenerator.Create(),

                "La Collezione Esclusiva",
                "Autunno/Inverno",
                DateTime.Now,
                "LFG"
            ), autoSave: true);

        // 3. SCONTO (no FK)
        var sconto = await _scontoRepo.InsertAsync(
            new Sconto(_guidGenerator.Create(),
                "WELCOME10",  
                10m,
                DateTime.Now,
                DateTime.Now.AddMonths(3),
                "LFG",
                "percentuale",
                100
            ), autoSave: true);

        // 7. CLIENTE (no FK)
        var cliente = await _clienteRepo.InsertAsync(
            new Cliente(_guidGenerator.Create(),
            
                "Mario",
                "Rossi",
                "M",
                "mario.rossi@test.it",
                "3331234567",
                "LFG",
                "Italiana",
                new DateTime(1995, 5, 20)
            ), autoSave: true);

        // 4. PRODOTTO (FK -> Categoria)
        var prodotto = await _prodottoRepo.InsertAsync(
            new Prodotto(_guidGenerator.Create(),

                catTshirt.Id,
                "LFG Essential T-Shirt - Dual Logos",
                "49.90",
                "LFG",
                "T-shirt nera con doppio logo LFG",
                "LFG-TS-001"
                       
            ), autoSave: true);

        // 5. VARIANTE_PRODOTTO (FK -> Prodotto)
        var variante = await _varianteRepo.InsertAsync(
            new VarianteProdotto(_guidGenerator.Create(),

                prodotto.Id,
                25,
                "M",
                "Nero",
                "Cotone",
                "/images/lfg-ts-001-nero.png"

            ), autoSave: true);

        // 8. INDIRIZZO (FK -> Cliente)
        await _indirizzoRepo.InsertAsync(
            new Indirizzo(_guidGenerator.Create(),

                cliente.Id,
                "Via Roma 1",
                "20100",    
                "Italia",
                "Milano",
                "MI"
                
            ), autoSave: true);

        // 9. LISTA_DESIDERI (FK -> Cliente)
        var lista = await _listaRepo.InsertAsync(
            new ListaDesideri(_guidGenerator.Create(),

                cliente.Id,
                new DateTime(2026, 1, 1),
                "Lista dei desideri di Mario Rossi"

            ), autoSave: true);

        // 10. ORDINE (FK -> Cliente)
        var ordine = await _ordineRepo.InsertAsync(
            new Ordine(_guidGenerator.Create(),

                cliente.Id,
                sconto.Id,
                DateTime.Now,
                49.90m,
                "In lavorazione",
                "Via Roma 1, Milano",
                "Carta"
            ), autoSave: true);

        // 12. PAGAMENTO (FK -> Ordine)
        await _pagamentoRepo.InsertAsync(
            new Pagamento(_guidGenerator.Create(),
            
                ordine.Id,
                49.90m,
                DateTime.Now,
                "TXN-0001",
                "Carta",
                "Completato"

            ), autoSave: true);

        // 11. RIGA_ORDINE (associativa con attributi: FK Ordine + FK Variante)
        await _rigaRepo.InsertAsync(
            new RigaOrdine(_guidGenerator.Create(),

                ordine.Id,
                variante.Id,
                1,
                49.90m
                
            ), autoSave: true);

        // 13. ELEMENTO_LISTA (associativa con attributo: FK Lista + FK Variante)
        await _elementoRepo.InsertAsync(
            new ElementoLista(_guidGenerator.Create(),
            
                variante.Id,
                lista.Id,
                DateTime.Now
                
            ), autoSave: true);

        // 14. RECENSIONE (FK -> Cliente + Prodotto)
        await _recensioneRepo.InsertAsync(
            new Recensione(_guidGenerator.Create(),

                cliente.Id,
                prodotto.Id,
                5,
                DateTime.Now,
                "Ottima qualità, logo bellissimo."

            ), autoSave: true);

        // 6. COLLEZIONE_PRODOTTO e 15. ORDINE_SCONTO sono N:M pure (join).
        // Se sono navigation collection, popolale aggiungendo agli elenchi
        // delle entità correlate; se Suite le ha generate come entità join
        // con repository proprio, inserisci qui le coppie di FK.
    }
}