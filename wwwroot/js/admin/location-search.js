$(document).ready(function () {

    const searchBox = $("#locationSearch");
    const results = $("#locationResults");

    function searchLocations(term) {

        $.get("/Admin/Locations/Search", { term: term }, function (data) {

            let html = "";

            data.forEach(function (loc) {

                html += `
                    <div class="list-group-item location-item"
                         data-id="${loc.locationId}">
                        ${loc.locationName}
                    </div>
                `;

            });

            results.html(html);

        });

    }

    // show all locations when clicking
    searchBox.on("focus", function () {

        searchLocations("");

    });

    // filter locations
    searchBox.on("keyup", function () {

        const term = $(this).val();

        searchLocations(term);

    });

    // location selected
    $(document).on("click", ".location-item", function () {

        const id = $(this).data("id");
        const name = $(this).text();

        $("#LocationId").val(id);
        $("#locationSearch").val(name);

        $("#locationResults").empty();

        console.log("Selected location:", id);

        if (typeof loadLocationImages === "function") {
            loadLocationImages(id);
        }

    });

});