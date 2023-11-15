$(document).ready(function () {

    $(".card").on("click", function () {
        let id = $(this).attr("id");
        window.location.href = "/Produtos/Details/" + id;
    })
})
