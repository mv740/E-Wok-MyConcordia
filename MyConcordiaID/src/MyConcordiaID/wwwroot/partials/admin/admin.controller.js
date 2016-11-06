(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('AdminController', AdminController);

    AdminController.$inject = ['$http', 'myConfig'];

    function AdminController($http, myConfig) {
        self = this;

        self.submitButton = "Submit"
        self.someProp = 'Check This value displays.. confirms controller initalised'
        self.opened = {};
        self.open = function ($event) {

            $event.preventDefault();
            $event.stopPropagation();

            self.opened = {};
            self.opened[$event.target.id] = true;


        };

        self.format = 'dd-MM-yyyy'


        self.submit = function UpdatePeriod() {
            self.submitButton = "Sending..."

            var startDate = new Date(self.dtFrom);
            var endDate = new Date(self.dtTo);

            var startString = startDate.toDateString();
            var lengthStart = startString.length;

            var endString = endDate.toDateString();
            var lengthEnd = endString.length;

            var dateStart = startString.substring(4, lengthStart);
            var dateEnd = endString.substring(4, lengthEnd);

            var data =
                {
                    "year": self.academicYear,
                    "startDate": dateStart,
                    "endDate": dateEnd,
                };

            $http.post(myConfig.baseUrl + myConfig.picturePeriod, data)
                .then(function success(response)
                {
                    self.submitButton = "Submit"
                }, function failure(response) {
            });

        }



    }
})();