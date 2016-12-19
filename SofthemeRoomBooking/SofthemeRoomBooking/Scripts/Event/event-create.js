$(document).ready(function() {
    $('#event-create-form .event-input').on('keyup blur', function () {
        if ($('#event-create-form').valid()) {
            $('#saveButton').attr('disabled', false);
        } else {
            $('#saveButton').attr('disabled', 'disabled');
        }
    });

    $('#Private').change(function () {
        $('#AllowRegistration')[0].checked = !this.checked;
        $('#AllowRegistration')[0].disabled = this.checked;
    });

    $('#ShowOrganizator').change(function () {
        $('#Nickname')[0].disabled = this.checked;
    });

    $('#closeButton').bind('click', function () {
        $('#popup-create-event').html('');
        $('#popup-create-event').hide();
    });

    $('#saveButton').bind('click', function (e) {
        e.preventDefault();

        var eventValidator = eventValidate();

        if ($('#Nickname').val() === '' && !$('#ShowOrganizator')[0].checked) {
            eventValidator.showErrors(false, 'Не указан организатор события.');
            return false;
        }

        if (eventValidator.validate() && $('#event-create-form').valid()) {
            if ($('#ShowOrganizator')[0].checked) {
                $('#Nickname').val('');
            }

            $.ajax({
                url: window.location.origin + '/Event/CreateEvent',
                method: 'POST',
                data: $('#event-create-form').serialize(),
                success: function (result) {
                    if (result.success) {
                        window.location.href = result.redirectTo;
                    } else if (result.errorMessage) {
                        eventValidator.showErrors(false, result.errorMessage);
                    }
                }
            });
            return true;
        } else {
            return false;
        }
    });

    initDateTime('word', null, $('#saveButton'));

    $('#Day').val(new Date().getDate());
    $('#Month').val(new Date().getMonth());
    $('#Year').val(new Date().getFullYear());

    $('#StartHour').val($('#event-timestart #hours').text());
    $('#StartMinutes').val($('#event-timestart #minutes').text());
    $('#EndHour').val($('#event-timefinish #hours').text());
    $('#EndMinutes').val($('#event-timefinish #minutes').text());

    setDefaultEventSettings();
});