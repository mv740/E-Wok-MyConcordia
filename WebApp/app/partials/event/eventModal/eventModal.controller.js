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
            caption: "Harry's SuperMart",
            subCaption: "Top 5 stores in last month by revenue",
            numberPrefix: "$",
            theme: "ocean"
        },
        data: [{
            label: "Bakersfield Central",
            value: "880000"
        },
            {
                label: "Garden Groove harbour",
                value: "730000"
            },
            {
                label: "Los Angeles Topanga",
                value: "590000"
            },
            {
                label: "Compton-Rancho Dom",
                value: "520000"
            },
            {
                label: "Daly City Serramonte",
                value: "330000"
            }]
    };

    //////////////////////////

    function updateEvent() {
        eventModal.event = event;

        /*eventService.getEventPictures(event.id).then(function (value) {
            eventModal.event.pendingPicture = value.data.pendingPicture;

            var archivedPictures = value.data.archivedPictures;

            if (value.data.profilePicture){
                archivedPictures.unshift(value.data.profilePicture);
            }
            eventModal.event.previousPictures = archivedPictures;

            eventModal.loading = false;
        });*/
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

