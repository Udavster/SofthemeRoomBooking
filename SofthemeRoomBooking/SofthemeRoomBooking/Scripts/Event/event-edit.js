$(document).ready(function () {
    $('#AllowRegistration')[0].disabled = $('#Private')[0].checked;

    $('#event-edit-form .event-input').on('keyup blur', function (e) {
        var validator = $('#event-edit-form').validate();

        if (validator.element(e.target)) {
            $('.submit-btn').attr('disabled', false);
        } else {
            $('.submit-btn').attr('disabled', 'disabled');
        }
    });

    $('#minimizeButton').bind('click', function () {
        $('#popup-edit-event').html('');
        $('#popup-edit-event').hide();
    });

    $('#cancelButton').bind('click', function (e) {
        e.preventDefault();
        
        $('#popup-edit-event').html('');
        $('#popup-edit-event').hide();
    });

    $('.submit-btn').bind('click', function (e) {
        e.preventDefault();
        var eventValidator = eventValidate();

        if (!checkSetOrganizator()) return false;

        var nickname = $('#Nickname').val();
        if ($('#ShowOrganizator')[0].checked) {
            $('#Nickname').val('');
        }

        if (eventValidator.validate() && $('#event-edit-form').valid()) {
            
            $.ajax({
                url: window.location.origin + '/Event/EditEventPartial',
                method: 'POST',
                data: $('#event-edit-form').serialize(),
                success: function (result) {
                    if (result.success) {
                        window.location.href = result.redirectTo;
                    } else {
                        eventValidator.showErrors(false, result.errorMessage);
                        $('#Nickname').val(nickname);
                    }
                }
            });
            return true;
        } else {
            $('.submit-btn').attr('disabled', 'disabled');
            $('#Nickname').val(nickname);
            return false;
        }
    });

    initDateTime('word', null, $('.submit-btn'));

    if ($('#Nickname').val() === '') {
        $('#Nickname')[0].disabled = $('#ShowOrganizator')[0].checked = true;
    }

    if ($('#Id').val() !== '') {
        var year = parseInt($('#Year').val()),
            month = parseInt($('#Month').val()) - 1,
            day = parseInt($('#Day').val()),

            startHour = parseInt($('#StartHour').val()),
            startMinutes = parseInt($('#StartMinutes').val()),
            finishHour = parseInt($('#FinishHour').val()),
            finishMinutes = parseInt($('#FinishMinutes').val());

        setEventDateTime(new Date(year, month, day, startHour, startMinutes), new Date(year, month, day, finishHour, finishMinutes));
    }
});