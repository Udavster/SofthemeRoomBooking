function DatePicker() {
    if (this === window) {
        console.error('Use keyword new before function name DatePicker()');
        return false;
    }

    var wrap, label,
        currDay, currMonth, currYear,
        workDay, workMonth, workYear,
        todayMonth = new Date().getMonth(),
        todayYear = new Date().getFullYear(),
        dayClickHandler;


    this.getCurrentDay = function() {
        console.log($("#datepicker-body .day.selected").html());
        return { day: currDay, month: currMonth, year: currYear };
    }.bind(this);

    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" ];
    var dayNames = ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"];

    this.init = function (newWrap) {

        wrap = $(newWrap || "#datepicker");
        createDatapickerStruct();
        label = wrap.find("#current-month");

        wrap.find("#to-prev-month").bind("click", function () { this.switchMonth(false); }.bind(this));
        wrap.find("#to-next-month").bind("click", function () { this.switchMonth(true); }.bind(this));
        wrap.find("#back-to-today-left").bind("click", function () { this.switchMonth(null); }.bind(this));
        wrap.find("#back-to-today-right").bind("click", function () { this.switchMonth(null); }.bind(this));

        currMonth = todayMonth;
        currYear = todayYear;

        this.switchMonth(null);
        this.switchDay(null);

        workMonth = currMonth;
        workYear = currYear;
        workDay = currDay;
    }.bind(this);

    this.addDayClickHandler = function (handler) {
        dayClickHandler = handler;
    }

    this.changeDate = function (event) {
        if ($(event.target).hasClass("disabled")) return;

        var dayOfWeek = parseInt($(event.target).data('weekday'));

        if (isNaN(dayOfWeek)) {
            dayOfWeek = parseInt($(event.target).parent().data('weekday'));
        }

        if (dayClickHandler != undefined) {
            try {               
                dayClickHandler(currDay, dayOfWeek, currMonth, currYear);
            } catch (ex) {
                console.warn("Exception at dayClickHandler");
                console.log(ex);
            }
        }
    }.bind(this);

    this.switchMonth = function (forward) {

        if (forward !== null) {
            if (forward) {
                if (months[currMonth] === "December") {
                    currMonth = 0;
                    currYear++;
                } else {
                    currMonth++;
                }
            } else {
                if (months[currMonth] === "January") {
                    currMonth = 11;
                    currYear--;
                } else {
                    currMonth--;
                }
            }
        } else {
            currMonth = todayMonth;
            currYear = todayYear;
        }

        var calendar = this.initDatepickerStruct(currMonth, currYear);

        $(".day", calendar.days).bind("click", selectDay);
        $(".day", calendar.days).bind("click", this.changeDate);

        $("td.disabled .day", calendar.days).unbind("click", selectDay);
        $("td.disabled .day", calendar.days).unbind("click", this.changeDate);

        backToTodayLabel(currMonth, currYear);
        setToday(calendar.days);

        wrap.find("#datepicker-body").find("tbody").replaceWith(calendar.days);
        label.text(calendar.label);

    }.bind(this);

    this.switchDay = function (forward) {
        var days = wrap.find(".day"),
            weekends = wrap.find("td.disabled").find(".day");

        var currentDay = forward === null ? wrap.find("#today")[0] : wrap.find(".selected")[0];
        var currIndex = $.inArray(currentDay, days);

        var newIndex = currIndex;
        if (forward === null || forward) {
            if (forward) {
                if (currIndex === days.length - 1) {
                    this.switchMonth(true);

                    days = wrap.find(".day");
                    weekends = wrap.find("td.disabled .day");
                    newIndex = 0;
                } else {
                    newIndex++;
                }
            }

            while ($.inArray(days[newIndex], weekends) >= 0) {
                if (newIndex === days.length - 1) {
                    this.switchMonth(true);

                    days = wrap.find(".day");
                    weekends = wrap.find("td.disabled .day");
                    newIndex = 0;

                    continue;
                }

                newIndex++;
            }
        } else {
            if (currIndex === 0) {
                this.switchMonth(false);

                days = wrap.find(".day");
                weekends = wrap.find("td.disabled .day");
                newIndex = days.length - 1;
            } else {
                newIndex--;
            }

            while ($.inArray(days[newIndex], weekends) >= 0) {
                if (newIndex === 0) {
                    this.switchMonth(false);

                    days = wrap.find(".day");
                    weekends = wrap.find("td.disabled .day");
                    newIndex = days.length - 1;

                    continue;
                }

                newIndex--;
            }
        }

        currDay = newIndex + 1;
        days[newIndex].click();
    }.bind(this);

    this.getDayNames = function (dayNum) {
        if (isNaN(dayNum) || (dayNum < 1) || (dayNum > 7))
            return null;
        return dayNames[dayNum - 1];
    }.bind(this);

    this.initDatepickerStruct = function (month, year) {
        var lastDay = new Date(year, month + 1, 0).getDate(),
			firstWeekDay = new Date(year, month, 0).getDay(),
			lastDayLastMonth = new Date(year, month, 0).getDate(),
            weekedsOrAnotherMonth = true,
			dayNextMonth = 1,
            day = 1,
			days = [];

        if (this.initDatepickerStruct.cache[year]) {
            if (this.initDatepickerStruct.cache[year][month]) {
                return this.initDatepickerStruct.cache[year][month];
            }
        } else {
            this.initDatepickerStruct.cache[year] = {};
        }

        var i;
        for (i = 0; i < 6; i++) {
            days[i] = [];
            for (var j = 0; j < 7; j++) {
                if (i === 0) {
                    if (j < firstWeekDay) {
                        days[i][j] = '<td class="disabled">' + (lastDayLastMonth - firstWeekDay + 1 + j) + '</td>';
                    } else {
                        if (j === 5 || j === 6) {
                            days[i][j] = '<td class="disabled"><span class="day">' + day++ + '</span></td>';
                        } else {
                            days[i][j] = '<td data-weekday="' + j + '"><span class="day">' + day++ + '</span></td>';
                        }
                    }
                } else if (day <= lastDay) {
                    if (j === 5 || j === 6) {
                        days[i][j] = '<td class="disabled"><span class="day">' + day++ + '</span></td>';
                    } else {
                        days[i][j] = '<td data-weekday="' + j + '"><span class="day">' + day++ + '</span></td>';
                    }
                } else {
                    if (i === 4) {
                        days[i][j] = '<td class="disabled">' + dayNextMonth++ + '</td>';
                        weekedsOrAnotherMonth = false;
                    } else if (weekedsOrAnotherMonth) {
                        days[i][j] = '<td class="disabled">' + dayNextMonth++ + '</td>';
                    } else {
                        days[i][j] = '<td></td>';
                    }
                }
            }
        }

        for (i = 0; i < days.length; i++) {
            days[i] = "<tr>" + days[i].join("") + "</tr>";
        }

        days = $("<tbody>" + days.join("") + "</tbody>");
        
        if (month === todayMonth && year === todayYear) {
            $(".day", days).filter(function() {
                     return $(this).text() === new Date().getDate().toString();
                }).attr("id", "today");
        }

        this.initDatepickerStruct.cache[year][month] = { days: days, label: months[month] + ", " + year };

        return this.initDatepickerStruct.cache[year][month];
    }

    function backToTodayLabel(month, year) {
        wrap.find("#label-today").addClass("hidden");

        if (year < todayYear || (month < todayMonth && year === todayYear)) {
            wrap.find("#back-to-today-right").removeClass("hidden");
        } else if (year > todayYear || (month > todayMonth && year === todayYear)) {
            wrap.find("#back-to-today-left").removeClass("hidden");
        } else {
            wrap.find("#label-today").removeClass("hidden");
            wrap.find("#back-to-today-left").addClass("hidden");
            wrap.find("#back-to-today-right").addClass("hidden");
        }
    }

    function selectDay() {
        var selected = wrap.find(".selected")[0];

        if (selected === this) return;

        if ($(this).attr("id") === "today") {
            $(this).removeClass("today");
        }

        if ($(selected).attr("id") === "today") {
            $(selected).addClass("today");
        }

        $(selected).removeClass("selected");
        $(this).addClass("selected");
        
        currDay = parseInt($(this).text(), 10);
        
        $('#current-month').data('day', currDay).attr('data-day', currDay);
        $('#current-month').data('month', currMonth).attr('data-month', currMonth);
        $('#current-month').data('year', currYear).attr('data-year', currYear);
    }

    function setToday(days) {
        $(".selected", days).removeClass("selected");

        var today = $("#today", days)[0];
        var weekends = $("td.disabled .day", days);
        var isWeekday = !(today in weekends);

        if (today && isWeekday) {
            $(today).removeClass("today");
            $(today).addClass("selected");

            today.click();
        }
        
        if (currMonth === workMonth && currYear === workYear) {
            $(".day", days).filter(function () {
                return $(this).text() === workDay.toString();
            }).addClass('selected').click();
        }
    }

    function createDatapickerStruct() {
        var struct =
			'<div id="datepicker-header">' +
			'<span class="to-prev-month" id="to-prev-month"><i class="fa fa-caret-left" aria-hidden="true"></i></span>'
			+
			'<span class="current-month" id="current-month" data-day="" data-month="" data-year=""></span>'
			+
			'<span class="to-next-month" id="to-next-month"><i class="fa fa-caret-right" aria-hidden="true"></i></span>'
			+
			'</div>' +
			'<div id="datepicker-body">' +
			'<table>' +
			'<thead>' +
			'<tr>' +
			'<td>Пн</td>' +
			'<td>Вт</td>' +
			'<td>Ср</td>' +
			'<td>Чт</td>' +
			'<td>Пт</td>' +
			'<td>Сб</td>' +
			'<td>Вс</td>' +
			'</tr>' +
			'</thead>' +
			'<tbody>' +
			'</tbody>' +
			'</table>' +
			'</div>' +
			'<div id="datepicker-footer">' +
			'<span class="back-to-today-left hidden" id="back-to-today-left">' +
			'<i class="fa fa-long-arrow-left" aria-hidden="true"></i>' +
			'<span>Сегодня</span>' +
			'</span>' +
			'<span class="label-today" id="label-today">Сегодня</span>' +
			'<span class="back-to-today-right hidden" id="back-to-today-right">' +
			'<span>Сегодня</span>' +
			'<i class="fa fa-long-arrow-right" aria-hidden="true"></i>' +
			'</span>' +
			'</div>';

        wrap.append(struct);
    }

    this.initDatepickerStruct.cache = {};
    return {
        init: this.init,
        addDayClickHandler: this.addDayClickHandler,
        switchMonth: this.switchMonth,
        switchDay: this.switchDay,
        getDayNames: this.getDayNames,
        getCurrentDay: this.getCurrentDay
    };
    
};