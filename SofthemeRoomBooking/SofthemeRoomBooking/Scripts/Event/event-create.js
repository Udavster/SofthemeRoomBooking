$("#closeButton").bind("click", function () {
    $("#popup-create-event").html("");
    $("#popup-create-event").hide();
});

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
            url: window.location.origin + "/Event/CreateEvent",
            method: "POST",
            data: $("#event-form").serialize(),
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

$("#Day").val(new Date().getDate());
$("#Month").val(new Date().getMonth());
$("#Year").val(new Date().getFullYear());

$("#StartHour").val($("#event-timestart #hours").text());
$("#StartMinutes").val($("#event-timestart #minutes").text());
$("#EndHour").val($("#event-timefinish #hours").text());
$("#EndMinutes").val($("#event-timefinish #minutes").text());

setDefaultEventSettings();

