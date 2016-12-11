var cal = new DatePicker();
var calendarMemo = {
    "roomArr": ['Einstein classroom', 'Tesla classroom', 'Newton classroom'],
    "events": {

    }
}
calendarMemo2 = {
    "roomArr": ['Einstein classroom2', 'Tesla classroom2', 'Newton classroom2'],
    "events": {
        //'Einstein classroom2': [{ 'Title': 'event in the first room', 'Start': { 'h': 4, 'm': 0 }, 'Finish': { 'h': 5, 'm': 50 } }],
        //'Tesla classroom2': [{ 'Title': 'event in the room', 'Start': { 'h': 5, 'm': 0 }, 'Finish': { 'h': 6, 'm': 0 } }],
        //'Newton classroom2': [{ 'Title': 'event in the room', 'Start': { 'h': 4, 'm': 30 }, 'Finish': { 'h': 5, 'm': 0 } }],
        //'room42': [{ 'Title': 'event in the room', 'Start': { 'h': 2, 'm': 0 }, 'Finish': { 'h': 3, 'm': 0 } }],
        //'room5':[{'Title':'event in the room', 'Start':{'h':6,'m':0}, 'Finish': {'h':9,'m':0}}]             
    }
}

function Loading(show) {
    if (show) {
        $('.calendar-events__loading-glass').show();
    } else {
        $('.calendar-events__loading-glass').hide();
    }
}

function getDate(Date) {
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
            console.log(rez);
            calendarMemo = rez;
            a.constructFromMemo(calendarMemo);
        },

        error: function () {
            //console.warn("error");
            //retries
        }

    });
}

$(document).ready(function () {

    a = new Calendar("calendar-events", calendarMemo);
    a.addEventOnClickHandler(function () { alert('Clicked'); });

    // a.setToday("07, Пт");
    cal.init(null, function (date, dayOfWeek, month, year) {
        getDate(year + "" + tformat(month + 1) + tformat(date));
        var weekdays = ["Пн", "Вт", "Ср", "Чт", "Пт"];
        a.setToday(tformat(date) + ", " + weekdays[dayOfWeek]);
    });
    $("#" + a.name + "-fw-control").click(function () {

        $("#datepicker").hide();
        $(".calendars__month").css('display', 'none');
        $(this).attr('active', 'false');
        $("#" + a.name + "-pw-control").attr('active', 'true');
        a.changeWidth("calc(100% - 100px)");
    });
    $("#" + a.name + "-pw-control").click(function () {
        $("#datepicker").show();
        $(".calendars__month").css('display', 'block');
        $(this).attr('active', 'false');
        $("#" + a.name + "-fw-control").attr('active', 'true');
        a.changeWidth(1024);
    });

    var today = new Date();

    if (today.getDay() == 6) {
        today.setDate(today.getDate() + 2);
    } else if (today.getDay() == 0) {
        today.setDate(today.getDate() + 1);
    }

    a.setToday(tformat(today.getDate()) + ", " + cal.getDayNames(today.getDay()));
    getDate(today.getFullYear() + "" + tformat(today.getMonth() + 1) + tformat(today.getDate()));

});