/**
 * Created by Simon on 2017-02-03.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('EventsController', EventsController);

  EventsController.$inject = ['$rootScope', '$scope', '$state', 'StudentService', 'EventService'];

  function EventsController($rootScope, $scope, $state, StudentService, EventService){
    var ev = this;

    ev.seeEventDetails = function(eventid){
      console.log(eventid);//eventid is the index where the clicked event data is
      EventService.data = ev.events[eventid];
      $state.go('app.barcode');
    };

    $scope.$on('$ionicView.beforeEnter', function (e){
      if (e.targetScope !== $scope) {
        return;
      } else {
        console.log("enter");

        getEventList();

      }
    });


    function getEventList(){
      StudentService.fetchEvents()
        .then(function successCallback(response){
          ev.events = response.data;

        }, function errorCallback(response){
          console.log(response);
        });
    }

  }


})();
