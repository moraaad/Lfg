# MANUALE TECNICO — Progetto LFG | GLF

## 1. Contesto del progetto

Applicazione basata su **Framework ABP 3.0.1 Pro**, architettura **non-layered**, **Razor Pages**.

Consiste nella realizzazione di una **vetrina e-commerce dual-brand LFG / GLF**.

Le due sezioni fanno parte dello stesso brand "LFG | GLF", ma al suo interno contengono due identità separate che raccontano due stili di vita antipodici, i quali però si riuniscono sotto lo stesso tetto, mettendo in mostra la diversità dei due mondi. Da qui la doppia lettura del nome: *Life is Good* (LFG) e *Good Life is Better / La Firma Giusta* (GLF).

Il tutto si appoggia a una **base di dati SQL Server (gestita in SSMS)**, con entità reali e popolate. Esempi di entità di dominio:

- `Collezione` (Nome, Stagione, Anno, Sezione)
- `Prodotto` (Nome, Descrizione, Prezzo, CodiceSku, Sezione, CategoriaId, CollezioneId)
- `VarianteProdotto` (Taglia, Colore, Materiale, QtaMagazzino, UrlImmagine)
- `Ordine`, `RigaOrdine`, `Cliente`, `Indirizzo`, `Recensione`, `Lista_Desideri`, `Sconto`

### Ruolo di ABP e della Suite

ABP viene usato principalmente per il suo servizio **Suite**, che permette di generare entità complete di funzionalità **CRUD**, fornendo lo scheletro (entità, repository, AppService, DTO, pagine admin) per popolare e gestire la base di dati.

Con i privilegi **ADMIN** di ABP è quindi possibile *create & edit* dei dati nel DB tramite le pagine CRUD generate.

A questo impianto amministrativo è stata affiancata una **schermata pubblica "estranea" ad ABP** — la **Vetrina** — per dare visione del sito a utenti registrati e non, mettendo in mostra il brand LFG|GLF

### Comportamento della Vetrina pubblica

All'interno della vetrina si naviga liberamente come in qualsiasi e-commerce (catalogo, dettaglio prodotto, collezioni). Le funzionalità finali che modificano dati — acquistare, recensire, aggiungere ai preferiti — sono accessibili **solo previa registrazione** a una delle due sezioni. La navigazione è libera; l'azione richiede identità.

---

## 2. Tecnologie e loro gestione

| Area | Tecnologia | Note |
|------|------------|------|
| Framework backend | ABP 3.0.1 Pro (non-layered) | Suite per scaffolding CRUD |
| UI | Razor Pages | Pagine vetrina con `Layout = null` |
| Database | SQL Server / SSMS | Prefisso tabelle `App*` |
| ORM | Entity Framework Core | Migrations, `IRepository` |
| Autenticazione | ABP Identity | `IdentityUser`, `ICurrentUser` |
| Frontend | CSS puro (no Tailwind/SASS), JS vanilla, SVG inline | File centrale `wwwroot/css/vetrina.css` |
| 3D (sperimentale) | three.js | Prototipo visualizzatore |

La vetrina è deliberatamente disaccoppiata dall'infrastruttura ABP amministrativa: pagine `[AllowAnonymous]`, head scritto a mano, stile centralizzato in un unico CSS.

---

## 3. Accesso ai dati: Repository vs AppService

### La regola portante

Le pagine vetrina `[AllowAnonymous]` accedono ai dati **via Repository**, mai via AppService.

**Motivo:** gli AppService generati da Suite portano attributi di autorizzazione (`[Authorize(LFGPermissions...)]`) pensati per l'admin. Chiamarli da una pagina pubblica scatena `AbpAuthorizationException`.

### Iniezione del repository

```c#
private readonly IRepository<LFG.Prodotti.Prodotto, Guid> _prodottoRepo;

public IndexModel(IRepository<LFG.Prodotti.Prodotto, Guid> prodottoRepo)
{
    _prodottoRepo = prodottoRepo;
}
```

`IRepository<TEntity, TKey>` ha **due** parametri di tipo: l'entità e il tipo della chiave primaria (`Guid`).

> **Nota su `LFG.Prodotti.Prodotto`:** l'entità Prodotto va referenziata col tipo completo per via di una collisione di namespace annidato omonimo generato da Suite.

> **Caso chiave composta:** le tabelle associative pure (es. la vecchia `ProdottoColleziones`) non hanno un `Id` singolo — la chiave è composta dalle due FK (`GetKeys()` ritorna `[ProdottoId, CollezioneId]`). Questo tipo di entità **non** si dichiara con `IRepository<T, TKey>` e dà errori di dependency injection. È uno dei motivi che ha portato a semplificare la relazione da N:M a 1:N.

---

## 4. Query via Repository — pattern usati

### Lettura filtrata (lambda)

```csharp
var varianti = await _varianteRepo.GetListAsync(v => v.ProdottoId == id);
```

### Ricerca del singolo record (null se assente)

```csharp
var lista = await _listaDesideriRepo.FirstOrDefaultAsync(l => l.ClienteId == cliente.Id);
```

### Controllo di esistenza (anti-doppio)

```csharp
var esiste = await _recensioneRepo.AnyAsync(
    r => r.ProdottoId == id && r.ClienteId == cliente.Id);
```

### Inserimento

```csharp
await _recensioneRepo.InsertAsync(new Recensione(...), autoSave: true);
```

### Pattern get-or-create

Riusato per **wishlist** e **carrello**: cerca il record; se non esiste, crealo. È il modo con cui la lista preferiti e l'ordine-bozza nascono automaticamente al primo utilizzo.

```csharp
var lista = await _listaDesideriRepo.FirstOrDefaultAsync(l => l.ClienteId == cliente.Id);
if (lista == null)
{
    lista = new ListaDesideri(Guid.NewGuid(), cliente.Id, Clock.Now, "Preferiti");
    await _listaDesideriRepo.InsertAsync(lista, autoSave: true);
}
```

---

## 5. Relazioni fra entità

Nel progetto convivono tre tipi di relazione, ed è essenziale trattarli in modo diverso.

1) ### FK diretta (1:N)

Esempio: `Prodotto.CategoriaId`, e `Prodotto.CollezioneId` (dopo il refactoring).

```csharp
// prodotti di una categoria/collezione: filtro diretto sulla FK
var prodotti = await _prodottoRepo.GetListAsync(p => p.CollezioneId == idCollezione);
```

> La FK punta **sempre** all'`Id` dell'entità di destinazione, mai a un altro campo. In Suite si crea come **navigation property (1-n)**, non come proprietà scalare a mano.

2) ### N:M via tabella associativa

Esempio: la vecchia relazione `Collezione ↔ Prodotto` tramite `Collezione_Prodotto`.

Non esiste FK diretta fra i due lati: si passa **attraverso** l'associativa. Per "i prodotti di una collezione" si leggono le righe dell'associativa con quella collezione e si risale ai prodotti.

3) ### Chiave composta

La tabella associativa pura aveva PK = (ProdottoId + CollezioneId), senza `Id` singolo:

```csharp
public override object[] GetKeys()
{
    return new object[] { ProdottoId, CollezioneId };
}
```

Questo tipo di entità complica la DI. Nel progetto la relazione è stata poi **migrata da N:M a 1:N** (un prodotto appartiene a una sola collezione), aggiungendo `CollezioneId` a Prodotto e rimuovendo la tabella associativa.

---

## 6. Transazioni: la UnitOfWork

Quando più scritture devono essere **atomiche** (o tutte o nessuna), si racchiudono in una `[UnitOfWork]`.

### Caso 1 — Registrazione (rete anti-orfani)

Crea `IdentityUser` + `Cliente` insieme; se la seconda scrittura fallisce, l'utente appena creato viene cancellato per non lasciare record orfani.

```csharp
[UnitOfWork]
public async Task<IActionResult> OnPostAsync()
{
    Sezione = Sezione.ToUpperInvariant(); // normalizzazione sezione

    var user = new IdentityUser(Guid.NewGuid(), Input.Email, Input.Email); // creazione dell'utente
    var createResult = await _userManager.CreateAsync(user, Input.Password); // generazione della password --> chiama il manager per usare la funzione CreateAsync
    if (!createResult.Succeeded) { /* mostra errori */ return Page(); }

    try
    {
        var cliente = new Cliente(Guid.NewGuid(), /* ... */, userId: user.Id); // se i dati inseriti sono corretti prova a creare lo user 
        await _clienteRepo.InsertAsync(cliente);
    }
    catch
    {
        await _userManager.DeleteAsync(user); // rollback manuale dell'utente in caso di error fase 2
        throw;
    }

    return RedirectToPage("RegisterConfirmation"); //
}
```

### Caso 2 — Conferma ordine (checkout transazionale)

Dentro un'unica UnitOfWork: ricontrolla la disponibilità di **ogni** riga, e se anche una sola è insufficiente **annulla tutto** senza scalare nulla. Se tutte ok: scala `QtaMagazzino`, salva indirizzo e metodo di pagamento, cambia lo stato dell'ordine da "Bozza" a "Confermato". Nessuno scalo parziale possibile.

---

## 7. Utente corrente e identità di sezione (fase D3 / D4)

### Recupero dell'utente loggato e della sua sezione

Iniettando `ICurrentUser`: nella classe

```csharp
public async Task<string?> GetSezioneCorrenteAsync()
{
    var userId = _currentUser.Id;
    if (userId == null) return null;                 // non loggato

    var cliente = await _clienteRepository
        .FirstOrDefaultAsync(c => c.UserId == userId);

    return cliente?.Sezione;                          // null se loggato senza Cliente (es. admin)
}
```

### Metodi "self" e `[AllowAnonymous]`

I metodi che leggono i dati del proprio utente (`GetSezioneCorrenteAsync`, `GetClienteCorrenteAsync`, `VerificaAccessoSezioneAsync`) vanno marcati `[AllowAnonymous]` sulla **classe di implementazione** (non sull'interfaccia), per sovrascrivere l'`[Authorize]` ereditato dalla classe base generata da Suite. Senza questo, la vetrina che li chiama riceve `AbpAuthorizationException` pur essendo l'utente loggato.

### Gating cross-sezione (guardia server)

```csharp
public async Task VerificaAccessoSezioneAsync(string sezioneProdotto) // gli passo la sezione
{
    var sezioneUtente = await GetSezioneCorrenteAsync(); // chiamo metodo che ritorna la sezione dell'utente loggato
    if (sezioneUtente == null)
        throw new AbpAuthorizationException("Devi effettuare l'accesso."); 

    if (!string.Equals(sezioneUtente, sezioneProdotto, StringComparison.OrdinalIgnoreCase))
        throw new BusinessException("LFG:CrossSezione");
}
```

Uso tipico nell'handler di scrittura, come **prima riga**, prima di qualsiasi modifica:

```csharp
var prodotto = await _prodottoRepo.GetAsync(id);
await _clientiAppService.VerificaAccessoSezioneAsync(prodotto.Sezione);
// solo se non lancia eccezione, si procede
```

---

## 8. Sicurezza applicativa: mai fidarsi del client

Principio ripetuto in tutto il progetto:

- **La sezione e i dati sensibili si leggono sempre dal DB**, mai da un campo che arriva dal form. (`prodotto.Sezione` letto dal repository, non dal client.)
- **Anti-manomissione:** un `IndirizzoId` scelto al checkout deve essere verificato lato server come appartenente al cliente corrente, prima di salvarlo.
- **I controlli UI sono cosmesi:** bottoni nascosti/disabilitati migliorano l'esperienza, ma la vera difesa è la guardia server, perché una POST può essere ricostruita a mano.
- **Anti-doppio server:** recensione unica per cliente/prodotto, prodotto unico nella wishlist — verificati con `AnyAsync` prima di inserire, non solo nascondendo il form.

---

## 9. Gestione dei dati "sporchi": normalizzazione

Lezione trasversale: quando i dati provengono da inserimento manuale, aspettarsi incoerenze e normalizzare **prima** di costruirci logica sopra.

### Prezzi (denaro — mai float)

Formati incoerenti a DB (`"49.90"` con punto, `"100,00"` con virgola, valori sopra 1000). Risolti normalizzando a un **formato canonico** (punto decimale, niente separatore migliaia) e parsando con `CultureInfo.InvariantCulture` tramite un helper centralizzato (`PrezzoHelper`). Mai `float`/`double` sui soldi, sempre `decimal`.

### Percorsi immagine

Formati misti (PNG/JPJ) (`images\file.png` con backslash, `/Images/` con maiuscola, estensioni mancanti). 

Risolti con query UPDATE mirate in SSMS:

```sql
-- backslash -> slash e slash iniziale mancante

UPDATE AppVarianteProdotti
SET UrlImmagine = '/' + REPLACE(UrlImmagine, '\', '/')
WHERE UrlImmagine LIKE 'images\%';
```

Nota: su server Linux i percorsi sono **case-sensitive** — uniformare `/images/` (minuscolo) evita bug che comparirebbero solo in produzione.

---

## 10. Migrations: la disciplina

Ciclo standard, ripetuto per ogni modifica di schema:

1. Modifica l'entità in **Suite** come **relazione/navigation property**, mai come proprietà scalare duplicata.
2. `dotnet ef migrations add NomeMigration`
3. **Ispeziona il file generato** prima di applicarlo (colonne, FK, tabelle toccate).
4. **Backup DB.**
5. `dotnet run -- --migrate-database`

### Punti critici appresi

--- Il loop di Suite

Suite fa un build interno prima di rigenerare. Se il progetto non compila, Suite si blocca ("build failed") e non riesce a rigenerare — creando un ciclo. Si spezza facendo compilare a mano il minimo indispensabile (rimuovendo il codice orfano), poi si lascia completare a Suite.

## 11. Frontend della vetrina

- **CSS puro** centralizzato in `wwwroot/css/vetrina.css`, classi con prefisso `v-*`. Nessun Tailwind/SASS.
- Pagine con **`Layout = null`**: head scritto a mano, che carica `vetrina.css` e i font (Barlow, Space Mono).
- **Icone SVG inline**, non Font Awesome (evita dipendenze e problemi di caricamento su reti che bloccano i CDN).
- **Identità dual-brand** espressa via classi di sezione (`shop--lfg` / `shop--glf`, badge sezione): la distinzione cromatica LFG vs GLF è preservata come vincolo di design.
- **Persistenza dati negli artefatti** e interazioni gestite con JS vanilla (galleria immagini, popup gating, animazioni di restyling con transizioni `cubic-bezier`).

---

## 12. Disciplina Git (lezione appresa sul campo)

3 branch --> main , Develop , Restyling ... 
- commit eseguiti , quotidianamente a fine di ogni workflow 
- dopo aver eseguito le pipline di dovere , si è fatta la copia dei commit e la sincronizzazione con Git hub
- ed infine si fa il merge con la branch principale (main) di tutti i commit dei vari branch

Regola d'oro prima di ogni merge:

```bash
git status                    # verifica cosa è modificato
git add -A                    # stage di TUTTO (CSS + tutti i .cshtml)
git commit -m "descrizione"
git status                    # DEVE dire "working tree clean"
# solo ora si mergia
```

Sviluppo su branch dedicati (es. `feature/restyling`) per isolare il lavoro sperimentale e poter tornare indietro senza perdere ciò che funziona.

---

## Appendice — Flusso di una funzionalità tipo (aggiunta al carrello)

Sintesi che mette insieme i pattern:

```csharp
public async Task<IActionResult> OnPostCarrelloAggiungiAsync(Guid id, Guid varianteId) // definizione di un metodo che gli passa le varianti del prodotto con il loro id
{
    // 1. Guardia server (sezione letta dal DB, mai dal form)
    var prodotto = await _prodottoRepo.GetAsync(id); // prendo l'id del prodotto
    await _clientiAppService.VerificaAccessoSezioneAsync(prodotto.Sezione); // call al AppService , che prende i dati dal DB 

    // 2. Utente corrente
    var cliente = await _clientiAppService.GetClienteCorrenteAsync(); // prendo il cliente corrente 
    if (cliente == null) { /* messaggio */ return Page(); }

    // 3. Get-or-create dell'ordine-bozza
    var bozza = await _ordineRepo.FirstOrDefaultAsync(
        o => o.ClienteId == cliente.Id && o.Stato == "Bozza"); // se l'ordine non è in Bozza --> si crea un nuovo ordine da inserire nel carrello 
    if (bozza == null)
    {
        bozza = new Ordine(Guid.NewGuid(), cliente.Id, scontoId: null,
            indirizzoId: null, dataOrdine: Clock.Now, importoTotale: 0m, stato: "Bozza");
        await _ordineRepo.InsertAsync(bozza, autoSave: true);
    }

    // 4. Aggiunta/incremento riga (prezzo via PrezzoHelper, decimal)
    // ... InsertAsync della RigaOrdine ...

    return Page();
}
```

