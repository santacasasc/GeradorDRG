﻿"use strict";

var $validator = $.validator;

$validator.setDefaults({
    ignore: ":hidden:not(.do-not-ignore)"
});

$validator.addMethod('conexao', function (value) {

    return value != "";

}, 'Conexão Inválida');

$(document).ready(function () {
    $('#rootwizard').bootstrapWizard({
        'tabClass': 'nav nav-pills',
        'onNext': function (tab, navigation, index) {
            var $valid = $("#form").valid();
            if (!$valid) {
                return false;
            }
        }
    });

});

$(document).on("click", "#verifica-conexao", function (e) {

    var url = "../Configuracoes/_VerificaConexao/?URLBase=" + encodeURIComponent($("#BancoURL").val()) + "&banco=" + $("#BancoId").val() + "&sistema=" + $("#SistemaId").val();

    $.ajax({
        type: "GET",
        url: url,
        cache: false,
        success: function (text) {
            var response = text;

            if (response == "S") {
                MotivoAlta();
                TipoInternacao();
                $("#conexao").val("S");
            }
        },
        complete: function () {
            $("#form").valid();
        }

    });




});

function TipoInternacao() {
    var url = "../Configuracoes/_TipoInternacao/?URLBase=" + encodeURIComponent($("#BancoURL").val()) + "&banco=" + $("#BancoId").val() + "&sistema=" + $("#SistemaId").val();
    $.ajax({
        type: "GET",
        url: url,
        cache: false,
        success: function (text) {
            var response = text;
            $("#div-tipo-internacao").append(response);
        }
    });
}

function MotivoAlta() {
    var url = "../Configuracoes/_MotivoAlta/?URLBase=" + encodeURIComponent($("#BancoURL").val()) + "&banco=" + $("#BancoId").val() + "&sistema=" + $("#SistemaId").val()
    $.ajax({
        type: "GET",
        url: url,
        cache: false,
        success: function (text) {
            var response = text;
            $("#div-motivo-alta").append(response);
        }
    });
}

$(document).on("click", ".add-item", function (e) {
    e.preventDefault();

    var $this = $(this);

    $this.data("count", $this.data("count") + 1);

    var url = "../Configuracoes/" + $this.data("url") + "/" + $this.data("count");

    $.ajax({
        type: "GET",
        url: url,
        cache: false,
        success: function (text) {
            var response = text;
            $($this.data("target")).append(response);
        }
    });

});

$(document).on("click", ".remove-item", function (e) {
    e.preventDefault();

    var $this = $(this);

    var $add = $($this.data("parent"));
    $add.data("count") = $add.data("count") - 1;

    $this.parent('div').remove();
});

$(document).on("change", "#SistemaId", function (e) {

    var id = $(this).val();

    var url = '/Configuracoes/BuscaBanco/:parentId:';

    var $banco = $("#BancoId");

    $banco.attr("disabled", true);

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