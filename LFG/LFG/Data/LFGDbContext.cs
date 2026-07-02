using LFG.ImmagineVarianti;
using LFG.ElementoListe;
using LFG.RigaOrdini;
using LFG.Recensioni;
using LFG.Pagamenti;
using LFG.Ordini;
using LFG.ListeDesideri;
using LFG.Indirizzi;
using LFG.VarianteProdotti;
using LFG.Prodotti;
using LFG.Clienti;
using LFG.Sconti;
using LFG.Collezioni;
using LFG.Categorie;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Gdpr;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.AIManagement.EntityFrameworkCore;
using LFG.Entities.Books;

namespace LFG.Data;

public class LFGDbContext : AbpDbContext<LFGDbContext>
{
    public DbSet<ImmagineVariante> ImmagineVarianti { get; set; } = null!;
    public DbSet<ElementoLista> ElementoListe { get; set; } = null!;
    public DbSet<RigaOrdine> RigaOrdini { get; set; } = null!;
    public DbSet<Recensione> Recensioni { get; set; } = null!;
    public DbSet<Pagamento> Pagamenti { get; set; } = null!;
    public DbSet<Ordine> Ordini { get; set; } = null!;
    public DbSet<ListaDesideri> ListeDesideri { get; set; } = null!;
    public DbSet<Indirizzo> Indirizzi { get; set; } = null!;
    public DbSet<VarianteProdotto> VarianteProdotti { get; set; } = null!;
    public DbSet<Prodotto> Prodotti { get; set; } = null!;
    public DbSet<Cliente> Clienti { get; set; } = null!;
    public DbSet<Sconto> Sconti { get; set; } = null!;
    public DbSet<Collezione> Collezioni { get; set; } = null!;
    public DbSet<Categoria> Categorie { get; set; } = null!;
    public DbSet<Book> Books { get; set; }

    public const string DbTablePrefix = "App";
    public const string DbSchema = null;

    public LFGDbContext(DbContextOptions<LFGDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        /* Include modules to your migration db context */
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigurePermissionManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureGdpr();
        builder.ConfigureLanguageManagement();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureAIManagement();
        builder.Entity<Book>(b => {
            b.ToTable(DbTablePrefix + "Books", DbSchema);
            b.ConfigureByConvention();
            //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });
        /* Configure your own entities here */
        if (builder.IsHostDatabase())
        {
            builder.Entity<Categoria>(b => {
                b.ToTable(DbTablePrefix + "Categorie", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Nome).HasColumnName(nameof(Categoria.Nome)).IsRequired().HasMaxLength(CategoriaConsts.NomeMaxLength);
                b.Property(x => x.Descrizione).HasColumnName(nameof(Categoria.Descrizione)).HasMaxLength(CategoriaConsts.DescrizioneMaxLength);
                b.Property(x => x.Sezione).HasColumnName(nameof(Categoria.Sezione)).IsRequired().HasMaxLength(CategoriaConsts.SezioneMaxLength);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Collezione>(b => {
                b.ToTable(DbTablePrefix + "Collezioni", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Nome).HasColumnName(nameof(Collezione.Nome)).IsRequired().HasMaxLength(CollezioneConsts.NomeMaxLength);
                b.Property(x => x.Stagione).HasColumnName(nameof(Collezione.Stagione)).IsRequired().HasMaxLength(CollezioneConsts.StagioneMaxLength);
                b.Property(x => x.Anno).HasColumnName(nameof(Collezione.Anno));
                b.Property(x => x.Sezione).HasColumnName(nameof(Collezione.Sezione)).IsRequired().HasMaxLength(CollezioneConsts.SezioneMaxLength);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Sconto>(b => {
                b.ToTable(DbTablePrefix + "Sconti", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Codice).HasColumnName(nameof(Sconto.Codice)).IsRequired().HasMaxLength(ScontoConsts.CodiceMaxLength);
                b.Property(x => x.Tipo).HasColumnName(nameof(Sconto.Tipo));
                b.Property(x => x.Valore).HasColumnName(nameof(Sconto.Valore));
                b.Property(x => x.LimiteUtilizzi).HasColumnName(nameof(Sconto.LimiteUtilizzi));
                b.Property(x => x.ValidoDal).HasColumnName(nameof(Sconto.ValidoDal));
                b.Property(x => x.ValidoAl).HasColumnName(nameof(Sconto.ValidoAl));
            });
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<VarianteProdotto>(b => {
                b.ToTable(DbTablePrefix + "VarianteProdotti", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Taglia).HasColumnName(nameof(VarianteProdotto.Taglia));
                b.Property(x => x.Colore).HasColumnName(nameof(VarianteProdotto.Colore));
                b.Property(x => x.Materiale).HasColumnName(nameof(VarianteProdotto.Materiale));
                b.Property(x => x.UrlImmagine).HasColumnName(nameof(VarianteProdotto.UrlImmagine)).HasMaxLength(VarianteProdottoConsts.UrlImmagineMaxLength);
                b.Property(x => x.QtaMagazzino).HasColumnName(nameof(VarianteProdotto.QtaMagazzino));
                b.HasOne<Prodotto>().WithMany(x => x.VarianteProdotti).HasForeignKey(x => x.ProdottoId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Indirizzo>(b => {
                b.ToTable(DbTablePrefix + "Indirizzi", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Paese).HasColumnName(nameof(Indirizzo.Paese));
                b.Property(x => x.Citta).HasColumnName(nameof(Indirizzo.Citta));
                b.Property(x => x.Provincia).HasColumnName(nameof(Indirizzo.Provincia));
                b.Property(x => x.Via).HasColumnName(nameof(Indirizzo.Via)).IsRequired().HasMaxLength(IndirizzoConsts.ViaMaxLength);
                b.Property(x => x.Cap).HasColumnName(nameof(Indirizzo.Cap)).IsRequired().HasMaxLength(IndirizzoConsts.CapMaxLength);
                b.HasOne<Cliente>().WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.SetNull);
            });
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Pagamento>(b => {
                b.ToTable(DbTablePrefix + "Pagamenti", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Metodo).HasColumnName(nameof(Pagamento.Metodo));
                b.Property(x => x.Stato).HasColumnName(nameof(Pagamento.Stato));
                b.Property(x => x.Importo).HasColumnName(nameof(Pagamento.Importo));
                b.Property(x => x.DataPagamento).HasColumnName(nameof(Pagamento.DataPagamento));
                b.Property(x => x.IdTransazione).HasColumnName(nameof(Pagamento.IdTransazione)).IsRequired().HasMaxLength(PagamentoConsts.IdTransazioneMaxLength);
                b.HasOne<Ordine>().WithMany().HasForeignKey(x => x.OrdineId).OnDelete(DeleteBehavior.SetNull);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Recensione>(b => {
                b.ToTable(DbTablePrefix + "Recensioni", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Valutazione).HasColumnName(nameof(Recensione.Valutazione)).IsRequired().HasMaxLength(RecensioneConsts.ValutazioneMaxLength);
                b.Property(x => x.Commento).HasColumnName(nameof(Recensione.Commento)).HasMaxLength(RecensioneConsts.CommentoMaxLength);
                b.Property(x => x.DataRecensione).HasColumnName(nameof(Recensione.DataRecensione));
                b.HasOne<Cliente>().WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne<Prodotto>().WithMany().HasForeignKey(x => x.ProdottoId).OnDelete(DeleteBehavior.SetNull);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Prodotto>(b => {
                b.ToTable(DbTablePrefix + "Prodotti", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Nome).HasColumnName(nameof(Prodotto.Nome)).IsRequired().HasMaxLength(ProdottoConsts.NomeMaxLength);
                b.Property(x => x.Descrizione).HasColumnName(nameof(Prodotto.Descrizione)).HasMaxLength(ProdottoConsts.DescrizioneMaxLength);
                b.Property(x => x.Prezzo).HasColumnName(nameof(Prodotto.Prezzo)).IsRequired();
                b.Property(x => x.CodiceSku).HasColumnName(nameof(Prodotto.CodiceSku)).HasMaxLength(ProdottoConsts.CodiceSkuMaxLength);
                b.Property(x => x.Sezione).HasColumnName(nameof(Prodotto.Sezione)).IsRequired().HasMaxLength(ProdottoConsts.SezioneMaxLength);
                b.HasOne<Categoria>().WithMany().HasForeignKey(x => x.CategoriaId).OnDelete(DeleteBehavior.SetNull);
                b.HasMany(x => x.VarianteProdotti).WithOne().HasForeignKey(x => x.ProdottoId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.Colleziones).WithOne().HasForeignKey(x => x.ProdottoId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<ProdottoColleziones>(b => {
                b.ToTable(DbTablePrefix + "ProdottoColleziones", DbSchema);
                b.ConfigureByConvention();
                b.HasKey(x => new { x.ProdottoId, x.CollezioneId });
                b.HasOne<Prodotto>().WithMany(x => x.Colleziones).HasForeignKey(x => x.ProdottoId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasOne<Collezione>().WithMany().HasForeignKey(x => x.CollezioneId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(x => new { x.ProdottoId, x.CollezioneId });
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Ordine>(b => {
                b.ToTable(DbTablePrefix + "Ordini", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.DataOrdine).HasColumnName(nameof(Ordine.DataOrdine));
                b.Property(x => x.Stato).HasColumnName(nameof(Ordine.Stato)).HasMaxLength(OrdineConsts.StatoMaxLength);
                b.Property(x => x.ImportoTotale).HasColumnName(nameof(Ordine.ImportoTotale));
                b.Property(x => x.IndSpedizione).HasColumnName(nameof(Ordine.IndSpedizione)).HasMaxLength(OrdineConsts.IndSpedizioneMaxLength);
                b.Property(x => x.MetodoPagamento).HasColumnName(nameof(Ordine.MetodoPagamento)).HasMaxLength(OrdineConsts.MetodoPagamentoMaxLength);
                b.HasOne<Cliente>().WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne<Sconto>().WithMany().HasForeignKey(x => x.ScontoId).OnDelete(DeleteBehavior.SetNull);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<RigaOrdine>(b => {
                b.ToTable(DbTablePrefix + "RigaOrdini", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Quantita).HasColumnName(nameof(RigaOrdine.Quantita)).IsRequired().HasMaxLength(RigaOrdineConsts.QuantitaMaxLength);
                b.Property(x => x.PrezzoUnitario).HasColumnName(nameof(RigaOrdine.PrezzoUnitario)).IsRequired().HasMaxLength((int)RigaOrdineConsts.PrezzoUnitarioMaxLength);
                b.HasOne<Ordine>().WithMany().HasForeignKey(x => x.OrdineId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne<VarianteProdotto>().WithMany().HasForeignKey(x => x.VarianteProdottoId).OnDelete(DeleteBehavior.SetNull);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<ListaDesideri>(b => {
                b.ToTable(DbTablePrefix + "ListeDesideri", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.DataCreazione).HasColumnName(nameof(ListaDesideri.DataCreazione));
                b.Property(x => x.NomeLista).HasColumnName(nameof(ListaDesideri.NomeLista)).IsRequired().HasMaxLength(ListaDesideriConsts.NomeListaMaxLength);
                b.HasOne<Cliente>().WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.SetNull);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<ElementoLista>(b => {
                b.ToTable(DbTablePrefix + "ElementoListe", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.DataAggiunta).HasColumnName(nameof(ElementoLista.DataAggiunta));
                b.HasOne<VarianteProdotto>().WithMany().IsRequired().HasForeignKey(x => x.VarianteProdottoId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<ListaDesideri>().WithMany().HasForeignKey(x => x.ListaDesideriId).OnDelete(DeleteBehavior.SetNull);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<Cliente>(b => {
                b.ToTable(DbTablePrefix + "Clienti", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Nome).HasColumnName(nameof(Cliente.Nome)).IsRequired().HasMaxLength(ClienteConsts.NomeMaxLength);
                b.Property(x => x.Cognome).HasColumnName(nameof(Cliente.Cognome)).IsRequired().HasMaxLength(ClienteConsts.CognomeMaxLength);
                b.Property(x => x.Genere).HasColumnName(nameof(Cliente.Genere)).IsRequired();
                b.Property(x => x.DataNascita).HasColumnName(nameof(Cliente.DataNascita));
                b.Property(x => x.Email).HasColumnName(nameof(Cliente.Email)).IsRequired().HasMaxLength(ClienteConsts.EmailMaxLength);
                b.Property(x => x.Telefono).HasColumnName(nameof(Cliente.Telefono)).IsRequired().HasMaxLength(ClienteConsts.TelefonoMaxLength);
                b.Property(x => x.Sezione).HasColumnName(nameof(Cliente.Sezione)).IsRequired().HasMaxLength(ClienteConsts.SezioneMaxLength);
                b.Property(x => x.Nazionalita).HasColumnName(nameof(Cliente.Nazionalita)).IsRequired();
                b.Property(x => x.UserId).HasColumnName(nameof(Cliente.UserId));
            });
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<ImmagineVariante>(b => {
                b.ToTable(DbTablePrefix + "ImmagineVarianti", DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.VarianteProdottoId).HasColumnName(nameof(ImmagineVariante.VarianteProdottoId));
                b.Property(x => x.Url).HasColumnName(nameof(ImmagineVariante.Url)).IsRequired().HasMaxLength(ImmagineVarianteConsts.UrlMaxLength);
                b.Property(x => x.Ordine).HasColumnName(nameof(ImmagineVariante.Ordine));
                b.HasOne<VarianteProdotto>().WithMany().HasForeignKey(x => x.VarianteProdottoId).OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}