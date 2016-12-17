function DatePickerChosen() {

    if (this === window) {
        console.error('Use keyword new before function name DatePicker()');
        return false;
    }

    var numberMonths = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
        wordMonths = ['Января', 'Февраля', 'Марта', 'Апреля', 'Мая', 'Июня', 'Июля', 'Августа', 'Сентября', 'Октября', 'Ноября', 'Декабря'];

    var wrap, separator,
        day, month, year, months,
        startDate, endDate;

    this.init = function (newWrap, newSeparator, monthType, newStartDate, newEndDate, arrowClickHandler) {
        wrap = $(newWrap || '#datepicker-chosen');
        separator = newSeparator === null ? '' : newSeparator;

        months = monthType === 'number' ? numberMonths : wordMonths;

        startDate = (newStartDate instanceof Date) ? newStartDate : 0;
        endDate = (newEndDate instanceof Date) ? newEndDate : 0;

        createDatePickerChosenStruct();

        day = wrap.find('#day');
        month = wrap.find('#month');
        year = wrap.find('#year');

        wrap.find('#arrow-up-day').bind('click', function (event) { this.switchDay(true, event); }.bind(this));
        wrap.find('#arrow-down-day').bind('click', function (event) { this.switchDay(false, event); }.bind(this));
        wrap.find('#arrow-up-month').bind('click', function (event) { this.switchMonth(true, event); }.bind(this));
        wrap.find('#arrow-down-month').bind('click', function (event) { this.switchMonth(false, event); }.bind(this));
        wrap.find('#arrow-up-year').bind('click', function (event) { this.switchYear(true, event); }.bind(this));
        wrap.find('#arrow-down-year').bind('click', function (event) { this.switchYear(false, event); }.bind(this));

        var currentDay = new Date().getDate(),
            currentMonth = new Date().getMonth(),
            currentYear = new Date().getFullYear();

        if (separator !== '') {
            wrap.find('.datepicker-chosen-day').removeClass('datepicker-chosen-day').addClass('datepicker-chosen-default');
            wrap.find('.datepicker-chosen-month').removeClass('datepicker-chosen-month').addClass('datepicker-chosen-default');
            wrap.find('.datepicker-chosen-year').removeClass('datepicker-chosen-year').addClass('datepicker-chosen-default');
        }

        currentDay = ('0' + currentDay).slice(-2);
        currentYear = separator !== '' ? (currentYear.toString()).slice(2) : currentYear;

        $(day).text(currentDay);
        $(month).text(months[currentMonth]);
        $(year).text(currentYear);

        this.checkLimitDate();
        this.OnDateChanged = arrowClickHandler;
    }.bind(this);

    this.getDateInfo = function () {

        var currentDay = parseInt(day.text(), 10),
            currentMonth = months.indexOf($(month).text()),
            currentYear = separator !== '' ? parseInt('20' + wrap.find('#year').text()) : parseInt(wrap.find('#year').text()),
            lastDay = new Date(currentYear, currentMonth + 1, 0).getDate(),
            lastDayOfPrevMonth = new Date(currentYear, currentMonth, 0).getDate(),
            lastDayOfNextMonth = new Date(currentYear, currentMonth + 2, 0).getDate();

        return {
            currentDay: currentDay,
            currentMonth: currentMonth,
            currentYear: currentYear,
            lastDay: lastDay,
            lastDayOfPrevMonth: lastDayOfPrevMonth,
            lastDayOfNextMonth: lastDayOfNextMonth
        };
    }

    this.switchDay = function (forward, event) {
        var dateInfo = this.getDateInfo(),
            currentDay = dateInfo.currentDay;

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

        currentDay = ('0' + currentDay).slice(-2);

        $(day).text(currentDay);
        this.changeDateHandler(event);
        this.checkLimitDate();
    };

    this.switchMonth = function (forward, event) {
        var dateInfo = this.getDateInfo(),
            currentDay = dateInfo.currentDay,
            currentMonth = dateInfo.currentMonth;

        if (forward) {
            if (months[currentMonth] === 'Декабря' || months[currentMonth] === '12') {
                currentMonth = 0;
                this.switchYear(true, event);
            } else {
                currentMonth = currentMonth + 1;
            }

            if (currentDay > dateInfo.lastDayOfNextMonth) {
                currentDay = dateInfo.lastDayOfNextMonth;
                $(day).text(currentDay);
            }
        } else {
            if (months[currentMonth] === 'Января' || months[currentMonth] === '01') {
                currentMonth = 11;
                this.switchYear(false, event);
            } else {
                currentMonth = currentMonth - 1;
            }

            if (currentDay > dateInfo.lastDayOfPrevMonth) {
                currentDay = dateInfo.lastDayOfPrevMonth;
                $(day).text(currentDay);
            }
        }

        $(month).text(months[currentMonth]);
        this.changeDateHandler(event);
        this.checkLimitDate();
    };

    this.switchYear = function (forward, event) {
        var dateInfo = this.getDateInfo(),
            currentDay = dateInfo.currentDay,
            currentMonth = dateInfo.currentMonth,
            currentYear = dateInfo.currentYear;

        if (forward) {
            currentYear++;
        } else {
            currentYear--;
        }

        if (currentMonth === 1) {
            var lastDayOfFebrary = new Date(currentYear, currentMonth + 1, 0).getDate();

            if (currentDay > lastDayOfFebrary) {
                $(day).text(lastDayOfFebrary);
            }
        }

        currentYear = separator !== '' ? (currentYear.toString()).slice(2) : currentYear;

        $(year).text(currentYear);
        this.changeDateHandler(event);
        this.checkLimitDate();
    };

    this.checkLimitDate = function () {
        var dateInfo = this.getDateInfo();

        if (startDate !== 0) {
            if (dateInfo.currentYear <= startDate.getFullYear()) {
                wrap.find('#arrow-down-year').css('visibility', 'hidden');
                var startYear = separator !== '' ? (startDate.getFullYear().toString()).slice(2) : startDate.getFullYear();
                $(year).text(startYear);

                if (dateInfo.currentMonth <= startDate.getMonth()) {
                    wrap.find('#arrow-down-month').css('visibility', 'hidden');
                    $(month).text(months[startDate.getMonth()]);

                    if (dateInfo.currentDay <= startDate.getDate()) {
                        wrap.find('#arrow-down-day').css('visibility', 'hidden');
                        $(day).text(('0' + startDate.getDate()).slice(-2));
                    } else {
                        wrap.find('#arrow-down-day').css('visibility', 'visible');
                    }
                } else {
                    wrap.find('#arrow-down-month').css('visibility', 'visible');
                    wrap.find('#arrow-down-day').css('visibility', 'visible');
                }
            } else {
                wrap.find('#arrow-down-year').css('visibility', 'visible');
                wrap.find('#arrow-down-month').css('visibility', 'visible');
                wrap.find('#arrow-down-day').css('visibility', 'visible');
            }
        }

        if (endDate !== 0) {
            if (dateInfo.currentYear >= endDate.getFullYear()) {
                wrap.find('#arrow-up-year').css('visibility', 'hidden');

                if (dateInfo.currentMonth >= endDate.getMonth()) {
                    wrap.find('#arrow-up-month').css('visibility', 'hidden');

                    if (dateInfo.currentDay >= endDate.getDate()) {
                        wrap.find('#arrow-up-day').css('visibility', 'hidden');
                    } else {
                        wrap.find('#arrow-up-day').css('visibility', 'visible');
                    }
                } else {
                    wrap.find('#arrow-up-month').css('visibility', 'visible');
                    wrap.find('#arrow-up-day').css('visibility', 'visible');
                }
            } else {
                wrap.find('#arrow-up-year').css('visibility', 'visible');
                wrap.find('#arrow-up-month').css('visibility', 'visible');
                wrap.find('#arrow-up-day').css('visibility', 'visible');
            }
        }
    }

    this.changeDateHandler = function (event) {
        var isClickedArrow = $(event.target).closest('tr').hasClass('datepicker-chosen-arrow');

        if (!isClickedArrow) {
            return;
        }

        var day = parseInt(wrap.find('#day').text());
        var month = months.indexOf(wrap.find('#month').text());
        var year = parseInt(wrap.find('#year').text());

        if (this.OnDateChanged != undefined) {
            try {
                this.OnDateChanged(day, month, year);
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
            + '<td></td>'
            + '<td id="arrow-up-month" class="datepicker-chosen-month">'
            + '<i class="fa fa-caret-up" aria-hidden="true"></i>'
            + '</td>'
            + '<td></td>'
            + '<td id="arrow-up-year" class="datepicker-chosen-year">'
            + '<i class="fa fa-caret-up" aria-hidden="true"></i>'
            + '</td>'
            + '</tr>'
            + '<tr class="datepicker-chosen-date">'
            + '<td id="day" class="datepicker-chosen-day"></td>'
            + '<td>' + separator + '</td>'
            + '<td id="month"></td>'
            + '<td>' + separator + '</td>'
            + '<td id="year" class="datepicker-chosen-year"></td>'
            + '</tr>'
            + '<tr  class="datepicker-chosen-arrow">'
            + '<td id="arrow-down-day" class="datepicker-chosen-day">'
            + '<i class="fa fa-caret-down" aria-hidden="true"></i>'
            + '</td>'
            + '<td></td>'
            + '<td id="arrow-down-month" class="datepicker-chosen-month">'
            + '<i class="fa fa-caret-down" aria-hidden="true"></i>'
            + '</td>'
            + '<td></td>'
            + '<td id="arrow-down-year" class="datepicker-chosen-year">'
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
        switchMonth: this.switchMonth,
        switchYear: this.switchYear
    };
};