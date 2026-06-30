using System.Collections.Generic;

namespace LFG.Prodotti;

public class PrezzoNormalizzazioneResultDto
{
    public int TotaleEsaminati { get; set; }
    public int TotaleModificati  { get; set; }
    public int TotaleAnomalie    { get; set; }
    public List<PrezzoConversioneLog> Log { get; set; } = new();
}

public class PrezzoConversioneLog
{
    public System.Guid   ProdottoId  { get; set; }
    public string        Nome        { get; set; } = "";
    public string        PrimaDi     { get; set; } = "";
    public string        DopoDi      { get; set; } = "";
    public bool          Modificato  { get; set; }
    public bool          Anomalia    { get; set; }
}
