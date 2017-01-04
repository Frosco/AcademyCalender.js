// Adds a custom right-click context menu to the elemnt passed into the AddContextMenu function.
// Idea: http://stackoverflow.com/questions/4495626/making-custom-right-click-context-menus-for-my-web-app?noredirect=1&lq=1

'use strict';

function AddContextMenuToEvent(event, element) {
    // Trigger action when the contexmenu is about to be shown
    //$(element).attr('eventId', event.id);



    $(element).on("contextmenu", function (e) {

        // Avoid the real one
        e.preventDefault();

        $(".custom-menu").attr("eventId", event.id);

        // Show contextmenu in the right position (the mouse)
        $(".custom-menu").finish().toggle(100).
        css({
            top: e.pageY + "px",
            left: e.pageX + "px"
        });
    });


    // If the document is clicked somewhere
    $(document).on("mousedown", function (e) {

        // If the clicked element is not the menu
        if (!$(e.target).parents(".custom-menu").length > 0) {

            // Hide it
            $(".custom-menu").hide(100);
        }
    });
}

// If the menu element is clicked
$(".custom-menu li").click(function () {
    var eventId = $(".custom-menu").attr("eventId");
    var event = $('#calendar').fullCalendar('clientEvents', eventId)[0];

    // This is the triggered action name
    switch ($(this).attr("data-action")) {

        // A case for each action. Your actions here
        case "edit": EditTitle(event); break;
        case "delete": DeleteBooking(event); break;
    }

    // Hide it AFTER the action was triggered
    $(".custom-menu").hide(100);
});

function EditTitle(event) {
    // Cloning the event to be able to perform a rollback if the AJAX call in UpdateBooking fails.
    var formerEvent = jQuery.extend({}, event);

    var title = prompt("Ange en rubrik:");
    if (!title)
        return;

    event.title = title;

    event.details = prompt("Ange en beskrivning (optional)", "");

    $('#calendar').fullCalendar('updateEvent', event);

    var Rollback = function() {
        alert("It was not possible to change the event due to a server error.");
        $('#calendar').fullCalendar('updateEvent', formerEvent)
    }

    UpdateBooking(event, Rollback);
}



