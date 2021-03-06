function tformat(num) {
    return ("0" + num).slice(-2);
}
function formatText(text) {
    if (text && text.length > 20) {
        return text.slice(0, 17) + "...";
    }
    return text;
}
function compareTime(a, b) {
    if (60 * a['h'] + a['m'] > 60 * b['h'] + b['m']) {
        return 1;
    }
    return -1;
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

function Event(id, start, finish, title, description, classes) {
    this.Id = id;
    this.Start = start;
    this.Finish = finish;
    this.Title = title;
    this.Description = description;
    this.Classes = classes;
    this.addedElements = "";
}

function EmptyEvent(start, finish, title, description) {
    var rez = new Event(-1, start, finish, title, description, 'event-empty');
    rez.Empty = true;
    rez.addedElements = '<i class="fa fa-plus event-empty__plus" aria-hidden="true"></i>';
    return rez;
}

function PrivateEvent(id, start, finish, title, description) {
    var rez = new Event(id, start, finish, title, description, 'event-private');
    rez.addedElements = '<i class="fa fa-loc event-pivate__lock" aria-hidden="true"></i>';
    rez.Private = true;
    return rez;
}

function EventPrivacyCheck(event) {
    if (event.Empty || event.Publicity) return event;
    event.Classes += ' event-private';
    event.addedElements = '<i class="fa fa-loc event-pivate__lock" aria-hidden="true"></i>';
    event.Private = true;
    return event;
}

function Slider(name, startHour, finishHour, boundingElName, dragHandler, borderBox, static , bottomText, overflow, timeline) {

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
        this["$self"].hide();
    }.bind(this);

    this.setTimeline = function (timeline) {
            this.timeline = timeline;
        }.bind(this);

    this.show = function () {
        this["$self"].show();
    }.bind(this);

    this.left = function () {
        var leftStr = this["$self"].css('left');
        return leftStr.slice(0, leftStr.length - 2);
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
        this.dragHandler(part, this);
        //var part = x/(this.bounding.width+4); 
        var hourDuration = this.finishHour - this.startHour + 1;
        var hours = Math.floor(hourDuration * part);
        var minutes = Math.floor(60 * (hourDuration * part - hours)); //m = 60*(24*p-h); m/60 = 24*p - h; p = (m/60 + h)/24
        $('#' + this.name + '-time').html(tformat(hours + this.startHour) + ":" + tformat(minutes));

        hours += this.startHour;
        var time = { 'h': hours, 'm': minutes };
        this.highlight(time);

    }.bind(this);

    this.highlight = function(time) {
        var prev = null;
        var prevk = null;
        this.previ = 0;

        if (this.timeline&&this.timeline[0]) {
            var tk = Object.keys(this.timeline[0])[0];
            var t = JSON.parse(tk);
            if (compareTime(t, time) > 0) {
                this.complexApply(0, -1);
                return;
            }
            prevk = tk;
            prev = t;
            for (var i = 1; i < this.timeline.length; i++) {
                tk = Object.keys(this.timeline[i])[0];
                t = JSON.parse(tk);
                if ((compareTime(prev, time) < 0) && (compareTime(t, time) > 0)) {
                    this.complexApply(i, this.previ);
                    return;
                }
                prevk = tk;
                prev = t;
            }
            if (compareTime(prev, time) < 0) {
                this.complexApply(this.timeline.length, -1);
            }
        }
    }

    this.complexApply = function(i, previ) {

        if (previ === i) return;
        for (var k = 0; k < 12; k++) {
            var room = $('.room__general.room-' + k + '');
            room.css('background', '');
            room.children('.text').css('background-color', '');
            $('.room__general-blocked').css('background', '#d7d9de');
            $('.room__general-blocked').children('.text').css('background-color', '#acb0b6');
        }
        if (i === this.timeline.length) return;
            for (var j = 0; j < i; j++) {
                this.apply(this.apply(this.timeline[j][Object.keys(this.timeline[j])[0]]), true);
            }
        this.previ = i;
    }

    this.apply = function (info, forward) {
        for (key in info) {
            if (info.hasOwnProperty(key)) {
                if (info[key] === true) {
                    var room = $('.room__general.room-' + key + '');
                    room.css('background', '#f95752');
                    room.children('.text').css('background-color', 'red');
                } else {
                    var room = $('.room__general.room-' + key + '');
                    room.css('background', '');
                    room.children('.text').css('background-color', '');
                    $('.room__general-blocked').css('background', '#d7d9de');
                    $('.room__general-blocked').children('.text').css('background-color', '#acb0b6');
                }
            }
        }

    }

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
        if (!overflow && hours > this.finishHour) {
            hours = this.finishHour + 1;
            minutes = 0;
        }
        if (!overflow && hours < this.startHour) {
            hours = this.startHour + 1;
            minutes = 0;
        }

        var part = (minutes / 60.0 + hours - this.startHour) / (this.finishHour - this.startHour + 1);

        var x = (part * (this.bounding.width - this.borderBox.left - this.borderBox.right)) + this.borderBox.left;

        this["$self"].css('left', x);
        $('#' + this.name + '-time').html(tformat(hours) + ":" + tformat(minutes));

        this.highlight({ 'h': hours, 'm': minutes });

        return x;
    }.bind(this);


    this.setStartingPos = function (borderBox) {
        this.getRelativeCoords(0, 0);
        this.changePos(this.$self.position().left, this);
        if (!borderBox || !borderBox.top || !borderBox.bottom) return;
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
    this.overflow = overflow;
    this.timeline = timeline;

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

        this.timeSlider = new Slider("time-slider", this.startHour, this.finishHour, this.className + "__background-layer", function () { }, { 'left': 0, 'right': 0, 'top': 2, 'bottom': -4 }, true, "Сейчас", true);
        this.timeSlider.setBoundingWidth($('.calendar-events__room-hour-tr').width());

        this.sliderBorderBox = { 'left': 105, 'right': 0, 'top': 2, 'bottom': 0 };
        var slideHandle = function (pos) {
            self.changeBackgroundPos.apply(self, [pos]);
            self.changeNowVisibility(pos, self.timeSlider);
        }
        this.slider = new Slider("drag-slider", this.startHour, this.finishHour, this.className, slideHandle, this.sliderBorderBox, null, null, null);

        $("#drag-slider").css('z-index', 4);

        this.updateTimeTimer(this.timeSlider, false);
        this.updateTimeTimer(this.slider, true);
        this.timeSlider.hide();
        $("#drag-slider").mousedown(function () { this.timeSlider.show() }.bind(this));
        setInterval(function () { this.updateTimeTimer(this.timeSlider) }.bind(this), 60000);

        $(this.className + '__now-link').click(function () { this.nowClickHandler() }.bind(this));
    }

    updateTimeTimer(slider, changePos) {
        var now = new Date();
        var x = slider.changeTime(now.getHours(), now.getMinutes());
        if (changePos) {
            slider.changePos(x, slider);
        }

    }

    nowClickHandler() {
        this.updateTimeTimer(this.slider, true);
        this.timeSlider.hide();
    }

    changeNowVisibility(position, slider) {
        if ((position < 0) || (position > 1)) return;
        var width = $(this.className + '__visible-events').width();
        var left = -((this.finishHour - this.startHour + 1) * this.hourWidth - width) * position;

        if (slider.left() < -left) {
            $(this.className + '__now-link-left').show();
            return;
        }

        if (slider.left() > -left + width) {
            $(this.className + '__now-link-right').show();
            return;
        }

        $(this.className + '__now-link-left').hide();
        $(this.className + '__now-link-right').hide();
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
        $(this.className + '__visible-wrapper').css('height', 40 + 60 + calendarHeight + "px");
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
        this.updateTimeTimer(this.timeSlider, false);
        this.updateTimeTimer(this.slider, true);
        this.timeSlider.hide();
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
        for (var i = this.startHour; i < this.finishHour + 1; i++) {
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
        for (var i in calendarMemo["events"]) {

            if (calendarMemo["events"].hasOwnProperty(i)) {
                var roomId = calendarMemo["events"][i]['roomName'];
                var events = calendarMemo["events"][i]['events'];

                if (events == undefined) return;

                visibleEventsLayer.append('<div data-roomnum=' +
                    i + ' id="room-' + i + '" data-roomid='+roomId+' class="' + 
                    this.name + '__room-events clearable"></div>');

                var $room = $('#room-' + i);
                $room.css('top', (currentHeight) + 'px');

                for (var j = 0; j < events.length; j++) {
                    this.addEvent($room, EventPrivacyCheck(events[j]));
                }
                currentHeight += this.roomHeight;
            }

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
        var eventTag = '<div class="' + this.name + '__room-event event ' + addedClasses + '" style="width: ' + width + 'px; left: ' + left + 'px">';
        if (duration > 0.5) eventTag += '<div class="event__time">' + time + '</div>';
        else eventTag += '<div class="event__time event__time-center">' + tformat(startTime["h"]) + ":" + tformat(startTime["m"]) + '</div>' + '<div class="event__time event__time-center">-</div>' + '<div class="event__time event__time-center">' + tformat(endTime["h"]) + ":" + tformat(endTime["m"]) + '</div>';
        if (event.addedElements && (duration > 0.33)) eventTag += event.addedElements;
        if (duration > 0.5) eventTag += '<div>' + formatText(event['Title']) + '</div>';
        eventTag += '</div>';
        var eventHtml = $(eventTag);

        eventHtml.attr('data-id', event.Id);
        eventHtml.attr('data-sh', startTime.h);
        eventHtml.attr('data-sm', startTime.m);
        eventHtml.attr('data-eh', endTime.h);
        eventHtml.attr('data-em', endTime.m);
        eventHtml.attr('data-desc', event.Description);
        eventHtml.attr('data-title', event.Title);

        $room.append(eventHtml);

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

        var $nowLinks = $('<div class = "calendar-events__now-line">' +
	                        '<a class = "calendar-events__now-link calendar-events__now-link-left control">Сейчас&#160;<i class="fa fa-long-arrow-left" aria-hidden="true"></i></a>' +
	                        '<a class = "calendar-events__now-link calendar-events__now-link-right control">Сейчас&#160;<i class="fa fa-long-arrow-right" aria-hidden="true"></i></a>' +
                          '</div>');

        $calendar.append($controls);
        $calendar.append($visibleWrapper);
        $calendar.append($nowLinks);

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
        this.Auth = memo.Auth;

        if (this.slider && this.timeSlider) {
            this.updateTimeTimer(this.timeSlider, false);
            this.updateTimeTimer(this.slider, true);
            this.timeSlider.hide();
        }
    }

    createTimeLine(memo) {
        var timeline = {};


        for (var key in memo['events']) {
            if (memo['events'].hasOwnProperty(key)) {
                for (var i in memo['events'][key]['events']) {
                    if (memo['events'][key]['events'].hasOwnProperty(i)) {
                        var event = memo['events'][key]['events'][i];
                        var start = JSON.stringify(event.Start);

                        if (!(start in timeline)) {
                            timeline[start] = {};
                        }
                        timeline[start][memo['events'][key]['roomName']] = true;

                        var finish = JSON.stringify(event.Finish);
                        if (!(finish in timeline)) {
                            timeline[finish] = {};
                        }
                        timeline[finish][memo['events'][key]['roomName']] = false;
                    }
                }
            }
        }
        this.timeline = [];
        for (var key in timeline) {
            if (timeline.hasOwnProperty(key)) {
                var obj = {};
                obj[key] = timeline[key];
                this.timeline.push(obj);
            }
        }

        
        this.timeline.sort(function (a, b) {
             a = JSON.parse(Object.keys(a)[0]);
             b = JSON.parse(Object.keys(b)[0]);
             return 60 * (a['h'] - b['h']) + (a['m'] - b['m'])
        });
        
        this.slider.setTimeline(this.timeline);
    }


    sortEventsInMemo(memo) {
        if (!memo.Auth) return memo;

        for (var key in memo["events"]) {
            if (memo['events'].hasOwnProperty(key)) {
                this.sortEventsInRoom(memo["events"][key]['events']);
                memo["events"][key]['events'] = memo["events"][key]['events'].concat(this.findEmptyPlaces(memo["events"][key]['events'], { 'h': this.startHour, 'm': 0 }, { 'h': this.finishHour + 1, 'm': 0 }));
            }
        }

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

        for (var j = 0; j < evArr.length; j++) {
            var start = {};
            var hourStart = { 'h': evArr[j]['Start']['h'] + 1, 'm': 0 };

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
