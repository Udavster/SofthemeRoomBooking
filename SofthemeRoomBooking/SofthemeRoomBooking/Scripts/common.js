$(document).ready(function ($) {
    var url = document.location.href;
    $.each($('.menu a'), function () {
        if (this.href === url) {
            $(this).addClass('menu__link-active');
        };
    });
}(jQuery));


$(document).ready(function () {
    $.validator.setDefaults({
        unhighlight: function (element) {
            $(element).siblings('div.error-box').addClass('hidden');
            $('.input-submit').attr('disabled', false);
        },
        highlight: function (element) {
            $(element).siblings('div.error-box').removeClass('hidden');
            if ($('.div.error-box.hidden') === 0) {
                $('.input-submit').attr('disabled', true);
            }
        }
    });
});