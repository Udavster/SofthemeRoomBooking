$(document).ready(function() {
    $('.menu li a').on('click', function(event) {
        $('.menu li a').removeClass('menu__link-active');
        $(this).addClass('menu__link-active');
    });
});