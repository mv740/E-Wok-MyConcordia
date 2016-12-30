'use strict';

angular
    .module('myApp')
    .controller('StudentModalCtrl', StudentModalCtrl);

StudentModalCtrl.$inject = ['$modal', '$modalInstance', 'studentService' , 'student'];

function StudentModalCtrl($modal, $modalInstance, studentService, student) {

    var studentModal = this;

    studentModal.sendValidation = sendValidation;
    studentModal.enlargeImage = enlargeImage;
    studentModal.loadLogs = loadLogs;
    studentModal.close = $modalInstance.close;
    studentModal.loading = true;
    studentModal.emptyProfilePicture = 'images/empty-profile.png';

    $modalInstance.opened.then(updateStudent);
    $modalInstance.result.then(resetModal);

    //////////////////////////

    function sendValidation(id, valid){
        studentModal.student.sendingValidation = true;
        studentService.sendValidation(id,valid).then(function(){
            studentModal.student.sendingValidation = false;
            if (valid) studentModal.student.wasValidated = true;
            else if (!valid) studentModal.student.wasRevoked = true;
        });
    }

    function updateStudent() {
        studentModal.student = student;

        studentService.getStudentPictures(student.id).then(function (value) {
            studentModal.student.pendingPicture = value.data.pendingpicture;
            studentModal.student.previousPictures = value.data.previousPictures;

            studentModal.loading = false;
        });
    }

    function enlargeImage(image) {
        $modal.open({templateUrl: "partials/review/modals/imageModal/imageModal.html",
            controller: 'ImageModalCtrl as imageModal',
            windowClass: 'modal',
            keyboard: true,
            resolve: {
                student: function () {
                    return image;
                }
            }});
    }

    function loadLogs(){
        studentService.getStudentLogs(studentModal.student.netname).then(function(value){
            studentModal.logs = value.data;
        });
    }

    function resetModal(){
        studentModal.logs = undefined;
    }

}

