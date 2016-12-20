$(document).ready(function() {
    hoverRoom();
});

function hoverRoom() {
    $('.room__general')
        .mouseover(function() {
            $('.room__general:hover').children().css('display', 'block');
            var equip = $(this).data('num').split(',');

            $('.equipmentuser')
                .children()
                .each(function(i, elem) {

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
            $('.room__arrow').hide();
        });
}