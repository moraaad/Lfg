using System;

namespace LFG.Clienti;

public abstract class ClienteExcelDtoBase
{
    public string Nome { get; set; } = null!;
    public string Cognome { get; set; } = null!;
    public string Genere { get; set; } = null!;
    public DateTime? DataNascita { get; set; }

    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Sezione { get; set; } = null!;
    public string Nazionalita { get; set; } = null!;
    public Guid? UserId { get; set; }
}