$(document).ready(function () {
    $('.room__general .room__general-blocked').click(function () {
        $('.room__open').show();
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

    clickOnRoom();

    changeRoomNameEquipment();

    unlockRoom();

    lockRoom();

    closeAdminEquipmentPopup();
});

var openRoomById = function (RoomId) {
    console.log('.room-' + RoomId);
    openRoom($('.room-' + RoomId));
    
}
var roomClickHighlightHandler = function (event) {
    var $this = $(event.target);
    if (!$this.hasClass('room__general')) {
        $this = $this.parent();
    }
    console.log($this);
    if (!$this.hasClass('room__general')) return;
    openRoom($this);
}
var openRoom = function ($this) {
    //button for open/close/change room
    $('.room__close').data('id', $this.data("roomid"));
    $('.room__open').data('id', $this.data("roomid"));
    $('.room__open').hide();
    $('.room__close').hide();
    $('.room__change').data('roomid', $this.data('roomid'));

    if ($this.hasClass('room__general-blocked')) {
        $('.room__open').show();
        $('.week__scheduler').hide();
        $('.room').css('margin-bottom', '50px');
    } else {
        $('.room__close').show();
        $('.week__scheduler').show();
        $('.room').css('margin-bottom', '');
    }
    $('.room__change').show();

    $('.room__path').hide();
    $('.room__image').find($('#' + $this.data('pathid'))).show();
    $('.room__general').css('background', '#9fa6b6');
    $('.room__general').children('.text').css('background-color', '#858c9a');
    $('.room__general-blocked').css('background', '#d7d9de');
    $('.room__general-blocked').children('.text').css('background-color', '#acb0b6');
    $this.css('background', '#f95752');
    $this.children('.text').css('background-color', 'red');
}
function clickOnRoom() {
    $('.room__general')
    .mouseover(function () {
        $('.room__general:hover').children().css('display', 'block');
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
    .mouseout(function () {
        $('.room__equipment').hide();
        $('.room__arrow').hide();
    }).click(roomClickHighlightHandler);
}

function changeRoomNameEquipment() {
    $('.room__change').click(function () {

        var _this = $(this);
        $('.active-room').removeClass('active-room');
        $.ajax({
            url: window.location.origin + "/Room/RoomEquipment",
            type: "GET",
            data: 'id=' + _this.data('roomid'),
            success: function (res) {
                $('.room__general').each(function (i, elem) {
                    if ($(elem).data('roomid') == _this.data("roomid")) {
                        $(elem).addClass('active-room').parents('.room__image').addClass('room-change');
                        $(elem).children().css('display','block');
                        $(elem).find('.text').hide();
                        $('.room__general').css('pointer-events', 'none');
                    }
                });

                $('.equipmentadmin').html(res).show();
            }
        });

    });
}

function unlockRoom() {
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
}

function lockRoom() {
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
}

function closeAdminEquipmentPopup() {
    //hide equipment admin
    $(document).on('click', '.equipmentadmin-cancel', function () {
        $('.room').find('.active-room').find('.text').show();
        $('.room__general').css('pointer-events', '');
        $('.equipmentadmin').hide();
        $('.active-room').removeClass('active-room');
        $('.room__arrow').hide();
        $('.room__general').each(function (i, elem) {
            $(elem).parents('.room__image').removeClass('room-change');
        });
    });
}
