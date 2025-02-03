function ConsultNameAPI() {

    let identification = $("#identification").val();

    if (identification.length > 0) {
        $.ajax({
            type: "GET",
            url: "https://apis.gometa.org/cedulas/" + identification,
            dataType: "json",
            success: function (result) {
                $("#full_name").val(result.full_name);
            }
        });
    }
    else {
        $("#full_name").val("");
    }
}

function AvoidBlankSpace(evt) {
    if (evt.keyCode === 32) {
        return false;
    }
    return true;
}


function FnDecimal(evt, element) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode >= 48 && charCode <= 57) {
        return true;
    }
    if (charCode == 44) {
        if (element.value.indexOf(',') <= 0) {
            return true;
        }
        else {
            return false;
        }
    }
    return false;
}

function FnInt(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode >= 48 && charCode <= 57) {
        return true;
    }
    return false;
}

$(document).ready(function () {
    $('#showData').DataTable({
        "scrollX": true,
        "fixedColumns": {
            "left": 0, // Fija 0 columnas a la izquierda
            "right": 1 // Fija 1 columna a la derecha (la columna "Acciones")
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.16/i18n/Spanish.json"
        }
    });
});