/**
 * Created by Simon on 2017-02-03.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('EventsController', EventsController);

  EventsController.$inject = ['$rootScope', '$scope', '$state', 'StudentService'];

  function EventsController($rootScope, $scope, $state, StudentService){
    var ev = this;

    ev.seeEventDetails = function(eventid){
      console.log(eventid);//eventid is the index where the clicked event data is
      $state.go('app.barcode');
    };

    $scope.$on('$ionicView.enter', function (e){
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
          ev.value = response.data[0].eventID;
          ev.second = response.data[0];
          ev.events = response.data;

        }, function errorCallback(response){
          console.log(response);
        });
    }

  }


})();
