$(document).ready(function() {
    $('.equipmentadmin-general').bind('keyup blur', function(e) {
        var $this = e.target;
        if ($($this).hasClass('equipmentadmin-inputname')) {
            roomNameValidation($this);
        } else {
            equipmentValidation($this);
        }
        if ($('.equipmentadmin .invalid').length === 0) {
            showErrors(true);
            $('#saveButton').attr('disabled', false);
        } else {
            $('#saveButton').attr('disabled', 'disabled');
        }
    });
});

function roomNameValidation(name) {
    var roomName = $(name).val();
    var maxLength = parseInt($(name).data('valLengthMax'),10);
    var errors = "";
    var isValid = true;

    if (roomName === "") {
        errors += $(name).data('valRequired');
        $(name).addClass('invalid');
        isValid = false;
    }
    if (roomName.length > maxLength) {
        errors += errors === "" ? $(name).data('valLength') : " " + $(name).data('valLength');
        $(name).addClass('invalid');
        isValid = false;
    }

    if (!isValid) {
        showErrors(false, errors);
        return false;
    }
    $(name).removeClass('invalid');

    return true;
}

function equipmentValidation(quantity) {

    var quantityVal = $(quantity).val();

    var minLenght = $(quantity).data('valRangeMin');
    var maxLenght = $(quantity).data('valRangeMax');
    var errors = "";
    var isValid = true;

    if (isNaN($(quantity).val())) {
        errors += "Количество должно быть числом";
        $(quantity).addClass('invalid');
        isValid = false;
    }

    if (quantityVal==="") {
        errors += $(quantity).data('valRequired');
        $(quantity).addClass('invalid');
        isValid = false;
    }

    if (quantityVal > maxLenght || quantityVal < minLenght) {
        errors += errors === "" ? $(quantity).data('valRange') : " " + $(quantity).data('valRange');
        $(quantity).addClass('invalid');
        isValid = false;
    }

    if (!isValid) {
        showErrors(false , errors);
        return false;
    }
    $(quantity).removeClass('invalid');

    return true;
}

function showErrors(isValid, errors) {
    var messageContaiter = '<span>' + errors + '</span>';

    if (isValid) {
        if (!$('#event-errors').hasClass('hidden-error')) {
            $('#event-errors').addClass('hidden-error');
        }
    } else {
        $('.error-text').html(messageContaiter);

        if ($('#event-errors').hasClass('hidden-error')) {
            $('#event-errors').removeClass('hidden-error');
        }
    }
}
