/**
 * Created by Simon on 2017-02-06.
 */

angular.module('starter.services')
  .factory('EventService',function(){

    var EventService = {};

    EventService.addEvent = function(event){
      EventService.data = event;
    };

    return EventService;
  });
