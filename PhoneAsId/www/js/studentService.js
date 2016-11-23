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
      console.log('getStudentInfo called');
      return $http.get(urlBase + "/account");
    };

    return StudentService;
  }]);
