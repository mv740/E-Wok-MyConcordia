'use strict';

angular
    .module('myApp')
    .controller('AttendeeModalCtrl', AttendeeModalCtrl);

AttendeeModalCtrl.$inject = ['$mdDialog', 'attendee'];

function AttendeeModalCtrl($mdDialog, attendee) {

    var attendeeModal = this;
    attendeeModal.attendee = attendee;
    attendeeModal.attendeeRoles = ['Creator', 'Mod', 'Scanner', 'Attendee'];



    //////////////////////////


    attendeeModal.hide = function() {
        if (attendeeModal.attendee.role == attendee.role)
            $mdDialog.hide();
        else
            $mdDialog.hide(attendeeModal.attendee.role);
    };

    attendeeModal.cancel = function() {
        $mdDialog.cancel();
    };

}

