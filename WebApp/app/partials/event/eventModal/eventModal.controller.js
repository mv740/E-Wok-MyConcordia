'use strict';

angular
    .module('myApp')
    .controller('EventModalCtrl', EventModalCtrl);

EventModalCtrl.$inject = ['$scope', '$uibModal', '$uibModalInstance', 'eventService', 'event'];

function EventModalCtrl($scope, $modal, $modalInstance, eventService, event) {

    var eventModal = this;

    eventModal.enlargeImage = enlargeImage;
    eventModal.close = $modalInstance.close;
    eventModal.emptyProfilePicture = 'images/empty-profile.png';

    $scope.$on("eventModal.updateEvent", updateEvent);
    $modalInstance.opened.then(updateEvent);

    eventModal.stats = {
        chart: {
            caption: "Administration and attendees statistics",
            subCaption: "",
            numberPrefix: "",
            theme: "zune"
        },
        data: []
    };

    //////////////////////////

    function updateEvent() {
        eventModal.event = event;
        eventService.getStats(eventModal.event.information.eventId).then(function(value){
            eventModal.stats.data = [
                {
                    label: "Mods",
                    value: value.administration.mods
                },
                {
                    label: "Scanners",
                    value: value.administration.scanners
                },
                {
                    label: "Registered",
                    value: value.attendees.registered
                },
                {
                    label: "Attending",
                    value: value.attendees.registered
                },
                {
                    label: "Tracking",
                    value: value.attendees.tracking
                }
            ];

        });

    }

    function enlargeImage(image) {
        $modal.open({templateUrl: "partials/review/modals/imageModal/imageModal.html",
            controller: 'ImageModalCtrl as imageModal',
            windowClass: 'app-modal-window-xl',
            keyboard: true,
            resolve: {
                image: function () {
                    return image;
                }
            }});
    }


}

