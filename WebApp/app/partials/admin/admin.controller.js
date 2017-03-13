(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('AdminController', AdminController);

    AdminController.$inject = ['adminService', 'dateParsingService', '$filter'];

    function AdminController(adminService, dateParsingService, $filter) {
        var adminTab = this;

        adminTab.fpOptions = {
            navigation: false,
            keyboardScrolling: false,
        };


        var defaultStartDate = "18-12-2016";
        var defaultEndDate = "24-18-2016";
        var lengthOfAYearString = 4; // number of characters for a year to be valid

        adminTab.academicYear = null;
        adminTab.yearEntered = false;
        adminTab.startDateEntered = false;
        adminTab.endDateEntered = false;

        adminTab.loading = true;

        adminService.getUpdatePeriod().then(function(value) {


            // only parse the dates if a date is set i.e. not set to default
            if (value.startDate != defaultStartDate && value.endDate != defaultEndDate) {
                var year = value.year;
                adminTab.startDate = dateParsingService.parseUpdatePeriod(value.startDate);
                adminTab.endDate = dateParsingService.parseUpdatePeriod(value.endDate);

                adminTab.currentUpdatePeriod = "Academic Year: " + year + "-" + (year + 1)
                    + ", from " + adminTab.startDate.month + " " + adminTab.startDate.day + ", " + adminTab.startDate.year
                    + " to " + adminTab.endDate.month + " " + adminTab.endDate.day + ", " + adminTab.endDate.year;
            }
            else {
                adminTab.currentUpdatePeriod = "There is no update period currently set";
            }
            adminTab.loading = false;
        });


        adminTab.submit = function UpdatePeriod() {
                var dateFormat = 'MM-dd-yyyyTHH:mm:ss';
                adminTab.startDate = $filter('date')(adminTab.dtFrom, dateFormat);
                adminTab.endDate = $filter('date')(adminTab.dtTo, dateFormat);

                var updatePeriod = {
                        "year": adminTab.academicYear,
                        "startDate": adminTab.startDate,
                        "endDate": adminTab.endDate
                    };


                adminTab.yearEntered = adminTab.academicYear != null;
                adminTab.startDateEntered = adminTab.dtFrom != undefined;
                adminTab.endDateEntered = adminTab.dtTo != undefined;

                if (adminTab.yearEntered && adminTab.startDateEntered && adminTab.endDateEntered) {

                    var validDateRange = adminTab.dtFrom.getTime() < adminTab.dtTo.getTime();

                    if (validDateRange) {
                        adminService.submitUpdatePeriod(updatePeriod)
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

        adminTab.setYearEntered = function() {
            adminTab.yearEntered = adminTab.academicYear != null;
        };

        adminTab.setStartDateEntered = function() {
            adminTab.startDateEntered = adminTab.dtFrom != null;
        }

        adminTab.setEndDateEntered = function() {
            adminTab.endDateEntered = adminTab.dtTo != null;
        }

        var currentDate = new Date();
        var currentYear = currentDate.getFullYear();

        adminTab.academicYearOptions = [];
        for (var i = 0; i < 5; i++) {
            adminTab.academicYearOptions[i] = currentYear + i - 1;
            /*
             Calculated this way so that the academic periods include at least the current academic year. For academic
             year 2016-2017, if we are in 2017, it will display at least 2017 + 0 - 1 = 2016. In 2016, it will display
             2016 + 0 - 1 = 2015.
             */
        }

        Mousetrap.bind('enter', adminTab.submit);
    }
})();