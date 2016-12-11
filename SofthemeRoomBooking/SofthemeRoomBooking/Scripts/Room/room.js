$(document).ready(function () {
    $('.room__general')
        .mouseover(function () {
            $('.equipmentuser__tablenumber').html($(this).data('num'));
            $('.room__equipment').show();
        })
        .mouseout(function() {
            $('.room__equipment').hide();
        }).click(function () {
            $('.room__path').hide();
            $('.room__image').find($('#' + $(this).data('pathid'))).show();
        });
});

