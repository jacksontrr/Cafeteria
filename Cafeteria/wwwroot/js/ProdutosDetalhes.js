$(document).ready(function () {
    $("#backButton").show()
    $("#increase").click(function () {
        var quantity = parseInt($("#quantity").val());
        quantity++;
        $("#quantity").val(quantity);
    });

    $("#decrease").click(function () {
        var quantity = parseInt($("#quantity").val());
        if (quantity > 1) {
            quantity--;
            $("#quantity").val(quantity);
        }
    });
});
