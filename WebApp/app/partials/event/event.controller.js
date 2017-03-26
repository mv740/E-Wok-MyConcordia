/**
 * Created by franc on 1/30/2017.
 */

'use strict';

angular
    .module('myApp')
    .controller('EventController', EventController);


EventController.$inject = ['$filter', '$uibModal', '$timeout', '$mdDialog', '$translate', 'eventService'];

function EventController($filter, $modal, $timeout, $mdDialog, $translate, eventService) {



    var eventTab = this;
    eventTab.submit = submit;
    eventTab.modify = modify;
    eventTab.openEventModal = openEventModal;
    eventTab.create = create;
    eventTab.cancel = cancel;
    eventTab.checkAttendees = checkAttendees;
    eventTab.openAttendeeDialog = openAttendeeDialog;
    eventTab.addUser = addUser;
    eventTab.selectThisEvent = selectThisEvent;
    eventTab.setFilter = setFilter;
    eventTab.isFilterTarget = isFilterTarget;
    eventTab.updateStatistics = updateStatistics;
    eventTab.localize = localize;
    eventTab.filters = [
        {
            value: "All",
            translated: "PARTIALS.EVENT.EVENT.CHIPS.ALL"
        },
        {
            value: "Cancelled",
            translated: "PARTIALS.EVENT.EVENT.CHIPS.CANCELLED"
        },
        {
            value: "Postponed",
            translated: "PARTIALS.EVENT.EVENT.CHIPS.POSTPONED"
        },
        {
            value: "Rescheduled",
            translated: "PARTIALS.EVENT.EVENT.CHIPS.RESCHEDULED"
        },
        {
            value: "Scheduled",
            translated: "PARTIALS.EVENT.EVENT.CHIPS.SCHEDULED"
        },
        {
            value: "Open",
            translated: "PARTIALS.EVENT.EVENT.CHIPS.OPEN"
        },
        {
            value: "Closed",
            translated: "PARTIALS.EVENT.EVENT.CHIPS.CLOSED"
        }
    ];


    eventTab.readonly = true;
    eventTab.removable = false;
    eventTab.eventTypes = [
        {
            value: 'Open',
            translated: "PARTIALS.EVENT.EVENT.CHIPS.OPEN"
        },
        {
            value: 'Closed',
            translated: "PARTIALS.EVENT.EVENT.CHIPS.CLOSED"
        }];

    getEvents();

    eventTab.fpOptions = {
        navigation: false,
        keyboardScrolling: false
    };

    eventTab.stats = {
        chart: {
            caption: "Administration and attendees statistics",
            subCaption: "",
            numberPrefix: "",
            theme: "zune"
        },
        data: []
    };

    eventTab.pie = {
        chart: {
            caption: "Percentage of people who attended",
            //subcaption: "",
            startingangle: "120",
            //showlabels: "0",
            //showlegend: "0",
            enablemultislicing: "0",
            slicingdistance: "15",
            showpercentvalues: "1",
            showpercentintooltip: "0",
            //plottooltext: "Age group : $label Total visit : $datavalue",
            theme: "fint"
        },
        data: []
    };

    ////////////////////////////////////////////////////////////

    function updateStatistics() {
        eventService.getStats(eventTab.selectedEvent.information.eventId).then(function(value){
            eventTab.stats.data = [
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
                    value: value.attendees.attending
                },
                {
                    label: "Tracking",
                    value: value.attendees.tracking
                }
            ];

            var registered = value.attendees.registered - value.administration.mods - value.administration.scanners - value.administration.creator;
            eventTab.pie.data = [
                {
                    label: "Attending",
                    value: value.attendees.attending
                },
                {
                    label: "Did not come",
                    value: registered
                }
            ];
        });

    }

    function submit(){
        if (eventTab.creating.eventID) eventService.updateEvent(eventTab.creating).then(function(){});
        else eventService.submit(eventTab.creating).then(function(){
            eventTab.fpControls.moveTo(1);
            getEvents();
        });
    }

    function getEvents() {
        eventService.getAllEvents().then(function(result) {
            eventTab.events = result;
        });
    }

    function create(){
        eventTab.creating = {};
    }

    function modify(eventTarget) {
        eventTab.creating = eventTarget.information;
    }

    function cancel(eventTarget){
        eventService.cancelEvent(eventTarget.information).then(function(){
            getEvents();
        });
    }

    function openEventModal(eventTarget){
        $modal.open({templateUrl: "partials/event/eventModal/eventModal.html",
            controller: 'EventModalCtrl as eventModal',
            windowClass: 'app-modal-window',
            keyboard: true,
            resolve: {
                event: function () {
                    return eventTarget;
                }
            }});
    }

    function checkAttendees(eventTarget) {
        eventTab.attendees = [];
        eventService.getEventAttendees(eventTarget.information.eventId).then(function (result) {
            eventTab.attendees = result;
            eventTab.loggedInAttendee = eventTab.attendees[0];
        });
    }

    function openAttendeeDialog(attendeeTarget) {
        $mdDialog.show({
            controller: 'AttendeeDialogCtrl as attendeeDialog',
            templateUrl: "partials/event/attendeeDialog/attendeeDialog.html",
            parent: angular.element(document.body),
            targetEvent: attendeeTarget,
            clickOutsideToClose:true,
            locals: { attendee: attendeeTarget, loggedInAttendee: eventTab.loggedInAttendee },
            openFrom: { top: -50, width: 30, height: 80 },
            closeTo: { left: 1500 },
            preserveScope: false
        })
            .then(function(answer) {
                console.log("dialog closed. answer received. attendees refreshed");
                checkAttendees(eventTab.selectedEvent);
            }, function() { // When dialog box closes without any action, nothing happens.
                console.log("dialog closed. no actions performed");
            });
    }

    function addUser() {
        var userNetnameOrId = eventTab.newUserNetnameOrId;
        if (userNetnameOrId != "") {
            var newUser = {
                userId: "",
                userNetname: "",
                role: "Attendee",
                eventID: eventTab.selectedEvent.information.eventId
            };
            if (userNetnameOrId.match(/^[0-9]*$/g) != null) {
                newUser.userId = eventTab.newUserNetnameOrId;
            }
            else if (userNetnameOrId.match(/^[a-zA-Z]*_?[a-zA-Z]*$/g) != null){
                newUser.userNetname = eventTab.newUserNetnameOrId;
            }
            eventService.addUser(newUser)
                .then(
                    function(result) {
                        console.log("successfully added new user");
                        eventTab.newUserNetnameOrId = "";
                        checkAttendees(eventTab.selectedEvent);
                    },
                    function(failure) {
                        console.log("failed to add new user");
                    }
                );
        }
    }

    function selectThisEvent(eventTarget) {
        eventTab.selectedEvent = eventTarget;
    }
  
    function setFilter(filter){
        eventTab.currentFilter = filter;
    }

    function isFilterTarget(result){
        var isCancelledAndTargetted = eventTab.currentFilter == "Cancelled" && result.information.status == "Cancelled";

        var noCurrentFilterAndNotCancelled = !eventTab.currentFilter && result.information.status != "Cancelled";
        var eventTargettedByFilter = eventTab.currentFilter == "All" || (result.information.status == eventTab.currentFilter || result.information.type == eventTab.currentFilter);
        var eventTargettedByFilterButIsNotCancelled = result.information.status != "Cancelled" && eventTargettedByFilter;
        return noCurrentFilterAndNotCancelled || ( isCancelledAndTargetted || eventTargettedByFilterButIsNotCancelled);
    }

    function localize(langKey) {
        $translate.use(langKey);
        eventTab.frenchSelected = langKey == 'fr';
    }
    Mousetrap.bind('enter', eventTab.addUser);
}
