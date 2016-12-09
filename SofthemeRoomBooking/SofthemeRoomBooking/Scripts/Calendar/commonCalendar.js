        var cal = new DatePicker();
        var calendarMemo = {
            "roomArr": ['Einstein classroom', 'Tesla classroom', 'Newton classroom'],
            "events": {
                        'Einstein classroom':[{'name':'event in the first room', 'startTime': {'h':0,'m':0}, 'endTime': {'h':3,'m':50}}],
                        'Tesla classroom':[{'name':'event in the room', 'startTime':{'h':1,'m':0}, 'endTime': {'h':3,'m':0}}],
                        'Newton classroom':[{'name':'event in the room', 'startTime':{'h':2,'m':0}, 'endTime': {'h':2,'m':29}}],
                        //'room5':[{'name':'event in the room', 'startTime':{'h':6,'m':0}, 'endTime': {'h':9,'m':0}}]                            
                        }
        }
        var calendarMemo2 = {
            "roomArr": ['Einstein classroom2', 'Tesla classroom2', 'Newton classroom2', 'room42'],
            "events": {
                        'Einstein classroom2':[{'name':'event in the first room', 'startTime': {'h':4,'m':0}, 'endTime': {'h':5,'m':50}}],
                        'Tesla classroom2':[{'name':'event in the room', 'startTime':{'h':5,'m':0}, 'endTime': {'h':6,'m':0}}],
                        'Newton classroom2':[{'name':'event in the room', 'startTime':{'h':4,'m':30}, 'endTime': {'h':5,'m':0}}],
                        'room42':[{'name':'event in the room', 'startTime':{'h':2,'m':0}, 'endTime': {'h':3,'m':0}}],
                        //'room5':[{'name':'event in the room', 'startTime':{'h':6,'m':0}, 'endTime': {'h':9,'m':0}}]                            
                        }
        } 

		$(document).ready(function(){
				
			a = new Calendar("calendar-events", calendarMemo);
                a.addEventOnClickHandler(function(){alert('Clicked');});
                a.setToday("07, Пт");
			cal.init(null, function(date, dayOfWeek){
				var weekdays = ["Пн","Вт","Ср","Чт","Пт"];
				a.setToday(tformat(date)+", "+weekdays[dayOfWeek]);
				if(dayOfWeek%2===1){
					a.constructFromMemo(calendarMemo2);
				}else{
					a.constructFromMemo(calendarMemo);
				}
			}); 
			$("#"+a.name+"-fw-control").click(function(){
				
				$("#datepicker").hide();
    		    $(".calendars__month").css('display','none');
    			$(this).attr('active','false');
    			$("#" + a.name + "-pw-control").attr('active', 'true');
				a.changeWidth("calc(100% - 100px)");
			});
			$("#"+a.name+"-pw-control").click(function(){
			    $("#datepicker").show();
			    $(".calendars__month").css('display', 'block');
			    $(this).attr('active', 'false');
			    $("#" + a.name + "-fw-control").attr('active', 'true');
			    a.changeWidth(1024);
			    
			});
		});