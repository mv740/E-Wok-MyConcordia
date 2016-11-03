/**
 * Created by franc on 10/15/2016.
 */

angular.module('myApp.searchModal.gallery',[])


    .controller('GalleryCtrl', ['$rootScope', '$scope',function($rootScope, $scope) {

        var gallery = $scope;

        $rootScope.$on('gallery.updateGallery', function (event, images) {
            gallery.images = images;
            $rootScope.$broadcast('searchModal.doneLoading');
        });

        var IMAGE_WIDTH = 405;


        gallery.setFullscreen = function(image) {
            $rootScope.$broadcast('imageModal.enlargeImage', image);
        };

    }]);