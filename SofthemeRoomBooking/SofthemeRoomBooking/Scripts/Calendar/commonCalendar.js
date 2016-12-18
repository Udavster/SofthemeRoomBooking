var calendarMemo = {
    "roomArr": ['Einstein classroom', 'Tesla classroom', 'Newton classroom'],
    "events": {

    }
}
//calendarMemo2 = {
//    "roomArr": ['Einstein classroom2', 'Tesla classroom2', 'Newton classroom2'],
//    "events": {
        //'Einstein classroom2': [{ 'Title': 'event in the first room', 'Start': { 'h': 4, 'm': 0 }, 'Finish': { 'h': 5, 'm': 50 } }],
        //'Tesla classroom2': [{ 'Title': 'event in the room', 'Start': { 'h': 5, 'm': 0 }, 'Finish': { 'h': 6, 'm': 0 } }],
        //'Newton classroom2': [{ 'Title': 'event in the room', 'Start': { 'h': 4, 'm': 30 }, 'Finish': { 'h': 5, 'm': 0 } }],
        //'room42': [{ 'Title': 'event in the room', 'Start': { 'h': 2, 'm': 0 }, 'Finish': { 'h': 3, 'm': 0 } }],
        //'room5':[{'Title':'event in the room', 'Start':{'h':6,'m':0}, 'Finish': {'h':9,'m':0}}]             
//    }
//}

function Loading(show) {
    if (show) {
        $('.calendar-events__loading-glass').show();
    } else {
        $('.calendar-events__loading-glass').hide();
    }
}

function getDate(a, Date) {
    Loading(true);
    $.ajax({
        url: '/Calendar',
        data: { date: Date },
        method: 'get',
        dataType: "json",

        success: function (data, textStatus) {
            Loading(false);

            if (data.error) {
                console.log("Date format is envalid or internal exception occured");
                return;
            }

            var rez = {};
            var data2 = {};

            for (var i = 0; i < data.Events.length; i++) {
                data2[data.Rooms[i].Name] = data.Events[i];
            }
            var roomArr = [];
            for (var i = 0; i < data.Rooms.length; i++) {
                roomArr.push(data.Rooms[i].Name);
            }

            rez["roomArr"] = roomArr;
            rez["events"] = data2;
            rez["Auth"] = data["Authenticated"];

            calendarMemo = rez;
            console.log(calendarMemo, rez['events']);
            calendarMemo = a.sortEventsInMemo(calendarMemo);
            a.constructFromMemo(calendarMemo);
        },

        error: function () {
            //console.warn("error");
            //retries
        }

    });
}

var monthNames = [
    'Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'
];

var createEvent = function (event) {
    if (!a.Auth) return;
    Loading(true);
    $.ajax({
        url: window.location.origin + "/Event/EditEventPartial",
        type: 'GET',
        success: function (result) {
            $('#popup-edit-event').html(result);
            $('#popup-edit-event').show();

            var $event;
            if ($(event.target).hasClass('event-empty')) {
                $event = $(event.target);
            } else if ($(event.target).parent().hasClass('event-empty')) {
                $event = $(event.target).parent();
            }

            var daySelected = cal.getCurrentDay();

            var startHour = parseInt($event.data('sh'), 10);
            var startMinutes = parseInt($event.data('sm'), 10);
            var finishHour = parseInt($event.data('eh'), 10);
            var finishMinutes = parseInt($event.data('em'), 10);

            var startTime = new Date(daySelected.day, daySelected.month, daySelected.year, startHour, startMinutes);
            var finishTime = new Date(daySelected.day, daySelected.month, daySelected.year, finishHour, finishMinutes);
            debugger;
            setDateTime(startTime, finishTime);

            var roomNum = $event.parent().data('roomnum');

            setRoom(roomNum);
            
            setDefaultEventSettings();

            Loading(false);
        },
        error: function () {
            //console.warn("error");
            //retries
        }
    });
};
var editEvent = function (event) {
    $event = $(event.target);
    

    if (!$event.hasClass('event')) {
        $event = $event.parent();
        if (!$event.hasClass('event')) return;
    }

    $.ajax({
        url: window.location.origin + "/Event/EditEvent",
        data: 'eventId=' + $event.data('id'),
        type: 'GET',
        success: function (result) {
            console.log(result);
            $('#popup-edit-event').html(result);
            $('#popup-edit-event').show();
        }
    });
};

$(document).ready(function () {
    var commonCalendar = new CommonCalendar();
});

function CommonCalendar(eventHandler, emptyHandler) {
    this.buildCalendars = function() {
        cal = new DatePicker();
        a = new Calendar("calendar-events", calendarMemo);
        a.addEventOnClickHandler(function() { alert('Clicked'); });
        a.changeWidth(930);
        
        cal.init(null);
        cal.addDayClickHandler(this.getClickedDate);
        
        $("#" + a.name + "-fw-control")
            .click(function() {

                $("#datepicker").hide();
                $(".calendars__month").css('display', 'none');
                $(this).attr('active', 'false');
                $("#" + a.name + "-pw-control").attr('active', 'true');
                //a.changeWidth("calc(100% - 100px)");
                a.changeWidth("100%");
            });
        $("#" + a.name + "-pw-control")
            .click(function() {
                $("#datepicker").show();
                $(".calendars__month").css('display', 'block');
                $(this).attr('active', 'false');
                $("#" + a.name + "-fw-control").attr('active', 'true');
                a.changeWidth(930);
            });
    }

    this.getClickedDate = function (date, dayOfWeek, month, year) {
        getDate(a, year + "" + tformat(month + 1) + tformat(date));
        var weekdays = ["Пн", "Вт", "Ср", "Чт", "Пт"];
        a.setToday(tformat(date) + ", " + weekdays[dayOfWeek]);
    };

    this.getSchedule = function() {
        var today = new Date();

        if (today.getDay() == 6) {
            today.setDate(today.getDate() + 2);
        } else if (today.getDay() == 0) {
            today.setDate(today.getDate() + 1);
        }

        a.setToday(tformat(today.getDate()) + ", " + cal.getDayNames(today.getDay()));
        getDate(a, today.getFullYear() + "" + tformat(today.getMonth() + 1) + tformat(today.getDate()));
    }

    var clickHandler = function(event) {
        var $target = $(event.target);

        if (!$target.hasClass("event-empty") && (!$target.parent().hasClass("event-empty"))) {
            if (this.eventHandler) {
                 this.eventHandler(event);
            }
        } else {
            if (this.emptyHandler) {
                 this.emptyHandler(event);
            }
        }

    }.bind(this);

    this.eventHandler = editEvent;
    this.emptyHandler = createEvent;
    this.buildCalendars();
    this.getSchedule();
    a.addEventOnClickHandler(clickHandler);
    a.addNextPrevDayHandler(function(next) { cal.switchDay(next); });
}


