using System;

namespace LFG.Collezioni;

public abstract class CollezioneExcelDtoBase
{
    public string Nome { get; set; } = null!;
    public string Stagione { get; set; } = null!;
    public DateTime Anno { get; set; }

    public string Sezione { get; set; } = null!;
}