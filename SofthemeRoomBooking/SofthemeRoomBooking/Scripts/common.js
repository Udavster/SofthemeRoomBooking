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
            $(element).siblings('div .error-box').addClass('hidden');
        },
        highlight: function (element) {
            $(element).siblings('div .error-box').removeClass('hidden');
        }
    });
});