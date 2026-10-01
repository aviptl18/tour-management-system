$(document).ready(function () {

    console.log('✅ Step 2 JS loaded');

    const locationDropdown = $('#locationDropdown');
    const gallery = $('#gallery');
    const uploadInput = $('#uploadInput');
    const hiddenInputs = $('#hiddenInputs');

    const imageError = $('#imageError');
    const coverError = $('#coverError');

    // ------------------------------------------------
    // Normalize Razor-injected values
    // ------------------------------------------------
    const preSelected = (window.preSelected || []).map(Number);

    const preCoverNum =
        window.preCoverNum !== null && window.preCoverNum !== undefined
            ? Number(window.preCoverNum)
            : null;

    // ------------------------------------------------
    // Normalize server image object
    // ------------------------------------------------
    function normalizeImageObject(img) {
        const id = img.imageId ?? img.ImageId ?? img.id ?? img.Id;
        const url = img.imageUrl ?? img.ImageUrl ?? img.url ?? img.Url;
        return { id: Number(id), url: String(url) };
    }

    // ------------------------------------------------
    // Render gallery
    // ------------------------------------------------
    function renderGallery(images) {

        gallery.empty();

        if (!images || !images.length) {
            gallery.append('<em>No images found for this location.</em>');
            return;
        }

        images.forEach(raw => {
            const item = normalizeImageObject(raw);

            const card = $('<div class="img-card"></div>');

            const img = `
                <img src="${item.url}"
                     class="img-thumb"
                     alt="" />
            `;

            const overlay = $('<div class="img-overlay"></div>');

            const checkbox = $(`
                <input type="checkbox"
                       class="select-image image-checkbox"
                       value="${item.id}">
            `);

            const radio = $(`
                <input type="radio"
                       name="CoverImageId"
                       class="cover-image image-radio"
                       value="${item.id}">
            `);

            // 🔥 Restore state (back / reload)
            if (preSelected.includes(item.id)) {
                checkbox.prop('checked', true);
            }

            if (preCoverNum === item.id) {
                radio.prop('checked', true);
                checkbox.prop('checked', true);
            }

            overlay.append(checkbox, radio);
            card.append(img, overlay);
            gallery.append(card);
        });

        syncHiddenInputs();
    }

    // ------------------------------------------------
    // Sync hidden inputs (ONLY SelectedImageIds)
    // ------------------------------------------------
    function syncHiddenInputs() {
        hiddenInputs.empty();

        $('.select-image:checked').each(function () {
            const v = $(this).val();
            if (/^\d+$/.test(v)) {
                hiddenInputs.append(
                    `<input type="hidden" name="SelectedImageIds" value="${v}" />`
                );
            }
        });
    }

    // ------------------------------------------------
    // Location change → load images
    // ------------------------------------------------
    locationDropdown.on('change', function () {

        const locId = $(this).val();

        gallery.empty();
        hiddenInputs.empty();
        imageError.addClass('d-none');
        coverError.addClass('d-none');

        if (!locId) return;

        $.get(`/Admin/Tours/GetImagesByLocation?id=${locId}`, function (data) {
            renderGallery(data);
        });
    });

    // ------------------------------------------------
    // Image checkbox change
    // ------------------------------------------------
    $(document).on('change', '.select-image', function () {

        imageError.addClass('d-none');

        const card = $(this).closest('.img-card');
        const radio = card.find('.cover-image');

        // 🔥 If unchecked image was cover → remove cover
        if (!$(this).prop('checked') && radio.prop('checked')) {
            radio.prop('checked', false);
        }

        syncHiddenInputs();
    });

    // ------------------------------------------------
    // Cover image change
    // ------------------------------------------------
    $(document).on('change', '.cover-image', function () {

        coverError.addClass('d-none');

        const card = $(this).closest('.img-card');
        const checkbox = card.find('.select-image');

        // 🔥 FORCE cover image to be selected
        if (!checkbox.prop('checked')) {
            checkbox.prop('checked', true);
        }

        syncHiddenInputs();
    });

    // ------------------------------------------------
    // Upload images
    // ------------------------------------------------
    uploadInput.on('change', function () {

        const locId = locationDropdown.val();
        if (!locId) {
            alert('Select location first.');
            return;
        }

        const files = uploadInput[0].files;
        if (!files || files.length === 0) return;

        const form = new FormData();
        for (let f of files) form.append('files', f);
        form.append('locationId', locId);

        $.ajax({
            url: '/Admin/Tours/UploadLocationImages',
            method: 'POST',
            data: form,
            processData: false,
            contentType: false,
            success: function (data) {
                renderGallery(data);
                uploadInput.val(null);
            }
        });
    });

    // ------------------------------------------------
    // FORM SUBMIT VALIDATION
    // ------------------------------------------------
    $('#nextBtn').closest('form').on('submit', function (e) {

        let valid = true;

        if ($('.select-image:checked').length === 0) {
            imageError.removeClass('d-none');
            valid = false;
        }

        const cover = $('input[name="CoverImageId"]:checked');

        if (cover.length === 0) {
            coverError.removeClass('d-none');
            valid = false;
        } else {
            const coverId = cover.val();
            const selectedIds = $('.select-image:checked')
                .map(function () { return $(this).val(); })
                .get();

            // 🔥 Safety check (cover ∈ selected)
            if (!selectedIds.includes(coverId)) {
                coverError.text('Cover image must be from selected images.');
                coverError.removeClass('d-none');
                valid = false;
            }
        }

        if (!valid) {
            e.preventDefault();
        }
    });

    // ------------------------------------------------
    // Restore gallery on reload / back
    // ------------------------------------------------
    const initLoc = locationDropdown.val();
    if (initLoc) {
        locationDropdown.trigger('change');
    }

});
