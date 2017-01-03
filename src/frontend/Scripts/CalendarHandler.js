
'use strict';

var baseUrl = 'http://localhost:18332/api/calendar/';
var roomList = new Array();

$(document).ready(function () {

    GetRooms();

});

function findRoom (roomId) {
    for (var i = 0; i < roomList.length; i++) {
        if (roomList[i].id === roomId) {
            return roomList[i];
        }
    }
}

function getCalendar(roomId) {


    // Find the room with the ID passed into the function

    var room = findRoom(roomId);

    ShowRoomIcons(room);

    $('#calendar').fullCalendar('destroy');
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: 'agendaWeek',
        editable: true,
        viewRender: function (view, element) {
            LoadViewEvents(room.id);
        },
        // Add the details to the displayed element 
        eventRender: function (event, element) {
            element.append(event.occupantName);
            element.append("\n");
            element.append(event.details);
            AddContextMenuToEvent(event, element);
        },
        eventOverlap: false,
        selectable: true,
        selectOverlap: false,
        select: function (start, end) {
            $(".custom-menu").hide(100); // Hiding a possibly open context menu.
            CreateBooking(start, end, room, 2);
        },
        eventDrop: function (event, delta, revertFunc) {
            UpdateBooking(event, revertFunc);
        },
        eventResize: function (event, delta, revertFunc) {
            UpdateBooking(event, revertFunc);
        },
    });
}

function LoadViewEvents(roomId) {
    var startTime = $('#calendar').fullCalendar('getView').intervalStart.format();
    var endTime = $('#calendar').fullCalendar('getView').intervalEnd.format();

    var url = baseUrl + roomId + '/' + startTime + '/' + endTime;

    LoadEvents(url);
}


function LoadEvents(url) {

    $.getJSON(url, function (result) {
        var existingEvents = $('#calendar').fullCalendar('clientEvents');
        var existingEventIDs = new Array();

        // Write the IDs of all existing events to a new array
        $.each(existingEvents, function (index) {
            existingEventIDs.push(this.id);
        })

        var newEvents = new Array();

        for (var i = 0; i < result.length; i++) {

            // Check if the event coming from the API is not yet displayed in the calendar (through checking with the ID list).
            if (($.inArray(result[i].id, existingEventIDs) === -1)) {
                newEvents.push({
                    id: result[i].id,
                    start: result[i].startTime.toLocaleString(),
                    end: result[i].endTime.toLocaleString(),
                    title: result[i].title,
                    occupantName: result[i].occupantName,
                    roomId: result[i].roomId,
                    occupantId: result[i].occupantId
                });

                if (result[i].details) // if truthy, i.e. not null
                    newEvents[newEvents.length - 1].details = result[i].details;
            }

        }

        $('#calendar').fullCalendar('renderEvents', newEvents, true);
    });
}

function GetRooms() {
    var roomUrl = baseUrl + 'rooms';

    $.getJSON(roomUrl, function (result) {

        // The room list is saved to an array accessible for all functions.
        roomList = result;

        for (var i = 0; i < result.length; i++) {
            $('.dropdown-menu').append(
                $('<li>').append(
                $('<a>').attr('href', 'javascript:getCalendar(' + result[i].id + ')')
                .append(result[i].name + ' (' + result[i].capacity + ')')
                ));
        }
    })
}

function ShowRoomIcons(room) {
    $("#title").text(room.name + ' (' + room.capacity + ' platser)');

    if (room.hasTvScreen === true) {
        $('#Tvimage').fadeIn();
    }
    else {
        $('#Tvimage').fadeOut();
    }

    if (room.hasProjector === true) {
        $('#Projectorimage').fadeIn();
    }
    else {
        $('#Projectorimage').fadeOut();
    }

    if (room.hasWhiteBoard === true) {
        $('#Whiteboardimage').fadeIn();
    }
    else {
        $('#Whiteboardimage').fadeOut();
    }
}

function CreateBooking(start, end, room, occupantId) {

    var title = prompt("Ange en rubrik:");
    if (!title)
        return;

    var details = prompt("Ange en beskrivning (optional)", "");

    var booking = new Booking(start.format(), end.format(), room.id, occupantId, title, details);

    $.ajax({
        type: 'POST',
        url: (baseUrl + 'book'),
        data: JSON.stringify(booking),
        contentType: 'application/json',
        success: function (result) {
            var eventUrl = baseUrl + result.roomId + '/' + result.startTime + '/' + result.endTime;
            LoadEvents(eventUrl);
        }
    });

}

function UpdateBooking(event, revertFunc) {
    var booking = new Booking(event.start.format(), event.end.format(), event.roomId, event.occupantId, event.title, event.details);
    booking.id = event.id;

    $.ajax({
        type: 'PUT',
        url: (baseUrl + 'book/' + event.id),
        data: JSON.stringify(booking),
        contentType: 'application/json',
        error: function (jqXHR, textStatus, errorThrown) {
            revertFunc();
        }
    });
}


function Booking(startTime, endTime, roomId, occupantId, title, details) {
    this.startTime = startTime;
    this.endTime = endTime;
    this.roomId = roomId;
    this.occupantId = occupantId;
    this.title = title;
    this.details = details;
}