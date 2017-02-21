'use strict';

angular
    .module('myApp')
    .controller('AttendeeModalCtrl', AttendeeModalCtrl);

AttendeeModalCtrl.$inject = ['$scope', '$modal', '$modalInstance', 'attendee'];

function AttendeeModalCtrl($scope, $modal, $modalInstance, attendee) {

    var attendeeModal = this;
    var attendeeRoles = ['Creator', 'Mod', 'Scanner', 'Attendee'];

    attendeeModal.close = $modalInstance.close;

    $scope.$on("attendeeModal.updateAttendee", updateAttendee);
    $modalInstance.opened.then(updateAttendee);

    //////////////////////////

    function updateAttendee() {
        attendeeModal.attendee = attendee;
    }



}

