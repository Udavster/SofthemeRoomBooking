$(document).ready(function() {
    $('#Private').change(function () {
        $('#AllowRegistration')[0].checked = !this.checked;
        $('#AllowRegistration')[0].disabled = this.checked;
    });
    
    $('#ShowOrganizator').change(function () {
        var $that = $('#Nickname');
        var $form = $that.closest('form');
        var validator = $form.validate();

        if (this.checked) {
            if (validator.settings.unhighlight) {
                validator.settings.unhighlight($that[0]);
            }

            var $errors = $form.find('.field-validation-error span');
            $errors.each(function () { validator.settings.success($(this)); });

            $('.submit-btn').attr('disabled', false);
        } else {
            if (!validator.element($that[0])) {
                $('.submit-btn').attr('disabled', 'disabled');
            }
        }
        
        $('#Nickname')[0].disabled = this.checked;
    });
});

function setDefaultEventSettings() {
    $('#Private')[0].checked = false;
    $('#AllowRegistration')[0].checked = true;
    $('#ShowOrganizator')[0].checked = true;
};

function setEventDateTime(startTime, finishTime) {
    if (!startTime || !(startTime instanceof Date)) {
        startTime = new Date();
    }

    if (!finishTime || !(finishTime instanceof Date)) {
        finishTime = new Date();
    }
    
    $('#event-date #day').text(('0' + startTime.getDate()).slice(-2));
    $('#event-date #month').text(getMonthName(startTime.getMonth()));
    $('#event-date #year').text(startTime.getFullYear());

    $('#event-timestart #hours').text(('0' + startTime.getHours()).slice(-2));
    $('#event-timestart #minutes').text(('0' + startTime.getMinutes()).slice(-2));
    $('#event-timefinish #hours').text(('0' + finishTime.getHours()).slice(-2));
    $('#event-timefinish #minutes').text(('0' + finishTime.getMinutes()).slice(-2));

    $('#Year').val(startTime.getFullYear());
    $('#Month').val(startTime.getMonth() + 1);
    $('#Day').val(startTime.getDate());

    $('#StartHour').val(startTime.getHours());
    $('#StartMinutes').val(startTime.getMinutes());
    $('#FinishHour').val(finishTime.getHours());
    $('#FinishMinutes').val(finishTime.getMinutes());

    $('#event-date #arrow-up-day').click();
    $('#event-date #arrow-down-day').click();
};

function setEventRoom(idRoom) {
    $('#IdRoom').val(idRoom);
}

function getNearestWeekday(date) {
    if (!date || !(date instanceof Date)) {
        date = new Date();
    }

    var secInOneDay = 24*3600;
    while (date.getDay() === 6 || date.getDay() === 0) {
        date.setTime(date.getTime() + secInOneDay);
    }

    return date;
}

function getMonthName(monthNum) {
    if (isNaN(monthNum)) {
        return null;
    }

    var months = ['Января', 'Февраля', 'Марта', 'Апреля', 'Мая', 'Июня', 'Июля', 'Августа', 'Сентября', 'Октября', 'Ноября', 'Декабря'];

    return months[monthNum];
}

function initDateTime(monthType, dateSepartor, submitButton) {

    dateSepartor = dateSepartor === null ? '' : dateSepartor;

    var eventDate = new DatePickerChosen();
    eventDate.init('#event-date', dateSepartor, monthType, new Date(), null, function (day, month, year) {
        $('#Day').val(day);
        $('#Month').val(month + 1);
        $('#Year').val(year);

        if (eventValidate().validate()) {
            submitButton.attr('disabled', false);
        } else {
            submitButton.attr('disabled', 'disabled');
        }
    });

    var eventTimeStart = new TimePicker();
    eventTimeStart.init('#event-timestart', 9, 20, function (hours, minutes) {
        $('#StartHour').val(hours);
        $('#StartMinutes').val(minutes);

        if (eventValidate().validate()) {
            submitButton.attr('disabled', false);
        } else {
            submitButton.attr('disabled', 'disabled');
        }
    });

    var eventTimeFinish = new TimePicker();
    eventTimeFinish.init('#event-timefinish', 9, 20, function (hours, minutes) {
        $('#FinishHour').val(hours);
        $('#FinishMinutes').val(minutes);

        if (eventValidate().validate()) {
            submitButton.attr('disabled', false);
        } else {
            submitButton.attr('disabled', 'disabled');
        }
    });
}

function eventValidate() {
    var day = parseInt($('#Day').val(), 10),
        month = parseInt($('#Month').val(), 10) - 1,
        year = parseInt($('#Year').val(), 10),
        starthour = parseInt($('#StartHour').val(), 10),
        startminutes = parseInt($('#StartMinutes').val(), 10),
        endhour = parseInt($('#FinishHour').val(), 10),
        endminutes = parseInt($('#FinishMinutes').val(), 10);

    function validationDurationEvent() {
        if (endhour > starthour || (endhour === starthour && endminutes - startminutes >= 20)) {
            return { isValid: true };
        }

        return { isValid: false, error: 'Минимальное время бронирования аудитории 20 минут.' };
    }

    function validationStartTimeEvent() {
        if (starthour > endhour || (starthour === endhour && startminutes > endminutes)) {
            return { isValid: false, error: 'Время начала события не может быть больше времени окончания.' };
        }

        if (new Date(year, month, day, starthour, startminutes) <= new Date()) {
            return { isValid: false, error: 'Нельзя назначить событие в прошлом.' };
        }

        return { isValid: true };
    }

    function validationWeekday() {
        var currDate = new Date(year, month, day);

        var isWeekday = !(currDate.getDay() === 6 || currDate.getDay() === 0);

        return { isWeekday: isWeekday, error: isWeekday ? '' : 'Нельзя назначить событие в выходной день.' };
    };

    function validate() {
        var isValidStartTime = validationStartTimeEvent(),
            isValidDuration = validationDurationEvent(),
            isWeekday = validationWeekday(),
            errorMessageTime = isValidStartTime.error,
            errorMessageDuration = isValidDuration.error,
            errorMessageWeekday = isWeekday.error;

        var errors = '';
        var isValid = true;

        if (!isValidDuration.isValid) {
            errors += errorMessageDuration;
            isValid = false;
        }

        if (!isValidStartTime.isValid) {
            errors += errors === '' ? errorMessageTime : ' ' + errorMessageTime;
            isValid = false;
        }

        if (!isWeekday.isWeekday) {
            errors += errors === '' ? errorMessageWeekday : ' ' + errorMessageWeekday;
            isValid = false;
        }

        if (!isValid) {
            showErrors(false, errors);
        } else {
            showErrors(true);
            return true;
        }

        return false;
    }

    function showErrors(isValid, message) {
        var messageContaiter = '<span>' + message + '</span>';

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

    return { validate: validate, showErrors: showErrors };
}

function checkSetOrganizator() {
    var eventValidator = eventValidate();

    if ($('#Nickname').val() === '' && !$('#ShowOrganizator')[0].checked) {
        eventValidator.showErrors(false, 'Не указан организатор события.');

        $('.submit-btn').attr('disabled', 'disabled');

        return false;
    }

    $('.submit-btn').attr('disabled', false);

    return true;
}
