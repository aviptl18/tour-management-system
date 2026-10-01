document.addEventListener("DOMContentLoaded", () => {

    const pricingContainer = document.getElementById("pricingContainer");
    const baseError = document.getElementById("basePriceError");
    const draftInput = document.querySelector('input[name="DraftId"]');

    if (!draftInput) return;

    const draftId = draftInput.value;

    function token() {
        return document.querySelector('input[name="__RequestVerificationToken"]').value;
    }

    function refresh(html) {
        pricingContainer.innerHTML = html;
    }

    function enableActionButton() {

        if (window.isEditMode) {

            const updateBtn = document.getElementById("updateBtn");
            if (updateBtn) updateBtn.disabled = false;

        } else {

            const publishBtn = document.getElementById("publishBtn");
            if (publishBtn) publishBtn.disabled = false;

        }
    }


    // ======================
    // ADD OPTION DROPDOWN
    // ======================
    document.addEventListener("click", e => {

        if (e.target.id !== "btnAddOptionRow") return;

        const holder = document.getElementById("optionSelectorContainer");

        if (!holder) return;

        if (holder.querySelector("#inlineOptionDropdown")) return;

        holder.innerHTML = `
            <select id="inlineOptionDropdown" class="form-control mt-2">
                <option value="">Select option</option>
            </select>
        `;

        fetch(`/Admin/Tours/GetAvailableOptions?draftId=${draftId}`)
            .then(r => r.json())
            .then(data => {

                const dd = document.getElementById("inlineOptionDropdown");

                data.forEach(o => {

                    const opt = document.createElement("option");
                    opt.value = o.optionId;
                    opt.text = `${o.optionName} (₹${o.basePrice})`;

                    dd.appendChild(opt);
                });

            });

    });


    // ADD OPTION
    document.addEventListener("change", e => {

        if (e.target.id !== "inlineOptionDropdown" || !e.target.value) return;

        fetch('/Admin/Tours/AddPricingOption', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token()
            },
            body: `draftId=${draftId}&optionId=${e.target.value}`
        })
            .then(r => r.text())
            .then(refresh);

    });


    // REMOVE OPTION
    document.addEventListener("click", e => {

        if (!e.target.classList.contains("remove-option")) return;

        fetch('/Admin/Tours/RemovePricingOption', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token()
            },
            body: `componentId=${e.target.dataset.id}&draftId=${draftId}`
        })
            .then(r => r.text())
            .then(refresh);

    });


    // UPDATE BASE PRICE
    document.addEventListener("change", e => {

        if (!e.target.classList.contains("base-price")) return;

        const value = e.target.value;

        fetch('/Admin/Tours/UpdateBasePrice', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token()
            },
            body: `componentId=${e.target.dataset.id}&price=${value}&draftId=${draftId}`
        })
            .then(async r => {

                if (!r.ok) {

                    baseError.textContent = await r.text();
                    return null;
                }

                baseError.textContent = "";

                return r.text();
            })
            .then(html => {

                if (html) refresh(html);

            });

    });


    // ======================
    // SAVE DRAFT
    // ======================
    const saveBtn = document.getElementById("saveDraftBtn");

    if (saveBtn) {

        saveBtn.addEventListener("click", function () {

            const form = document.querySelector("form");

            if (!$(form).valid()) return;

            fetch('/Admin/Tours/SaveDraft', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': token()
                },
                body: `draftId=${draftId}`
            })
                .then(res => res.json())
                .then(data => {

                    if (data.success) {

                        alert("Draft saved successfully");

                        enableActionButton();

                    }
                    else {

                        alert("Please complete all required fields.");

                    }

                });

        });

    }

});