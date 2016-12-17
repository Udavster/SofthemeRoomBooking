function NextWeekDate() {
    var nextWeek = new Date();

    return {
        get: function () {
            return nextWeek.getDate();
        },
        set: function (newDate) {

            nextWeek.setDate(newDate);

        },
        getFull: function () {
            return nextWeek;
        }
    }
}

var roomClickHandler = function (event, prevDate) {
    var $this = $(event.target);
    if (!$this.hasClass('room__general')) {
        $this = $this.parent();
    }
    if (!$this.hasClass('room__general')) return;
    getRoomScheduler($this, prevDate);
}

var getRoomSchedulerById = function(RoomId, prevDate) {
    var $this = $('.room-' + RoomId);
    getRoomScheduler($this, prevDate);
}

var getRoomScheduler = function($this, prevDate) {
    $('#calendar').html('');
    console.log($this);
    $('#calendar').data('current-room', $this.data('roomid'));

    var date1 = new Date();
    date1.setDate(prevDate.getDate());
    prevDate.setDate(date1.getDate());

    var data = {
        date: convertDate(date1),
        id: $this.data('roomid')
    }

    initCalendar(date1, data);
    nWeek = new NextWeekDate();
}

$(document).ready(function () {
    localStorage.removeItem('cachedEvents');
    var currentDate = new Date();
    var prevDate = new Date();
    //0 - sunday, 6 - saturday
    if (currentDate.getDay() === 0) {
        prevDate.setDate(currentDate.getDate() - 2);
    }
    else {
        prevDate.setDate(currentDate.getDate() - 1);
    }

    nWeek = new NextWeekDate();
   
    nWeek.set(prevDate.getDate());
    
    $(document).on('click', "#next-week", function () {
        var plusDay = (nWeek.getFull().getDay() === 5) ? 10 : 9;

        nWeek.set(nWeek.get() + +plusDay);

        var data = {
            date: convertDate(nWeek.getFull()),
            id: $('#calendar').data('current-room')
        }
        $('#calendar').html('');
        initCalendar(nWeek.getFull(), data);
    });

    $(document).on('click', "#prev-week", function () {

        var plusDay = (nWeek.getFull().getDay() === 1) ? 10 : 9;

        nWeek.set(nWeek.get() - +plusDay);

        var data = {
            date: convertDate(nWeek.getFull()),
            id: $('#calendar').data('current-room')
        }
        $('#calendar').html('');
        initCalendar(nWeek.getFull(), data);
    });
    

    $('#calendar').show();

    $('.room__general').click(function (event) { roomClickHandler(event, prevDate) });

});

function initCalendar(oldPrevDate, data) {

    generateMenu();
    if (oldPrevDate.getDay() === 5 || oldPrevDate.getDay() === 0) {
        $('.scheduler__header').css('width', '860px');
    }
    var currentEvents = JSON.parse(localStorage.getItem('cachedEvents')) || [];
    var currentEvent = {
        room: data.id,
        events: [],
        date: oldPrevDate
    };

    var oldEvents = [];
    if (currentEvents.length > 0) {
        currentEvents.forEach(function (elem, i) {
            
            var eDate = convertDate(new Date(elem.date));
            var oDate = convertDate(new Date(oldPrevDate));
            
            if (eDate === oDate && elem.room === data.id) {
                oldEvents = elem.events;
                return false;
            }
        });
    }

    if (oldEvents.length > 0) {
        console.log('CACHED INFO');
        var prevDate = new Date(oldPrevDate);
        for (var i = 0; i < 8; i++) {

            generateCalendarCol(prevDate, oldEvents[i], i, data.id);
           
            if (new Date().setHours(0, 0, 0, 0) === prevDate.setHours(0, 0, 0, 0)) {
                drawCursor(i);
            }
            if (prevDate.getDay() === 6) {
                prevDate.setDate(prevDate.getDate() + 2);
            }
            else {
                prevDate.setDate(prevDate.getDate() + 1);
            }
            if (i === 7) {
                getMonthYear(prevDate);

            }
        }
    } else {
        $.getJSON("/Room/Events", data, function (jsonObject) {
            var prevDate = new Date(oldPrevDate);
            currentEvent.events = jsonObject;
            currentEvents.push(currentEvent);
            for (var i = 0; i < 8; i++) {

                generateCalendarCol(prevDate, jsonObject[i], i, data.id);
            
                if (new Date().setHours(0, 0, 0, 0) === prevDate.setHours(0, 0, 0, 0)) {
                    drawCursor(i);
                }
                if (prevDate.getDay() === 6) {
                    prevDate.setDate(prevDate.getDate() + 2);
                }
                else {
                    prevDate.setDate(prevDate.getDate() + 1);
                }
                if (i === 4) {
                    getMonthYear(prevDate);

                }
            }
            localStorage.setItem('cachedEvents', JSON.stringify(currentEvents));
        });
    }

}
function generateMenu() {
    var calendar = $('#calendar');
    var $controls = $('<div class="scheduler__header"></div>');
    $controls.append('<div id="prev-week" class="calendar-events__prev-day-control control"><i class="fa fa-caret-left" aria-hidden="true"></i> </div>');
    $controls.append('<div id="next-week" class="calendar-events__next-day-control control"><i class="fa fa-caret-right" aria-hidden="true"></i></div>');
    $controls.append('<div id="weekscheduler__month-year" class="calendar-events__today"></div>');

    calendar.append($controls);
}

function generateCalendarCol(date, events, iteration, roomId) {
    var calendar = $('#calendar');
    var ttt = iteration;
    var dateArray = ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"];
    var isAuth = $('.week__scheduler').data('auth');
    var minInHour = 60;
    var heightCol = 48;
    //Generate HTML strucrure

    //set column border active if date = current date

    if (new Date().setHours(0, 0, 0, 0) === date.setHours(0, 0, 0, 0) && date.getDay() != 6) {
        var structure = '<div class="calendar-col-active c-' + ttt + '">';
    } else {
        if (date.getDay() === 6 || date.getDay() === 0) {
            var structure = '<div class="calendar-col-weekend c-' + ttt + '">';
        } else {
            var structure = '<div class="calendar-col c-' + ttt + '">';
        }
    }

    //if saturday
    if (date.getDay() == 6 || date.getDay() == 0) {
        structure += '<div class="item-head-weekend">' +
            dateArray[6] +
            ' ' +
            '<div style="float:right">' +
            dateArray[0] +
            '</div>' +
            '</div>';
        structure += '<div class="calendar-items-weekend"></div>'
    } else {
        structure += '<div class="item-head">' +
            dateArray[date.getDay()] +
            '<div class="item-day">' +
            +date.getDate() +
            '.' +
            (date.getMonth() + 1) +
            '</div>' +
            '</div>';
        var startTime = (iteration % 2 == 0) ? 9 : 10;

        for (var i = 0; i < 11; i++) {

            if (isAuth ==="True") {
                structure += '<div class="calendar-item item-' +
                    i +
                    '" data-start=' +(i + 9) +
                    ' data-finish=' + (i + 10) +
                    ' data-day=' + date.getDate() +
                    ' data-month=' + (date.getMonth() + 1) +
                    ' data-year=' + date.getFullYear() +
                    ' data-roomid=' + roomId +
                    ' >';
            } else {
                structure += '<div class="calendar-item not-empty item-' +
                    i +
                    '" data-start=' + (i + 9) +
                    ' data-finish=' + (i + 10) +
                    ' data-day=' + date.getDate() +
                    ' data-month=' + (date.getMonth() + 1) +
                    ' data-year=' + date.getFullYear() +
                    ' >';
            }

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
        var difference = (eventEndHours - eventStartHours) * minInHour + (eventEndMinutes - eventStartMinutes);
        var top = Math.round((eventStartMinutes) / minInHour * heightCol);
        var height = Math.round(difference / minInHour * heightCol);
        if (!events[i].isPublic) {
            var eventInfo = '<div class="event-private" data-id=' + events[i].eventId + ' style="top:' + top + 'px;height:' + height + 'px;">';
        } else {
            var eventInfo = '<div class="event-info" data-id='+events[i].eventId + ' style="top:' + top + 'px;height:' + height + 'px;">';
        }
            currentItem.addClass('not-empty');
        if (difference > 40) {
            eventInfo += '<div class="event-time">' +
                tformat(eventStartHours) +
                ':' +
                tformat(eventStartMinutes) +
                ' - ' +
                tformat(eventEndHours) +
                ':' +
                tformat(eventEndMinutes) +
                '</div>';
            eventInfo += '<div class="event-title">' + events[i].title + '</div>';
        } else {
            eventInfo += '<div class="event-time">' +
                tformat(eventStartHours) +
                ':' +
                tformat(eventStartMinutes) +
                ' - ' +
                tformat(eventEndHours) +
                ':' +
                tformat(eventEndMinutes) +
                '</div>';
        }
        $(eventInfo).appendTo($(currentItem));
    }
}

function drawCursor(i) {
    var minInHour = 60;
    var heightCol = 48;
    var curDate = new Date();
    var curHour = curDate.getHours();
    var curMinute = curDate.getMinutes();
    var currentItem = $('.c-' + i).find('.item-' + (curHour - 9));
    var top = Math.round((curMinute) / minInHour * heightCol);
    var ctimeDiv = '<div class="cursor" style="top:' +
        top +
        'px"><div class="cursor__arrows cursor__left-arrow"><i class="fa fa-caret-right" style="color:gray; font-size:25px" aria-hidden="true"></i></div><div class="cursor__arrows cursor__right-arrow"><i class="fa fa-caret-left" style="color:gray ;font-size:25px" aria-hidden="true"></i></div></div>';
    $(ctimeDiv).appendTo($(currentItem));
}

function getMonthYear(date) {
    var monthArr = [
'Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'
    ];
    var currentItem = $('#calendar').find('#weekscheduler__month-year');
    var currentMonth = monthArr[date.getMonth()];
    var currentYear = date.getFullYear();
    currentItem.text(currentMonth + ',' + currentYear);
}
function tformat(num){
    return ("0" + num).slice(-2);
}

function convertDate(date) {
    var yyyy = date.getFullYear().toString();
    var mm = (date.getMonth() + 1).toString();
    var dd = date.getDate().toString();

    var mmChars = mm.split('');
    var ddChars = dd.split('');

    return yyyy + '-' + (mmChars[1] ? mm : "0" + mmChars[0]) + '-' + (ddChars[1] ? dd : "0" + ddChars[0]);
}