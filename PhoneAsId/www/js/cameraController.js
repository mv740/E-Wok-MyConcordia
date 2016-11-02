/**
 * Created by Simon on 2016-11-01.
 */

angular.module('starter.controllers').controller('CameraCtrl', function($scope, $cordovaCamera) {
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
    correctOrientation:true
  };

  $scope.takePicture = function () {


    options.sourceType=Camera.PictureSourceType.CAMERA;

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

    $scope.showUploadSpinnerGif = true;
    $scope.hideTakePictureButton = true;
    $scope.hideLoadPictureButton = true;
    $scope.hideSendPictureButton = true;

    ft.upload($scope.pictureUrl, serverURL + "/api/student/ProfilePicture",
      function (e) {
        alert("Upload sent");
        //hide uploading spinner when callback succeeded
        $scope.$apply(function(){
          $scope.showUploadSpinnerGif = false;
        });
        //$location.path('/id');
        //TODO redirect ID page
      },
      function (e) {
        alert("Upload failed");
        //hide uploading spinner when callback failed
        $scope.$apply(function(){
          $scope.showUploadSpinnerGif = false;
          $scope.hideTakePictureButton = false;
          $scope.hideLoadPictureButton = false;
          $scope.hideSendPictureButton = false;
        });
      }, options);
  }
})
