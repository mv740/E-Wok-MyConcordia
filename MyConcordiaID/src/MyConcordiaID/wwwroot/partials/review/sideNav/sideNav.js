/**
 * Created by franc on 10/15/2016.
 */

angular.module('myApp.sideNav',[])


    .controller('SideNavCtrl', ['$scope', function($scope) {

        var sideNav = $scope;

        sideNav.toggleNav = function () {
            if (!sideNav.hamState) {
                sideNav.closeNav();
            }
            else openNav();
        };

        
        sideNav.closeNav = function () {
            document.getElementById("mySidenav").style.width = "0";
            sideNav.hamState = false;
        };

        function openNav() {
            document.getElementById("mySidenav").style.width = "250px";
        }

    }]);