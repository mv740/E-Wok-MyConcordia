(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('AdminController', AdminController);

    AdminController.$inject = ['$filter', '$translate', '$scope', 'adminService', 'dateParsingService'];

    function AdminController($filter, $translate, $scope, adminService, dateParsingService) {
        var adminTab = this;

        // user isn't allowed to scroll around the various sections unless he or she uses the provided buttons
        adminTab.fpOptions = {
            navigation: false,
            keyboardScrolling: false
        };


        var defaultStartDate = "18-12-2016"; // default value provided by DB
        var defaultEndDate = "24-18-2016";

        //initialize
        adminTab.academicYear = null;
        adminTab.yearEntered = false;
        adminTab.startDateEntered = false;
        adminTab.endDateEntered = false;

        adminTab.loading = true;

        fetchUpdatePeriod(); // fetch and display the latest update period
        resetForm(); // clear whatever data could be lying around in the form

        $scope.$on('adminTab.localizeDate', localizeDate);

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

                if (adminTab.yearEntered && adminTab.startDateEntered && adminTab.endDateEntered) { // this check is performed just in case the user somehow reaches the submit button

                    var validDateRange = adminTab.dtFrom.getTime() < adminTab.dtTo.getTime();

                    if (validDateRange) {
                        adminService.submitUpdatePeriod(updatePeriod)
                            .then(function success(response) {
                                adminTab.fpControls.moveTo(1); // scroll to the top
                                resetForm(); // deletes the form after the data has been successfully sent.
                                fetchUpdatePeriod(); // This is to refresh the update period being displayed
                            }, function failure(response) {
                            });
                    }
                    else {
                        alert("The date range selected is invalid.\nPlease ensure the \"From\" date is before the \"To\" date.");

                    }
                }
        };

        adminTab.setYearEntered = function() {
            adminTab.yearEntered = adminTab.academicYear != null;
        };

        adminTab.setStartDateEntered = function() {
            adminTab.startDateEntered = adminTab.dtFrom != null;
        };

        adminTab.setEndDateEntered = function() {
            adminTab.endDateEntered = adminTab.dtTo != null;
        };

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

        function fetchUpdatePeriod() {
            adminService.getUpdatePeriod().then(function(value) {

                // only parse the dates if a date is set i.e. not set to default
                if (value.startDate != defaultStartDate && value.endDate != defaultEndDate) {
                    var year = value.year;
                    adminTab.fetchedAcademicYear = year + "-" + (year + 1);

                    var localeOptions = {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    };

                    var langKey = $translate.use();

                    // Have 1 attribute to store the actual date object, and one that will be localized and displayed to screen
                    adminTab.startDate = dateParsingService.parseUpdatePeriod(value.startDate);
                    adminTab.startDateLocalized = adminTab.startDate.toLocaleString(langKey + "-CA", localeOptions);

                    // Have 1 attribute to store the actual date object, and one that will be localized and displayed to screen
                    adminTab.endDate = dateParsingService.parseUpdatePeriod(value.endDate);
                    adminTab.endDateLocalized = adminTab.endDate.toLocaleString(langKey + "-CA", localeOptions);


                }
                else {
                    adminTab.currentUpdatePeriod = "There is no update period currently set";
                }
                adminTab.loading = false;
            });
        }

        function resetForm() {
            adminTab.academicYear = "";
            adminTab.dtFrom = "";
            adminTab.dtTo = "";
            adminTab.currentUpdatePeriod = "";
        }

        function localizeDate() {
            if (adminTab.startDate != undefined && adminTab.endDate != undefined) {
                var localeOptions = {
                    year: 'numeric',
                    month: 'long',
                    day: 'numeric'
                };

                var langKey = $translate.use();
                adminTab.startDateLocalized = adminTab.startDate
                    .toLocaleString(langKey + "-CA", localeOptions);

                adminTab.endDateLocalized = adminTab.endDate
                    .toLocaleString(langKey + "-CA", localeOptions);
            }
        }
        Mousetrap.bind('enter', adminTab.submit);
    }
})();