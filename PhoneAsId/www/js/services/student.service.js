/**
 * Created by NSPACE on 11/10/2016.
 */

angular.module('starter.services', [])

  .factory('StudentService', ['$http', function ($http) {
    var urlBase = 'https://myconcordiaid.azurewebsites.net/api';
    var StudentService = {};
    StudentService.studentInfo = '';

    /**
     * Rest api call for student information that will be displayed on the ID page
     *
     * @return HttpPromise which will on success contain a JSON with student ID information
     */

    StudentService.fetchStudentIdInfo = function () {
      console.log('fetchStudentIdInfo called');
      return $http.get(urlBase + "/student/account");
    };

    /**
     * Rest api call for marshalling card information that will be displayed on the marshalling card info page
     *
     * @return HttpPromise which will on success contain a JSON with marshalling card info information
     */

    StudentService.fetchMarshallingCardInfo = function () {
      console.log('fetchMarshallingCardInfo called');
      return $http.get(urlBase + "/graduation");
    };

    /**
     * Rest api call for UpdatePeriod that will determined if a user can update his profile picture
     *
     * @return HttpPromise which will on success contain a JSON
     */
    StudentService.fetchUpdatePeriod = function(){
      console.log('fetchUpdatePeriod called');
      return $http.get(urlBase +'/student/UpdatePeriod');
    };

    StudentService.fetchEvents = function(){
      return $http.get(urlBase +'/Event/user');
    };

    return StudentService;
  }]);
