using System;

namespace LFG.Pagamenti;

public abstract class PagamentoDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}