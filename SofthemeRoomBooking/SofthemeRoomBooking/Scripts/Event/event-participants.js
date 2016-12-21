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
    
    var showErrors = eventValidate().showErrors;

    $.ajax({
        url: window.location.origin + '/Event/AddParticipant',
        method: 'POST',
        data: $('#event-addparticipant-form').serialize(),
        success: function (result) {
            if (result.success) {
                    window.location.href = result.redirectTo;
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


function Loading(loading, updateSelector) {
    var selector = $(updateSelector);
    if (loading) {
        $(selector).html('');
        $('#loading').show();
    } else {
        $('#loading').hide();
    }
};