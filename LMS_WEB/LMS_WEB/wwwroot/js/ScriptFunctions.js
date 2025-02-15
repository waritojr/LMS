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

$(document).ready(function () {
    let filterIndex = 1; // Índice para los nuevos filtros

    // Agregar nuevo filtro
    $('#btn-add-filter').click(function () {
        const newFilter = `
                        <div class="filter-row mb-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <select class="form-control field-select" name="filters[${filterIndex}].Field">
                                        <option value="title">Título</option>
                                        <option value="name_author">Autor</option>
                                        <option value="isbn">ISBN</option>
                                        <option value="classification_name">Clasificación</option>
                                        <option value="subject_book">Tema</option>
                                        <option value="publisher">Editorial</option>
                                        <option value="publication_date">Fecha de Publicación</option>
                                        <option value="availability_book">Disponibilidad</option>
                                        <option value="id_classification">Tipo de Clasificación</option>
                                        <option value="id_language">Idioma</option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <div class="value-container">
                                        <input type="text" class="form-control" name="filters[${filterIndex}].Value" placeholder="Valor">
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <button type="button" class="btn btn-danger btn-remove-filter">
                                        <i class="fa fa-trash"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    `;

        $('#filters-container').append(newFilter);
        filterIndex++;

        // Habilitar botones de eliminar si hay más de un filtro
        if ($('.filter-row').length > 1) {
            $('.btn-remove-filter').prop('disabled', false);
        }
    });

    // Eliminar filtro
    $(document).on('click', '.btn-remove-filter', function () {
        if ($('.filter-row').length > 1) {
            $(this).closest('.filter-row').remove();
        }

        // Deshabilitar botones de eliminar si solo hay un filtro
        if ($('.filter-row').length === 1) {
            $('.btn-remove-filter').prop('disabled', true);
        }
    });

    // Cambiar el campo de valor según el tipo de filtro seleccionado
    $(document).on('change', '.field-select', function () {
        const selectedField = $(this).val();
        const valueContainer = $(this).closest('.row').find('.value-container');

        if (selectedField === "publication_date") {
            valueContainer.html(`
                    <div class="row">
                        <div class="col-md-6 d-flex align-items-center">
                            <label class="me-2 mb-0">Desde</label> &nbsp;
                            <input type="date" class="form-control" name="filters[${filterIndex - 1}].Value" placeholder="Desde">
                        </div>
                        <div class="col-md-6 d-flex align-items-center">
                            <label class="me-2 mb-0">Hasta</label> &nbsp;
                            <input type="date" class="form-control" name="filters[${filterIndex - 1}].ValueExtra" placeholder="Hasta">
                        </div>
                     </div>
                        `);
        } else if (selectedField === "availability_book") {
            valueContainer.html(`
                            <select class="form-control" name="filters[${filterIndex - 1}].Value">
                                <option value="true">Disponible</option>
                                <option value="false">No Disponible</option>
                            </select>
                        `);
        } else if (selectedField === "id_classification") {
            valueContainer.html(`
                            <select class="form-control" name="filters[${filterIndex - 1}].Value">
        @if (ViewBag.Classifications != null)
        {
            foreach (var item in ViewBag.Classifications)
            {
                                                <option value="@item.Value">@item.Text</option>
            }
        }
                            </select>
                        `);
        } else if (selectedField === "id_language") {
            valueContainer.html(`
                            <select class="form-control" name="filters[${filterIndex - 1}].Value">
        @if (ViewBag.Languages != null)
        {
            foreach (var item in ViewBag.Languages)
            {
                                                <option value="@item.Value">@item.Text</option>
            }
        }
                            </select>
                        `);
        } else {
            valueContainer.html(`
                            <input type="text" class="form-control" name="filters[${filterIndex - 1}].Value" placeholder="Valor">
                        `);
        }
    });
});