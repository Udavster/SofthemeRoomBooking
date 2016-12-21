$(document)
    .ready(function() {
        $(".form__label")
            .click(function(evt) {
                console.log(evt);
                if (evt.altKey) {
                    console.log('Autofill');
                    $('#Name').val('Danil');
                    $('#Surname').val('Shypik');
                    $('#Email').val('shypikd@gmail.com');
                    $('#Password').val('qwe123Q!');
                    $('#ConfirmPassword').val('qwe123Q!');
                }
            });
    });
