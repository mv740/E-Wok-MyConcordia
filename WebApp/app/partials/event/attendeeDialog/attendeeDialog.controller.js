'use strict';

angular
    .module('myApp')
    .controller('AttendeeDialogCtrl', AttendeeDialogCtrl);

AttendeeDialogCtrl.$inject = ['$mdDialog', 'attendee', 'eventService'];

function AttendeeDialogCtrl($mdDialog, attendee, eventService) {

    var attendeeDialog = this;

    // Required to make a deep copy otherwise, manipulations on the passed attendee will be by reference and not by value.
    var attendeeCopy = {
        id: attendee.id,
        role: attendee.role,
        status: attendee.status,
        studentAccount: {
            netName: attendee.studentAccount.netName,
            id: attendee.studentAccount.id,
            firstName: attendee.studentAccount.firstName,
            lastName: attendee.studentAccount.lastName
        }
    };

    attendeeDialog.attendee = attendeeCopy;
    attendeeDialog.attendeeRoles = ['Creator', 'Mod', 'Scanner', 'Attendee'];
    attendeeDialog.attendeeOriginalRole = attendeeCopy.role;


    //////////////////////////


    attendeeDialog.setNewRole = setNewRole;
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

