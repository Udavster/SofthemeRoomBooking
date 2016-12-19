$(document).ready(function () {
    var title = $('#Title').val(),
        description = $('#Description').val(),
        nickname = $('#Nickname').val(),
        showOrganizator = $('#ShowOrganizator').is(':checked'),
        allowRegistration = $('#AllowRegistration').is(':checked'),
        isPrivate = $('#Private').is(':checked'),

        currday = ('0' + $('#Day').val()).slice(-2),
        currmonth = ('0' + $('#Month').val()).slice(-2),
        curryear = $('#Year').val().slice(2),

        startHour = ('0' + $('#StartHour').val()).slice(-2),
        startMinutes = ('0' + $('#StartMinutes').val()).slice(-2),
        endHour = ('0' + $('#EndHour').val()).slice(-2),
        endMinutes = ('0' + $('#EndMinutes').val()).slice(-2);

    initDateTime('number', '.', $('#saveButton'));

    $('.datepicker-chosen-arrow').css('visibility', 'hidden');
    $('.timepicker-arrow').css('visibility', 'hidden');

    $('#event-date #day').text(('0' + $('#Day').val()).slice(-2));
    $('#event-date #month').text(('0' + $('#Month').val()).slice(-2));
    $('#event-date #year').text($('#Year').val().slice(2));

    $('#event-timestart #hours').text(('0' + $('#StartHour').val()).slice(-2));
    $('#event-timestart #minutes').text(('0' + $('#StartMinutes').val()).slice(-2));
    $('#event-timefinish #hours').text(('0' + $('#EndHour').val()).slice(-2));
    $('#event-timefinish #minutes').text(('0' + $('#EndMinutes').val()).slice(-2));

    $('#Nickname')[0].disabled = $('#ShowOrganizator')[0].checked;
    $('#AllowRegistration')[0].disabled = $('#Private')[0].checked;

    $('#editButton').bind('click', function () {
        $('#editButton').addClass('hidden');
        $('#cancelEventButton').addClass('hidden');
        $('.eventindex-info').addClass('hidden');


        $('#cancelEditButton').removeClass('hidden');
        $('#saveButton').removeClass('hidden');
        $('.eventindex-edit').removeClass('hidden');

        $('.datepicker-chosen-arrow').css('visibility', 'visible');
        $('.timepicker-arrow').css('visibility', 'visible');
    });

    $('#cancelEditButton').bind('click', function () {
        $('#editButton').removeClass('hidden');
        $('#cancelEventButton').removeClass('hidden');
        $('.eventindex-info').removeClass('hidden');

        $('#cancelEditButton').addClass('hidden');
        $('#saveButton').addClass('hidden');
        $('.eventindex-edit').addClass('hidden');

        $('.datepicker-chosen-arrow').css('visibility', 'hidden');
        $('.timepicker-arrow').css('visibility', 'hidden');

        $('.eventindex-edit #Title').val(title);
        $('.eventindex-edit #Description').val(description);
        $('.eventindex-edit #Nickname').val(nickname);
        $('.eventindex-edit #ShowOrganizator')[0].checked = showOrganizator;
        $('.eventindex-edit #AllowRegistration')[0].checked = allowRegistration;
        $('.eventindex-edit #Private')[0].checked = isPrivate;

        if (isPrivate) {
            $('#AllowRegistration')[0].disabled = true;
        }

        if (!showOrganizator) {
            $('#Nickname')[0].disabled = false;
        }

        $('#event-date #day').text(currday);
        $('#event-date #month').text(currmonth);
        $('#event-date #year').text(curryear);

        $('#event-timestart #hours').text(startHour);
        $('#event-timestart #minutes').text(startMinutes);
        $('#event-timefinish #hours').text(endHour);
        $('#event-timefinish #minutes').text(endMinutes);

        displayErrors(true);
    });

    $('#cancelEventButton').bind('click', function (e) {
        e.preventDefault();

        var eventId = parseInt($('#Id').val(), 10);

        $.ajax({
            url: window.location.origin + '/Event/CancelEventView',
            method: 'GET',
            data: { id: eventId },
            dataType: 'html',
            success: function (result) {
                $('#popup-confirmation').html(result);
                $('#popup-confirmation').show();
            }
        });
    });

    $('#saveButton').bind('click', function (e) {
        e.preventDefault();
        debugger;
        var eventValidator = eventValidate();

        if ($('#Nickname').val() === '' && !$('#ShowOrganizator')[0].checked) {
            eventValidator.showErrors(false, 'Не указан организатор события.');
            return false;
        }

        if (eventValidator.validate() && $('#eventindex-editform').valid()) {
            if ($('#ShowOrganizator')[0].checked) {
                $('#Nickname').val('');
            }

            $.ajax({
                url: window.location.origin + '/Event/EditEvent',
                method: 'POST',
                data: $('#eventindex-editform').serialize(),
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

    $('#eventindex-editform .event-input').on('keyup blur', function () {
        if ($('#eventindex-editform').valid()) {
            $('#saveButton').attr('disabled', false);
        } else {
            $('#saveButton').attr('disabled', 'disabled');
        }
    });
});
