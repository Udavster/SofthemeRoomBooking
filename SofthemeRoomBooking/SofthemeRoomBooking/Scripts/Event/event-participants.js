$(document).ready(function () {
    $('#event-addparticipant-form .event-input').on('keyup blur', function (e) {

        if (!$('#event-addparticipant-form #event-errors').hasClass('hidden-error')) {
            $('#event-addparticipant-form #event-errors').addClass('hidden-error');
        }

        var validator = $('#event-addparticipant-form').validate();
        
        if (validator.element(e.target)) {
            $('#addParticipant').attr('disabled', false);
        } else {
            $('#addParticipant').attr('disabled', 'disabled');
        }
    });
});

$('#addParticipant').bind('click', function (e) {
    e.preventDefault();
    debugger;
    
    var showErrors = eventValidate().showErrors;

    $.ajax({
        url: window.location.origin + '/Event/AddParticipant',
        method: 'POST',
        data: $('#event-addparticipant-form').serialize(),
        success: function (result) {
            if (result.success) {
                updateParticipants();
            } else {
                showErrors(false, result.errorMessage);
            }
        }
    });
});

function clearAddParticipantForm() {
    $('#Email').val('');

    $.ajax({
        url: window.location.origin + '/Event/AddParticipant',
        method: 'GET',
        data: $('#event-addparticipant-form').serialize(),
        success: function (result) {
            $('.eventindex-participants').html(result);
        }
    });
};

function updateParticipants() {
    debugger;
    var eventId = parseInt($('#IdEvent').val());

    $.ajax({
        url: window.location.origin + '/Event/EventParticipants',
        method: 'GET',
        data: { id: eventId },
        dataType: 'html',
        beforeSend: function () {
            Loading(true, '.eventindex-participants');
        },
        complete: function () {
            Loading(false);
        },
        success: function (result) {
            $(".eventindex-participants").html(result);
        }
    });
};

function deleteParticipant(e) {
    e.preventDefault();

    var participantId = parseInt($('.eventindex-participants #Id').val(), 10);

    $.ajax({
        url: window.location.origin + '/Event/DeleteParticipant',
        method: 'GET',
        data: new { participantId : participantId },
        dataType: 'html',
        success: function (result) {
            $('#popup-confirmation').html(result);
            $('#popup-confirmation').show();
        }
    });
}

function Loading(loading, updateSelector) {
    if (updateSelector === undefined) return;
    var selector = $(updateSelector);

    if (loading) {
        $(selector).html('');
        $('#loading').show();
    } else {
        $('#loading').hide();
    }
};