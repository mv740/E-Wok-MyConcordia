/**
 * Created by franc on 1/30/2017.
 */

'use strict';

angular
    .module('myApp')
    .controller('EventController', EventController);

EventController.$inject = [];

function EventController() {
    var event = this;

    event.fpOptions = {
        navigation: false,
        scrollingSpeed: 1000
    };

};