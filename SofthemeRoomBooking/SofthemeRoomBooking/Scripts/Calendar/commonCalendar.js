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
    Loading(true);
    $.ajax({
        url: window.location.origin + "/Event/CreateEvent",
        type: 'GET',
        success: function (result) {
            $('#calendar__popup-create-event').html(result);
            $('#calendar__popup-create-event').show();

            var $event;
            if ($(event.target).hasClass('event-empty')) {
                $event = $(event.target);
            } else if ($(event.target).parent().hasClass('event-empty')) {
                $event = $(event.target).parent();
            }

            var startHour = $event.data('sh');
            var startMinutes = $event.data('sm');
            var endHour = $event.data('eh');
            var endMinutes = $event.data('em');

            $('#event-timestart #hours').html(tformat(startHour));
            $('#event-timestart #minutes').html(tformat(startMinutes));
            $("#StartHour").val(startHour);
            $("#StartMinutes").val(startMinutes);

            $('#event-timefinish #hours').html(tformat(endHour));
            $('#event-timefinish #minutes').html(tformat(endMinutes));
            $("#EndHour").val(endHour);
            $("#EndMinutes").val(endMinutes);

            
            var daySelected = cal.getCurrentDay();
            $('.datepicker-chosen-date #day').html(daySelected.day);
            $('.datepicker-chosen-date #month').html(monthNames[daySelected.month]);
            $('.datepicker-chosen-date #year').html(daySelected.year);

            $("#Day").val(daySelected.day);
            $("#Month").val(daySelected.month + 1);
            $("#Year").val(daySelected.year);
            


            var roomNum = $event.parent().data('roomnum');
            $('#IdRoom option')[roomNum].selected = true;
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

        cal.init(null);
        cal.addDayClickHandler(this.getClickedDate);
      
        $("#" + a.name + "-fw-control")
            .click(function() {

                $("#datepicker").hide();
                $(".calendars__month").css('display', 'none');
                $(this).attr('active', 'false');
                $("#" + a.name + "-pw-control").attr('active', 'true');
                a.changeWidth("calc(100% - 100px)");
            });
        $("#" + a.name + "-pw-control")
            .click(function() {
                $("#datepicker").show();
                $(".calendars__month").css('display', 'block');
                $(this).attr('active', 'false');
                $("#" + a.name + "-fw-control").attr('active', 'true');
                a.changeWidth(990);
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


