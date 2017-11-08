// Write your JavaScript code.
"use strict";

$(document).ready(function () {
    var steps_link = $('div.steps div a'),
        steps_contents = $('.step-content'),
        nexts = $('.nextBtn');

    steps_contents.hide();

    steps_link.click(function (e) {
        e.preventDefault();
        var $target = $($(this).attr('href')),
            $item = $(this);

        if (!$item.hasClass('disabled')) {
            steps_link.removeClass('btn-primary').addClass('btn-default');
            $item.addClass('btn-primary');
            steps_contents.hide();
            $target.show();
            $target.find('input:eq(0)').focus();
        }
    });

    nexts.click(function () {
        var curStep = $(this).closest(".step-content"),
            curStepBtn = curStep.attr("id"),
            nextwizard = $('div.steps div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url']"),
            isValid = true;

        $(".form-group").removeClass("has-error");

        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid) {
            nextwizard.removeClass('disabled').trigger('click');
        }
    });

    $('div.steps div a.btn-primary').trigger('click');

});
