$("#closeButton").bind("click", function () {
    $("#popup-create-event").hide();
});

$("#event-submit").bind("click", function (e) {
    e.preventDefault();

    var dateTimeValidator = dateTimeEventValidate();

    if (dateTimeValidator.validate() && $("#event-form").valid()) {

        $.ajax({
            url: window.location.origin + "/Event/CreateEvent",
            method: "POST",
            async: true,
            data: $("#event-form").serialize(),
            success: function (result) {
                if (result.redirectTo) {
                    window.location.href = result.redirectTo;
                } else if (result.errorMessage) {
                    dateTimeValidator.showErrors(false, result.errorMessage);
                } 
            }
        });
        return true;
    } else {
        return false;
    }
});