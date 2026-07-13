$(function () {
    var l = abp.localization.getResource('LFG');

    var prodottoService = window.lFG.prodotti.prodotti;

    var lastNpIdId = '';
    var lastNpDisplayNameId = '';

    var _lookupModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Shared/LookupModal',
        scriptUrl: abp.appPath + 'Pages/Shared/lookupModal.js',
        modalClass: 'navigationPropertyLookup',
    });

    $('.lookupCleanButton').on('click', '', function () {
        $(this).parent().find('input').val('');
    });

    _lookupModal.onClose(function () {
        var modal = $(_lookupModal.getModal());
        $('#' + lastNpIdId).val(modal.find('#CurrentLookupId').val());
        $('#' + lastNpDisplayNameId).val(modal.find('#CurrentLookupDisplayName').val());
    });

    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Prodotti/CreateModal',
        scriptUrl: abp.appPath + 'Pages/Prodotti/createModal.js',
        modalClass: 'prodottoCreate',
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Prodotti/EditModal',
        scriptUrl: abp.appPath + 'Pages/Prodotti/editModal.js',
        modalClass: 'prodottoEdit',
    });

    var getFilter = function () {
        return {
            filterText: $('#FilterText').val(),
            nome: $('#NomeFilter').val(),
            descrizione: $('#DescrizioneFilter').val(),
            prezzo: $('#PrezzoFilter').val(),
            codiceSku: $('#CodiceSkuFilter').val(),
            sezione: $('#SezioneFilter').val(),
            categoriaId: $('#CategoriaIdFilter').val(),
            collezioneId: $('#CollezioneIdFilter').val(),
        };
    };

    var dataTableColumns = [
        {
            rowAction: {
                items: [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('LFG.Prodotti.Edit'),
                        action: function (data) {
                            editModal.open({
                                id: data.record.prodotto.id,
                            });
                        },
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted('LFG.Prodotti.Delete'),
                        confirmMessage: function () {
                            return l('DeleteConfirmationMessage');
                        },
                        action: function (data) {
                            prodottoService.delete(data.record.prodotto.id).then(function () {
                                abp.notify.success(l('SuccessfullyDeleted'));
                                dataTable.ajax.reloadEx();
                            });
                        },
                    },
                ],
            },
        },
        { data: 'prodotto.nome' },
        { data: 'prodotto.descrizione' },
        { data: 'prodotto.prezzo' },
        { data: 'prodotto.codiceSku' },
        { data: 'prodotto.sezione' },
        {
            data: 'categoria.nome',

            defaultContent: '',
        },
        {
            data: 'collezione.nome',

            defaultContent: '',
        },
    ];

    var showDetailRows = abp.auth.isGranted('LFG.VarianteProdotti');
    if (showDetailRows) {
        dataTableColumns.unshift({
            class: 'details-control text-center',
            orderable: false,
            data: null,
            defaultContent: '<i class="fa fa-chevron-down"></i>',
            width: '0.1rem',
        });
    } else {
        $('#DetailRowTHeader').remove();
    }

    if (abp.auth.isGranted('LFG.Prodotti.Delete')) {
        dataTableColumns.unshift({
            targets: 0,
            data: null,
            orderable: false,
            className: 'select-checkbox',
            width: '0.5rem',
            render: function (data) {
                return (
                    '<input type="checkbox" class="form-check-input select-row-checkbox" data-id="' +
                    data.prodotto.id +
                    '"/>'
                );
            },
        });
    } else {
        $('#BulkDeleteCheckboxTheader').remove();
    }

    var dataTable = $('#ProdottiTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            responsive: true,
            order: [[3, 'desc']],
            ajax: abp.libs.datatables.createAjax(prodottoService.getList, getFilter),
            columnDefs: dataTableColumns,
        })
    );

    dataTable.on('xhr', function () {
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
        $('#select_all').prop('indeterminate', false);
        $('#select_all').prop('checked', false);
    });

    function selectOrUnselectAllCheckboxes(selectAll) {
        $('.select-row-checkbox').each(function () {
            $(this).prop('checked', selectAll);
        });
    }

    $('#select_all').click(function () {
        if ($(this).is(':checked')) {
            selectOrUnselectAllCheckboxes(true);
        } else {
            $('.select-row-checkbox').each(function () {
                selectOrUnselectAllCheckboxes(false);
            });
        }

        showOrHideContextMenu();
    });

    dataTable.on('change', "input[type='checkbox'].select-row-checkbox", function () {
        var unSelectedCheckboxes = $("input[type='checkbox'].select-row-checkbox:not(:checked)");

        if (unSelectedCheckboxes.length >= 1) {
            var dataRecordTotal = dataTable.context[0].json.data.length;
            if (unSelectedCheckboxes.length === dataRecordTotal) {
                $('#select_all').prop('indeterminate', false);
                $('#select_all').prop('checked', false);
            } else {
                $('#select_all').prop('indeterminate', true);
            }
        } else {
            $('#select_all').prop('indeterminate', false);
            $('#select_all').prop('checked', true);
        }

        showOrHideContextMenu();
    });

    var showOrHideContextMenu = function () {
        var selectedCheckboxes = $("input[type='checkbox'].select-row-checkbox:is(:checked)");
        var selectedCheckboxCount = selectedCheckboxes.length;
        var dataRecordTotal = dataTable.context[0].json.data.length;
        var recordsTotal = dataTable.context[0].json.recordsTotal;

        if (selectedCheckboxCount >= 1) {
            $('#bulk-delete-context-menu').removeClass('d-none');

            $('#items-selected-info-message').html(
                selectedCheckboxCount === 1
                    ? l('OneItemOnThisPageIsSelected')
                    : l('NumberOfItemsOnThisPageAreSelected', selectedCheckboxCount)
            );

            $('#items-selected-info-message').removeClass('d-none');

            if (selectedCheckboxCount === dataRecordTotal && recordsTotal > dataRecordTotal) {
                $('#select-all-items-btn').html(l('SelectAllItems', recordsTotal));
                $('#select-all-items-btn').removeClass('d-none');

                $('#select-all-items-btn').off('click');
                $('#select-all-items-btn').click(function () {
                    $(this).data('selected', true);
                    $(this).addClass('d-none');
                    $('#items-selected-info-message').html(l('AllItemsAreSelected', recordsTotal));
                    $('#clear-selection-btn').removeClass('d-none');
                });

                $('#clear-selection-btn').off('click');
                $('#clear-selection-btn').click(function () {
                    $('#select-all-items-btn').data('selected', false);
                    $('#select_all').prop('checked', false);
                    selectOrUnselectAllCheckboxes(false);
                    showOrHideContextMenu();
                });
            } else {
                $('#select-all-items-btn').addClass('d-none');
                $('#select-all-items-btn').data('selected', false);
                $('#clear-selection-btn').addClass('d-none');
            }

            $('#delete-selected-items').off('click');
            $('#delete-selected-items').click(function () {
                if ($('#select-all-items-btn').data('selected') === true) {
                    abp.message.confirm(l('DeleteAllRecords'), function (confirmed) {
                        if (!confirmed) {
                            return;
                        }

                        prodottoService.deleteAll(getFilter()).then(function () {
                            dataTable.ajax.reloadEx();
                            selectOrUnselectAllCheckboxes(false);
                            showOrHideContextMenu();
                        });
                    });
                } else {
                    var selectedCheckboxes = $(
                        "input[type='checkbox'].select-row-checkbox:is(:checked)"
                    );
                    var selectedRecordsIds = [];

                    for (var i = 0; i < selectedCheckboxes.length; i++) {
                        selectedRecordsIds.push($(selectedCheckboxes[i]).data('id'));
                    }

                    abp.message.confirm(
                        l('DeleteSelectedRecords', selectedCheckboxes.length),
                        function (confirmed) {
                            if (!confirmed) {
                                return;
                            }

                            prodottoService.deleteByIds(selectedRecordsIds).then(function () {
                                dataTable.ajax.reloadEx();
                                selectOrUnselectAllCheckboxes(false);
                                showOrHideContextMenu();
                            });
                        }
                    );
                }
            });
        } else {
            $('#bulk-delete-context-menu').addClass('d-none');
            $('#select-all-items-btn').addClass('d-none');
            $('#items-selected-info-message').addClass('d-none');
            $('#clear-selection-btn').addClass('d-none');
        }
    };

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    $('#NewProdottoButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $('#SearchForm').submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    $('#ExportToExcelButton').click(function (e) {
        e.preventDefault();

        prodottoService.getDownloadToken().then(function (result) {
            var input = getFilter();
            var url =
                abp.appPath +
                'api/app/prodotti/as-excel-file' +
                abp.utils.buildQueryString([
                    { name: 'downloadToken', value: result.token },
                    { name: 'filterText', value: input.filterText },
                    { name: 'nome', value: input.nome },
                    { name: 'descrizione', value: input.descrizione },
                    { name: 'prezzo', value: input.prezzo },
                    { name: 'codiceSku', value: input.codiceSku },
                    { name: 'sezione', value: input.sezione },
                    { name: 'categoriaId', value: input.categoriaId },
                    { name: 'collezioneId', value: input.collezioneId },
                ]);

            var downloadWindow = window.open(url, '_blank');
            downloadWindow.focus();
        });
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
        var iconCss = $('#AdvancedFilterSection').is(':visible')
            ? 'fa ms-1 fa-angle-up'
            : 'fa ms-1 fa-angle-down';
        $(this).find('i').attr('class', iconCss);
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reloadEx();
            selectOrUnselectAllCheckboxes(false);
            showOrHideContextMenu();
        }
    });

    $('#AdvancedFilterSection select').change(function () {
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    $('#ProdottiTable').on('click', 'td.details-control', function () {
        $(this).find('i').toggleClass('fa-chevron-down').toggleClass('fa-chevron-up');

        var tr = $(this).parents('tr');
        var row = dataTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            var data = row.data();

            detailRows(data).done(function (result) {
                row.child(result).show();
                initDataGrids(data);
            });

            tr.addClass('shown');
        }
    });

    function detailRows(data) {
        return $.ajax(abp.appPath + 'Prodotti/ChildDataGrid?prodottoId=' + data.prodotto.id).done(
            function (result) {
                return result;
            }
        );
    }

    function initDataGrids(data) {
        initVarianteProdottoGrid(data);
    }

    //<suite-custom-code-block-1>
    //</suite-custom-code-block-1>

    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    //<suite-custom-code-block-3>
    //</suite-custom-code-block-3>

    function initVarianteProdottoGrid(data) {
        if (!abp.auth.isGranted('LFG.VarianteProdotti')) {
            return;
        }

        var prodottoId = data.prodotto.id;

        var varianteProdottoService = window.lFG.varianteProdotti.varianteProdotti;

        var varianteProdottoCreateModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'VarianteProdotti/CreateModal',
            scriptUrl: abp.appPath + 'Pages/VarianteProdotti/createModal.js',
            modalClass: 'varianteProdottoCreate',
        });

        var varianteProdottoEditModal = new abp.ModalManager({
            viewUrl: abp.appPath + 'VarianteProdotti/EditModal',
            scriptUrl: abp.appPath + 'Pages/VarianteProdotti/editModal.js',
            modalClass: 'varianteProdottoEdit',
        });

        var varianteProdottoDataTable = $('#VarianteProdottiTable-' + prodottoId).DataTable(
            abp.libs.datatables.normalizeConfiguration({
                processing: true,
                serverSide: true,
                paging: true,
                searching: false,
                responsive: true,
                scrollX: true,
                autoWidth: true,
                scrollCollapse: true,
                order: [[1, 'asc']],
                ajax: abp.libs.datatables.createAjax(varianteProdottoService.getListByProdottoId, {
                    prodottoId: prodottoId,
                    maxResultCount: 5,
                }),
                columnDefs: [
                    {
                        rowAction: {
                            items: [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('LFG.VarianteProdotti.Edit'),
                                    action: function (data) {
                                        varianteProdottoEditModal.open({
                                            id: data.record.id,
                                        });
                                    },
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('LFG.VarianteProdotti.Delete'),
                                    confirmMessage: function () {
                                        return l('DeleteConfirmationMessage');
                                    },
                                    action: function (data) {
                                        varianteProdottoService
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l('SuccessfullyDeleted'));
                                                varianteProdottoDataTable.ajax.reloadEx();
                                            });
                                    },
                                },
                            ],
                        },
                        width: '1rem',
                    },
                    { data: 'taglia', width: '0.1rem' },
                    { data: 'colore', width: '0.1rem' },
                    { data: 'materiale', width: '0.1rem' },
                    { data: 'urlImmagine', width: '0.1rem' },
                    { data: 'qtaMagazzino', width: '0.1rem' },
                ],
            })
        );

        varianteProdottoCreateModal.onResult(function () {
            varianteProdottoDataTable.ajax.reloadEx();
        });

        varianteProdottoEditModal.onResult(function () {
            varianteProdottoDataTable.ajax.reloadEx();
        });

        $('button.NewVarianteProdottoButton')
            .off('click')
            .on('click', function (e) {
                varianteProdottoCreateModal.open({
                    prodottoId: $(this).data('prodotto-id'),
                });
            });
    }
});
