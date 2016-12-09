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

function getDate(Date) {
    $.ajax({
        url: 'http://localhost:36252/Calendar',             // ?date=20140511
      //  data: { date: Date },
        method: 'get',
        dataType: "json",                     // тип загружаемых данных
        success: function (data, textStatus) { // вешаем свой обработчик на функцию success
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
        complete: function(xhr, status) {
            console.log(xhr);
            console.log(status);
        }
    });
}

$(document).ready(function () {

    a = new Calendar("calendar-events", calendarMemo);
    a.addEventOnClickHandler(function () { alert('Clicked'); });
    a.setToday("07, Пт");
    cal.init(null, function (date, dayOfWeek, month, year) {
        getDate(year + "" + tformat(month + 1) + tformat(date));
        var weekdays = ["Пн", "Вт", "Ср", "Чт", "Пт"];
        a.setToday(tformat(date) + ", " + weekdays[dayOfWeek]);
        
        //if (dayOfWeek % 2 === 1) {
        //    a.constructFromMemo(calendarMemo2);
        //} else {
        //    a.constructFromMemo(calendarMemo);
        //}
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
});