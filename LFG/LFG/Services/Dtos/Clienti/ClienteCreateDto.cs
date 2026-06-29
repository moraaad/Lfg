using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LFG.Clienti;

public abstract class ClienteCreateDtoBase
{
    [Required]
    [StringLength(ClienteConsts.NomeMaxLength, MinimumLength = ClienteConsts.NomeMinLength)]
    public string Nome { get; set; } = null!;
    [Required]
    [StringLength(ClienteConsts.CognomeMaxLength, MinimumLength = ClienteConsts.CognomeMinLength)]
    public string Cognome { get; set; } = null!;
    [Required(AllowEmptyStrings = true)]
    public string Genere { get; set; } = null!;
    public DateTime? DataNascita { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(ClienteConsts.EmailMaxLength, MinimumLength = ClienteConsts.EmailMinLength)]
    public string Email { get; set; } = null!;
    [Required]
    [StringLength(ClienteConsts.TelefonoMaxLength, MinimumLength = ClienteConsts.TelefonoMinLength)]
    public string Telefono { get; set; } = null!;
    [Required]
    [StringLength(ClienteConsts.SezioneMaxLength, MinimumLength = ClienteConsts.SezioneMinLength)]
    public string Sezione { get; set; } = null!;
    [Required(AllowEmptyStrings = true)]
    public string Nazionalita { get; set; } = null!;
}