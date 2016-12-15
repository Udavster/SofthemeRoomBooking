$(document).ready(function () {
    // var a = new Calendar("calendar-events");
    // a.addEventOnClickHandler(function(){alert('Clicked');});
    // a.setToday("02, Пт");
});

function tformat(num) {
    return ("0" + num).slice(-2);
}

function isContained(event, time) {
    if (60 * event['Start']['h'] + event['Start']['m'] > 60 * time['h'] + time['m']) {
        return false;
    }
    if (60 * event['Finish']['h'] + event['Finish']['m'] < 60 * time['h'] + time['m']) {
        return false;
    }
    return true;
}

function Event(start, finish, title, description, classes) {
    this.Start = start;
    this.Finish = finish;
    this.Title = title;
    this.Description = description;
    this.Classes = classes;
    this.addedElements = "";
}

function EmptyEvent(start, finish, title, description) {
    var rez = new Event(start, finish, title, description, 'event-empty');
    rez.addedElements = '<i class="fa fa-plus event-empty__plus" aria-hidden="true"></i>';
    return rez;
}

function Slider(name, startHour, finishHour, boundingElName, dragHandler, borderBox, static , bottomText) {

    this.getBoundingLeftRight = function (elementName) {
        $box = $(elementName);

        return {
            "left": $box.offset().left + 2, //border
            "right": $box.offset().left + $box.width() - 2,
            "width": $box.width() - 4,
            "top": $box.offset().top + 2,
            "bottom": $box.offset().top + $box.height - 2
        }
    }

    this.hide = function () {
        $("#"+this.name).hide();
    }.bind(this);

    this.show = function () {
        $("#" + this.name).show();
    }.bind(this);

    this.getRelativeCoords = function (x, y) {
        if (this.bounding == undefined) {
            this.bounding = this.getBoundingLeftRight(this.boundingElName);
        }
        var posx = x - this.bounding.left;

        return { 'x': x - this.bounding.left, 'y': y };
    }
    this.mousedown = function (e) {
        var self = this;

        this.clicked = true;
        e.preventDefault();

        var handlerMouseUp = function () {
            self.mouseup.apply(self);
        }
        $(document).mouseup(handlerMouseUp);

        var handlerMove = function (e) {
            e.preventDefault();
            self.drag.apply(self, [e]);
        }
        $(document).mousemove(handlerMove);


    }

    this.mouseup = function () {
        this.clicked = false;
    }

    this.drag = function (e) {
        if (!this.clicked) return;
        var pos = this.getRelativeCoords(e.pageX, e.pageY);
        if (!this.static) this.changePos(pos.x, this);
    }

    this.changePos = function (x, self) {
        x = (x > this.borderBox.left) ? x : this.borderBox.left;
        x = (x < this.bounding.width - this.borderBox.right) ? x : this.bounding.width - this.borderBox.right;
        this["$self"].css('left', x);
        var part = (x - this.borderBox.left) / (this.bounding.width - this.borderBox.left - this.borderBox.right);
        this.dragHandler(part);
        //var part = x/(this.bounding.width+4); 
        var hourDuration = this.finishHour - this.startHour + 1;
        console.log(hourDuration);
        var hours = Math.floor(hourDuration * part);
        var minutes = Math.floor(60 * (hourDuration * part - hours)); //m = 60*(24*p-h); m/60 = 24*p - h; p = (m/60 + h)/24
        $('#' + this.name + '-time').html(tformat(hours+this.startHour) + ":" + tformat(minutes));
    }.bind(this);

    this.setBoundingWidth = function (width, update) {

        if (!this.bounding || update) {
            this.bounding = this.getBoundingLeftRight(this.boundingElName);
        }

        if (width) {
            this.forcedBoundingWidth = width;
        }
        if (!this.forcedBoundingWidth) {
            return;
        }

        this.bounding.width = this.forcedBoundingWidth;
    }.bind(this);

    this.changeTime = function (hours, minutes) {
        var part = (minutes / 60.0 + hours - this.startHour) / (this.finishHour - this.startHour + 1);

        var x = (part * (this.bounding.width - this.borderBox.left - this.borderBox.right)) + this.borderBox.left;

        this["$self"].css('left', x);
        $('#' + this.name + '-time').html(tformat(hours) + ":" + tformat(minutes));
        return x;
    }.bind(this);

    this.setStartingPos = function (borderBox) {
        this.getRelativeCoords(0, 0);
        console.log(this["$self"]);
        this.changePos(this.$self.position().left, this);
        if (!borderBox || !borderBox.top || !borderBox.bottom) return;
        console.log(borderBox.top + borderBox.bottom);
        //this["$self"].css('height', 'calc("100% - 100px )');
        var heightDifference = borderBox.top + borderBox.bottom;
        var heightAttr = heightDifference > 0 ?
            'height: calc(100% + ' + heightDifference + 'px)' :
            'height: calc(100% - ' + (-heightDifference) + 'px)';
        this["$self"].get()[0].setAttribute("style", heightAttr);

        this["$self"].css('top', borderBox + 'px');
    }.bind(this);

    this.name = name;
    this.startHour = startHour;
    this.finishHour = finishHour;
    this.boundingElName = boundingElName;
    this.bottomText = bottomText;

    var sliderHtml = '<div id="' + this.name + '" class="calendar-events__slider vertical-slider">' +
            '<div class="vertical-slider__top">' +
                '<i class="fa fa-caret-down vertical-slider__icon" aria-hidden="true"></i>' +
            '</div>' +
        '<div class="vertical-slider__bottom">' +
            '<i class="fa fa-caret-up vertical-slider__icon" aria-hidden="true"></i>' +
            '<p id="' + this.name + '-time" class="vertical-slider__text">13:25</p>';
    if (this.bottomText) {
        sliderHtml += '<p  class="vertical-slider__text">' + this.bottomText + '</p>';
    }
    sliderHtml += '</div>' +
            '</div>';
    $(this.boundingElName).prepend(sliderHtml);

    this.borderBox = borderBox;

    this.dragHandler = dragHandler;

    var self = this;
    this.$self = $('#' + this.name);

    this.clicked = false;

    self.drugBinded = function (e) { self.mousedown.apply(self, [e]) };
    $('#' + self.name).mousedown(self.drugBinded);

    this.static = static;
    if (this.static) $('#' + this.name + '.calendar-events__slider').addClass('calendar-events_slider-disabled');

    this.getRelativeCoords(0, 0);
    this.changePos(this.$self.position().left, this);
    this.setStartingPos(this.borderBox);
}

class Calendar {

    constructor(name, calendarMemo) {
        this.name = name;
        this.className = "." + name;
        this.calendarDescRowHeight = 31;
        this.hourWidth = 120;
        this.roomHeight = 50;
        this.startHour = 9;
        this.finishHour = 19;
        this.constructBase($(this.className));
        this.constructFromMemo(calendarMemo);
        //getBoundingLeftRight(".calendar__visible-events");
        var self = this;
        this.sliderBorderBox = { 'left': 105, 'right': 0, 'top': 2, 'bottom': 0 };
        var slideHandle = function (pos) {
            self.changeBackgroundPos.apply(self, [pos]);
        }
        //console.log(this.className+"__background-layer");
        this.slider = new Slider("drag-slider", this.startHour, this.finishHour, this.className, slideHandle, this.sliderBorderBox);

        $("#drag-slider").css('z-index', 4);

        this.timeSlider = new Slider("time-slider", this.startHour, this.finishHour, this.className + "__background-layer", function () { }, { 'left': 0, 'right': 0, 'top': 2, 'bottom': -4 }, true, "Сейчас");
        this.timeSlider.setBoundingWidth($('.calendar-events__room-hour-tr').width());

        

        this.updateTimeTimer(this.timeSlider, false);
        this.updateTimeTimer(this.slider, true);
        this.timeSlider.hide();
        $("#drag-slider").mousedown(function() { this.timeSlider.show() }.bind(this));
        setInterval(function () { this.updateTimeTimer(this.timeSlider) }.bind(this), 60000);
    }

    updateTimeTimer(slider, changePos) {
        var now = new Date();
        var x = slider.changeTime(now.getHours(), now.getMinutes());
        if (changePos) {
            slider.changePos(x, slider);
        }

    }

   nowClickHandler() {
        this.updateTimeTimer(this.slider);
        this.timeSlider.hide();
    }

    addEventOnClickHandler(handler) {
        try {
            this.eventOnClickHandler = handler;
            if (handler) {
                $(this.className + '__room-event').click(handler);
            }
        } catch (ex) {
            console.warn("On event click handler: ");
            console.log(ex);
        }
    }
    changeHeight(calendarHeight) {
        $(this.className + '__visible-wrapper').css('height', 40 + 60 + calendarHeight + "px"); //TODO: delete magic numbers (40 - padding top, 60 - ?)
        calendarHeight += 'px';
        $(this.className).css('height', calendarHeight);
        $(this.className + '__background-layer').css('height', calendarHeight);
        $(this.className + '__background-layer').css('height', calendarHeight);
    }

    changeWidth(width) {
        if (!isNaN(width)) {
            width += 'px';
        }
        $(this.className).css('width', width);
        this.slider.setBoundingWidth(null, true);
    }

    addRooms(calendarMemo) {
        var roomList = $(this.className + "__room-list");
        var $roomCells = $(this.className + '__event-hour-layer');
        for (var i = 0; i < calendarMemo["roomArr"].length; i++) {
            roomList.append('<div class="' + this.name + '__room clearable "><span>' + calendarMemo["roomArr"][i] + '</span></div>');
            $roomCells.append('<div class="' + this.name + '__room-hours clearable ' + this.name + '__room-hour-tr ' + this.name + '__room-hours-cell"></div>');
        }
    }

    addRoomHours($roomHours) {
        for (var i = this.startHour; i < this.finishHour+1; i++) {
            $roomHours.append('<div class="' + this.name + '__room-hour clearable ' + this.name + '__room-hour-inner"><div class="' + this.name + '__room-hour-inner"></div></div>')
        }
        $(this.className + "__room-hours").css('width', ((this.finishHour - this.startHour + 1) * this.hourWidth) + 2 + "px");
    }

    setEvents(calendarMemo, append) {
        if ((append == undefined) || (append == false)) {
            $('.calendar-events__room-events').remove();
        }
        if (calendarMemo == undefined) {
            return;
        }
        var visibleEventsLayer = $(this.className + "__background-layer");
        var currentHeight = this.calendarDescRowHeight + 1;
        for (var i = 0; i < calendarMemo["roomArr"].length; i++) {
            var roomName = calendarMemo["roomArr"][i];
            var events = calendarMemo["events"][roomName];
            if (events == undefined) return;
            visibleEventsLayer.append('<div data-eventnum="' + i + '" id="room-' + i + '" class="' + this.name + '__room-events clearable"></div>');
            var $room = $('#room-' + i);
            $room.css('top', (currentHeight) + 'px');

            for (var j = 0; j < events.length; j++) {
                this.addEvent($room, events[j]);
            }
            currentHeight += this.roomHeight;
        }
        this.addEventOnClickHandler(this.eventOnClickHandler);
    }

    addEvent($room, event) {
        var startTime = event['Start'];
        var endTime = event['Finish'];
        var duration = (60 * (endTime["h"] - startTime["h"]) + (endTime["m"] - startTime["m"])) / 60.0;
        var width = duration * this.hourWidth - 2;
        var left = (startTime["h"] - this.startHour + startTime["m"] / 60.0) * this.hourWidth;
        var time = tformat(startTime["h"]) + ":" + tformat(startTime["m"]) + "-" + tformat(endTime["h"]) + ":" + tformat(endTime["m"]);
        var addedClasses = (event['Classes']) ? event['Classes'] : "";
        var eventTag = '<div class="' + this.name + '__room-event event '+addedClasses+'" style="width: ' + width + 'px; left: ' + left + 'px">';
        if (duration > 0.5) eventTag += '<div class="event__time">' + time + '</div>';
        else eventTag += '<div class="event__time event__time-center">' + tformat(startTime["h"]) + ":" + tformat(startTime["m"]) + '</div>' + '<div class="event__time event__time-center">-</div>' + '<div class="event__time event__time-center">' + tformat(endTime["h"]) + ":" + tformat(endTime["m"]) + '</div>';
        if (event.addedElements && (duration > 0.33)) eventTag += event.addedElements;
        if (duration > 0.5) eventTag += '<div>' + event['Title'] + '</div>';
        eventTag += '</div>';
        $room.append(eventTag);
    }

    constructBase($calendar) {
        var $controls = $('<div class="calendar-events__controls-days"></div>')
        var $calendarSwitch = $('<div class="calendar-events__switch switch-calendars"></div>');
        $calendarSwitch.append('<i id = "' + this.name + '-fw-control" class="fa fa-align-justify switch-calendars__option control" aria-hidden="true" active="true"></i>');
        $calendarSwitch.append('<i id = "' + this.name + '-pw-control" class="fa fa-th-list switch-calendars__option control" aria-hidden="true" active="false"></i>');

        $controls.append($calendarSwitch);

        $controls.append('<div class="calendar-events__prev-day-control calendar-events__day-control control"><i id="prev-day" class="fa fa-caret-left day-control" aria-hidden="true"></i> </div>');
        $controls.append('<div class="calendar-events__next-day-control calendar-events__day-control control"><i id="next-day" class="fa fa-caret-right day-control" aria-hidden="true"></i></div>');

        $controls.append('<div id="calendar-events-today" class="calendar-events__today">18, Пт</div>');

        var $roomList = $('<div class="calendar-events__room-list"></div>');
        $roomList.append('<div class="calendar-events__room calendar-events__description-row"></div>');
        $calendar.append($roomList);

        var $visibleWrapper = $('<div class="calendar-events__visible-wrapper"></div>');

        var $visibleEvents = $('<div class="calendar-events__visible-events"></div>');
        var $loading = $('  <div class="calendar-events__loading-glass loading-glass"><div class="loading-glass__loader"><i class="fa fa-spinner rotating" aria-hidden="true"></div></i></div>');
        $visibleEvents.append($loading);

        var $background = $('<div class="calendar-events__background-layer"></div>');
        var $hourLayer = $('<div class="calendar-events__event-hour-layer"></div>');
        $hourLayer.append('<div class="calendar-events__room-hours calendar-events__description-row"></div>');
        $background.append($hourLayer);
        $visibleEvents.append($background);
        $visibleWrapper.append($visibleEvents);


        $calendar.append($controls);
        $calendar.append($visibleWrapper);
    }
    constructFromMemo(memo) {
        $('.clearable').remove();

        var $descriptions = $(this.className + "__room-hours" + this.className + "__description-row");
        for (var i = this.startHour; i < this.finishHour + 1; i++) {
            $descriptions.append('<div class="' + this.name + '__room-hour ' + this.name + '__room-hour-desc clearable" ><div>' + i + ':00</div></div>')
        }

        var calendarHeight = this.calendarDescRowHeight + memo["roomArr"].length * this.roomHeight + 2; //border                    
        this.changeHeight(calendarHeight);

        this.addRooms(memo);

        var $roomHours = $(this.className + '__room-hours-cell');

        this.addRoomHours($roomHours);
        this.setEvents(memo);
    }

    sortEventsInMemo(memo) {
        for (var key in memo["events"]) {
            if (memo["roomArr"].indexOf(key) > -1) {
                this.sortEventsInRoom(memo["events"][key]);
                memo["events"][key] = memo["events"][key].concat(this.findEmptyPlaces(memo["events"][key], { 'h': this.startHour, 'm': 0 }, { 'h': this.finishHour + 1, 'm': 0 }));
            }
        }
        console.log(memo);
        return memo;
    }
    sortEventsInRoom(room) {
        room.sort(function (a, b) { return 60 * (a['Start']['h'] - b['Start']['h']) + (a['Start']['m'] - b['Start']['m']) });
    }



    findEmptyPlaces(events, minTime, maxTime) {
        var evArr = [];

        var len = events.length;
        if (len == 0) {
            for (var l = minTime['h']; l < maxTime['h']; l++) {
                evArr.push(new EmptyEvent({ 'h': l, 'm': 0 }, { 'h': l + 1, 'm': 0 }, 'Blank'));
            }

            return evArr;
        }

        evArr.push(new EmptyEvent(minTime, events[0]['Start'], 'Blank'));
        for (var i = 1; i < len; i++) {
            evArr.push(new EmptyEvent(events[i - 1]['Finish'], events[i]['Start'], 'Blank'));
        }

        evArr.push(new EmptyEvent(events[len - 1]['Finish'], maxTime, 'Blank'));
        var rez = [];
        var k = 0;
        console.warn(evArr);

        for (var j = 0; j < evArr.length; j++) {
            var start = {};
            var hourStart = { 'h': evArr[j]['Start']['h'] + 1, 'm': 0 };
            console.log('363l ' + isContained(evArr[j], hourStart));

            if (isContained(evArr[j], hourStart)) {
                rez.push(new EmptyEvent(evArr[j]['Start'], hourStart, 'Blank'));
                for (var hs = hourStart['h']; hs < evArr[j]['Finish']['h']; hs++) {
                    rez.push(new EmptyEvent({ 'h': hs, 'm': 0 }, { 'h': hs + 1, 'm': 0 }, 'Blank'));
                }
                rez.push(new EmptyEvent({ 'h': evArr[j]['Finish']['h'], 'm': 0 }, evArr[j]['Finish'], 'Blank'));
            } else {
                rez.push(evArr[j]);
            }
        }

        return rez;
    }

    changeBackgroundPos(position) {
        if ((position < 0) || (position > 1)) return;
        var width = $(this.className + "__visible-events").width();
        var left = -((this.finishHour - this.startHour + 1) * this.hourWidth - width) * position;
        $(this.className + '__background-layer').css('left', left + 'px');
    }

    setToday(str) {
        $("#calendar-events-today").html(str);
    }

    addNextPrevDayHandler(handler) {
        if (handler) {
             this.nextPrevHandler = handler;
        }
        $(this.className + " .day-control")
            .click(function (event) {
                if (!this.nextPrevHandler) return;
                this.nextPrevHandler(($(event.target).attr('id') === "next-day"));
            }.bind(this));
    }
}
