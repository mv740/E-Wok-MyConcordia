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


    attendeeDialog.attendeeRoleValueTranslationMapping = {
        value: attendeeDialog.attendee.role,
        translated: "PARTIALS.EVENT.ATTENDEE.ROLE." + (attendeeDialog.attendee.role).toUpperCase()
    };

    //////////////////////////


    attendeeDialog.setNewRole = setNewRole;
    attendeeDialog.answer = answer;
    attendeeDialog.hide = hide;
    attendeeDialog.cancel = cancel;
    attendeeDialog.userCanModifyRole = userCanModifyRole;
    attendeeDialog.getListOfSettableRoles = getListOfSettableRoles();
    attendeeDialog.deleteAttendee = deleteAttendee;

    function setNewRole() {
        var attendeeID = attendeeDialog.attendee.id;
        var attendeeNewRole = attendeeDialog.attendee.role;
        eventService.setUserRole({
            userId: attendeeID,
            role: attendeeNewRole
        }).then(function(result) {
            console.log("Successfully updated user role");
            attendeeDialog.answer(true);
        }, function(failure) {
            console.log("Failed to update user role");
        });
    }

    function answer(answer) {
        $mdDialog.hide(answer);
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
            attendeeDialog.listOfSettableRoles = [
                {
                    value: 'Mod',
                    translated: "PARTIALS.EVENT.ATTENDEE.DIALOG.ROLE.MOD"
                },
                {
                    value: 'Scanner',
                    translated: "PARTIALS.EVENT.ATTENDEE.DIALOG.ROLE.SCANNER"
                },
                {
                    value: 'Attendee',
                    translated: "PARTIALS.EVENT.ATTENDEE.DIALOG.ROLE.ATTENDEE"
                }];
        else if (loggedInAttendee.role == "Mod")
            attendeeDialog.listOfSettableRoles = [
                {
                    value: 'Scanner',
                    translated: "PARTIALS.EVENT.ATTENDEE.DIALOG.ROLE.SCANNER"
                },
                {
                    value: 'Attendee',
                    translated: "PARTIALS.EVENT.ATTENDEE.DIALOG.ROLE.ATTENDEE"
                }];
    }

    function deleteAttendee() {
        var attendeeID = attendeeDialog.attendee.id;
        var attendeeRole = attendeeDialog.attendee.role;
        eventService.deleteUser({
            userId: attendeeID,
            role: attendeeRole
        }).then(function (result) {
            console.log("Successfully deleted user");
            attendeeDialog.answer(true);
        },
        function (failure) {
            console.log("Failed to delete user");
        });
    }
}

