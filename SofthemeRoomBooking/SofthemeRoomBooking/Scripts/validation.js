$(document).ready(function () {
    $.validator.setDefaults({
        unhighlight: function(element) {
            $(element).siblings('div.error-box').addClass('hidden');
        },
        highlight: function(element) {
            $(element).siblings('div.error-box').removeClass('hidden');
        }
    });
});