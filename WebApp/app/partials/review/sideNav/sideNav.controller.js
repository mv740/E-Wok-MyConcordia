/**
 * Created by franc on 10/15/2016.
 */

angular
    .module('myApp')
    .controller('SideNavCtrl', SideNavCtrl);

function SideNavCtrl() {

    var sideNav = this;

    sideNav.toggleNav = toggleNav;
    sideNav.closeNav = closeNav;

    //////////////////

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

    function addOverlay() {
        $('<div class="modal-backdrop" style= "background-color: rgba(0, 0, 0, 0.5); z-index: 1;"></div>').appendTo($("#viewport")).hide().fadeIn();
    }

    function removeOverlay() {
        $(".modal-backdrop").remove();
    }
}