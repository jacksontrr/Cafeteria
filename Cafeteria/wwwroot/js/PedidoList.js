$(document).ready(function () {
    $(".finish").click(function () {
        var id = $(this).closest(".div-infor").find(".pedidoId").val();
        var url = "/Pedidos/Finish/" + id;
        $.ajax({
            url: url,
            type: "POST",
            success: function (data) {
                if (data.success) {
                    location.reload();
                }
                else {
                    alert("Erro ao finalizar pedido!");
                }
            }
        });
    })

    $(".in-preparation").click(function () {
        var id = $(this).closest(".div-infor").find(".pedidoId").val();
        var url = "/Pedidos/InPreparation/" + id;
        $.ajax({
            url: url,
            type: "POST",
            success: function (data) {
                if (data.success) {
                    location.reload();
                }
                else {
                    alert("Erro ao finalizar pedido!");
                }
            }
        });
    })
    
    let urlWs = "wss://" + window.location.host + "/ws"; // Para local
    //let urlWs = "ws://" + window.location.host + "/ws"; // Para produção

    const socket = new WebSocket(urlWs);
    
    socket.onopen = function (e) {
        console.log("[open] Connection established");
    };

    socket.onmessage = function (event) {
        console.log(`[message] Data received from server: ${event.data}`);
        if(event.data == "Atualizar"){
            location.reload();
        }
    };

    socket.onclose = function (event) {
        if (event.wasClean) {
            console.log(`[close] Connection closed cleanly, code=${event.code} reason=${event.reason}`);
        } else {
            console.log('[close] Connection died');
        }
    };

    socket.onerror = function (error) {
        console.log(`[error] ${error.message}`);
    };
})