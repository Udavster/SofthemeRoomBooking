function setDefaultEventSettings() {
    $("#Private")[0].checked = false;
    $("#AllowRegistration")[0].checked = true;
    $("#ShowOrganizator")[0].checked = true;
};


function setEventDateTime(startTime, finishTime) {
    var months = ['Января', 'Февраля', 'Марта', 'Апреля', 'Мая', 'Июня', 'Июля', 'Августа', 'Сентября', 'Октября', 'Ноября', 'Декабря'];

    if (!startTime || !(startTime instanceof Date)) {
        startTime = new Date();
    }

    if (!finishTime || !(finishTime instanceof Date)) {
        finishTime = new Date();
    }

    $('#event-date #day').text(('0' + startTime.getDate()).slice(-2));
    $('#event-date #month').text(months[startTime.getMonth()]);
    $('#event-date #year').text(startTime.getFullYear());

    $('#event-timestart #hours').text(('0' + startTime.getHours()).slice(-2));
    $('#event-timestart #minutes').text(('0' + startTime.getMinutes()).slice(-2));
    $('#event-timefinish #hours').text(('0' + finishTime.getHours()).slice(-2));
    $('#event-timefinish #minutes').text(('0' + finishTime.getMinutes()).slice(-2));

    $('#Year').val(startTime.getFullYear());
    $('#Month').val(startTime.getMonth());
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

function initDateTime(monthType, dateSepartor, submitButton) {

    dateSepartor = dateSepartor === null ? '' : dateSepartor;

    var eventDate = new DatePickerChosen();
    eventDate.init("#event-date", dateSepartor, monthType, new Date(), null, function (day, month, year) {
        $("#Day").val(day);
        $("#Month").val(month + 1);
        $("#Year").val(year);

        if (eventValidate().validate()) {
            submitButton.attr("disabled", false);
        } else {
            submitButton.attr("disabled", "disabled");
        }
    });

    var eventTimeStart = new TimePicker();
    eventTimeStart.init("#event-timestart", 9, 20, function (hours, minutes) {
        $("#StartHour").val(hours);
        $("#StartMinutes").val(minutes);

        if (eventValidate().validate()) {
            submitButton.attr("disabled", false);
        } else {
            submitButton.attr("disabled", "disabled");
        }
    });

    var eventTimeFinish = new TimePicker();
    eventTimeFinish.init("#event-timefinish", 9, 20, function (hours, minutes) {
        $("#FinishHour").val(hours);
        $("#FinishMinutes").val(minutes);

        if (eventValidate().validate()) {
            submitButton.attr("disabled", false);
        } else {
            submitButton.attr("disabled", "disabled");
        }
    });
}

function eventValidate() {
    var day = parseInt($("#Day").val(), 10),
        month = parseInt($("#Month").val(), 10) - 1,
        year = parseInt($("#Year").val(), 10),
        starthour = parseInt($("#StartHour").val(), 10),
        startminutes = parseInt($("#StartMinutes").val(), 10),
        endhour = parseInt($("#FinishHour").val(), 10),
        endminutes = parseInt($("#FinishMinutes").val(), 10);

    function isValidDurationEvent() {
        if (endhour > starthour || (endhour === starthour && endminutes - startminutes >= 20)) {
            return { isValid: true };
        }

        return { isValid: false, error: "Минимальное время бронирования аудитории 20 минут." };
    }

    function isValidStartTimeEvent() {
        if (starthour > endhour || (starthour === endhour && startminutes > endminutes)) {
            return { isValid: false, error: "Время начала события не может быть больше времени окончания." };
        }

        if (new Date(year, month, day, starthour, startminutes) <= new Date()) {
            return { isValid: false, error: "Нельзя назначить событие в прошлом." };
        }

        return { isValid: true };
    }

    function validate() {
        var isValidStartTime = isValidStartTimeEvent(),
            isValidDuration = isValidDurationEvent(),
            errorMessageTime = isValidStartTime.error,
            errorMessageDuration = isValidDuration.error;

        if (!isValidDuration.isValid && !isValidStartTime.isValid) {
            showErrors(false, errorMessageTime + " " + errorMessageDuration);
        } else if (!isValidStartTime.isValid) {
            showErrors(false, errorMessageTime);
        } else if (!isValidDuration.isValid) {
            showErrors(false, errorMessageDuration);
        } else {
            showErrors(true);
            return true;
        }

        return false;
    }

    function showErrors(isValid, message) {
        var messageContaiter = "<span>" + message + "</span>";

        if (isValid) {
            if (!$("#event-errors").hasClass("hidden-error")) {
                $("#event-errors").addClass("hidden-error");
            }
        } else {
            $(".error-text").html(messageContaiter);

            if ($("#event-errors").hasClass("hidden-error")) {
                $("#event-errors").removeClass("hidden-error");
            }
        }
    }

    return { validate: validate, showErrors: showErrors };
}
