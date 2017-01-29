'use strict';

angular
    .module('myApp')
    .controller('ImageModalCtrl', ImageModalCtrl);

ImageModalCtrl.$inject = ['$rootScope', '$modalInstance', 'studentService', 'dateParsingService', 'image'];

function ImageModalCtrl($rootScope, $modalInstance, studentService, dateParsingService, image) {

    var imageModal = this;
    imageModal.close = $modalInstance.close;
    imageModal.sendValidation = sendValidation;
    imageModal.approved = approved;
    imageModal.denied = denied;

    $modalInstance.opened.then(setImage);

    ////////////////////////////////////

    function setImage() {
        imageModal.image = image;
        imageModal.image.updated = dateParsingService.parse(imageModal.image.updated);
        imageModal.image.created = dateParsingService.parse(imageModal.image.created);
    }

    function sendValidation(id, valid){
        //allow to send validation only if the request applies to the status of the picture. eg validate a previously denied picture
        if ((valid && denied()) || (!valid && approved())){
            imageModal.sendingBackToValidation = true;
            studentService.validateArchived(id, valid).then(function(){
                imageModal.sendingBackToValidation = false;
                if (valid){
                    imageModal.wasValidated = true;
                }
                else if (!valid){
                    imageModal.wasRevoked = true;
                }

                setTimeout(function(){
                    $modalInstance.close();
                    $rootScope.$broadcast("studentModal.updateStudent");
                }, 2000);
            });
        }

    }

    function approved(){
        return (imageModal.image.status == 'Approved');
    }
    function denied(){
        return (imageModal.image.status == 'Denied' || imageModal.image.status == 'Archived');
    }

    function submitComment(){
        studentService.submitComment(imageModal.image.iD_PK, imageModal.image.comments);
    }

}

