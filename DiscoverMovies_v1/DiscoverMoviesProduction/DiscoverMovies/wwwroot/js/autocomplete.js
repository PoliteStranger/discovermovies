

// On load/Ready
$(document).ready(function () {


    //console.log("ready!");


    // Setup Autocomplete. Uses Post request to perform tag searches on hydrus.
    $("#search-field").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: 'POST',
                url: "/api/autocomplete",
                contentType: 'application/json',
                data: JSON.stringify(request.term), // API uses strings in and out!
                success: function (data) {
                    response(jQuery.parseJSON(data)); // API uses strings in and out!
                }
            });
        },
        // 3 characters min to trigger
        minLength: 3,
        delay: 200,
        // Visual of list
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
});
