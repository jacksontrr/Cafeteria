$(document).ready(function () {

    // search 
    $("#search-send").on("click", function () {
        let search = $("#search").val();
        if (search != "") {
            window.location.href = "/Produtos/Search/" + search;
        }
    }
});