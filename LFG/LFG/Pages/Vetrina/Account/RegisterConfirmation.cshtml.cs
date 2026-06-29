using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LFG.Pages.Vetrina.Account;

// Schermata "registrazione completata".
// Mostra un messaggio e un bottone verso la sezione di provenienza.
[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Sezione { get; set; } = "LFG";

    // URL della sezione a cui dare accesso dopo la conferma.
    public string ShopUrl => $"/Vetrina/Shop?sezione={(SezioneValida() ? Sezione.ToUpperInvariant() : "LFG")}";

    public void OnGet()
    {
        if (!SezioneValida()) Sezione = "LFG";
    }

    private bool SezioneValida() =>
        string.Equals(Sezione, "LFG", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Sezione, "GLF", StringComparison.OrdinalIgnoreCase);
}