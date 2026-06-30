using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using LFG.Clienti;

namespace LFG.Pages.Vetrina.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly IdentityUserManager _userManager;
    private readonly IRepository<Cliente, Guid> _clienteRepo;

    public RegisterModel(
        IdentityUserManager userManager,
        IRepository<Cliente, Guid> clienteRepo)
    {
        _userManager = userManager;
        _clienteRepo = clienteRepo;
    }

    [BindProperty(SupportsGet = true)]
    public string Sezione { get; set; } = "LFG";

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required] public string Nome { get; set; } = "";
        [Required] public string Cognome { get; set; } = "";
        public string? Telefono { get; set; }
        public string? Genere { get; set; }
        public string? Nazionalita { get; set; }
        public DateTime? DataNascita { get; set; }
    }

    public void OnGet()
    {
        if (!SezioneValida()) Sezione = "LFG";
    }

    [UnitOfWork]
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        if (!SezioneValida()) Sezione = "LFG";

        Sezione = Sezione.ToUpperInvariant();

        // check: email già usata da un Cliente
        var esiste = await _clienteRepo.AnyAsync(c => c.Email == Input.Email);
        if (esiste)
        {
            ModelState.AddModelError(string.Empty, "Questa email risulta già registrata.");
            return Page();
        }

        var user = new IdentityUser(
            id: Guid.NewGuid(),
            userName: Input.Email,
            email: Input.Email);

        var createResult = await _userManager.CreateAsync(user, Input.Password);
        if (!createResult.Succeeded)
        {
            foreach (var e in createResult.Errors)
                ModelState.AddModelError(string.Empty, TraduciErrore(e.Code, e.Description));
            return Page();
        }

        var cliente = new Cliente(
            id: Guid.NewGuid(),
            nome: Input.Nome,
            cognome: Input.Cognome,
            genere: Input.Genere ?? "",
            email: Input.Email,
            telefono: Input.Telefono ?? "",
            sezione: Sezione,
            nazionalita: Input.Nazionalita ?? "",
            dataNascita: Input.DataNascita,
            userId: user.Id);

        try
        {
            await _clienteRepo.InsertAsync(cliente); // senza autoSave per ATOMICIZZARE la unite of work a fine metodo OnPostAsync
        }
        catch
        {
            await _userManager.DeleteAsync(user); // rollback manuale dell'utente
            throw;
        } 

        return RedirectToPage("RegisterConfirmation", new { sezione = Sezione });
    }

    private bool SezioneValida() =>
        string.Equals(Sezione, "LFG", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Sezione, "GLF", StringComparison.OrdinalIgnoreCase);

    private static string TraduciErrore(string code, string fallback) =>
        code.Contains("Duplicate")
            ? "Questa email risulta già registrata. Usane un'altra o accedi."
            : fallback;
}
