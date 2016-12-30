'use strict';

angular
    .module('myApp')
    .controller('ImageModalCtrl', ImageModalCtrl);

ImageModalCtrl.$inject = ['$modalInstance', 'image'];

function ImageModalCtrl($modalInstance, image) {

    var imageModal = this;
    imageModal.close = $modalInstance.close;

    $modalInstance.opened.then(setImage);

    ////////////////////////////////////

    function setImage() {
        imageModal.image = image;
    }

}

