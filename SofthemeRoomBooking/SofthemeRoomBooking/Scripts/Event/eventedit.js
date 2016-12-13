$("#minimizeButton").bind("click", function () {
    $("#popup-edit-event").hide();
});

$("#even-submit").bind("click", function () {

    var validator = $("#event-form").validate();

    if (!dateTimeEventValidate() || !$("#event-form").valid()) {
        validator.invalidElements();
        $(this).attr("disabled", true);
        return false;
    } else {
        var month = $("#Month").val();
        $("#Month").val(month + 1);
        $("#event-form").submit();
    }
});