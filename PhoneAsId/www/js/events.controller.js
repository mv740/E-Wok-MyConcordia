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

    $scope.$on('$ionicView.enter', function (e){
      if (e.targetScope !== $scope) {
        return;
      } else {
        console.log("enter");

        getEventList();
      }
    });


    ev.second = "2nd item"

    function getEventList(){
      StudentService.fetchEvents()
        .then(function successCallback(response){
          ev.value = response;
        }, function errorCallback(response){
          ev.value = response;
          console.log(response);
        });
    }

  }


})();
