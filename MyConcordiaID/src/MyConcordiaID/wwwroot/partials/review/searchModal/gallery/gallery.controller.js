/**
 * Created by franc on 10/15/2016.
 */

angular
    .module('myApp')
    .controller('GalleryCtrl', ['$rootScope',function($rootScope) {

        var gallery = this;

        $rootScope.$on('gallery.updateGallery', function (event, images) {
            gallery.images = images;
            $rootScope.$broadcast('searchModal.doneLoading');
        });

        var IMAGE_WIDTH = 405;


        gallery.setFullscreen = function(image) {
            $rootScope.$broadcast('imageModal.enlargeImage', image);
        };

    }]);