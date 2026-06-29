var abp = abp || {};

//<suite-custom-code-block-1>
//</suite-custom-code-block-1>

abp.modals.prodottoEdit = function () {
    var initModal = function (publicApi, args) {
        var l = abp.localization.getResource('LFG');

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

        publicApi.onOpen(function () {
            $('#CollezionesLookup').select2({
                dropdownParent: $('#ProdottoEditModal'),
                ajax: {
                    url: abp.appPath + 'api/app/prodotti/colleziones-lookup',
                    type: 'GET',
                    data: function (params) {
                        return { filter: params.term, maxResultCount: 10 };
                    },
                    processResults: function (data) {
                        var mappedItems = _.map(data.items, function (item) {
                            return { id: item.id, text: item.displayName };
                        });

                        return { results: mappedItems };
                    },
                },
            });
        });

        var getNewCollezionesIndex = function () {
            var idTds = $($(document).find('#CollezionesTableRows')).find('td[name="id"]');

            if (idTds.length === 0) {
                return 0;
            }

            return parseInt($(idTds[idTds.length - 1]).attr('index')) + 1;
        };

        var getCollezionesIds = function () {
            var ids = [];
            var idTds = $("#CollezionesTableRows td[name='id']");

            for (var i = 0; i < idTds.length; i++) {
                ids.push(idTds[i].innerHTML.trim());
            }

            return ids;
        };

        $('#AddCollezionesButton').on('click', '', function () {
            var $select = $('#CollezionesLookup');
            var id = $select.val();
            var existingIds = getCollezionesIds();
            if (!id || id === '') {
                return;
            }

            if (existingIds.indexOf(id) >= 0) {
                abp.message.warn(l('ItemAlreadyAdded'));
                return;
            }

            $('#CollezionesTable').show();

            var displayName = $select.find('option').filter(':selected')[0].innerHTML;

            var newIndex = getNewCollezionesIndex();

            $('#CollezionesTableRows').append(
                '                                <tr style="text-align: center; vertical-align: middle;" index="' +
                    newIndex +
                    '">\n' +
                    '                                    <td style="display: none" name="id" index="' +
                    newIndex +
                    '">' +
                    id +
                    '</td>\n' +
                    '                                    <td style="display: none">' +
                    '                                        <input value="' +
                    id +
                    '" id="SelectedCollezionesIds[' +
                    newIndex +
                    ']" name="SelectedCollezionesIds[' +
                    newIndex +
                    ']"/>\n' +
                    '                                    </td>\n' +
                    '                                    <td style="text-align: left">' +
                    displayName +
                    '</td>\n' +
                    '                                    <td style="text-align: right">\n' +
                    '                                        <button class="btn btn-danger btn-sm text-light collezionesDeleteButton" index="' +
                    newIndex +
                    '"> <i class="fa fa-trash"></i> </button>\n' +
                    '                                    </td>\n' +
                    '                                </tr>'
            );
        });

        $(document).on('click', '.collezionesDeleteButton', function (e) {
            e.preventDefault();
            var index = $(this).attr('index');
            $('#CollezionesTableRows')
                .find('tr[index=' + index + ']')
                .remove();

            setTimeout(function () {
                var rows = $(document).find('#CollezionesTableRows').find('tr');

                if (rows.length === 0) {
                    $('#CollezionesTable').hide();
                }

                for (var i = 0; i < rows.length; i++) {
                    $(rows[i]).attr('index', i);
                    $(rows[i]).find('th[scope="Row"]').empty();
                    $(rows[i])
                        .find('th[scope="Row"]')
                        .append(i + 1);
                    $($(rows[i]).find('td[name="id"]')).attr('index', i);
                    $($(rows[i]).find('input')).attr('id', 'SelectedCollezionesIds[' + i + ']');
                    $($(rows[i]).find('input')).attr('name', 'SelectedCollezionesIds[' + i + ']');
                    $($(rows[i]).find('button')).attr('index', i);
                }
            }, 200);
        });
    };

    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    return {
        initModal: initModal,
    };
};

//<suite-custom-code-block-3>
//</suite-custom-code-block-3>
