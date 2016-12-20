$(document).ready(function () {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: 'agendaWeek',
        editable: true,
        viewRender: function (view, element) {
            LoadEvents();
        }
    });

    //LoadEvents();

});

//$('#calendar').fullCalendar().viewRender(function (view, element) {
//    LoadEvents()

//});

//function($('#calendar').fullCalendar('getView'),)

function LoadEvents() {
    var roomId = 1;
    var startTime = $('#calendar').fullCalendar('getView').intervalStart.format();
    var endTime = $('#calendar').fullCalendar('getView').intervalEnd.format();
    console.log(startTime + ' ' + endTime);


    var url = 'http://localhost:18332/api/calendar/' + roomId + '/' + startTime + '/' + endTime;

    $.getJSON(url, function (result) {
        console.log(result);
        var events = new Array();
        var existingEvents = $('#calendar').fullCalendar('clientEvents');

        for (var i = 0; i < result.length; i++) {


            events.push({
                id: result[i].id,
                start: result[i].startTime.toLocaleString(),
                end: result[i].endTime.toLocaleString(),
                title: result[i].title
            });
        }
        console.log(events);
        $('#calendar').fullCalendar('renderEvents', events, true);
    });
}