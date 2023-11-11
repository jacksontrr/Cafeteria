$(document).ready(function () {

    $("#backButton").show()
    $(".increase").click(function () {
        let quantidade = $(this).prev(".quantity").val();
        quantidade++;
        if (quantidade > 1) {
            $(this).prev(".quantity").prev(".decrease").html('-')
        }
        $(this).prev(".quantity").val(quantidade);

        let quantidadeOriginal = $(this).prev(".quantity").attr("data-quantity");

        if (quantidade != quantidadeOriginal) {
            $(this).closest(".div-infor").removeClass("h-100").addClass("h-50");
            $(this).closest(".div-infor").next(".div-save").removeClass("d-none");
        } else {
            $(this).closest(".div-infor").addClass("h-100").removeClass("h-50");
            $(this).closest(".div-infor").next(".div-save").addClass("d-none");
        }

        CalculaTotal();
    });
    $(".decrease").click(function () {
        let quantidade = $(this).next(".quantity").val();
        let $this = $(this);
        quantidade--;
        if (quantidade <= 0) {
            // titulo do modal
            $("#modal #modalLabel").html("Deseja remover o produto do carrinho?");
            let imagem = $this.closest(".row").find("img").attr("src");
            let produto = $this.closest(".div-infor").find(".name").text();
            let preco = $this.closest(".div-infor").find(".price").text();

            // corpo do modal
            let html = '<div class="row">';
            html += '<div class="col-12">';
            html += '<div class="card">';
            html += '<div class="card-body">';
            html += '<div class="row">';
            html += '<div class="col-4">';
            html += '<img src="' + imagem + '" class="img-fluid" />';
            html += '</div>';
            html += '<div class="col-8">';
            html += '<h5 class="card-title">' + produto + '</h5>';
            html += '<p class="card-text">' + preco + '</p>';
            html += '</div>';
            html += '</div>';
            html += '</div>';
            html += '</div>';
            html += '</div>';
            html += '</div>';

            $("#modal .modal-body").html(html);

            $("#modal .modal-footer").html('<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button><button type="button" class="btn btn-danger btn-remover">Remover</button>')

            $("#modal").modal("show");

            let produtoId = $this.closest(".card").prev(".productId").val();

            $(".btn-remover").click(function () {
                $.ajax({
                    url: "/Carrinho/RemoveProductCart",
                    method: "POST",
                    data: {
                        productId: produtoId
                    }
                }).done(function (data) {
                    window.location.reload();
                });
            });
            return;
        }
        $(this).next(".quantity").val(quantidade);
        if (quantidade == 1) {
            $(this).html('<i class="fas fa-trash-alt"></i>');
        }


        let quantidadeOriginal = $(this).next(".quantity").attr("data-quantity");

        if (quantidade != quantidadeOriginal) {
            $(this).closest(".div-infor").removeClass("h-100").addClass("h-50");
            $(this).closest(".div-infor").next(".div-save").removeClass("d-none");
        } else {
            $(this).closest(".div-infor").addClass("h-100").removeClass("h-50");
            $(this).closest(".div-infor").next(".div-save").addClass("d-none");
        }

        CalculaTotal();
    });
    let CalculaTotal = function () {
        let total = 0;
        let totalQuantidade = 0;
        $(".card").each(function () {
            if ($(this).find(".quantity").length > 0) {
                let quantidade = parseInt($(this).find(".quantity").val());
                let preco = $(this).find(".price").text();
                total += quantidade * parseFloat(preco.split(' ')[1].replace(",", "."));
                totalQuantidade += quantidade;
            }
        });

        $("#total-price").html(total.toLocaleString('pt-BR', { minimumFractionDigits: 2, style: 'currency', currency: 'BRL' }));
        $("#total-quantity").html(totalQuantidade);
    }


});

