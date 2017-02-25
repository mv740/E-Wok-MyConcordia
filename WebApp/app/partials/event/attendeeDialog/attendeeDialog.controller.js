'use strict';

angular
    .module('myApp')
    .controller('AttendeeDialogCtrl', AttendeeDialogCtrl);

AttendeeDialogCtrl.$inject = ['$mdDialog', 'attendee', 'loggedInAttendee', 'eventService'];

function AttendeeDialogCtrl($mdDialog, attendee, loggedInAttendee, eventService) {

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
    attendeeDialog.attendeeOriginalRole = attendeeCopy.role;


    //////////////////////////


    attendeeDialog.setNewRole = setNewRole;
    attendeeDialog.hide = hide;
    attendeeDialog.cancel = cancel;
    attendeeDialog.userCanModifyRole = userCanModifyRole;
    attendeeDialog.getListOfSettableRoles = getListOfSettableRoles();

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
    }

    function userCanModifyRole(attendee) {
        if (loggedInAttendee.id != attendee.id) { // User can't modify his or her own role
            if (loggedInAttendee.role == "Creator") // The Creator can modify any attendee's role except his or hers.
                return true;
            else if (loggedInAttendee.role == "Mod") // The Mod can modify the roles of Scanners and Attendees only.
                return attendee.role != "Mod" && attendee.role != "Creator";
            else // Scanners and Attendees cannot modify roles
                return false;
        }
        else
            return false;
    }

    function getListOfSettableRoles() {
        if (loggedInAttendee.role == "Creator") // The Creator can modify any attendee's role except his or hers.
            attendeeDialog.listOfSettableRoles = ['Mod', 'Scanner', 'Attendee'];
        else if (loggedInAttendee.role == "Mod")
            attendeeDialog.listOfSettableRoles = ['Scanner', 'Attendee'];
    }
}

