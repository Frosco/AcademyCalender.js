$(document).ready(function () {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: 'agendaWeek',
        editable: true,
        events: [
    {
        title: 'Möte',
        start: '2016-12-15'
    }
        ]
    });
    $('#calendar').fullCalendar('addEventSource', source)
});