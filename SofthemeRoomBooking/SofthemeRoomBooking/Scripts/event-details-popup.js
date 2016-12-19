$(document).on('click', '#cancelevent', function () {
    var $this = $(this);
    var id = $this.data('id');
    $.ajax({
        url: window.location.origin + "/Event/CancelEventView",
        data: 'id=' + id,
        type: 'GET',
        success: function (res) {
            $('#confirmation_popup').html(res)
                                    .show();
        }
    });
});