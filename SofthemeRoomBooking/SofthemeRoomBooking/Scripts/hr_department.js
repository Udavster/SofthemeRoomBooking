$(document).ready(function() {
    $('#exitButton').click(function () {

    });

    $('#deleteButton').click(function () {

    });

    $('#editButton').click(function () {
        $('#exitButton').addClass('hidden');
        $('#deleteButton').removeClass('hidden');

        $('.container-info').addClass('hidden');
        $('.container-edit-profile').removeClass('hidden');
    });
});