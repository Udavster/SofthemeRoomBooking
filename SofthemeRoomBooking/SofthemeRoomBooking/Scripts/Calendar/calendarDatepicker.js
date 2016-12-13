function DatePicker() {
    if (this == window) {
        console.error('Use keyword new before function name DatePicker()');
        return;
    }

    var wrap,
        label,
        months = [
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
            "November", "December"
        ];
    this.dayNames = ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"];


    function monthToNum(Month) {
        if (!isNaN(Month) && (Month >= 0) && (Month < 12)) return Month;
        for (var i = 0; i < months.length; i++) {
            if (months[i] == Month) return i;
        }
        return -1;
    }

    this.renewMonthYear = function () {
        if (label && label.text() != "") {
            currDate = label.text().trim().split(", "),
                this.currMonth = currDate[0],
                this.currYear = parseInt(currDate[1], 10);
        }
    }.bind(this);

    this.init = function (newWrap, dayClickHandler) {
        var currDate = new Date();
        this.currMonth = currDate.getMonth(),
		this.currYear = currDate.getFullYear();

        wrap = $(newWrap || "#datepicker");
        createDatapickerStruct();
        label = wrap.find("#current-month");

        wrap.find("#to-prev-month").bind("click", function () { this.switchMonth(false); }.bind(this));
        wrap.find("#to-next-month").bind("click", function () { this.switchMonth(true); }.bind(this));
        wrap.find("#back-to-today-left").bind("click", function () { this.switchMonth(null, this.currMonth, this.currYear); }.bind(this));
        wrap.find("#back-to-today-right").bind("click", function () { this.switchMonth(null, this.currMonth, this.currYear); }.bind(this));

        this.switchMonth(null, this.currMonth, this.currYear);

        this.dayClickHandler = dayClickHandler;
    }.bind(this);

    this.switchMonth = function (next, month, year) {
        var currDate, calendar;

        if (!month) {
            if (next) {
                if (this.currMonth === "December") {
                    month = 0;
                } else {
                    month = months.indexOf(this.currMonth) + 1;
                }
            } else {
                if (this.currMonth === "January") {
                    month = 11;
                } else {
                    month = months.indexOf(this.currMonth) - 1;
                }
            }
        }

        if (!year) {
            if (next && month === 0) {
                year = this.currYear + 1;
            } else if (!next && month === 11) {
                year = this.currYear - 1;
            } else {
                year = this.currYear;
            }
        }

        backToToday(month, year);

        calendar = this.initDatepickerStruct(month, year);

        setDefault(calendar.days);

        $(".day", calendar.days).bind("click", selectDay);

        $("#datepicker-body", wrap).find("tbody").replaceWith(calendar.days);
        $("#current-month").text(calendar.label);

        this.renewMonthYear();

        var handler = function (event) {
            if ($(event.target).hasClass('disable')) return;
            var date = parseInt($(event.target).text());
            var dayOfWeek = parseInt($(event.target).data('weekday'));
            if (isNaN(dayOfWeek)) {
                var dayOfWeek = parseInt($(event.target).parent().data('weekday'));
            }

            if (this.dayClickHandler != undefined) {
                try {
                    this.dayClickHandler(date, dayOfWeek, monthToNum(this.currMonth), this.currYear);
                } catch (ex) {
                    console.warn('Exception at dayClickHandler');
                    console.log(ex);
                }
            }
        };

        $("#datepicker-body td").click(handler.bind(this));
    }.bind(this);

    this.initDatepickerStruct = function (month, year) {
        var lastDay = new Date(year, month + 1, 0).getDate(),
			lastWeekDay = new Date(year, month, lastDay).getDay(),
			firstWeekDay = new Date(year, month, 0).getDay(),
			lastDayLastMonth = new Date(year, month, 0).getDate();
        weekedsOrAnotherMonth = true,
			dayNextMonth = 1;
        day = 1,
			days = [];

        if (this.initDatepickerStruct.cache[year]) {
            if (this.initDatepickerStruct.cache[year][month]) {
                return this.initDatepickerStruct.cache[year][month];
            }
        } else {
            this.initDatepickerStruct.cache[year] = {};
        }

        for (var i = 0; i < 6; i++) {
            days[i] = [];
            for (var j = 0; j < 7; j++) {
                if (i === 0) {
                    if (j === 5 || j === 6) {
                        days[i][j] = '<td class="disable">' + day++ + '</td>';
                    } else if (j === firstWeekDay) {
                        days[i][j] = '<td data-weekday="' + j + '"><span class="day">' + day++ + '</span></td>';
                        firstWeekDay++;
                    } else {
                        days[i][j] = '<td class="disable">' + (lastDayLastMonth - firstWeekDay + 1 + j) + '</td>';
                    }
                } else if (day <= lastDay) {
                    if (j === 5 || j === 6) {
                        days[i][j] = '<td class="disable">' + day++ + '</td>';
                    } else {
                        days[i][j] = '<td data-weekday="' + j + '"><span class="day">' + day++ + '</span></td>';
                    }
                } else {
                    if (i === 4) {
                        days[i][j] = '<td class="disable">' + dayNextMonth++ + '</td>';
                        weekedsOrAnotherMonth = false;
                    } else if (weekedsOrAnotherMonth) {
                        days[i][j] = '<td class="disable">' + dayNextMonth++ + '</td>';
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

        if (month === new Date().getMonth() && year == new Date().getFullYear()) {
            $(".day", days).filter(function () { return $(this).text() === new Date().getDate().toString(); })
				.attr("id", "today")
				.addClass("selected");
        }

        this.initDatepickerStruct.cache[year][month] = { days: days, label: months[month] + ", " + year };

        return this.initDatepickerStruct.cache[year][month];
    }

    function backToToday(month, year) {
        var currMonth = new Date().getMonth();
        var currYear = new Date().getFullYear();

        if (month < currMonth) {
            $("#label-today").addClass("hidden");
            if (year <= currYear) {
                $("#back-to-today-right").removeClass("hidden");
            }
            else {
                $("#back-to-today-left").removeClass("hidden");
            }
        } else if (month > currMonth) {
            $("#label-today").addClass("hidden");
            if (year < currYear) {
                $("#back-to-today-right").removeClass("hidden");
            }
            else {
                $("#back-to-today-left").removeClass("hidden");
            }
        } else if (month === currMonth && year !== currYear) {
            $("#label-today").addClass("hidden");
            if (year < currYear) {
                $("#back-to-today-right").removeClass("hidden");
            }
            else {
                $("#back-to-today-left").removeClass("hidden");
            }
        } else {
            $("#label-today").removeClass("hidden");
            $("#back-to-today-left").addClass("hidden");
            $("#back-to-today-right").addClass("hidden");
        }
    }

    function selectDay() {
        var selected = $(".selected", "tbody");
        if ($(selected).attr("id") === "today") {
            $(selected).addClass("today");
        }

        $(selected).removeClass("selected");
        $(this).addClass("selected");
    }

    function setDefault(days) {
        var today = $("#today", days);

        $(".selected", days).removeClass("selected")

        if (today) {
            $(today).removeClass("today");
            $(today).addClass("selected");
        }
    }

    function createDatapickerStruct() {
        var struct =
			'<div id="datepicker-header">' +
			'<span class="to-prev-month" id="to-prev-month"><i class="fa fa-caret-left" aria-hidden="true"></i></span>'
			+
			'<span class="current-month" id="current-month" data-month="" data-year=""></span>'
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

    this.getDayNames = function(dayNum) {
        if (isNaN(dayNum) || (dayNum < 1) || (dayNum > 7))
            return null;
        return this.dayNames[dayNum-1];
    }.bind(this);

    this.initDatepickerStruct.cache = {};
    return {
        init: this.init,
        switchMonth: this.switchMonth,
        initDatepickerStruct: this.initDatepickerStruct,
        getDayNames: this.getDayNames
    };
    
};