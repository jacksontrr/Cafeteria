$(document).ready(function () {
    $("#backButton").show()
    let imagem = $("#imagem").attr("src")
    
    $("#Arquivo").on("change", function () {
        if (this.files && this.files[0]) {
            $("#RemoverImagem").parent("span").removeClass("d-flex").addClass("d-none")
            $("#imagem").attr("src", URL.createObjectURL(this.files[0]))
            $("#RemoverImagem").prop("checked", false)
        } else {      
            $("#imagem").attr("src", imagem)
        }
    })
});
