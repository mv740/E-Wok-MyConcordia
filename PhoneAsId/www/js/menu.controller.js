/**
 * Created by NSPACE on 11/25/2016.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('MenuController', MenuController);

  MenuController.$inject = ['AuthenticationService'];

  function MenuController(AuthenticationService) {
    var vm = this;
    vm.overlay = false;

    vm.onSwipeLeft = onSwipeLeft;
    vm.toggleDrawer = toggleDrawer;
    vm.logOut = logOut;
    vm.feedback = feedback;
    vm.logViewId = logViewId;
    vm.logViewEvents = logViewEvents;
    vm.logViewMarshallingCard = logViewMarshallingCard;
    vm.logUpdatePicture = logUpdatePicture;

    function logOut() {
      AuthenticationService.logOut();
    }

    function toggleDrawer(){
      if (vm.overlay)
      vm.overlay = false;
      else vm.overlay = true;
    }

    function onSwipeLeft(){
      vm.overlay = false;
    }

    //HockeyApp analytic

    function feedback() {
      hockeyapp.feedback();
    }

    function logViewId() {
      hockeyapp.trackEvent(null, null, "VIEW_ID_CARD");
    }

    function logViewMarshallingCard() {
      hockeyapp.trackEvent(success, null, "VIEW_MARSHALLING_CARD");
    }

    function logUpdatePicture() {
      hockeyapp.trackEvent(null, null, "VIEW_UPDATE_PICTURE");
    }

    function logViewEvents() {
      hockeyapp.trackEvent(null, null, "VIEW_EVENTS");
    }
  }
})();
