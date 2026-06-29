using LFG.ElementoListe;
using LFG.Web.Pages.ElementoListe;
using LFG.ElementoListe;
using LFG.ListeDesideri;
using LFG.Web.Pages.ListeDesideri;
using LFG.RigaOrdini;
using LFG.Web.Pages.RigaOrdini;
using LFG.RigaOrdini;
using LFG.Ordini;
using LFG.Web.Pages.Ordini;
using LFG.VarianteProdotti;
using LFG.Web.Pages.VarianteProdotti;
using LFG.Prodotti;
using LFG.Web.Pages.Prodotti;
using LFG.Recensioni;
using LFG.Web.Pages.Recensioni;
using LFG.Recensioni;
using LFG.Pagamenti;
using LFG.Web.Pages.Pagamenti;
using LFG.Pagamenti;
using LFG.Ordini;
using LFG.Web.Pages.Ordini;
using LFG.Ordini;
using LFG.ListeDesideri;
using LFG.Web.Pages.ListeDesideri;
using LFG.ListeDesideri;
using LFG.Indirizzi;
using LFG.Web.Pages.Indirizzi;
using LFG.Indirizzi;
using LFG.VarianteProdotti;
using LFG.Web.Pages.VarianteProdotti;
using LFG.Prodotti;
using LFG.Web.Pages.Prodotti;
using LFG.Prodotti;
using LFG.Web.Pages.Prodotti;
using LFG.Prodotti;
using LFG.Clienti;
using LFG.Web.Pages.Clienti;
using LFG.Clienti;
using LFG.Sconti;
using LFG.Web.Pages.Sconti;
using LFG.Sconti;
using LFG.Collezioni;
using LFG.Web.Pages.Collezioni;
using LFG.Collezioni;
using LFG.Categorie;
using LFG.Web.Pages.Categorie;
using System;
using LFG.Shared;
using LFG.Categorie;
using System.Linq;
using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using LFG.Entities.Books;
using LFG.Services.Dtos.Books;

namespace LFG.ObjectMapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LFGBookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);
    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LFGCreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);
    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LFGBookDtoToCreateUpdateBookDtoMapper : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);
    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}

[Mapper]
public partial class CategoriaToCategoriaDtoMappers : MapperBase<Categoria, CategoriaDto>
{
    public override partial CategoriaDto Map(Categoria source);
    public override partial void Map(Categoria source, CategoriaDto destination);
}

[Mapper]
public partial class CategoriaToCategoriaExcelDtoMappers : MapperBase<Categoria, CategoriaExcelDto>
{
    public override partial CategoriaExcelDto Map(Categoria source);
    public override partial void Map(Categoria source, CategoriaExcelDto destination);
}

[Mapper]
public partial class CategoriaDtoToCategoriaUpdateViewModelMapper : MapperBase<CategoriaDto, CategoriaUpdateViewModel>
{
    public override partial CategoriaUpdateViewModel Map(CategoriaDto source);
    public override partial void Map(CategoriaDto source, CategoriaUpdateViewModel destination);
}

[Mapper]
public partial class CategoriaUpdateViewModelToCategoriaUpdateDto : MapperBase<CategoriaUpdateViewModel, CategoriaUpdateDto>
{
    public override partial CategoriaUpdateDto Map(CategoriaUpdateViewModel source);
    public override partial void Map(CategoriaUpdateViewModel source, CategoriaUpdateDto destination);
}

[Mapper]
public partial class CategoriaCreateViewModelToCategoriaCreateDto : MapperBase<CategoriaCreateViewModel, CategoriaCreateDto>
{
    public override partial CategoriaCreateDto Map(CategoriaCreateViewModel source);
    public override partial void Map(CategoriaCreateViewModel source, CategoriaCreateDto destination);
}

[Mapper]
public partial class CollezioneToCollezioneDtoMappers : MapperBase<Collezione, CollezioneDto>
{
    public override partial CollezioneDto Map(Collezione source);
    public override partial void Map(Collezione source, CollezioneDto destination);
}

[Mapper]
public partial class CollezioneToCollezioneExcelDtoMappers : MapperBase<Collezione, CollezioneExcelDto>
{
    public override partial CollezioneExcelDto Map(Collezione source);
    public override partial void Map(Collezione source, CollezioneExcelDto destination);
}

[Mapper]
public partial class CollezioneDtoToCollezioneUpdateViewModelMapper : MapperBase<CollezioneDto, CollezioneUpdateViewModel>
{
    public override partial CollezioneUpdateViewModel Map(CollezioneDto source);
    public override partial void Map(CollezioneDto source, CollezioneUpdateViewModel destination);
}

[Mapper]
public partial class CollezioneUpdateViewModelToCollezioneUpdateDto : MapperBase<CollezioneUpdateViewModel, CollezioneUpdateDto>
{
    public override partial CollezioneUpdateDto Map(CollezioneUpdateViewModel source);
    public override partial void Map(CollezioneUpdateViewModel source, CollezioneUpdateDto destination);
}

[Mapper]
public partial class CollezioneCreateViewModelToCollezioneCreateDto : MapperBase<CollezioneCreateViewModel, CollezioneCreateDto>
{
    public override partial CollezioneCreateDto Map(CollezioneCreateViewModel source);
    public override partial void Map(CollezioneCreateViewModel source, CollezioneCreateDto destination);
}

[Mapper]
public partial class ScontoToScontoDtoMappers : MapperBase<Sconto, ScontoDto>
{
    public override partial ScontoDto Map(Sconto source);
    public override partial void Map(Sconto source, ScontoDto destination);
}

[Mapper]
public partial class ScontoToScontoExcelDtoMappers : MapperBase<Sconto, ScontoExcelDto>
{
    public override partial ScontoExcelDto Map(Sconto source);
    public override partial void Map(Sconto source, ScontoExcelDto destination);
}

[Mapper]
public partial class ScontoDtoToScontoUpdateViewModelMapper : MapperBase<ScontoDto, ScontoUpdateViewModel>
{
    public override partial ScontoUpdateViewModel Map(ScontoDto source);
    public override partial void Map(ScontoDto source, ScontoUpdateViewModel destination);
}

[Mapper]
public partial class ScontoUpdateViewModelToScontoUpdateDto : MapperBase<ScontoUpdateViewModel, ScontoUpdateDto>
{
    public override partial ScontoUpdateDto Map(ScontoUpdateViewModel source);
    public override partial void Map(ScontoUpdateViewModel source, ScontoUpdateDto destination);
}

[Mapper]
public partial class ScontoCreateViewModelToScontoCreateDto : MapperBase<ScontoCreateViewModel, ScontoCreateDto>
{
    public override partial ScontoCreateDto Map(ScontoCreateViewModel source);
    public override partial void Map(ScontoCreateViewModel source, ScontoCreateDto destination);
}

[Mapper]
public partial class ClienteToClienteDtoMappers : MapperBase<Cliente, ClienteDto>
{
    public override partial ClienteDto Map(Cliente source);
    public override partial void Map(Cliente source, ClienteDto destination);
}

[Mapper]
public partial class ClienteToClienteExcelDtoMappers : MapperBase<Cliente, ClienteExcelDto>
{
    public override partial ClienteExcelDto Map(Cliente source);
    public override partial void Map(Cliente source, ClienteExcelDto destination);
}

[Mapper]
public partial class ClienteDtoToClienteUpdateViewModelMapper : MapperBase<ClienteDto, ClienteUpdateViewModel>
{
    public override partial ClienteUpdateViewModel Map(ClienteDto source);
    public override partial void Map(ClienteDto source, ClienteUpdateViewModel destination);
}

[Mapper]
public partial class ClienteUpdateViewModelToClienteUpdateDto : MapperBase<ClienteUpdateViewModel, ClienteUpdateDto>
{
    public override partial ClienteUpdateDto Map(ClienteUpdateViewModel source);
    public override partial void Map(ClienteUpdateViewModel source, ClienteUpdateDto destination);
}

[Mapper]
public partial class ClienteCreateViewModelToClienteCreateDto : MapperBase<ClienteCreateViewModel, ClienteCreateDto>
{
    public override partial ClienteCreateDto Map(ClienteCreateViewModel source);
    public override partial void Map(ClienteCreateViewModel source, ClienteCreateDto destination);
}

[Mapper]
public partial class ProdottoToProdottoDtoMappers : MapperBase<Prodotto, ProdottoDto>
{
    [MapperIgnoreTarget(nameof(ProdottoDto.VarianteProdotti))]
    public override partial ProdottoDto Map(Prodotto source);
    [MapperIgnoreTarget(nameof(ProdottoDto.VarianteProdotti))]
    public override partial void Map(Prodotto source, ProdottoDto destination);
}

[Mapper]
public partial class ProdottoToProdottoExcelDtoMappers : MapperBase<Prodotto, ProdottoExcelDto>
{
    public override partial ProdottoExcelDto Map(Prodotto source);
    public override partial void Map(Prodotto source, ProdottoExcelDto destination);
}

[Mapper]
public partial class ProdottoWithNavigationPropertiesToProdottoWithNavigationPropertiesDtoMapper : MapperBase<ProdottoWithNavigationProperties, ProdottoWithNavigationPropertiesDto>
{
    public override partial ProdottoWithNavigationPropertiesDto Map(ProdottoWithNavigationProperties source);
    public override partial void Map(ProdottoWithNavigationProperties source, ProdottoWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class CategoriaToLookupDtoGuidMapper : MapperBase<Categoria, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(Categoria source);
    public override partial void Map(Categoria source, LookupDto<Guid> destination);

    public override void AfterMap(Categoria source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.Nome;
    }
}

[Mapper]
public partial class ProdottoDtoToProdottoUpdateViewModelMapper : MapperBase<ProdottoDto, ProdottoUpdateViewModel>
{
    [MapperIgnoreTarget(nameof(ProdottoUpdateViewModel.CollezionesIds))]
    public override partial ProdottoUpdateViewModel Map(ProdottoDto source);
    [MapperIgnoreTarget(nameof(ProdottoUpdateViewModel.CollezionesIds))]
    public override partial void Map(ProdottoDto source, ProdottoUpdateViewModel destination);
}

[Mapper]
public partial class ProdottoUpdateViewModelToProdottoUpdateDto : MapperBase<ProdottoUpdateViewModel, ProdottoUpdateDto>
{
    public override partial ProdottoUpdateDto Map(ProdottoUpdateViewModel source);
    public override partial void Map(ProdottoUpdateViewModel source, ProdottoUpdateDto destination);
}

[Mapper]
public partial class ProdottoCreateViewModelToProdottoCreateDto : MapperBase<ProdottoCreateViewModel, ProdottoCreateDto>
{
    public override partial ProdottoCreateDto Map(ProdottoCreateViewModel source);
    public override partial void Map(ProdottoCreateViewModel source, ProdottoCreateDto destination);
}

[Mapper]
public partial class VarianteProdottoDtoToVarianteProdottoUpdateViewModelMapper : MapperBase<VarianteProdottoDto, VarianteProdottoUpdateViewModel>
{
    public override partial VarianteProdottoUpdateViewModel Map(VarianteProdottoDto source);
    public override partial void Map(VarianteProdottoDto source, VarianteProdottoUpdateViewModel destination);
}

[Mapper]
public partial class VarianteProdottoUpdateViewModelToVarianteProdottoUpdateDto : MapperBase<VarianteProdottoUpdateViewModel, VarianteProdottoUpdateDto>
{
    public override partial VarianteProdottoUpdateDto Map(VarianteProdottoUpdateViewModel source);
    public override partial void Map(VarianteProdottoUpdateViewModel source, VarianteProdottoUpdateDto destination);
}

[Mapper]
public partial class VarianteProdottoCreateViewModelToVarianteProdottoCreateDto : MapperBase<VarianteProdottoCreateViewModel, VarianteProdottoCreateDto>
{
    public override partial VarianteProdottoCreateDto Map(VarianteProdottoCreateViewModel source);
    public override partial void Map(VarianteProdottoCreateViewModel source, VarianteProdottoCreateDto destination);
}

[Mapper]
public partial class VarianteProdottoToVarianteProdottoDtoMappers : MapperBase<VarianteProdotto, VarianteProdottoDto>
{
    public override partial VarianteProdottoDto Map(VarianteProdotto source);
    public override partial void Map(VarianteProdotto source, VarianteProdottoDto destination);
}

[Mapper]
public partial class IndirizzoToIndirizzoDtoMappers : MapperBase<Indirizzo, IndirizzoDto>
{
    public override partial IndirizzoDto Map(Indirizzo source);
    public override partial void Map(Indirizzo source, IndirizzoDto destination);
}

[Mapper]
public partial class IndirizzoToIndirizzoExcelDtoMappers : MapperBase<Indirizzo, IndirizzoExcelDto>
{
    public override partial IndirizzoExcelDto Map(Indirizzo source);
    public override partial void Map(Indirizzo source, IndirizzoExcelDto destination);
}

[Mapper]
public partial class IndirizzoWithNavigationPropertiesToIndirizzoWithNavigationPropertiesDtoMapper : MapperBase<IndirizzoWithNavigationProperties, IndirizzoWithNavigationPropertiesDto>
{
    public override partial IndirizzoWithNavigationPropertiesDto Map(IndirizzoWithNavigationProperties source);
    public override partial void Map(IndirizzoWithNavigationProperties source, IndirizzoWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class ClienteToLookupDtoGuidMapper : MapperBase<Cliente, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(Cliente source);
    public override partial void Map(Cliente source, LookupDto<Guid> destination);

    public override void AfterMap(Cliente source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.Email;
    }
}

[Mapper]
public partial class IndirizzoDtoToIndirizzoUpdateViewModelMapper : MapperBase<IndirizzoDto, IndirizzoUpdateViewModel>
{
    public override partial IndirizzoUpdateViewModel Map(IndirizzoDto source);
    public override partial void Map(IndirizzoDto source, IndirizzoUpdateViewModel destination);
}

[Mapper]
public partial class IndirizzoUpdateViewModelToIndirizzoUpdateDto : MapperBase<IndirizzoUpdateViewModel, IndirizzoUpdateDto>
{
    public override partial IndirizzoUpdateDto Map(IndirizzoUpdateViewModel source);
    public override partial void Map(IndirizzoUpdateViewModel source, IndirizzoUpdateDto destination);
}

[Mapper]
public partial class IndirizzoCreateViewModelToIndirizzoCreateDto : MapperBase<IndirizzoCreateViewModel, IndirizzoCreateDto>
{
    public override partial IndirizzoCreateDto Map(IndirizzoCreateViewModel source);
    public override partial void Map(IndirizzoCreateViewModel source, IndirizzoCreateDto destination);
}

[Mapper]
public partial class ListaDesideriToListaDesideriDtoMappers : MapperBase<ListaDesideri, ListaDesideriDto>
{
    public override partial ListaDesideriDto Map(ListaDesideri source);
    public override partial void Map(ListaDesideri source, ListaDesideriDto destination);
}

[Mapper]
public partial class ListaDesideriToListaDesideriExcelDtoMappers : MapperBase<ListaDesideri, ListaDesideriExcelDto>
{
    public override partial ListaDesideriExcelDto Map(ListaDesideri source);
    public override partial void Map(ListaDesideri source, ListaDesideriExcelDto destination);
}

[Mapper]
public partial class ListaDesideriWithNavigationPropertiesToListaDesideriWithNavigationPropertiesDtoMapper : MapperBase<ListaDesideriWithNavigationProperties, ListaDesideriWithNavigationPropertiesDto>
{
    public override partial ListaDesideriWithNavigationPropertiesDto Map(ListaDesideriWithNavigationProperties source);
    public override partial void Map(ListaDesideriWithNavigationProperties source, ListaDesideriWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class ListaDesideriDtoToListaDesideriUpdateViewModelMapper : MapperBase<ListaDesideriDto, ListaDesideriUpdateViewModel>
{
    public override partial ListaDesideriUpdateViewModel Map(ListaDesideriDto source);
    public override partial void Map(ListaDesideriDto source, ListaDesideriUpdateViewModel destination);
}

[Mapper]
public partial class ListaDesideriUpdateViewModelToListaDesideriUpdateDto : MapperBase<ListaDesideriUpdateViewModel, ListaDesideriUpdateDto>
{
    public override partial ListaDesideriUpdateDto Map(ListaDesideriUpdateViewModel source);
    public override partial void Map(ListaDesideriUpdateViewModel source, ListaDesideriUpdateDto destination);
}

[Mapper]
public partial class ListaDesideriCreateViewModelToListaDesideriCreateDto : MapperBase<ListaDesideriCreateViewModel, ListaDesideriCreateDto>
{
    public override partial ListaDesideriCreateDto Map(ListaDesideriCreateViewModel source);
    public override partial void Map(ListaDesideriCreateViewModel source, ListaDesideriCreateDto destination);
}

[Mapper]
public partial class OrdineToOrdineDtoMappers : MapperBase<Ordine, OrdineDto>
{
    public override partial OrdineDto Map(Ordine source);
    public override partial void Map(Ordine source, OrdineDto destination);
}

[Mapper]
public partial class OrdineToOrdineExcelDtoMappers : MapperBase<Ordine, OrdineExcelDto>
{
    public override partial OrdineExcelDto Map(Ordine source);
    public override partial void Map(Ordine source, OrdineExcelDto destination);
}

[Mapper]
public partial class OrdineWithNavigationPropertiesToOrdineWithNavigationPropertiesDtoMapper : MapperBase<OrdineWithNavigationProperties, OrdineWithNavigationPropertiesDto>
{
    public override partial OrdineWithNavigationPropertiesDto Map(OrdineWithNavigationProperties source);
    public override partial void Map(OrdineWithNavigationProperties source, OrdineWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class OrdineDtoToOrdineUpdateViewModelMapper : MapperBase<OrdineDto, OrdineUpdateViewModel>
{
    public override partial OrdineUpdateViewModel Map(OrdineDto source);
    public override partial void Map(OrdineDto source, OrdineUpdateViewModel destination);
}

[Mapper]
public partial class OrdineUpdateViewModelToOrdineUpdateDto : MapperBase<OrdineUpdateViewModel, OrdineUpdateDto>
{
    public override partial OrdineUpdateDto Map(OrdineUpdateViewModel source);
    public override partial void Map(OrdineUpdateViewModel source, OrdineUpdateDto destination);
}

[Mapper]
public partial class OrdineCreateViewModelToOrdineCreateDto : MapperBase<OrdineCreateViewModel, OrdineCreateDto>
{
    public override partial OrdineCreateDto Map(OrdineCreateViewModel source);
    public override partial void Map(OrdineCreateViewModel source, OrdineCreateDto destination);
}

[Mapper]
public partial class PagamentoToPagamentoDtoMappers : MapperBase<Pagamento, PagamentoDto>
{
    public override partial PagamentoDto Map(Pagamento source);
    public override partial void Map(Pagamento source, PagamentoDto destination);
}

[Mapper]
public partial class PagamentoToPagamentoExcelDtoMappers : MapperBase<Pagamento, PagamentoExcelDto>
{
    public override partial PagamentoExcelDto Map(Pagamento source);
    public override partial void Map(Pagamento source, PagamentoExcelDto destination);
}

[Mapper]
public partial class PagamentoWithNavigationPropertiesToPagamentoWithNavigationPropertiesDtoMapper : MapperBase<PagamentoWithNavigationProperties, PagamentoWithNavigationPropertiesDto>
{
    public override partial PagamentoWithNavigationPropertiesDto Map(PagamentoWithNavigationProperties source);
    public override partial void Map(PagamentoWithNavigationProperties source, PagamentoWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class OrdineToLookupDtoGuidMapper : MapperBase<Ordine, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(Ordine source);
    public override partial void Map(Ordine source, LookupDto<Guid> destination);

    public override void AfterMap(Ordine source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.IndSpedizione;
    }
}

[Mapper]
public partial class PagamentoDtoToPagamentoUpdateViewModelMapper : MapperBase<PagamentoDto, PagamentoUpdateViewModel>
{
    public override partial PagamentoUpdateViewModel Map(PagamentoDto source);
    public override partial void Map(PagamentoDto source, PagamentoUpdateViewModel destination);
}

[Mapper]
public partial class PagamentoUpdateViewModelToPagamentoUpdateDto : MapperBase<PagamentoUpdateViewModel, PagamentoUpdateDto>
{
    public override partial PagamentoUpdateDto Map(PagamentoUpdateViewModel source);
    public override partial void Map(PagamentoUpdateViewModel source, PagamentoUpdateDto destination);
}

[Mapper]
public partial class PagamentoCreateViewModelToPagamentoCreateDto : MapperBase<PagamentoCreateViewModel, PagamentoCreateDto>
{
    public override partial PagamentoCreateDto Map(PagamentoCreateViewModel source);
    public override partial void Map(PagamentoCreateViewModel source, PagamentoCreateDto destination);
}

[Mapper]
public partial class RecensioneToRecensioneDtoMappers : MapperBase<Recensione, RecensioneDto>
{
    public override partial RecensioneDto Map(Recensione source);
    public override partial void Map(Recensione source, RecensioneDto destination);
}

[Mapper]
public partial class RecensioneToRecensioneExcelDtoMappers : MapperBase<Recensione, RecensioneExcelDto>
{
    public override partial RecensioneExcelDto Map(Recensione source);
    public override partial void Map(Recensione source, RecensioneExcelDto destination);
}

[Mapper]
public partial class RecensioneWithNavigationPropertiesToRecensioneWithNavigationPropertiesDtoMapper : MapperBase<RecensioneWithNavigationProperties, RecensioneWithNavigationPropertiesDto>
{
    public override partial RecensioneWithNavigationPropertiesDto Map(RecensioneWithNavigationProperties source);
    public override partial void Map(RecensioneWithNavigationProperties source, RecensioneWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class ProdottoToLookupDtoGuidMapper : MapperBase<Prodotto, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(Prodotto source);
    public override partial void Map(Prodotto source, LookupDto<Guid> destination);

    public override void AfterMap(Prodotto source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.Nome;
    }
}

[Mapper]
public partial class RecensioneDtoToRecensioneUpdateViewModelMapper : MapperBase<RecensioneDto, RecensioneUpdateViewModel>
{
    public override partial RecensioneUpdateViewModel Map(RecensioneDto source);
    public override partial void Map(RecensioneDto source, RecensioneUpdateViewModel destination);
}

[Mapper]
public partial class RecensioneUpdateViewModelToRecensioneUpdateDto : MapperBase<RecensioneUpdateViewModel, RecensioneUpdateDto>
{
    public override partial RecensioneUpdateDto Map(RecensioneUpdateViewModel source);
    public override partial void Map(RecensioneUpdateViewModel source, RecensioneUpdateDto destination);
}

[Mapper]
public partial class RecensioneCreateViewModelToRecensioneCreateDto : MapperBase<RecensioneCreateViewModel, RecensioneCreateDto>
{
    public override partial RecensioneCreateDto Map(RecensioneCreateViewModel source);
    public override partial void Map(RecensioneCreateViewModel source, RecensioneCreateDto destination);
}

[Mapper]
public partial class CollezioneToLookupDtoGuidMapper : MapperBase<Collezione, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(Collezione source);
    public override partial void Map(Collezione source, LookupDto<Guid> destination);

    public override void AfterMap(Collezione source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.Nome;
    }
}

[Mapper]
public partial class ScontoToLookupDtoGuidMapper : MapperBase<Sconto, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(Sconto source);
    public override partial void Map(Sconto source, LookupDto<Guid> destination);

    public override void AfterMap(Sconto source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.Codice;
    }
}

[Mapper]
public partial class RigaOrdineToRigaOrdineDtoMappers : MapperBase<RigaOrdine, RigaOrdineDto>
{
    public override partial RigaOrdineDto Map(RigaOrdine source);
    public override partial void Map(RigaOrdine source, RigaOrdineDto destination);
}

[Mapper]
public partial class RigaOrdineToRigaOrdineExcelDtoMappers : MapperBase<RigaOrdine, RigaOrdineExcelDto>
{
    public override partial RigaOrdineExcelDto Map(RigaOrdine source);
    public override partial void Map(RigaOrdine source, RigaOrdineExcelDto destination);
}

[Mapper]
public partial class RigaOrdineWithNavigationPropertiesToRigaOrdineWithNavigationPropertiesDtoMapper : MapperBase<RigaOrdineWithNavigationProperties, RigaOrdineWithNavigationPropertiesDto>
{
    public override partial RigaOrdineWithNavigationPropertiesDto Map(RigaOrdineWithNavigationProperties source);
    public override partial void Map(RigaOrdineWithNavigationProperties source, RigaOrdineWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class VarianteProdottoToLookupDtoGuidMapper : MapperBase<VarianteProdotto, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(VarianteProdotto source);
    public override partial void Map(VarianteProdotto source, LookupDto<Guid> destination);

    public override void AfterMap(VarianteProdotto source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.UrlImmagine;
    }
}

[Mapper]
public partial class RigaOrdineDtoToRigaOrdineUpdateViewModelMapper : MapperBase<RigaOrdineDto, RigaOrdineUpdateViewModel>
{
    public override partial RigaOrdineUpdateViewModel Map(RigaOrdineDto source);
    public override partial void Map(RigaOrdineDto source, RigaOrdineUpdateViewModel destination);
}

[Mapper]
public partial class RigaOrdineUpdateViewModelToRigaOrdineUpdateDto : MapperBase<RigaOrdineUpdateViewModel, RigaOrdineUpdateDto>
{
    public override partial RigaOrdineUpdateDto Map(RigaOrdineUpdateViewModel source);
    public override partial void Map(RigaOrdineUpdateViewModel source, RigaOrdineUpdateDto destination);
}

[Mapper]
public partial class RigaOrdineCreateViewModelToRigaOrdineCreateDto : MapperBase<RigaOrdineCreateViewModel, RigaOrdineCreateDto>
{
    public override partial RigaOrdineCreateDto Map(RigaOrdineCreateViewModel source);
    public override partial void Map(RigaOrdineCreateViewModel source, RigaOrdineCreateDto destination);
}

[Mapper]
public partial class ElementoListaToElementoListaDtoMappers : MapperBase<ElementoLista, ElementoListaDto>
{
    public override partial ElementoListaDto Map(ElementoLista source);
    public override partial void Map(ElementoLista source, ElementoListaDto destination);
}

[Mapper]
public partial class ElementoListaToElementoListaExcelDtoMappers : MapperBase<ElementoLista, ElementoListaExcelDto>
{
    public override partial ElementoListaExcelDto Map(ElementoLista source);
    public override partial void Map(ElementoLista source, ElementoListaExcelDto destination);
}

[Mapper]
public partial class ElementoListaWithNavigationPropertiesToElementoListaWithNavigationPropertiesDtoMapper : MapperBase<ElementoListaWithNavigationProperties, ElementoListaWithNavigationPropertiesDto>
{
    public override partial ElementoListaWithNavigationPropertiesDto Map(ElementoListaWithNavigationProperties source);
    public override partial void Map(ElementoListaWithNavigationProperties source, ElementoListaWithNavigationPropertiesDto destination);
}

[Mapper]
public partial class ListaDesideriToLookupDtoGuidMapper : MapperBase<ListaDesideri, LookupDto<Guid>>
{
    public override partial LookupDto<Guid> Map(ListaDesideri source);
    public override partial void Map(ListaDesideri source, LookupDto<Guid> destination);

    public override void AfterMap(ListaDesideri source, LookupDto<Guid> destination)
    {
        destination.DisplayName = source.NomeLista;
    }
}

[Mapper]
public partial class ElementoListaDtoToElementoListaUpdateViewModelMapper : MapperBase<ElementoListaDto, ElementoListaUpdateViewModel>
{
    public override partial ElementoListaUpdateViewModel Map(ElementoListaDto source);
    public override partial void Map(ElementoListaDto source, ElementoListaUpdateViewModel destination);
}

[Mapper]
public partial class ElementoListaUpdateViewModelToElementoListaUpdateDto : MapperBase<ElementoListaUpdateViewModel, ElementoListaUpdateDto>
{
    public override partial ElementoListaUpdateDto Map(ElementoListaUpdateViewModel source);
    public override partial void Map(ElementoListaUpdateViewModel source, ElementoListaUpdateDto destination);
}

[Mapper]
public partial class ElementoListaCreateViewModelToElementoListaCreateDto : MapperBase<ElementoListaCreateViewModel, ElementoListaCreateDto>
{
    public override partial ElementoListaCreateDto Map(ElementoListaCreateViewModel source);
    public override partial void Map(ElementoListaCreateViewModel source, ElementoListaCreateDto destination);
}