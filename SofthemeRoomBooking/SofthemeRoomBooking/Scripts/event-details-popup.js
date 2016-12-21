$(document).on('click', '#cancelevent', function () {
    var $this = $(this);
    var id = $this.data('id');
    $.ajax({
        url: window.location.origin + "/Event/CancelEvent",
        data: 'id=' + id,
        type: 'GET',
        success: function (res) {
            $('#confirmation_popup').html(res)
                                    .show();
        }
    });
});

$(document).on('click', '.eventdetailedit-close', function () {
    $('.eventdetailedit').remove();
});


//close private info popup
$(document).on('click', '.eventdetailedit-close', function () {
    $('.eventdetail').remove();
});