var abp = abp || {};

abp.modals.varianteProdottoImmagini = function () {
    var MAX_FILE_PER_CARICAMENTO = 5;

    var initModal = function (publicApi, args) {
        function tokenAntiforgery() {
            return $('#ImmaginiUploadForm input[name="__RequestVerificationToken"]').val();
        }

        function idVariante() {
            return $('#ImmaginiUploadForm input[name="varianteProdottoId"]').val();
        }

        function costruisciItemHtml(img, index, totale) {
            var principale =
                index === 0
                    ? '<span class="badge bg-success position-absolute top-0 start-0">Principale</span>'
                    : '';
            var disabledUp = index === 0 ? 'disabled' : '';
            var disabledDown = index === totale - 1 ? 'disabled' : '';

            return (
                '<div class="immagine-variante-item border rounded p-2 text-center d-inline-block me-2 mb-2" data-id="' +
                img.id +
                '" style="width:120px;">' +
                '<div class="position-relative">' +
                '<img src="' +
                img.url +
                '" alt="immagine variante" style="width:100px;height:100px;object-fit:cover;" />' +
                principale +
                '</div>' +
                '<div class="btn-group btn-group-sm mt-1" role="group">' +
                '<button type="button" class="btn btn-outline-secondary btn-sposta" data-id="' +
                img.id +
                '" data-su="true" ' +
                disabledUp +
                '><i class="fa fa-arrow-up"></i></button>' +
                '<button type="button" class="btn btn-outline-secondary btn-sposta" data-id="' +
                img.id +
                '" data-su="false" ' +
                disabledDown +
                '><i class="fa fa-arrow-down"></i></button>' +
                '<button type="button" class="btn btn-outline-danger btn-elimina" data-id="' +
                img.id +
                '"><i class="fa fa-trash"></i></button>' +
                '</div>' +
                '</div>'
            );
        }

        function renderGalleria(immagini) {
            var $galleria = $('#ImmaginiGalleria');

            if (!immagini || immagini.length === 0) {
                $galleria.html('<p class="text-muted">Nessuna immagine caricata.</p>');
                return;
            }

            var html = '';
            $.each(immagini, function (index, img) {
                html += costruisciItemHtml(img, index, immagini.length);
            });
            $galleria.html(html);
        }

        function eseguiAzione(handler, formData) {
            return $.ajax({
                url: abp.appPath + 'VarianteProdotti/ImmaginiModal?handler=' + handler,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
            })
                .done(function (result) {
                    renderGalleria(result.immagini);
                })
                .fail(function (xhr) {
                    var messaggio =
                        (xhr.responseJSON && xhr.responseJSON.error && xhr.responseJSON.error.message) ||
                        'Si è verificato un errore.';
                    abp.message.error(messaggio);
                });
        }

        $(document).on('submit', '#ImmaginiUploadForm', function (e) {
            e.preventDefault();

            var fileInput = $('#ImmaginiFileInput')[0];
            if (!fileInput.files || fileInput.files.length === 0) {
                abp.message.warn('Seleziona almeno un file da caricare.');
                return;
            }

            if (fileInput.files.length > MAX_FILE_PER_CARICAMENTO) {
                abp.message.warn('Puoi caricare al massimo ' + MAX_FILE_PER_CARICAMENTO + ' immagini alla volta.');
                return;
            }

            var formData = new FormData(this);
            eseguiAzione('Upload', formData).done(function () {
                fileInput.value = '';
            });
        });

        $(document).on('click', '#ImmaginiGalleria .btn-sposta', function () {
            var formData = new FormData();
            formData.append('__RequestVerificationToken', tokenAntiforgery());
            formData.append('varianteProdottoId', idVariante());
            formData.append('id', $(this).data('id'));
            formData.append('su', $(this).data('su'));

            eseguiAzione('Sposta', formData);
        });

        $(document).on('click', '#ImmaginiGalleria .btn-elimina', function () {
            var id = $(this).data('id');

            abp.message.confirm('Eliminare questa immagine?', function (confermato) {
                if (!confermato) {
                    return;
                }

                var formData = new FormData();
                formData.append('__RequestVerificationToken', tokenAntiforgery());
                formData.append('varianteProdottoId', idVariante());
                formData.append('id', id);

                eseguiAzione('Elimina', formData);
            });
        });
    };

    return {
        initModal: initModal,
    };
};
