/**
 * Created by NSPACE on 11/10/2016.
 */

angular.module('starter.services', [])

  .factory('StudentService', ['$http','Settings', function ($http,Settings) {
    var StudentService = {};
    StudentService.studentInfo = '';

    /**
     * Rest api call for student information that will be displayed on the ID page
     *
     * @return HttpPromise which will on success contain a JSON with student ID information
     */

    StudentService.fetchStudentIdInfo = function () {
      console.log('fetchStudentIdInfo called');
      return $http.get(Settings.api + "/student/account");
    };

    /**
     * Rest api call for marshalling card information that will be displayed on the marshalling card info page
     *
     * @return HttpPromise which will on success contain a JSON with marshalling card info information
     */

    StudentService.fetchMarshallingCardInfo = function () {
      console.log('fetchMarshallingCardInfo called');
      return $http.get(Settings.api + "/graduation");
    };

    /**
     * Rest api call for UpdatePeriod that will determined if a user can update his profile picture
     *
     * @return HttpPromise which will on success contain a JSON
     */
    StudentService.fetchUpdatePeriod = function(){
      console.log('fetchUpdatePeriod called');
      return $http.get(Settings.api +'/student/UpdatePeriod');
    };


    /**
     * Rest api call for moderators of events to use to validate registration after scanning barcode ID
     *
     * @return HttpPromise which will say if a user is registered or not
     */
    StudentService.validateEventAttendee = function(userParameter){
      console.log('validateEventAttendee called');
      return $http.post(Settings.api +'/Event/scanner', userParameter);
    };

    StudentService.fetchEvents = function(){
      return $http.get(Settings.api +'/Event/user');
    };

    return StudentService;
  }]);
