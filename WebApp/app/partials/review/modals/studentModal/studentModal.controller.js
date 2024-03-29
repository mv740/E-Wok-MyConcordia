'use strict';

angular
    .module('myApp')
    .controller('StudentModalCtrl', StudentModalCtrl);

StudentModalCtrl.$inject = ['$scope', '$uibModal', '$uibModalInstance', 'studentService', 'student'];

function StudentModalCtrl($scope, $modal, $modalInstance, studentService, student) {

    var studentModal = this;

    studentModal.sendValidation = sendValidation;
    studentModal.enlargeImage = enlargeImage;
    studentModal.toggleLogs = toggleLogs;
    studentModal.submitComment = submitComment;
    studentModal.close = $modalInstance.close;
    studentModal.emptyProfilePicture = 'images/empty-profile.png';

    $scope.$on("StudentModal.resetModal", resetModal);
    $scope.$on("StudentModal.updateStudent", updateStudent);
    $modalInstance.opened.then(updateStudent);
    $modalInstance.result.then(resetModal);

    //////////////////////////

    function sendValidation(id, valid){
        studentModal.student.valid = valid;
        studentService.sendValidation(id,valid).then(
            function(){
                if (studentModal.student.valid) studentModal.student.wasValidated = true;
                else if (!studentModal.student.valid) studentModal.student.wasRevoked = true;

                setTimeout(function(){
                    resetModal();
                    //$modalInstance.close();
                    updateStudent();
                }, 2000);
            },
            function(){
                alert("Validation failed");
            }
        );
    }

    function updateStudent() {
        studentModal.student = student;

        studentService.getStudentPictures(student.id).then(function (value) {
            studentModal.student.pendingPicture = value.pendingPicture;

            var archivedPictures = value.archivedPictures;

            if (value.profilePicture){
                archivedPictures.unshift(value.profilePicture);
            }
            studentModal.student.previousPictures = archivedPictures;


        });
    }

    function enlargeImage(image) {
        $modal.open({templateUrl: "partials/review/modals/imageModal/imageModal.html",
            controller: 'ImageModalCtrl as imageModal',
            windowClass: 'app-modal-window-xl',
            keyboard: true,
            resolve: {
                image: function () {
                    return image;
                }
            }});
    }

    function toggleLogs(){
        studentModal.logsOpened = !studentModal.logsOpened;
        if (!studentModal.logs) {
            studentService.getStudentLogs(studentModal.student.netname).then(function (value) {
                studentModal.logs = value;
            });
        }
    }

    function resetModal(){
        studentModal.logs = undefined;
        studentModal.student = undefined;
        updateStudent();
    }

    function submitComment(){
        studentService.submitComment(studentModal.student.pendingPicture.iD_PK, studentModal.student.pendingPicture.comments);
    }

}

