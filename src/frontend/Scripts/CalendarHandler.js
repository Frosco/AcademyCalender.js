
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
        eventRender: function (event, element) {
            element.append(event.description)
        }
    });

    var booking = {
        startTime: '2016-12-23T14:00:00',
        endTime: '2016-12-23T16:00:00',
        roomID: 1,
        occupantId: 2,
        title: 'Mote',
        description: 'Very important'
    };

    console.log(booking);
    console.log(JSON.stringify(booking));

    $.ajax({
        type: 'POST',
        url: baseUrl + 'Book',
        contentType: 'application/json',
        dataType: 'application/json',
        data: JSON.stringify(booking),
        success: function (result) {
            alert(result);
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
                    description: result[i].occupantName
                });
            }

        }
        console.log(newEvents);
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