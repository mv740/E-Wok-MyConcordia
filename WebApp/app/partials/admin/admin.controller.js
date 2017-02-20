(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('AdminController', AdminController);

    AdminController.$inject = ['$http', 'myConfig', 'studentService', 'dateParsingService', '$filter'];

    function AdminController($http, myConfig, studentService, dateParsingService, $filter) {
        var self = this;

        self.fpOptions = {
            navigation: false,
            keyboardScrolling: false,
        };


        var defaultStartDate = "18-12-2016";
        var defaultEndDate = "24-18-2016";
        var lengthOfAYearString = 4; // number of characters for a year to be valid

        self.academicYear = null;
        self.yearEntered = false;
        self.startDateEntered = false;
        self.endDateEntered = false;

        self.loading = true;

        studentService.getUpdatePeriod().then(function(value) {

            self.dataObtained = value;

            // only parse the dates if a date is set i.e. not set to default
            if (value.startDate != defaultStartDate && value.endDate != defaultEndDate) {
                var year = value.year;
                self.startDate = dateParsingService.parseUpdatePeriod(value.startDate);
                self.endDate = dateParsingService.parseUpdatePeriod(value.endDate);

                self.currentUpdatePeriod = "Academic Year: " + year
                    + ", from " + self.startDate.month + " " + self.startDate.day + ", " + self.startDate.year
                    + " to " + self.endDate.month + " " + self.endDate.day + ", " + self.endDate.year;
            }
            else {
                self.currentUpdatePeriod = "There is no update period currently set";
            }
            self.loading = false;
        });


        self.submit = function UpdatePeriod() {
                var dateFormat = 'MM-dd-yyyy';

                self.dateStart = $filter('date')(self.dtFrom, dateFormat);
                self.dateEnd = $filter('date')(self.dtTo, dateFormat);

                var data =
                    {
                        "year": self.academicYear,
                        "startDate": self.dateStart,
                        "endDate": self.dateEnd
                    };


                self.yearEntered = self.academicYear != null;
                self.startDateEntered = self.dtFrom != undefined;
                self.endDateEntered = self.dtTo != undefined;

                if (self.yearEntered && self.startDateEntered && self.endDateEntered) {

                    var validDateRange = self.dtFrom.getTime() < self.dtTo.getTime();

                    if (validDateRange) {
                        $http.post(myConfig.baseUrl + myConfig.picturePeriod, data)
                            .then(function success(response) {

                            }, function failure(response) {
                            });
                    }
                    else {
                        alert("The date range selected is invalid.\nPlease ensure the \"From\" date is before the \"To\" date.");

                    }
                }
                else {

                }
        };

        self.setYearEntered = function() {
            self.yearEntered = self.academicYear != null;
        };

        self.setStartDateEntered = function() {
            self.startDateEntered = self.dtFrom != null;
        }

        self.setEndDateEntered = function() {
            self.endDateEntered = self.dtTo != null;
        }

        var currentDate = new Date();
        var currentYear = currentDate.getFullYear();

        self.academicYearOptions = [];
        for (var i = 0; i < 5; i++) {
            self.academicYearOptions[i] = currentYear + i - 1;
            /*
             Calculated this way so that the academic periods include at least the current academic year. For academic
             year 2016-2017, if we are in 2017, it will display at least 2017 + 0 - 1 = 2016. In 2016, it will display
             2016 + 0 - 1 = 2015.
             */
        }

        Mousetrap.bind('enter', self.submit);
    }
})();