$(document).on('click', '#changeevent', function () {
    var _this = $(this);
    var id = _this.data('id');
    $.ajax({
        url: window.location.origin + "/Event/EditEventPartial",
        data: 'eventId=' + id,
        type: 'GET',
        success: function (res) {
            $('#popup-edit-event').html(res);
            $('#popup-edit-event').show();

            $.validator.unobtrusive.parse("#event-edit-form");
        }
    });
});
