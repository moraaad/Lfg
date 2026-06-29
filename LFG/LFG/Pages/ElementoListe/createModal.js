var abp = abp || {};

//<suite-custom-code-block-1>
//</suite-custom-code-block-1>

abp.modals.elementoListaCreate = function () {
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
    };

    //<suite-custom-code-block-2>
    //</suite-custom-code-block-2>

    return {
        initModal: initModal,
    };
};

//<suite-custom-code-block-3>
//</suite-custom-code-block-3>
