$.validator.setDefaults({
    unhighlight: function (element) {
        $(element).siblings("div .error-box").addClass("hidden-error");
    },
    highlight: function (element) {
        $(element).siblings("div .error-box").removeClass("hidden-error");
    }
});