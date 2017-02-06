(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('AdminController', AdminController);

    AdminController.$inject = ['$http', '$mdToast', 'myConfig', 'studentService', 'dateParsingService', '$filter'];

    function AdminController($http, $mdToast, myConfig, studentService, dateParsingService, $filter) {
        var self = this;
var toast;
        self.fpOptions = {
            navigation: false,
            keyboardScrolling: false,
        };

        setTimeout(function(){
            //destroying
            if (typeof $.fn.fullpage.destroy == 'function') {
                $.fn.fullpage.destroy('all');
            }

//initializing
            $('#fpAdmin').fullpage(admin.fpOptions);
        });


        //having a timeout allows to execute after digest
        setTimeout(function(){
            $.fn.fullpage.setMouseWheelScrolling(false);
            $.fn.fullpage.setAllowScrolling(false);
        })


        self.moveSectionDown = moveSectionDown;
        self.moveSectionUp = moveSectionUp;


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
                var year = value.data.year;
                self.startDate = dateParsingService.parseUpdatePeriod(value.data.startDate);
                self.endDate = dateParsingService.parseUpdatePeriod(value.data.endDate);

                self.currentUpdatePeriod = "Academic Year: " + year
                    + ", from " + self.startDate.month + " " + self.startDate.day + ", " + self.startDate.year
                    + " to " + self.endDate.month + " " + self.endDate.day + ", " + self.endDate.year;
            }
            else {
                self.currentUpdatePeriod = "There is no update period currently set";
            }
            self.loading = false;
        });



        function moveSectionDown(){
            $.fn.fullpage.moveSectionDown();
        }

        function moveSectionUp(){
            $.fn.fullpage.moveSectionUp();
        }

        self.submit = function UpdatePeriod() {


                self.dateStart = $filter('date')(self.dtFrom, 'dd-MM-yyyy');
                self.dateEnd = $filter('date')(self.dtTo, 'dd-MM-yyyy');

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
                        toast = $mdToast.show(
                            $mdToast.simple()
                                .textContent('Working...')
                                .position("bottom right")
                                .hideDelay(0)
                        );
                        $http.post(myConfig.baseUrl + myConfig.picturePeriod, data)
                            .then(function success(response) {
                                $mdToast.hide(toast);
                            }, function failure(response) {
                            });
                    }
                    else {
                        alert("The date range selected is invalid.\nPlease ensure the \"From\" date is before the \"To\" date.");
                        $mdToast.hide(toast);
                    }
                }
                else {
                    $mdToast.hide(toast);
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

        Mousetrap.bind('enter', self.submit);
    }
})();