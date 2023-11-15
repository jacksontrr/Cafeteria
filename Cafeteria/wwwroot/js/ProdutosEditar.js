$(document).ready(function () {
    $("#backButton").show()
    let imagem = $("#imagem").attr("src")
    
    $("#Arquivo").on("change", function () {
        // verificar se tem alguma coisa
        if (this.files && this.files[0]) {
            $("#RemoverImagem").parent("span").removeClass("d-flex").addClass("d-none")
            $("#imagem").attr("src", URL.createObjectURL(this.files[0]))
            $("#RemoverImagem").prop("checked", false)
        } else {
            $("#RemoverImagem").parent("span").addClass("d-flex").removeClass("d-none")
            $("#imagem").attr("src", imagem)
        }
    })

    $("#RemoverImagem").on("click", function () {
        if ($(this).is("checked")) {
            $(this).attr("checked", true)
        } else {
            $(this).attr("checked", false)
        }
        //$("#Arquivo").val("")
        //$("#removerImagem").parent("span").addClass("d-none").removeClass("d-flex")
        //$("#imagem").attr("src", "")
    })
});
