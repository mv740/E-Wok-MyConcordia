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

    //////////////////

    $scope.$on('$routeChangeStart', function (event, next, current) {

        sideNav.isOnReviewPage = false;
        sideNav.isOnAdminPage = false;

        if (next.$$route.originalPath.match("review")){
           sideNav.isOnReviewPage = true;
        }
        else if(next.$$route.originalPath.match("admin")){
            sideNav.isOnAdminPage = true;
        }
    });

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