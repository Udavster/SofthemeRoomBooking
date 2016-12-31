$(document).ready(function() {

    $(document).on('click', '#changeevent', function () {
        var _this = $(this);
        $('#popup-edit-event').html('');
        var id = _this.data('id');
        console.log(id);
        $.ajax({
            url: window.location.origin + "/Event/EditEventPartial",
            data: 'eventId=' + id,
            type: 'GET',
            success: function (res) {
                $('#popup-edit-event').html(res);
                $('#popup-edit-event').show();
            }
        });
    });
});
