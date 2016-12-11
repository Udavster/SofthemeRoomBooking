$('#Publicity').bind('click', function () {
    $('#public-checkbox').attr('checked', !$(this).is(':checked'));
    $('#public-checkbox').attr('disabled', $(this).is(':checked'));
});

$('#organizator-checkbox').bind('click', function () {
    $('.eventcontainer-body__organizator-name').attr('disabled', $(this).is(':checked'));
});

var eventDate = new DatePickerChosen(),
    eventTimeStart = new TimePicker(),
    eventTimeFinish = new TimePicker();

jQuery.validator.addMethod('eventDateValidate', function () {
    var day = parseInt($('#Day').val(), 10),
        month = parseInt($('#Month').val(), 10);

    if (month >= new Date().getMonth()) {
        if (day >= new Date().getDate()) {
            return true;
        }
    }

    return false;
}, 'Невозможно забронированть аудиторию на прошедшие даты.');

jQuery.validator.addMethod('durationEventValidate', function () {
    var startHour = parseInt($('#StartHour').val(), 10),
        startMinutes = parseInt($('#StartMinutes').val(), 10),
        endHour = parseInt($('#EndHour').val(), 10),
        endMinutes = parseInt($('#EndMinutes').val(), 10);

    if (endHour > startHour || (endHour === startHour && endMinutes - startMinutes >= 20)) {
        return true;
    }

    return false;
}, 'Минимальное время бронирования аудитории 20 минут.');

var validator = $('#event-form').validate({
    ignore: [],
    errorElement: 'span',
    errorContainer: '#event-errors',
    invalidHandler: function (event, validator) {
        var errors = validator.numberOfInvalids();

        if (errors) {
            $('#event-errors').removeClass('hidden');
        } else {
            $('#event-errors').addClass('hidden');
        }
    },
    rules: {
        Day: { eventDateValidate: true },
        StartHour: { durationEventValidate: true }
    },
    errorPlacement: function (error) {
        error.addClass('error-text');
        error.appendTo('#event-errors');
    }
});

eventDate.init("#event-date", function (day, month) {
    $('#Day').val(day);
    $('#Month').val(month);
    validator.element(':input[name="Day"]');
});

eventTimeStart.init('#event-timestart', 9, 20, function (hours, minutes) {
    $('#StartHour').val(hours);
    $('#StartMinutes').val(minutes);
    validator.element(':input[name="StartHour"]');
});

eventTimeFinish.init('#event-timefinish', 9, 20, function (hours, minutes) {
    $('#EndHour').val(hours);
    $('#EndMinutes').val(minutes);
    validator.element('input[name="StartHour"]');
});

$('#Day').val($('#event-date #day').text());
$('#Month').val($('#event-date #day').text());
$('#StartHour').val($('#event-timestart #hours').text());
$('#StartMinutes').val($('#event-timestart #minutes').text());
$('#EndHour').val($('#event-timefinish #hours').text());
$('#EndMinutes').val($('#event-timefinish #minutes').text());