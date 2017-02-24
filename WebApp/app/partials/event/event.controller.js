/**
 * Created by franc on 1/30/2017.
 */

'use strict';

angular
    .module('myApp')
    .controller('EventController', EventController);

EventController.$inject = ['$filter', '$uibModal', 'eventService'];

function EventController($filter, $modal, eventService) {



    var eventTab = this;
    eventTab.submit = submit;
    eventTab.modify = modify;
    eventTab.openEventModal = openEventModal;
    eventTab.create = create;
    eventTab.cancel = cancel;
    eventTab.setFilter = setFilter;
    eventTab.isFilterTarget = isFilterTarget;
    eventTab.filters = [
        "All",
        "Cancelled",
        "Postponed",
        "Rescheduled",
        "Scheduled",
        "Open",
        "Closed"
    ];


    eventTab.readonly = true;
    eventTab.removable = false;

    getEvents();

    eventTab.fpOptions = {
        navigation: false,
        keyboardScrolling: false
    };

    ////////////////////////////////////////////////////////////


    function submit(){
        var dateFormat = 'MM-dd-yyyyTHH:mm:ss';
        eventTab.creating.timeBegin = $filter('date')(eventTab.creating.timeBegin, dateFormat);
        eventTab.creating.timeEnd = $filter('date')(eventTab.creating.timeEnd, dateFormat);
        if (eventTab.creating.eventID) eventService.updateEvent(eventTab.creating).then(function(){});
        else eventService.submit(eventTab.creating).then(function(){
            eventTab.fpControls.moveTo(1);
            getEvents();
        });
    }

    function get(eventId) {
        eventService.getThisEvent(eventId).then(function(result) {
                eventTab.selectedEvent = result;
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

    function setFilter(filter){
        eventTab.currentFilter = filter;
    }

    function isFilterTarget(result){
        return !eventTab.currentFilter || (eventTab.currentFilter == "All" || result.information.status == eventTab.currentFilter || result.information.type == eventTab.currentFilter);
    }
};