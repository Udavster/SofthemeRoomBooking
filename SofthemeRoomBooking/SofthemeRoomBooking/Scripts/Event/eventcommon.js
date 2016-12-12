$("#Publicity").bind("click", function () {
    $("#public-checkbox").attr("checked", !$(this).is(":checked"));
    $("#public-checkbox").attr("disabled", $(this).is(":checked"));
});

$("#organizator-checkbox").bind("click", function () {
    $(".eventcontainer-body__organizator-name").attr("disabled", $(this).is(":checked"));
});

var eventDate = new DatePickerChosen();
eventDate.init("#event-date", new Date(), null, function (day, month, year) {
    $("#Day").val(day);
    $("#Month").val(month);
    $("#Year").val(year);

    dateTimeEventValidate();
});

var eventTimeStart = new TimePicker();
eventTimeStart.init("#event-timestart", 9, 20, function (hours, minutes) {
    $("#StartHour").val(hours);
    $("#StartMinutes").val(minutes);

    dateTimeEventValidate();
});

var eventTimeFinish = new TimePicker();
eventTimeFinish.init("#event-timefinish", 9, 20, function (hours, minutes) {
    $("#EndHour").val(hours);
    $("#EndMinutes").val(minutes);

    dateTimeEventValidate();
});


function isValidTimeEvent() {
    var day = parseInt($('#Day').val(), 10),
        hour = parseInt($('#StartHour').val(), 10),
        minutes = parseInt($('#StartMinutes').val(), 10),
        currentDate = new Date().getDate(),
        currentHour = new Date().getHours(),
        currentMinutes = new Date().getMinutes();

    if (day === currentDate) {
        if (hour < currentHour) {
            return false;
        } else if (hour === currentHour) {
            if (minutes < currentMinutes) {
                return false;
            }
        }
    }

    return true;
}

function isValidDurationEvent() {
    var startHour = parseInt($("#StartHour").val(), 10),
        startMinutes = parseInt($("#StartMinutes").val(), 10),
        endHour = parseInt($("#EndHour").val(), 10),
        endMinutes = parseInt($("#EndMinutes").val(), 10);

    if (endHour > startHour || (endHour === startHour && endMinutes - startMinutes >= 20)) {
        return true;
    }

    return false;
}

function dateTimeEventValidate() {
    var errorMessageTime = "Нельзя назначить событие в прошлом.",
        errorMessageDuration = "Минимальное время бронирования аудитории 20 минут.";

    if (!isValidDurationEvent() && !isValidTimeEvent()) {
        showError(false, errorMessageTime + " " + errorMessageDuration);
    } else if (!isValidDurationEvent()) {
        showError(false, errorMessageDuration);
    } else if (!isValidTimeEvent()) {
        showError(false, errorMessageTime);
    } else {
        showError(true);
    }
}

function showError(isValid, message) {
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

$("#Day").val($("#event-date #day").text());
$("#Month").val($("#event-date #day").text());
$("#StartHour").val($("#event-timestart #hours").text());
$("#StartMinutes").val($("#event-timestart #minutes").text());
$("#EndHour").val($("#event-timefinish #hours").text());
$("#EndMinutes").val($("#event-timefinish #minutes").text());