﻿(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('AdminController', AdminController);

    AdminController.$inject = ['$http', 'myConfig', 'studentService', 'dateParsingService', '$filter'];

    function AdminController($http, myConfig, studentService, dateParsingService, $filter) {
        var self = this;

        var defaultStartDate = "18-12-2016";
        var defaultEndDate = "24-18-2016";
        var lengthOfAYearString = 4; // number of characters for a year to be valid

        self.academicYear = null;
        self.yearEntered = false;
        self.startDateEntered = false;
        self.endDateEntered = false;

        self.loading = true;

        studentService.getUpdatePeriod().then(function(value) {

            self.dataObtained = value.data;

            // only parse the dates if a date is set i.e. not set to default
            if (value.data.startDate != defaultStartDate && value.data.endDate != defaultEndDate) {
                var year = $filter('date')(value.data.year, 'yyyy');
                self.startDate = $filter('date')(value.data.startDate, self.format);
                self.endDate = $filter('date')(value.data.endDate, self.format);

                self.currentUpdatePeriod = "Academic Year: " + year
                    + ", from " + self.startDate.month + " " + self.startDate.day + ", " + self.startDate.year
                    + " to " + self.endDate.month + " " + self.endDate.day + ", " + self.endDate.year;
            }
            else {
                self.currentUpdatePeriod = "There is no update period currently set";
            }
            self.loading = false;
        });

        self.submitButton = "Submit";

        self.format = 'MMMM d, yyyy';

        self.submit = function UpdatePeriod() {
            self.submitButton = "Checking...";

            self.dateStart = $filter('date')(self.dtFrom, 'dd-MM-yyyy');
            self.dateEnd = $filter('date')(self.dtTo, 'dd-MM-yyyy');

            var data =
                {
                    "year": self.academicYear,
                    "startDate": self.dateStart,
                    "endDate": self.dateEnd
                };


            self.yearEntered = self.academicYear != "Academic Year" && self.academicYear.length == lengthOfAYearString;
            self.startDateEntered = self.dtFrom != undefined;
            self.endDateEntered = self.dtTo != undefined;

            if (self.yearEntered && self.startDateEntered && self.endDateEntered) {
                self.submitButton = "Sending...";
                $http.post(myConfig.baseUrl + myConfig.picturePeriod, data)
                    .then(function success(response) {
                        self.submitButton = "Submit";
                    }, function failure(response) {
                    });
            }
            else {
                self.submitButton = "Submit";
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
            self.academicYearOptions[i] = currentYear + i;
        }
    }
})();