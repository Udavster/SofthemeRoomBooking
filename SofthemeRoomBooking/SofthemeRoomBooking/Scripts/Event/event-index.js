$(document).ready(function () {
    var title = $('#Title').val();
    var description = $('#Description').val();
    var nickname = $('#Nickname').val();
    var showOrganizator = $('#ShowOrganizator')[0].checked = nickname === "";
    var allowRegistration = $('#AllowRegistration').is(':checked');
    var isPrivate = $('#Private').is(':checked');
    var idRoom = parseInt($('#IdRoom').val(), 10);

    var currDay = ('0' + $('#Day').val()).slice(-2);
    var currMonth = ('0' + $('#Month').val()).slice(-2);
    var currYear = $('#Year').val().slice(2);
    var currFullYear = $('#Year').val();

    var startHour = ('0' + $('#StartHour').val()).slice(-2);
    var startMinutes = ('0' + $('#StartMinutes').val()).slice(-2);
    var finishHour = ('0' + $('#FinishHour').val()).slice(-2);
    var finishMinutes = ('0' + $('#FinishMinutes').val()).slice(-2);
    
    initDateTime('number', '.', $('#saveButton'));

    $('.datepicker-chosen-arrow').css('visibility', 'hidden');
    $('.timepicker-arrow').css('visibility', 'hidden');

    $('#event-date #day').text(currDay);
    $('#event-date #month').text(currMonth);
    $('#event-date #year').text(currYear);

    $('#event-timestart #hours').text(startHour);
    $('#event-timestart #minutes').text(startMinutes);
    $('#event-timefinish #hours').text(finishHour);
    $('#event-timefinish #minutes').text(finishMinutes);

    $('#Nickname')[0].disabled = $('#ShowOrganizator')[0].checked;
    $('#AllowRegistration')[0].disabled = $('#Private')[0].checked;


    $('#editButton').bind('click', function () {
        $('#editButton').addClass('hidden');
        $('#cancelEventButton').addClass('hidden');
        $('.eventindex-info').addClass('hidden');


        $('#cancelEditButton').removeClass('hidden');
        $('#saveButton').removeClass('hidden');
        $('.eventindex-edit').removeClass('hidden');

        $('.datepicker-chosen-arrow').css('visibility', 'visible');
        $('.timepicker-arrow').css('visibility', 'visible');
    });

    $('#cancelEditButton').bind('click', function () {
        $('#editButton').removeClass('hidden');
        $('#cancelEventButton').removeClass('hidden');
        $('.eventindex-info').removeClass('hidden');

        $('#cancelEditButton').addClass('hidden');
        $('#saveButton').addClass('hidden');
        $('.eventindex-edit').addClass('hidden');

        $('.datepicker-chosen-arrow').css('visibility', 'hidden');
        $('.timepicker-arrow').css('visibility', 'hidden');
        debugger;
        $('.eventindex-edit #Title').val(title);
        $('.eventindex-edit #Description').val(description);
        $('.eventindex-edit #Nickname').val(nickname);
        $('.eventindex-edit #ShowOrganizator')[0].checked = showOrganizator;
        $('.eventindex-edit #AllowRegistration')[0].checked = allowRegistration;
        $('.eventindex-edit #Private')[0].checked = isPrivate;

        $('#AllowRegistration')[0].disabled = isPrivate;
        $('#Nickname')[0].disabled = showOrganizator;

        $('#Day').val(currDay);
        $('#Month').val(currMonth);
        $('#Year').val(currFullYear);

        $('#StartHour').val(startHour);
        $('#StartMinutes').val(startMinutes);
        $('#FinishHour').val(finishHour);
        $('#FinishMinutes').val(finishMinutes);

        $('#event-date #day').text(currDay);
        $('#event-date #month').text(currMonth);
        $('#event-date #year').text(currYear);

        $('#event-timestart #hours').text(startHour);
        $('#event-timestart #minutes').text(startMinutes);
        $('#event-timefinish #hours').text(finishHour);
        $('#event-timefinish #minutes').text(finishMinutes);

        $('#eventindex-edit-form').valid();
        eventValidate().validate();
    });

    $('#cancelEventButton').bind('click', function (e) {
        e.preventDefault();
        
        var eventId = parseInt($('#Id').val(), 10);

        $.ajax({
            url: window.location.origin + '/Event/CancelEvent',
            method: 'GET',
            data: { id: eventId },
            dataType: 'html',
            success: function (result) {
                $('#popup-confirmation').html(result);
                $('#popup-confirmation').show();
            }
        });
    });
    
    $('#saveButton').bind('click', function (e) {
        e.preventDefault();
        
        var eventValidator = eventValidate();

        if (!checkSetOrganizator()) return false;

        var nickname = $('#Nickname').val();
        if ($('#ShowOrganizator')[0].checked) {
            $('#Nickname').val('');
        }

        if (eventValidator.validate() && $('#eventindex-edit-form').valid()) {

            $.ajax({
                url: window.location.origin + '/Event/EditEvent',
                method: 'POST',
                data: $('#eventindex-edit-form').serialize(),
                success: function (result) {
                    if (result.success) {
                        window.location.href = result.redirectTo;
                    } else if (result.errorMessage) {
                        eventValidator.showErrors(false, result.errorMessage);
                        $('#Nickname').val(nickname);
                    }
                }
            });
            return true;
        } else {
            $('#saveButton').attr('disabled', 'disabled');
            $('#Nickname').val(nickname);
            return false;
        }
    });

    $('#eventindex-edit-form .event-input').on('keyup blur', function (e) {
        var validator = $('#eventindex-edit-form').validate();

        if (validator.element(e.target)) {
            if (e.target === $('#Nickname')[0]) {
                
                var $errors = $('.error-text span').text();
                if ($errors.includes('Не указан организатор события.')) {
                    
                    $errors = $errors.replace(/Не указан организатор события./g, '');
                    if ($errors === '') {
                        eventValidate().showErrors(true);
                        $('#saveButton').attr('disabled', false);
                    } else {
                        eventValidate().showErrors(false, $errors);
                    }
                }

                checkSetOrganizator();
            } else {
                $('#saveButton').attr('disabled', false);
            }           
        } else {
            $('#saveButton').attr('disabled', 'disabled');
        }
    });
    
    highlightRoom(idRoom);

    clickChange();

    clickCancel(idRoom);

    chooseRoom();
});

function highlightRoom(elem) {
    var room = $('.room__general.room-' + elem + '');
    $('.room__image').find($('#' + room.data('pathid'))).show();
    $('.room__image').css('pointer-events', 'none');
    room.css('background', '#f95752');
    room.children('.text').css('background-color', 'red');
}

function clickChange() {
    $(document).on('click', '#editButton', function () {
        $('.room__number').text('Выберите аудитория на этаже 10');
        $('.room__number').css('left', '244px');
        $('.room__image').css('pointer-events', 'auto');
        $('.room__general-blocked').css('pointer-events', 'none');
    });
}
function clickCancel(elem) {
    $(document).on('click', '#cancelEditButton', function () {
        $('.room__number').text('Этаж 10');
        $('.room__number').css('left', '458px');
        $('.room__image').css('pointer-events', 'none');
        $('.room__path').hide();
        $('.room__general').css('background', '#9fa6b6');
        $('.room__general').children('.text').css('background-color', '#858c9a');
        $('.room__general-blocked').css('background', '#d7d9de');
        $('.room__general-blocked').children('.text').css('background-color', '#acb0b6');
        var room = $('.room__general.room-' + elem + '');
        $('.room__image').find($('#' + room.data('pathid'))).show();
        $('.room__image').css('pointer-events', 'none');
        room.css('background', '#f95752');
        room.children('.text').css('background-color', 'red');
    });
}

function chooseRoom() {
    $(document).on('click', '.room__general', function () {
        _this = $(this);
        $('.room__path').hide();
        $('.room__image').find($('#' + _this.data('pathid'))).show();
        $('.room__general').css('background', '#9fa6b6');
        $('.room__general').children('.text').css('background-color', '#858c9a');
        $('.room__general-blocked').css('background', '#d7d9de');
        $('.room__general-blocked').children('.text').css('background-color', '#acb0b6');
        _this.css('background', '#f95752');
        _this.children('.text').css('background-color', 'red');

        var idRoom = $(this).data('roomid');
        $('#IdRoom').val(idRoom);
    });
}