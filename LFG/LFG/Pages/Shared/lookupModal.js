var abp = abp || {};

//<suite-custom-code-block-1>
//</suite-custom-code-block-1>

abp.modals.navigationPropertyLookup = function () {
    var initModal = function (publicApi, args) {
        var l = abp.localization.getResource('LFG');

        $('#NavigationPropertyLookupTableModal #LookupTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                processing: true,
                serverSide: true,
                paging: true,
                ordering: false,
                autoWidth: true,
                scrollCollapse: true,
                ajax: abp.libs.datatables.createAjax(args.serviceMethod()),
                columnDefs: [
                    {
                        rowAction: {
                            element: $('<button/>')
                                .addClass('btn btn-primary btn-sm m-btn--icon')
                                .text(l('Pick'))
                                .prepend($('<i/>').addClass('fa fa-fa-search'))
                                .click(function () {
                                    var pickedId = $(this).data().id;
                                    var pickedDisplayName = $(this).data().displayName;
                                    $('#NavigationPropertyLookupTableModal #CurrentLookupId').val(
                                        pickedId
                                    );
                                    $(
                                        '#NavigationPropertyLookupTableModal #CurrentLookupDisplayName'
                                    ).val(pickedDisplayName);
                                    $('#NavigationPropertyLookupTableModal #CancelButton').click();
                                }),
                        },
                    },
                    {
                        data: 'displayName',
                    },
                ],
            })
        );
    };

    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    return {
        initModal: initModal,
    };
};

//<suite-custom-code-block-3>
//</suite-custom-code-block-3>
