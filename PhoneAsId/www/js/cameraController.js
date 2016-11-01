/**
 * Created by Simon on 2016-11-01.
 */

angular.module('starter.controllers').controller('CameraCtrl', function($scope, $cordovaCamera) {
  $scope.pictureUrl = 'http://placehold.it/300x300';
  $scope.showSendPictureButton = false;
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
    correctOrientation:true
  };

  $scope.takePicture = function () {


    options.sourceType=Camera.PictureSourceType.CAMERA;

    $cordovaCamera.getPicture(options)
      .then(function (imageURI) {
        $scope.pictureUrl = imageURI;
        $scope.takePictureButtonText = 'Retake Picture';
        $scope.showSendPictureButton = true;
      }, function (error) {

      })
  }

  $scope.loadPicture = function () {


    options.sourceType = Camera.PictureSourceType.PHOTOLIBRARY;

    $cordovaCamera.getPicture(options)
      .then(function (imageURI) {
        $scope.pictureUrl = imageURI;
        $scope.showSendPictureButton = true;

      }, function (error) {

      })
  }

  //TODO upload animation?
  $scope.sendPicture = function() {
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

    ft.upload($scope.pictureUrl, serverURL + "/api/student/ProfilePicture",
      function (e) {
        alert("Upload sent");
      },
      function (e) {
        alert("Upload failed");
      }, options);

  }

})