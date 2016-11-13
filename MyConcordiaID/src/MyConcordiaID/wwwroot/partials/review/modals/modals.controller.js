'use strict';

angular
    .module('myApp')
    .controller('ModalsCtrl', ModalsCtrl);

ModalsCtrl.$inject = ['$scope', 'studentService'];

function ModalsCtrl($scope, studentService) {

    var modals = this;

    modals.sendValidation = studentService.sendValidation;
    modals.enlargeImage = enlargeImage;
    modals.loading = true;

    $scope.$on('modals.update', function (event, student) {
        updateStudent(student);
    });

    //////////////////////////

    function updateStudent(student) {
        modals.student = student;

        studentService.getStudentPictures(student.id).then(function (value) {
            modals.student.pendingPicture = value.data.pendingpicture;
            //modals.student.previousPictures = value.data.previousPictures;
            modals.student.previousPictures = [value.data.previouspicturE1, value.data.previouspicturE2, value.data.previouspicturE1, value.data.previouspicturE2, value.data.previouspicturE1, value.data.previouspicturE2, value.data.previouspicturE1, value.data.previouspicturE2];
            modals.loading = false;
        });
    }

    function enlargeImage(image) {
        modals.enlargedImage = image;
    }

}

