﻿$(document).ready(function() {
    $('#event-create-form .event-input').on('keyup blur', function () {
        if ($('#event-create-form').valid()) {
            $('#createButton').attr('disabled', false);
        } else {
            $('#createButton').attr('disabled', 'disabled');
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

    $('#createButton').bind('click', function (e) {
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
            $('#createButton').attr('disabled', 'disabled');
            return false;
        }
    });
    
    initDateTime('word', null, $('#createButton'));
    debugger;
    var weekday = parseInt($('#datepicker').find('#current-month').data('day'), 10),
        month = parseInt($('#datepicker').find('#current-month').data('month'), 10),
        year = parseInt($('#datepicker').find('#current-month').data('year'), 10);

    var startTime = new Date(year, month, weekday, 9);
    var finishTime = new Date(year, month, weekday, 9);

    setEventDateTime(startTime, finishTime);
    setDefaultEventSettings();
});