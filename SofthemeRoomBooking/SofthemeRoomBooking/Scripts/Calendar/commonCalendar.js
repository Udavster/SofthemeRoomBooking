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
//    "eventLine":{
//        [{'h': 1,'m': 20}:{1:true,3:false,5:true],{'h':2,'m':30}:{2:true, 3:true} ]
//    }
//}

function Loading(show) {
    if (show) {
        $('.calendar-events__loading-glass').show();
    } else {
        $('.calendar-events__loading-glass').hide();
    }
}

function getDate(a, dateInfo) {
    Loading(true);
    $.ajax({
        url: '/Calendar',
        data: {
            date: (dateInfo.getFullYear() + "" + tformat(dateInfo.getMonth() + 1) + "" + tformat(dateInfo.getDate()))
        }, 
        method: 'get',
        dataType: "json",

        success: function (data) {
            Loading(false);
            console.log(data);
            if (data.error) {
                console.log("Date format is envalid or internal exception occured");
                return;
            }

            var rez = {};
            var data2 = {};

            for (var i = 0; i < data.Events.length; i++) {
                data2[i] = { 'roomName': data.Rooms[i].Id, 'events': data.Events[i] };
            }
            var roomArr = [];
            rez["Rooms"] = {};
            for (var i = 0; i < data.Rooms.length; i++) {
                roomArr.push(data.Rooms[i].Name);
                roomArr[i].Id = i;
                rez["Rooms"][data.Rooms[i].Name] = data.Rooms[i].Id;

            }

            rez["roomArr"] = roomArr;
            rez["events"] = data2;
            rez["Auth"] = data["Authenticated"];

            calendarMemo = rez;
            console.log(calendarMemo);
            eventsCalendar.createTimeLine(calendarMemo);

            var today = new Date();
            today.setHours(0, 0, 0, 0);
            if (dateInfo >= today) {
                calendarMemo = eventsCalendar.sortEventsInMemo(calendarMemo);
            }
            eventsCalendar.constructFromMemo(calendarMemo);
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
    if (!eventsCalendar.Auth) return;
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

            var daySelected = datePicker.getCurrentDay();

            var startHour = parseInt($event.data('sh'), 10);
            var startMinutes = parseInt($event.data('sm'), 10);
            var finishHour = parseInt($event.data('eh'), 10);
            var finishMinutes = parseInt($event.data('em'), 10);

            var startTime = new Date(daySelected.day, daySelected.month, daySelected.year, startHour, startMinutes);
            var finishTime = new Date(daySelected.day, daySelected.month, daySelected.year, finishHour, finishMinutes);

            setEventDateTime(startTime, finishTime);

            var roomNum = $event.parent().data('roomnum');

            setEventRoom(roomNum);
            
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

    console.log($event.data('id'));

    $.ajax({
        url: window.location.origin + "/Event/EventDetails",
        data: 'id=' + $event.data('id'),
        type: 'GET',
        success: function (result) {
            console.log(result);
            $('#calendar__popup-edit-event').html(result);
            $('#calendar__popup-edit-event').show();
        }
    });
};

$(document).ready(function () {
    var commonCalendar = new CommonCalendar();
});

function CommonCalendar(eventHandler, emptyHandler) {
    this.buildCalendars = function() {
        datePicker = new DatePicker();
        eventsCalendar = new Calendar("calendar-events", calendarMemo);
        eventsCalendar.addEventOnClickHandler(function() { alert('Clicked'); });
        eventsCalendar.changeWidth(930);
        
        datePicker.init(null);
        datePicker.addDayClickHandler(this.getClickedDate);
        
        $("#" + eventsCalendar.name + "-fw-control")
            .click(function() {

                $("#datepicker").hide();
                $(".calendars__month").css('display', 'none');
                $(this).attr('active', 'false');
                $("#" + eventsCalendar.name + "-pw-control").attr('active', 'true');
                //eventsCalendar.changeWidth("calc(100% - 100px)");
                eventsCalendar.changeWidth("100%");
            });
        $("#" + eventsCalendar.name + "-pw-control")
            .click(function() {
                $("#datepicker").show();
                $(".calendars__month").css('display', 'block');
                $(this).attr('active', 'false');
                $("#" + eventsCalendar.name + "-fw-control").attr('active', 'true');
                eventsCalendar.changeWidth(930);
            });
    }

    this.getClickedDate = function (date, dayOfWeek, month, year) {
        getDate(eventsCalendar, new Date(year, month, date));
        var weekdays = ["Пн", "Вт", "Ср", "Чт", "Пт"];
        eventsCalendar.setToday(tformat(date) + ", " + weekdays[dayOfWeek]);
    };

    this.getSchedule = function() {
        var today = new Date();

        if (today.getDay() == 6) {
            today.setDate(today.getDate() + 2);
        } else if (today.getDay() == 0) {
            today.setDate(today.getDate() + 1);
        }

        eventsCalendar.setToday(tformat(today.getDate()) + ", " + datePicker.getDayNames(today.getDay()));
        getDate(eventsCalendar, today);
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
    eventsCalendar.addEventOnClickHandler(clickHandler);
    eventsCalendar.addNextPrevDayHandler(function(next) { datePicker.switchDay(next); });
}


