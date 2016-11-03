/**
 * Created by Simon on 2016-11-01.
 */

angular.module('starter.controllers').controller('CameraCtrl', function ($scope, $cordovaCamera, $ionicPopup, $location) {
  $scope.pictureUrl = 'http://placehold.it/300x300';
  $scope.takePictureButtonText = 'Take Picture';

  var options = {
    quality: 50,
    destinationType: Camera.DestinationType.FILE_URI,
    allowEdit: true, //This function allows cropping and editing the picture
    encodingType: Camera.EncodingType.JPEG,
    targetWidth: 300,
    targetHeight: 300,
    popoverOptions: CameraPopoverOptions,
    saveToPhotoAlbum: false,
    correctOrientation: true
  };

  $scope.takePicture = function () {


    options.sourceType = Camera.PictureSourceType.CAMERA;

    $cordovaCamera.getPicture(options)
      .then(function (imageURI) {
        $scope.pictureUrl = imageURI;
        $scope.takePictureButtonText = 'Retake Picture';
        $scope.hideSendPictureButton = false;
      }, function (error) {

      })
  }

  $scope.loadPicture = function () {


    options.sourceType = Camera.PictureSourceType.PHOTOLIBRARY;

    $cordovaCamera.getPicture(options)
      .then(function (imageURI) {
        $scope.pictureUrl = imageURI;
        $scope.hideSendPictureButton = false;

      }, function (error) {

      })
  }

  $scope.sendPicture = function () {
    var ft = new FileTransfer(),
      options = new FileUploadOptions();

    var serverURL = encodeURI("https://myconcordiaid.azurewebsites.net");

    options.fileKey = "file";
    options.fileName = 'filename.jpg';
    options.mimeType = "image/jpeg";
    options.chunkedMode = false;
    // options.params = {
    //   "timestamp": "Oct 12,2016"
    // };

    $scope.showUploadSpinnerGif = true;
    $scope.hideTakePictureButton = true;
    $scope.hideLoadPictureButton = true;
    $scope.hideSendPictureButton = true;

    ft.upload($scope.pictureUrl, serverURL + "/api/student/ProfilePicture",
      function (e) {
        $scope.showAlert('upload-success');

        //hide uploading spinner when callback succeeded
        $scope.$apply(function () {
          $scope.showUploadSpinnerGif = false;
        });
      },
      function (e) {
        $scope.showAlert('upload-fail');

        //hide uploading spinner when callback failed
        $scope.$apply(function () {
          $scope.showUploadSpinnerGif = false;
          $scope.hideTakePictureButton = false;
          $scope.hideLoadPictureButton = false;
          $scope.hideSendPictureButton = false;
        });
      }, options);
  }

  // ionic alert dialogues
  $scope.showAlert = function (alertType) {
    var alertPopup;

    if (alertType == 'upload-success') {
      alertPopup = $ionicPopup.alert({
        title: 'Upload successful',
        template: 'The picture has been successfully sent. Please visit the Birks Student Service Centre in person to have your photo validated.'
      });
      alertPopup.then(function (res) {
        console.log('Thank you for not eating my delicious ice cream cone');
        $location.path('/app/id');
      });
    } else if (alertType = "upload-fail") {
      alertPopup = $ionicPopup.alert({
        title: 'Upload failed',
        template: 'Uh oh, looks like something went wrong. Please try sending the photo again.'
      });
    }
  };
})
