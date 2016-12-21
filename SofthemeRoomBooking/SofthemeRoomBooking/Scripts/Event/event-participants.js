function addParticipant(e) {
    e.preventDefault();


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