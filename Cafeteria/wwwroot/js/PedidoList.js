$(document).ready(function () {
    $(".finish").click(function () {
        var id = $(this).closest(".div-infor").find(".pedidoId").val();
        var url = "/Pedidos/Finish/" + id;
        $.ajax({
            url: url,
            type: "POST",
            success: function (data) {
                if (data.success) {
                    alert("Pedido finalizado com sucesso!");
                    location.reload();
                }
                else {
                    alert("Erro ao finalizar pedido!");
                }
            }
        });
    })
})