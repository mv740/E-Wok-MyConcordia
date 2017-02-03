/**
 * Created by Simon on 2017-02-03.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('EventsController', EventsController);

  EventsController.$inject = ['$rootScope', '$scope', '$state'];

  function EventsController($rootScope, $scope, $state){
    var ev = this;

    ev.value = "1st item";
    ev.second = "2nd item"


  }
})();
