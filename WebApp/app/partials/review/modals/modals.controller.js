'use strict';

angular
    .module('myApp')
    .controller('ModalsCtrl', ModalsCtrl);

ModalsCtrl.$inject = ['$scope', 'studentService'];

function ModalsCtrl($scope, studentService) {

    var modals = this;

    modals.sendValidation = sendValidation;
    modals.enlargeImage = enlargeImage;
    modals.loading = true;
    modals.emptyProfilePicture = 'images/empty-profile.png';

    $scope.$on('modals.update', function (event, student) {
        updateStudent(student);
    });

    //////////////////////////

    function sendValidation(id, valid){
        modals.student.sendingValidation = true;
        studentService.sendValidation(id,valid).then(function(){
            modals.student.sendingValidation = false;
            if (valid) modals.student.wasValidated = true;
            else if (!valid) modals.student.wasRevoked = true;
        });
    }

    function updateStudent(student) {
        modals.student = student;

        studentService.getStudentPictures(student.id).then(function (value) {
            modals.student.pendingPicture = value.data.pendingpicture;
            modals.student.previousPictures = value.data.previousPictures;

            modals.loading = false;
        });
    }

    function enlargeImage(image) {
        modals.enlargedImage = image;
    }

}

