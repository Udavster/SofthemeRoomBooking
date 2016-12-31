$(document).ready(function () {
    $('#AllowRegistration')[0].disabled = $('#Private')[0].checked;

    $('#event-edit-form .event-input').on('keyup blur', function () {
        if ($('#event-edit-form').valid()) {
            $('.submit-btn').attr('disabled', false);
        } else {
            $('.submit-btn').attr('disabled', 'disabled');
        }
    });

    $('#Private').change(function () {
        $('#AllowRegistration')[0].checked = !this.checked;
        $('#AllowRegistration')[0].disabled = this.checked;
    });

    $('#ShowOrganizator').change(function () {
        $('#Nickname')[0].disabled = this.checked;
    });

    $('#minimizeButton').bind('click', function () {
        $('#popup-edit-event').hide('');
        $('#popup-edit-event').hide();
    });

    $('.submit-btn').bind('click', function (e) {
        e.preventDefault();
        var eventValidator = eventValidate();

        if ($('#Nickname').val() === '' && !$('#ShowOrganizator')[0].checked) {
            eventValidator.showErrors(false, 'Не указан организатор события.');
            return false;
        }

        if (eventValidator.validate() && $('#event-form').valid()) {
            if ($('#ShowOrganizator')[0].checked) {
                $('#Nickname').val('');
            }

            $.ajax({
                url: window.location.origin + '/Event/EditEventPartial',
                method: 'POST',
                data: $('#event-edit-form').serialize(),
                success: function (result) {
                    if (result.success) {
                        window.location.href = result.redirectTo;
                    } else {
                        eventValidator.showErrors(false, result.errorMessage);
                    }
                }
            });
            return true;
        } else {
            $('.submit-btn').attr('disabled', 'disabled');
            return false;
        }
    });

    initDateTime("word", null, $(".submit-btn"));

    if ($('#Nickname').val() === '') {
        $('#Nickname')[0].disabled = $('#ShowOrganizator')[0].checked = true;
    }

    if ($('#Id').val() !== '') {
        var year = parseInt($('#Year').val()),
            month = parseInt($('#Month').val()) - 1,
            day = parseInt($('#Day').val()),

            startHour = parseInt($('#StartHour').val()),
            startMinutes = parseInt($('#StartHour').val()),
            finishHour = parseInt($('#StartHour').val()),
            finishMinutes = parseInt($('#StartHour').val());

        setEventDateTime(new Date(year, month, day, startHour, startMinutes), new Date(year, month, day, finishHour, finishMinutes));
    }
});