/**
 * Created by franc on 10/15/2016.
 */

angular
    .module('myApp')
    .controller('SideNavCtrl', SideNavCtrl);


SideNavCtrl.$inject = ['AuthenticationService','$scope'];

function SideNavCtrl(AuthenticationService, $scope) {

    var sideNav = this;

    sideNav.toggleNav = toggleNav;
    sideNav.closeNav = closeNav;
    sideNav.logout = logout;
    sideNav.isLoggedIn = false;

    //////////////////

    $scope.$on('$routeChangeSuccess', function (event, next, current) {
        changeSelectedSideNavIcon(next);
        sideNav.isLoggedIn = AuthenticationService.isAuthenticated();
    });


    function changeSelectedSideNavIcon(next){
        sideNav.isOnReviewPage = false;
        sideNav.isOnAdminPage = false;
        sideNav.isOnEventPage = false;
        if (typeof next.$$route != "undefined") {
            if (next.$$route.originalPath.match("review")) {
                sideNav.isOnReviewPage = true;
            }
            else if (next.$$route.originalPath.match("admin")) {
                sideNav.isOnAdminPage = true;
            }
            else if (next.$$route.originalPath.match("event")) {
                sideNav.isOnEventPage = true;
            }
        }
    }

    function toggleNav() {
        if (!sideNav.hamState) {
            sideNav.closeNav();
        }
        else openNav();
    }

    function closeNav() {
        document.getElementById("mySidenav").style.width = "0";
        sideNav.hamState = false;
        removeOverlay();
    }

    function openNav() {
        addOverlay();
        document.getElementById("mySidenav").style.width = "170px";
    }

    function logout() {
        AuthenticationService.logOut();
    }

    function addOverlay() {
        $('<div class="modal-backdrop" style= "background-color: rgba(0, 0, 0, 0.5); z-index: 1;"></div>').appendTo($("#ng-view")).hide().fadeIn();
    }

    function removeOverlay() {
        $(".modal-backdrop").remove();
    }
}