using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using LFG.Ordini;
using LFG.Sconti;

namespace LFG.Helpers;

public static class ScontoHelper
{
    /// <summary>
    /// Valida uno sconto per l'uso corrente: sezione, finestra di validità, limite utilizzi.
    /// Gli utilizzi sono contati solo sugli ordini con Stato "Confermato".
    /// </summary>
    public static async Task<(bool Valido, string? Errore)> ValidaAsync(
        Sconto sconto, string? sezioneUtente, DateTime oggi, IRepository<Ordine, Guid> ordineRepo)
    {
        if (!string.Equals(sconto.Sezione, sezioneUtente, StringComparison.OrdinalIgnoreCase))
            return (false, "Questo codice non è valido per la tua sezione.");

        if (oggi.Date < sconto.ValidoDal.Date || oggi.Date > sconto.ValidoAl.Date)
            return (false, "Questo codice sconto è scaduto o non è ancora attivo.");

        if (sconto.LimiteUtilizzi.HasValue)
        {
            var ordiniConfermati = await ordineRepo.GetListAsync(
                o => o.ScontoId == sconto.Id && o.Stato == "Confermato");

            if (ordiniConfermati.Count >= sconto.LimiteUtilizzi.Value)
                return (false, "Questo codice sconto ha raggiunto il limite di utilizzi.");
        }

        return (true, null);
    }

    /// <summary>
    /// Applica lo sconto al totale. Tipo "Percentuale" sottrae una percentuale del totale,
    /// qualsiasi altro Tipo (es. "Euro") è trattato come importo fisso. Non scende mai sotto 0.
    /// </summary>
    public static decimal Calcola(Sconto sconto, decimal totale)
    {
        var scontato = string.Equals(sconto.Tipo, "Percentuale", StringComparison.OrdinalIgnoreCase)
            ? totale - totale * sconto.Valore / 100m
            : totale - sconto.Valore;

        return scontato < 0m ? 0m : scontato;
    }
}
