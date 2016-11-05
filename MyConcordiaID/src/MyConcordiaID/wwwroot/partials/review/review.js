'use strict';

angular.module('myApp.review', ['ngRoute', 'angularCSS'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/review', {
    templateUrl: 'partials/review/review.html',
    controller: 'ReviewCtrl',
    css: [
        'partials/review/review.css',
        'partials/review/searchBox/searchBox.css',
        'partials/review/searchResults/searchResults.css',
        'partials/review/searchModal/searchModal.css',
        'partials/review/searchModal/gallery/gallery.css',
        'partials/review/imageModal/imageModal.css',
        'partials/review/sideNav/sideNav.css'
        ]
  });
}])

.controller('ReviewCtrl', [function() {

}]);