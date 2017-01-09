'use strict';

angular
    .module('myApp')
    .controller('StudentModalCtrl', StudentModalCtrl);

StudentModalCtrl.$inject = ['$scope', '$modal', '$modalInstance', 'studentService' , 'student'];

function StudentModalCtrl($scope, $modal, $modalInstance, studentService, student) {

    var studentModal = this;

    studentModal.sendValidation = sendValidation;
    studentModal.enlargeImage = enlargeImage;
    studentModal.loadLogs = loadLogs;
    studentModal.close = $modalInstance.close;
    studentModal.loading = true;
    studentModal.emptyProfilePicture = 'images/empty-profile.png';

    $scope.$on("StudentModal.updateStudent", updateStudent);
    $modalInstance.opened.then(updateStudent);
    $modalInstance.result.then(resetModal);

    //////////////////////////

    function sendValidation(id, valid){
        studentModal.student.sendingValidation = true;
        studentModal.student.valid = valid;
        studentService.sendValidation(id,valid).then(
            function(){
                studentModal.student.sendingValidation = false;
                if (studentModal.student.valid) studentModal.student.wasValidated = true;
                else if (!studentModal.student.valid) studentModal.student.wasRevoked = true;
            },
            function(){
                alert("Validation failed");
            }
        );
    }

    function updateStudent() {
        studentModal.student = student;

        studentService.getStudentPictures(student.id).then(function (value) {
            studentModal.student.pendingPicture = value.data.pendingPicture;

            var archivedPictures = value.data.archivedPictures;

            if (value.data.profilePicture != null){
                //will be removed upon next team meeting as the data returned from the get request will be modified
                var tmp = {netname:"",
                    picture: value.data.profilePicture,
                    status: "validated",
                    timestamp: "salut100"};
                archivedPictures.unshift(tmp);
            }
            studentModal.student.previousPictures = archivedPictures;

            studentModal.loading = false;
        });
    }

    function enlargeImage(image) {
        $modal.open({templateUrl: "partials/review/modals/imageModal/imageModal.html",
            controller: 'ImageModalCtrl as imageModal',
            windowClass: 'modal',
            keyboard: true,
            resolve: {
                image: function () {
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

