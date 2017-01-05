'use strict';

angular
    .module('myApp')
    .controller('ImageModalCtrl', ImageModalCtrl);

ImageModalCtrl.$inject = ['$modalInstance', 'studentService', 'image'];

function ImageModalCtrl($modalInstance, studentService, image) {

    var imageModal = this;
    imageModal.close = $modalInstance.close;
    imageModal.sendPictureBackToValidation = sendPictureBackToValidation;

    $modalInstance.opened.then(setImage);

    ////////////////////////////////////

    function setImage() {
        imageModal.image = image;
    }

    function sendPictureBackToValidation(){
       studentService.sendPictureBackToValidation(imageModal.image.id, imageModal.image.picture).then(function(){
           //success
       });
    }

}

