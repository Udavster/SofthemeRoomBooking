$("#minimizeButton").bind("click", function () {
    $("#popup-edit-event").hide("");
    $("#popup-edit-event").hide();
});

$("#AllowRegistration")[0].disabled = $("#Private")[0].checked;

$("#event-submit").bind("click", function (e) {
    e.preventDefault();
    var eventValidator = eventValidate();

    if ($("#Nickname").val() === "" && !$("#ShowOrganizator")[0].checked) {
        eventValidator.showErrors(false, "Не указан организатор события.");
        return false;
    }

    if (eventValidator.validate() && $("#event-form").valid()) {
        if ($("#ShowOrganizator")[0].checked) {
            $("#Nickname").val("");
        }

        $.ajax({
            url: window.location.origin + "/Event/EditEventPartial",
            method: "POST",
            data: $("#event-form").serialize(),
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
        return false;
    }
});

if ($("#Nickname").val() === "") {
    $("#Nickname")[0].disabled = $("#ShowOrganizator")[0].checked = true;
}

setDateTime();



function setDateTime(startTime, finishTime) {

    var day = parseInt($("#Day").val()),
    month = parseInt($("#Month").val()) - 1,
    year = parseInt($("#Year").val()),
    startHour = parseInt($("#StartHour").val()),
    startMinutes = parseInt($("#StartMinutes").val()),
    finishHour = parseInt($("#FinishHour").val()),
    finishMinutes = parseInt($("#FinishMinutes").val()),
        
    months = ['Января', 'Февраля', 'Марта', 'Апреля', 'Мая', 'Июня', 'Июля', 'Августа', 'Сентября', 'Октября', 'Ноября', 'Декабря'];

    if (!startTime || !(startTime instanceof Date)) {

        startTime = new Date(year, month, day, startHour, startMinutes);
    }

    if (!finishTime || !(finishTime instanceof Date)) {

        finishTime = new Date(year, month, day, finishHour, finishMinutes);
    }

    $("#event-date #day").text(("0" + startTime.getDate()).slice(-2));
    $("#event-date #month").text(months[startTime.getMonth()]);
    $("#event-date #year").text(startTime.getFullYear());

    $("#event-timestart #hours").text(("0" + startTime.getHours()).slice(-2));
    $("#event-timestart #minutes").text(("0" + startTime.getMinutes()).slice(-2));
    $("#event-timefinish #hours").text(("0" + finishTime.getHours()).slice(-2));
    $("#event-timefinish #minutes").text(("0" + finishTime.getMinutes()).slice(-2));

    $("#event-date #arrow-up-day").click();
    $("#event-date #arrow-down-day").click();
};

function setRoom(idRoom) {
    $("#IdRoom").val(idRoom);
}