$(document).ready(function() {
    //show info about event
    $(document).on('click', '.event-info', function () {
        var _this = $(this);
        $('.eventdetailedit').remove();
        $('.eventdetail').remove();

        var id = _this.data("id");

        $.ajax({
            url: window.location.origin + "/Event/EventDetails",
            data: 'id=' + id,
            type: 'GET',
            success: function (res) {
                $('.week__scheduler').append(res);
            }
        });
    });
    //close event info popup
    $(document).on('click', '.eventdetailedit-close', function () {
        $('.eventdetailedit').remove();
    });

    //show info about private event
    $(document).on('click', '.event-private', function () {
        var _this = $(this);
        $('.eventdetail').remove();
        $('.eventdetailedit').remove();
        var id = _this.data("id");

        $.ajax({
            url: window.location.origin + "/Event/EventDetails",
            data: 'id=' + id,
            type: 'GET',
            success: function (res) {
                $('.week__scheduler').append(res);
            }
        });
    });

    //close private info popup
    $(document).on('click', '.eventdetailedit-close', function () {
        $('.eventdetail').remove();
    });

    //click empty field week scheduler
    $(document).on('click', '.calendar-item:not(.not-empty)', function () {
        var _this = $(this);
        var id = _this.data('roomid');
        var start = parseInt(_this.data('start'), 10);
        var finish = parseInt(_this.data('finish'), 10);
        var day = parseInt(_this.data('day'), 10);
        var month = parseInt(_this.data('month'), 10);
        var year = parseInt(_this.data('year'), 10);

        var startDate = new Date(year, month - 1, day, start);
        var endDate = new Date(year, month - 1, day, finish);

    });
    $(document).on("click", "#closeButton", function () {
        $(".eventcontainer").remove();
    });
    // click on chain to open event view
    $(document).on('click', '#eventdetail__link', function () {
        var _this = $(this);
        var id = _this.data('id');
        console.log(id);
        $.ajax({
            url: window.location.origin + "/Room/Index",
            data: 'id=' + id,
            type: 'GET',
            success: function (res) {
                if (res.redirect) {
                    window.location.href = res.redirect;
                }
            }
        });
    });
});