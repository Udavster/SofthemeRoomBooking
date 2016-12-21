$(document).ready(function() {
    $('#event-create-form .event-input').on('keyup blur', function (e) {
        var validator = $('#event-create-form').validate();
        
        if (validator.element(e.target)) {
            if (e.target === $('#Nickname')[0]) {

                var $errors = $('#event-create-form .error-text span').text();
                if ($errors.includes('Не указан организатор события.')) {

                    $errors = $errors.replace(/Не указан организатор события./g, '');
                    if ($errors === '') {
                        eventValidate().showErrors(true);
                        $('#createButton').attr('disabled', false);
                    } else {
                        eventValidate().showErrors(false, $errors);
                    }
                }

                checkSetOrganizator();
            } else {
                $('#createButton').attr('disabled', false);
            }
        } else {
            $('#createButton').attr('disabled', 'disabled');
        }
    });

    $('#closeButton').bind('click', function () {
        $('#popup-create-event').html('');
        $('#popup-create-event').hide();
    });

    $('#createButton').bind('click', function (e) {
        e.preventDefault();
        
        var eventValidator = eventValidate();

        if (!checkSetOrganizator()) return false;

        var nickname = $('#Nickname').val();
        if ($('#ShowOrganizator')[0].checked) {
            $('#Nickname').val('');
        }

        if (eventValidator.validate() && $('#event-create-form').valid()) {

            $.ajax({
                url: window.location.origin + '/Event/CreateEvent',
                method: 'POST',
                data: $('#event-create-form').serialize(),
                success: function (result) {
                    if (result.success) {
                        window.location.href = result.redirectTo;
                    } else {
                        eventValidator.showErrors(false, result.errorMessage);
                        $('#Nickname').val(nickname);
                    }
                }
            });
            return true;
        } else {
            $('#createButton').attr('disabled', 'disabled');
            $('#Nickname').val(nickname);
            return false;
        }
    });
    
    initDateTime('word', null, $('#createButton'));
    
    var weekday = getNearestWeekday();
    var startTime = new Date(weekday.getFullYear(), weekday.getMonth(), weekday.getDate(), 9);
    var finishTime = new Date(weekday.getFullYear(), weekday.getMonth(), weekday.getDate(), 10);

    setEventDateTime(startTime, finishTime);
    eventValidate().showErrors(true);
    setDefaultEventSettings();

    $('#Nickname')[0].disabled = $('#ShowOrganizator')[0].checked = true;
});