"use strict";

$(document).ready(function () {

    var div_paciente = $("#div_paciente"); //Fields wrapper
    var div_prestador = $("#div_prestador"); //Fields wrapper
    var add_paciente = $("#add_paciente"); //Add button ID
    var add_prestador = $("#add_prestador"); //Add button ID
    var div_altapaciente = $("#div_altapaciente");
    var add_altapaciente = $("#add_altapaciente");
    var x = 0; //initlal text box count
    var y = 0; //initlal text box count
    var z = 0;
    $(add_paciente).click(function (e) { //on add input button click
        e.preventDefault();
        $(div_paciente).append('<div class="form-group">' +
            '<input type="hidden" name="Pacientes.Index" value=' + y + ' />' +
            '<label class="control-label" for="Pacientes[' + y + '].Nome">Nome do paciente</label>' +
            '<input name="Pacientes[' + y + '].NomePaciente" class="form-control" id="Pacientes[' + y + '].NomePaciente" aria-required="true" required="required" type="text" value="">' +
            '<span class="text-danger field-validation-valid" data-valmsg-replace="true" data-valmsg-for="Pacientes[' + y + '].NomePaciente"></span>' +

            '<label class="control-label" for="Pacientes[' + y + '].Nome">Código do paciente</label>' +
            '<input name="Pacientes[' + y + '].CodPaciente" class="form-control" id="Pacientes[' + y + '].CodPaciente" aria-required="true" required="required" type="text" value="">' +
            '<span class="text-danger field-validation-valid" data-valmsg-replace="true" data-valmsg-for="Pacientes[' + y + '].CodPaciente"></span>' +
            '<a href="#" class="btn btn-danger remove_field">Remove</a>' +
            '</div>'); //add input box
        y++; //text box increment
    });

    $(add_prestador).click(function (e) { //on add input button click
        e.preventDefault();


        $(div_prestador).append('<div class="form-group">' +
            '<input type="hidden" name="Prestadores.Index" value=' + x + ' />' +
            '<label class="control-label" for="Prestadores[' + x + '].Nome">Nome do prestador</label>' +
            '<input name="Prestadores[' + x + '].NomePrestador" class="form-control" id="Prestadores[' + x + '].NomePrestador" aria-required="true" required="required" type="text" value="">' +
            '<span class="text-danger field-validation-valid" data-valmsg-replace="true" data-valmsg-for="Prestadores[' + x + '].NomePrestador"></span>' +

            '<label class="control-label" for="Prestadores[' + x + '].Nome">Código do prestador</label>' +
            '<input name="Prestadores[' + x + '].CodPrestador" class="form-control" id="Prestadores[' + x + '].CodPrestador" aria-required="true" required="required" type="text" value="">' +
            '<span class="text-danger field-validation-valid" data-valmsg-replace="true" data-valmsg-for="Prestadores[' + x + '].CodPrestador"></span>' +
            '<a href="#" class="btn btn-danger remove_field">Remove</a>' +
            '</div>');//add input box
        x++; //text box increment
    });

    $(add_altapaciente).click(function (e) { //on add input button click
        e.preventDefault();

        $(div_altapaciente).append('<div class="form-group">' +
            '<input type="hidden" name="MotivoAlta.Index" value=' + z + ' />' +
            '<label class="control-label" for="MotivoAlta[' + z + '].Nome">Código Motivo</label>' +
            '<input name="MotivoAlta[' + z + '].CodigoMotivo" class="form-control" id="MotivoAlta[' + z + '].CodigoMotivo" aria-required="true" required="required" type="text" value="">' +
            '<span class="text-danger field-validation-valid" data-valmsg-replace="true" data-valmsg-for="MotivoAlta[' + z + '].CodigoMotivo"></span>' +

            '<label class="control-label" for="MotivoAlta[' + z + '].Nome">Motivo Alta</label>' +
            '<input name="MotivoAlta[' + z + '].MotivoAlta" class="form-control" id="MotivoAlta[' + z + '].MotivoAlta" aria-required="true" required="required" type="text" value="">' +
            '<span class="text-danger field-validation-valid" data-valmsg-replace="true" data-valmsg-for="MotivoAlta[' + z + '].MotivoAlta"></span>' +

            '<label class="control-label" for="MotivoAlta[' + z + '].Nome">Tipo Alta</label>' +
            '<span class="text-danger field-validation-valid" data-valmsg-replace="true" data-valmsg-for="MotivoAlta[' + z + '].Tipo"></span>' +

            '<select name="MotivoAlta[' + z + '].Tipo" class="form-control" id="Motivoalta[' + z + '].Tipo" data-val-required="required" data-val="true">' +
            '<option value= "0" > A(Casa / Auto - Cuidado)</option> ' +
            '<option value="1">I (Instituição de longa permanência)</option>' +
            '<option value="2">D (Atenção domiciliar)</option>' +
            '<option value="3">P (Alta a pedido)</option>' +
            '<option value="4">C (Transferido para hospital de curta permanência)</option>' +
            '<option value="5">L (Transferido para hospital de longa permanência (unidade de cuidados crônicos, reabilitação))</option>' +
            '<option value="6">O (Óbito)</option>' +
            '<option value="7">E (Evasão)</option>' +
            '</select>' +
            '<a href="#" class="btn btn-danger remove_field">Remove</a>' +
            '</div>'); //add input box
        z++; //text box increment
    });


    $(div_prestador).on("click", ".remove_field", function (e) { //user click on remove text
        e.preventDefault(); $(this).parent('div').remove(); x--;
    })

    $(div_paciente).on("click", ".remove_field", function (e) { //user click on remove text
        e.preventDefault(); $(this).parent('div').remove(); x--;
    })

    $(div_altapaciente).on("click", ".remove_field", function (e) { //user click on remove text
        e.preventDefault(); $(this).parent('div').remove(); x--;
    })

});

////////////////////////////////////////////////////////////////////

$("#SistemaId").change(function () {
    var url = '/Configuracoes/BuscaBanco/:parentId:';
    var $banco = $("#BancoId");
    $banco.attr("disabled", true);

    var id = $(this).val();

    if (id != "") {

        $.getJSON(url.replace(':parentId:', id), function (items) {
            var newOptions = '<option value="">Selecione o Banco</option>';
            for (var id in items) {
                newOptions += '<option value="' + items[id].id + '">' + items[id].nome + '</option>';
            }
            $banco.empty();
            $banco.append(newOptions);
            $banco.removeAttr('disabled');

        });
    }

});