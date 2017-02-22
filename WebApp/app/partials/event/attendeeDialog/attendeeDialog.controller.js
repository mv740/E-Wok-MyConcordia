'use strict';

angular
    .module('myApp')
    .controller('AttendeeDialogCtrl', AttendeeDialogCtrl);

AttendeeDialogCtrl.$inject = ['$mdDialog', 'attendee', 'eventService'];

function AttendeeDialogCtrl($mdDialog, attendee, eventService) {

    var attendeeDialog = this;
    attendeeDialog.attendee = attendee;
    attendeeDialog.attendeeRoles = ['Creator', 'Mod', 'Scanner', 'Attendee'];
    attendeeDialog.attendeeOriginalRole = attendee.role;


    //////////////////////////


    attendeeDialog.setNewRole = setNewRole();
    attendeeDialog.hide = hide;
    attendeeDialog.cancel = cancel;

    function setNewRole() {
        var attendeeID = attendeeDialog.attendee.id;
        var attendeeNewRole = attendeeDialog.attendee.role;
        eventService.setUserRole({
            userId: attendeeID,
            role: attendeeNewRole
        }).then(function(result) {
            console.log("Successfully updated user role. Status: " + result.status);
        }, function(failure) {
           console.log("Failed to update user role. Status: " + failure.status);
        });
    }

    function hide() {
        $mdDialog.hide();
    }

    function cancel() {
        $mdDialog.cancel();
    };

}

