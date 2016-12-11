function DatePickerChosen() {

    if (this === window) {
        console.error('Use keyword new before function name DatePicker()');
        return;
    }

    var wrap, day, month,
        year = new Date().getFullYear(),
        months = ["Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря"];

    this.init = function (newWrap, arrowClickHandler) {
        wrap = $(newWrap || "#datepicker-chosen");

        createDatePickerChosenStruct();

        day = wrap.find("#day");
        month = wrap.find("#month");

        wrap.find("#arrow-up-day").bind("click", function (event) { this.switchDay(true, event); }.bind(this));
        wrap.find("#arrow-down-day").bind("click", function (event) { this.switchDay(false, event); }.bind(this));
        wrap.find("#arrow-up-month").bind("click", function (event) { this.switchMonth(true, event); }.bind(this));
        wrap.find("#arrow-down-month").bind("click", function (event) { this.switchMonth(false, event); }.bind(this));

        var currentDay = new Date().getDate();
        var currentMonth = new Date().getMonth();

        if (currentDay < 10) {
            currentDay = '0' + currentDay.toString();
        }

        $(day).text(currentDay);
        $(month).text(months[currentMonth]);

        this.OnDateChanged = arrowClickHandler;
    }.bind(this);

    this.getDateInfo = function () {
        var currentDay = parseInt(day.text(), 10),
            currentMonth = months.indexOf($(month).text()),     
            lastDay = new Date(year, currentMonth + 1, 0).getDate(),
            lastDayOfPrevMonth = new Date(year, currentMonth, 0).getDate(),
            lastDayOfNextMonth = new Date(year, currentMonth + 2, 0).getDate();

        return {
            currentDay: currentDay,
            currentMonth: currentMonth,
            lastDay: lastDay,
            lastDayOfPrevMonth: lastDayOfPrevMonth,
            lastDayOfNextMonth: lastDayOfNextMonth
        };
    }

    this.switchDay = function (forward, event) {
        var dateInfo = this.getDateInfo(),
            currentDay = dateInfo.currentDay,
            currentMonth = dateInfo.currentMonth;

        if (forward) {
            if (currentDay < dateInfo.lastDay) {
                currentDay += 1;
            } else {
                currentDay = 1;
                this.switchMonth(true, event);
            }
        } else {
            if (currentDay > 1) {
                currentDay -= 1;
            } else {
                currentDay = dateInfo.lastDayOfPrevMonth;
                this.switchMonth(false, event);
            }
        }

        if (currentDay < 10) {
            currentDay = '0' + currentDay.toString();
        }

        $(day).text(currentDay);
        this.changeDateHandler(event);
    };

    this.switchMonth = function (forward, event) {
        var dateInfo = this.getDateInfo(),
            currentDay = dateInfo.currentDay,
            currentMonth = dateInfo.currentMonth;

        if (forward) {
            currentMonth = months[currentMonth] === 'Декабря' ? 0 : currentMonth + 1;

            if (currentDay > dateInfo.lastDayOfNextMonth) {
                currentDay = dateInfo.lastDayOfNextMonth;
                $(day).text(currentDay);
            }
        } else {
            currentMonth = months[currentMonth] === 'Января' ? 11 : currentMonth - 1;

            if (currentDay > dateInfo.lastDayOfPrevMonth) {
                currentDay = dateInfo.lastDayOfPrevMonth;
                $(day).text(currentDay);
            }
        }

        $(month).text(months[currentMonth]);
        this.changeDateHandler(event);
    };

    this.changeDateHandler = function (event) {
        var isClickedArrow = $(event.target).closest('tr').hasClass('datepicker-chosen-arrow');

        if (!isClickedArrow) {
            return;
        }

        var day = parseInt(wrap.find('#day').text());
        var month = months.indexOf(wrap.find('#month').text());

        if (this.OnDateChanged != undefined) {
            try {
                this.OnDateChanged(day, month);
            } catch (ex) {
                console.log('Exception at arrowClickHandler');
            }
        }
    }.bind(this);

    function createDatePickerChosenStruct() {
        var struct =
            '<div class="datepicker-chosen-wrapper">'
            + '<table class="datepicker-chosen">'
            + '<tr class="datepicker-chosen-arrow">'
            + '<td id="arrow-up-day" class="datepicker-chosen-day">'
            + '<i class="fa fa-caret-up" aria-hidden="true"></i>'
            + '</td>'
            + '<td id="arrow-up-month" class="datepicker-chosen-month">'
            + '<i class="fa fa-caret-up" aria-hidden="true"></i>'
            + '</td>'
            + '</tr>'
            + '<tr class="datepicker-chosen-date">'
            + '<td id="day" class="datepicker-chosen-day"></td>'
            + '<td id="month" class="datepicker-chosen-month"></td>'
            + '</tr>'
            + '<tr  class="datepicker-chosen-arrow">'
            + '<td id="arrow-down-day" class="datepicker-chosen-day">'
            + '<i class="fa fa-caret-down" aria-hidden="true"></i>'
            + '</td>'
            + '<td id="arrow-down-month" class="datepicker-chosen-month">'
            + '<i class="fa fa-caret-down" aria-hidden="true"></i>'
            + '</td>'
            + '</tr>'
            + '</table>'
            + '</div>';

        wrap.append(struct);
    }

    return {
        init: this.init,
        switchDay: this.switchDay,
        switchMonth: this.switchMonth
    };
};