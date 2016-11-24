$(document).ready(function() {
    $('.menu li a').on('click', function(event) {
        $(this).addClass('menu__link-active');
    });
});

$(document).ready(function () {
    $.validator.setDefaults({
        unhighlight: function (element) {
            $(element).siblings('div.error-box').addClass('hidden');
        },
        highlight: function (element) {
            $(element).siblings('div.error-box').removeClass('hidden');
        }
    });
});