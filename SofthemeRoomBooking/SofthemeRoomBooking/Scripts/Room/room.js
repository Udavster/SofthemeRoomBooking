$(document).ready(function () {
    $('.room__general .room__general-blocked').click(function () {
        $('.room__open').show();
    });
    $('.room__general')
        .mouseover(function () {
            var equip = $(this).data('num').split(',');

            $('.equipmentuser').children().each(function (i, elem) {
                
                $(elem).data('info', equip[i]);
                if (equip[i] == '0') {
                    $(elem).hide();
                } else {
                    $(elem).show();
                }
            });


            $('.equipmentuser__tablenumber').html($('.equipmentuser__table-main').data('info'));
            $('.equipmentuser__boardnumber').html($('.equipmentuser__board-main').data('info'));
            $('.equipmentuser__laptopnumber').html($('.equipmentuser__laptop-main').data('info'));
            $('.equipmentuser__printnumber').html($('.equipmentuser__print-main').data('info'));
            $('.equipmentuser__proectnumber').html($('.equipmentuser__proect-main').data('info'));
            $('.room__equipment').show();
        })
        .mouseout(function() {
            $('.room__equipment').hide();
        }).click(function () {
            $('.room__open').hide();
            $('.room__close').hide();
            $('.room__change').data('roomid', $(this).data('roomid'));


            if ($(this).hasClass('room__general-blocked')) {            
                $('.room__open').show();         
            } else {
                $('.room__close').show();      
            }
            $('.room__change').show();
         
            $('.room__path').hide();
            $('.room__image').find($('#' + $(this).data('pathid'))).show();
            $('.room__general').css('background', '#9fa6b6');
            $('.room__general').children('.text').css('background-color', '#858c9a');
            $('.room__general-blocked').css('background', '#d7d9de');
            $('.room__general-blocked').children('.text').css('background-color', '#acb0b6');
            $(this).css('background', '#f95752');
            $(this).children('.text').css('background-color', 'red');
        });

    $('.room__change').click(function () {

        var _this = $(this);
        $('.active-room').removeClass('active-room');
        $.ajax({
            url: window.location.origin+"/Room/RoomEquipment",
            type: "GET",
            data: 'id=' + _this.data('roomid'),
            success: function (res) {
                $('.room__general').each(function(i, elem) {
                    if ($(elem).data('roomid') == _this.data("roomid")) {
                        $(elem).addClass('active-room').parents('.room__image').addClass('room-change');
                    }
                });
              
                $('.equipmentadmin').html(res).show();
            }
        });

    });
    //click open room
    $(document).on('click', '.room__open', function (e) {
        e.preventDefault();
        var _this = $(this);

        var id = _this.data('id');
        $.ajax({
            url: window.location.origin + "/Room/Open",
            data: 'id=' + id,
            type: 'GET',
            success: function (result) {
                $('#popup-confirmation').html(result);
                $('#popup-confirmation').show();
            }
        });
    });
    //click open room
    $(document).on('click', '.room__close', function (e) {
        e.preventDefault();
        var _this = $(this);
        var id = _this.data('id');

        $.ajax({
            url: window.location.origin + "/Room/Close",
            data: 'id=' + id,
            type: 'GET',
            success: function (result) {
                $('#popup-confirmation').html(result);
                $('#popup-confirmation').show();
            }
        });
    });

    //hide equipment admin
    $(document).on('click','.equipmentadmin-cancel', function() {
        $('.equipmentadmin').hide();
        $('.active-room').removeClass('active-room');
        $('.room__general').each(function (i, elem) {
                $(elem).parents('.room__image').removeClass('room-change');
        });
    });

    //show info about event
    $(document).on('click', '.event-info', function () {
        var _this = $(this);
        $('.eventdetailedit').remove();
        var id = _this.data("id");

        $.ajax({
            url: window.location.origin + "/Event/EventDetails",
            data: 'id=' + id,
            type: 'GET',
            success: function(res) {
                $('.week__scheduler').append(res);
            }
        });
    });

    //close event info popup
    $(document).on('click', '.eventdetailedit-close', function () {
        $('.eventdetailedit').remove();
    });

    //add data attribute to close button
    $(document).on('click', '.room__general', function () {
        var _this = $(this);
        $('.room__close').data('id', _this.data("roomid"));
    });

    //add data attribute to open button
    $(document).on('click', '.room__general', function () {
        var _this = $(this);
        $('.room__open').data('id', _this.data("roomid"));
    });
    //click empty field week scheduler
    $(document).on('click', '.calendar-item:not(.not-empty)', function () {
        var _this = $(this);

       //var start = _this.data('start');
       //var finish = _this.data('finish');
       //alert('clicked start =' + start + ' finish =' + finish + '');
       $.ajax({
           url: window.location.origin + "/Event/CreateEvent",
           type: 'GET',
           success: function (res) {
               $('.week__scheduler').append(res);
           }
       });
    });
    $(document).on("click","#closeButton", function () {
        $(".eventcontainer").remove();
    });

});

