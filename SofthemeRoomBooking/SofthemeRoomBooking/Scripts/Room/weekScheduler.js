$(document).ready(function () {
    var currentDate = new Date();
    var prevDate = new Date();
    //if sunday
    if (currentDate.getDay() == 0) {
        prevDate.setDate(currentDate.getDate() - 2);
    }
    else {
        prevDate.setDate(currentDate.getDate() - 1);
    }
    var data = {date : currentDate.toISOString().slice(0,10)}
    $.getJSON("/Home/Events",data, function (jsonObject) {

        for (var i = 0; i < 8; i++) {
            generateCalendarCol(prevDate, jsonObject[i].events, i);
            if (new Date().setHours(0, 0, 0, 0) === prevDate.setHours(0, 0, 0, 0)) {
                drawCursor(prevDate);
            }
            if (prevDate.getDay() == 6) {
                prevDate.setDate(prevDate.getDate() + 2);
            }
            else {
                prevDate.setDate(prevDate.getDate() + 1);
            }

        }
    });
    $('#calendar').show();
});

function generateCalendarCol(date, events, iteration) {
    var calendar = $('#calendar');
    var ttt = iteration;
    var dateArray = ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"]
    //Generate HTML strucrure

    //set column border active if date = current date

    if (new Date().setHours(0, 0, 0, 0) === date.setHours(0, 0, 0, 0) && date.getDay() != 6) {
        var structure = '<div class="calendar-col-active c-' + ttt + '">';
    }
    else {
        if (date.getDay() == 6 || date.getDay() == 0) {
            var structure = '<div class="calendar-col-weekend c-' + ttt + '">';
        }
        else {
            var structure = '<div class="calendar-col c-' + ttt + '">';
        }
    }

    //if saturday
    if (date.getDay() == 6 || date.getDay() == 0) {
        structure += '<div class="item-head-weekend">' + dateArray[6] + ' ' + '<div style="float:right">' + dateArray[0] + '</div>' + '</div>';
        structure += '<div class="calendar-items-weekend"></div>'
    }
    else {
        structure += '<div class="item-head">' + dateArray[date.getDay()] + '<div class="item-day">' + +date.getDate() + '.' + (date.getMonth() + 1) + '</div>' + '</div>';
        var startTime = (iteration % 2 == 0) ? 9 : 10;

        for (var i = 0; i < 11; i++) {

            structure += '<div class="calendar-item item-' + i + '">';

            if (iteration++ % 2 == 0) {
                structure += '<div class="time">' + startTime + ':00';
                startTime += 2;
                structure += '</div>';
            }
            structure += '</div>';
        }
    }

    $(structure).appendTo(calendar);
    //Add events
    for (var i = 0; i < events.length; i++) {
        var eventStart = new Date(events[i].startDate);
        var eventStartHours = eventStart.getHours();
        var eventStartMinutes = eventStart.getMinutes();

        var eventEnd = new Date(events[i].endDate);
        var eventEndHours = eventEnd.getHours();
        var eventEndMinutes = eventEnd.getMinutes();
        var currentItem = $('.c-' + ttt).find('.item-' + (eventStartHours - 9));

        //calculate height and top for displaying event
        var differense = (eventEndHours - eventStartHours) * 60 + (eventEndMinutes - eventStartMinutes);
        var top = Math.round((eventStartMinutes) / 60 * 70);
        var height = Math.round(differense / 60 * 70);
        if (events[i].isPrivate) {
            var eventInfo = '<div class="event-private" style="top:' + top + 'px;height:' + height + 'px;">';
        }
        else {
            var eventInfo = '<div class="event-info" style="top:' + top + 'px;height:' + height + 'px;">';
        }
        eventInfo += events[i].title;

        eventInfo += '</div>';
        $(eventInfo).appendTo($(currentItem));

    }

}

function drawCursor(date) {
    var curDate = new Date();
    var curHour = curDate.getHours();
    var curMinute = curDate.getMinutes();
    var currentItem = $('.c-' + 1).find('.item-' + (curHour - 9));
    var top = Math.round((curMinute) / 60 * 70);
    var ctimeDiv = '<div class="cursor" style="top:' + top + 'px"><div class="cursor__arrows cursor__left-arrow"><i class="fa fa-caret-right" style="color:gray; font-size:25px" aria-hidden="true"></i></div><div class="cursor__arrows cursor__right-arrow"><i class="fa fa-caret-left" style="color:gray ;font-size:25px" aria-hidden="true"></i></div></div>';
    $(ctimeDiv).appendTo($(currentItem));
}