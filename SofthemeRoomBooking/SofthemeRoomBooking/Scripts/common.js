$(document).ready(function ($) {
    var url = document.location.href;
    $.each($('.menu a'), function () {
        if (this.href === url) {
            $(this).addClass('menu__link-active');
        };
    });
}(jQuery));