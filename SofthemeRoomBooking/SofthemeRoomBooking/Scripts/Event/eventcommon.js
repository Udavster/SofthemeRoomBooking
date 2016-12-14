var eventDate = new DatePickerChosen();
eventDate.init("#event-date", new Date(), null, function (day, month, year) {
    $("#Day").val(day);
    $("#Month").val(month + 1);
    $("#Year").val(year);

    dateTimeEventValidate().validate();
});

var eventTimeStart = new TimePicker();
eventTimeStart.init("#event-timestart", 9, 20, function (hours, minutes) {
    $("#StartHour").val(hours);
    $("#StartMinutes").val(minutes);

    dateTimeEventValidate().validate();
});

var eventTimeFinish = new TimePicker();
eventTimeFinish.init("#event-timefinish", 9, 20, function (hours, minutes) {
    $("#EndHour").val(hours);
    $("#EndMinutes").val(minutes);

    dateTimeEventValidate().validate();
});

$("#event-form input").on("keyup blur", function () {
    if ($("#event-form").valid()) {
        $("#event-submit").prop("disabled", false);
    } else {
        $("#event-submit").prop("disabled", "disabled");
    }
});

function dateTimeEventValidate() {
    var day = parseInt($("#Day").val(), 10),
        starthour = parseInt($("#StartHour").val(), 10),
        startminutes = parseInt($("#StartMinutes").val(), 10),
        endhour = parseInt($("#EndHour").val(), 10),
        endminutes = parseInt($("#EndMinutes").val(), 10),
        currentDate = new Date().getDate(),
        currentHour = new Date().getHours(),
        currentMinutes = new Date().getMinutes();

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

        if (day === currentDate) {
            if (starthour < currentHour || (starthour === currentHour && startminutes < currentMinutes)) {
                return { isValid: false, error: "Нельзя назначить событие в прошлом." };
            }
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
            $("#event-submit").attr("disabled", false);
            return true;
        }

        $("#event-submit").attr("disabled", true);
        return false;
    }

    function showErrors(isValid, message) {
        var messageContaiter = "<span>" + message + "</span>";

        if (isValid) {
            if (!$("#event-errors").hasClass("hidden")) {
                $("#event-errors").addClass("hidden");
            }
        } else {
            $(".error-text").html(messageContaiter);

            if ($("#event-errors").hasClass("hidden")) {
                $("#event-errors").removeClass("hidden");
            }
        }
    }

    return { validate: validate, showErrors: showErrors };
}

$("#Day").val(new Date().getDate());
$("#Month").val(new Date().getMonth());
$("#Year").val(new Date().getFullYear());

$("#StartHour").val($("#event-timestart #hours").text());
$("#StartMinutes").val($("#event-timestart #minutes").text());
$("#EndHour").val($("#event-timefinish #hours").text());
$("#EndMinutes").val($("#event-timefinish #minutes").text());

$("#Private").bind("click", function () {
    $("#public-checkbox").attr("checked", !$(this).is(":checked"));
    $("#public-checkbox").attr("disabled", $(this).is(":checked"));
});

$("#organizator-checkbox").bind("click", function () {
    $(".eventcontainer-body__organizator-name").attr("disabled", $(this).is(":checked"));
});