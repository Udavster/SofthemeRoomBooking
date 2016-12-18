function TimePicker() {

    if (this === window) {
        console.error('Use keyword new before function name DatePicker()');
        return false;
    }

    var wrap,
        hours, hourStart, hourEnd,
        minutes;

    this.init = function(newWrap, newhourStart, newhourEnd, arrowClickHandler) {
        wrap = $(newWrap || "#timepicker");

        hourStart = (!newhourStart || newhourStart < 0) ? 0 : newhourStart;
        hourEnd = (!newhourEnd || newhourEnd > 23) ? 23 : newhourEnd;

        createTimePickerStruct();

        hours = wrap.find("#hours");
        minutes = wrap.find("#minutes");

        wrap.find("#arrow-up-hours").bind("click", function(event) { this.switchHours(true, event); }.bind(this));
        wrap.find("#arrow-down-hours").bind("click", function(event) { this.switchHours(false, event); }.bind(this));
        wrap.find("#arrow-up-minutes").bind("click", function(event) { this.switchMinutes(true, event); }.bind(this));
        wrap.find("#arrow-down-minutes").bind("click", function(event) { this.switchMinutes(false, event); }.bind(this));

        $(hours).text(('0' + hourStart).slice(-2));
        $(minutes).text('00');

        this.OnTimeChange = arrowClickHandler;
    }.bind(this);

    this.switchHours = function(forward, event) {
        var currHours = parseInt($(hours).text());

        if (forward) {
            currHours = currHours === hourEnd ? hourStart : currHours + 1;
        } else {
            currHours = currHours === hourStart ? hourEnd : currHours - 1;
        }

        if (currHours === hourEnd) {
            $(minutes).text('00');
        }

        if (currHours < 10) {
            currHours = '0' + currHours.toString();
        }

        $(hours).text(currHours);
        this.changeTimeHandler(event);
    };

    this.switchMinutes = function(forward, event) {
        var currHours = parseInt($(hours).text());
        var currMinutes = parseInt($(minutes).text());

        if (forward && currHours < hourEnd) {
            currMinutes = currMinutes === 59 ? 0 : currMinutes + 1;
        } else if (currHours < hourEnd) {
            currMinutes = currMinutes === 0 ? 59 : currMinutes - 1;
        }

        if (currMinutes < 10) {
            currMinutes = '0' + currMinutes.toString();
        }

        $(minutes).text(currMinutes);
        this.changeTimeHandler(event);
    };

    function createTimePickerStruct() {
        var struct =
            '<div class="timepicker-wrapper">'
            + '<table class="timepicker">'
            + '<tr>'
            + '<td id="arrow-up-hours" class="timepicker-arrow">'
            + '<i class="fa fa-caret-up" aria-hidden="true"></i>'
            + '</td>'
            + '<td></td>'
            + '<td id="arrow-up-minutes" class="timepicker-arrow">'
            + '<i class="fa fa-caret-up" aria-hidden="true"></i>'
            + '</td>'
            + '</tr>'
            + '<tr>'
            + '<td id="hours" class="timepicker-time">12</td>'
            + '<td>:</td>'
            + '<td id="minutes" class="timepicker-time">59</td>'
            + '</tr>'
            + '<tr>'
            + '<td id="arrow-down-hours" class="timepicker-arrow">'
            + '<i class="fa fa-caret-down" aria-hidden="true"></i>'
            + '</td>'
            + '<td></td>'
            + '<td id="arrow-down-minutes" class="timepicker-arrow">'
            + '<i class="fa fa-caret-down" aria-hidden="true"></i>'
            + '</td>'
            + '</tr>'
            + '</table>'
            + '</div>';

        wrap.append(struct);
    }

    this.changeTimeHandler = function(event) {
        var isClickedArrow = $(event.target).hasClass('timepicker-arrow')
                                ? $(event.target).hasClass('timepicker-arrow')
                                : $(event.target).closest('td').hasClass('timepicker-arrow');

        if (!isClickedArrow) {
            return;
        }

        var hours = parseInt(wrap.find('#hours').text());
        var minutes = parseInt(wrap.find('#minutes').text());

        if (this.OnTimeChange != undefined) {
            try {
                this.OnTimeChange(hours, minutes);
            } catch (ex) {
                console.log('Exception at arrowClickHandler. ' + ex);
            }
        }   
    }.bind(this);

    return {
        init: this.init,
        switchHours: this.switchHours,
        switchMinutes: this.switchMinutes
    };
};