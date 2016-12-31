$("#event-form .event-input").on("keyup blur", function () {
    if ($("#event-form").valid()) {
        $("#event-submit").attr("disabled", false);
    } else {
        $("#event-submit").attr("disabled", "disabled");
    }
});

$("#Private").change(function () {
    $('#AllowRegistration')[0].checked = !this.checked;
    $("#AllowRegistration")[0].disabled = this.checked;
});

$("#ShowOrganizator").change(function () {
    $("#Nickname")[0].disabled = this.checked;
});

setDefaltDateTime();

function setDefaultEventSettings() {
    $("#Private")[0].checked = false;
    $("#AllowRegistration")[0].checked = true;
    $("#ShowOrganizator")[0].checked = true;
};

function setDefaltDateTime() {
    var eventDate = new DatePickerChosen();
    eventDate.init("#event-date", null, "word", new Date(), null, function (day, month, year) {
        $("#Day").val(day);
        $("#Month").val(month + 1);
        $("#Year").val(year);

        if (eventValidate().validate()) {
            $("#event-submit").attr("disabled", false);
        } else {
            $("#event-submit").attr("disabled", "disabled");
        }
    });

    var eventTimeStart = new TimePicker();
    eventTimeStart.init("#event-timestart", 9, 20, function (hours, minutes) {
        $("#StartHour").val(hours);
        $("#StartMinutes").val(minutes);

        if (eventValidate().validate()) {
            $("#event-submit").attr("disabled", false);
        } else {
            $("#event-submit").attr("disabled", "disabled");
        }
    });

    var eventTimeFinish = new TimePicker();
    eventTimeFinish.init("#event-timefinish", 9, 20, function (hours, minutes) {
        $("#FinishHour").val(hours);
        $("#FinishMinutes").val(minutes);

        if (eventValidate().validate()) {
            $("#event-submit").attr("disabled", false);
        } else {
            $("#event-submit").attr("disabled", "disabled");
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
