/**
 * Created by franc on 10/15/2016.
 */

angular.module('myApp.searchModal.gallery',[])


    .controller('GalleryCtrl', ['$scope',function($scope) {

        var IMAGE_WIDTH = 405;
        $scope.gallery = {
            IMAGE_LOCATION : "images/",
            data : [
                '1.jpg',
                '2.jpg'
            ],
            fullscreen : '1.jpg'
        };

        // Retrieve and set data
       /* DataSource.get("images.json",function(data) {
            $scope.galleryData = data;
            $scope.selected = data[0];
        });*/

        // Scroll to appropriate position based on image index and width
        $scope.setFullscreen = function(image) {
            $scope.gallery.fullscreen = image;
            $scope.$apply();
        };

    }]);