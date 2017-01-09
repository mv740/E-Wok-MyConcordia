'use strict';

angular
    .module('myApp')
    .controller('ImageModalCtrl', ImageModalCtrl);

ImageModalCtrl.$inject = ['$rootscope', '$modalInstance', 'studentService', 'image'];

function ImageModalCtrl($rootscope, $modalInstance, studentService, image) {

    var imageModal = this;
    imageModal.close = $modalInstance.close;
    imageModal.sendPictureBackToValidation = sendPictureBackToValidation;

    $modalInstance.opened.then(setImage);

    ////////////////////////////////////

    function setImage() {
        imageModal.image = image;
    }

    function sendPictureBackToValidation(){
        imageModal.sendingBackToValidation = true;
       studentService.sendPictureBackToValidation(imageModal.image.id, imageModal.image.picture).then(function(){
           //succes
           imageModal.sendingBackToValidation = false;
           $rootscope.$broadcast("StudentModal.updateStudent");
           $modalInstance.close();

       });
    }

}

