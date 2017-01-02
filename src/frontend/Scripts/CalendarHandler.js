
'use strict';

var baseUrl = 'http://localhost:18332/api/calendar/';
var roomList = new Array();

$(document).ready(function () {

    GetRooms();

});

function getCalendar(roomID) {


    // Find the room with the ID passed into the function
    var findRoom = function (roomID) {
        for (var i = 0; i < roomList.length; i++) {
            if (roomList[i].id === roomID) {
                return roomList[i];
            }
        }
    }

    var room = findRoom(roomID);

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
            LoadEvents(room.id);
        },
        // Add the details to the displayed element 
        eventRender: function (event, element) {
            element.append(event.occupantName);
            element.append("\n");
            element.append(event.details);
        },
        selectable: true,
        select: function (start, end) {
            CreateBooking(start, end, room, 2);
            LoadEvents(room.id);
        }
    });
}


function LoadEvents(roomID) {



    var startTime = $('#calendar').fullCalendar('getView').intervalStart.format();
    var endTime = $('#calendar').fullCalendar('getView').intervalEnd.format();

    var url = baseUrl + roomID + '/' + startTime + '/' + endTime;

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
                    occupantName: result[i].occupantName
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
    var details = prompt("Ange en beskrivning (optional)", "");

    var booking = new Booking(start.format(), end.format(), room.id, occupantId, title, details);

    $.ajax({
        type: 'POST',
        url: (baseUrl + 'book'),
        data: JSON.stringify(booking),
        contentType: 'application/json',
        success: function (result) {
            console.log(result);
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