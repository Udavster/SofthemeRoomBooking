$(document)
    .ready(function() {
        $(".form__label")
            .click(function(evt) {
                console.log(evt);
                if (evt.altKey) {
                    console.log('Autofill');
                    $('#Email').val('');
                    $('#Password').val('');
                }
            });
    });