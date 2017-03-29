/**
 * Created by Simon on 2016-11-01.
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .controller('CameraCtrl', CameraCtrl);

    CameraCtrl.$inject = ['SessionService','$scope','$cordovaCamera', '$cordovaFile', '$cordovaFileTransfer', '$ionicPopup', '$location', '$rootScope','Settings', '$filter'];

    function CameraCtrl(SessionService, $scope, $cordovaCamera, $cordovaFile, $cordovaFileTransfer, $ionicPopup, $location, $rootScope, Settings, $filter) {
      var camCtrl = this;

      camCtrl.pictureUrl = 'http://placehold.it/300x300';
      camCtrl.description = "";

      //Basic options for the camera
      var options = {
        quality: 50,
        destinationType: Camera.DestinationType.DATA_URL,
        allowEdit: true, //This function allows cropping and editing the picture
        encodingType: Camera.EncodingType.JPEG,
        targetWidth: 300,
        targetHeight: 300,
        popoverOptions: CameraPopoverOptions,
        saveToPhotoAlbum: false,
        correctOrientation: true
      };


      camCtrl.takePicture = function () {

        //Take picture with the camera
        options.sourceType = Camera.PictureSourceType.CAMERA;

        $cordovaCamera.getPicture(options)
          .then(function (imageData) {
            camCtrl.pictureUrl = "data:image/jpeg;base64," + imageData;

            displayPicture();

          }, function (error) {

          })
      };

      camCtrl.loadPicture = function () {

        //Loads a picture from the library
        options.sourceType = Camera.PictureSourceType.PHOTOLIBRARY;

        $cordovaCamera.getPicture(options)
          .then(function (imageData) {
            camCtrl.pictureUrl = "data:image/jpeg;base64," + imageData;
            camCtrl.hideSendPictureButton = false;

            displayPicture();

          }, function (error) {

          })
      };

      //Sends the picture to the server
      camCtrl.sendPicture = function () {

        var ft = new FileTransfer();
        var options = new FileUploadOptions();

        var serverURL = encodeURI(Settings.baseUrl);

        var token = 'Bearer ' + SessionService.getAccessToken();
        var headers = {
          'Authorization': token
        };

        options.fileKey = "file";
        options.fileName = 'filename.jpg';
        options.mimeType = "image/jpeg";
        options.chunkedMode = false;
        options.headers = headers;
        // options.params = {
        //   "timestamp": "Oct 12,2016"
        // };

        camCtrl.showUploadSpinnerGif = true;


        ft.upload(camCtrl.pictureUrl, serverURL + "/api/student/ProfilePicture",
          function (e) {
            camCtrl.showAlert('upload-success');

            //hide uploading spinner when callback succeeded
            $scope.$apply(function () {
              camCtrl.showUploadSpinnerGif = false;
            });
          },
          function (e) {
            camCtrl.showAlert('upload-fail');

            console.log("camera upload error: " + e);
            console.log("PictureURLK:::: " + camCtrl.pictureUrl);

            //hide uploading spinner when callback failed
            $scope.$apply(function () {
              camCtrl.showUploadSpinnerGif = false;

            });
          }, options);
      }

      // ionic alert dialogues
      camCtrl.showAlert = function (alertType) {
        var alertPopup;

        if (alertType == 'upload-success') {

          $rootScope.canUpdate = false; //disable

          alertPopup = $ionicPopup.alert({
            title: '<b>' + $filter('translate')('popup_upload_succ_title') + '</b>',
            template: '{{"popup_upload_succ" | translate}}'
          });
          alertPopup.then(function (res) {
            console.log('Thank you for not eating my delicious ice cream cone');
            $location.path('/app/id');
          });
        } else if (alertType = "upload-fail") {
          alertPopup = $ionicPopup.alert({
            title: '<b>' + $filter('translate')('popup_upload_fail_title') + '</b>',
            template: '{{"popup_upload_fail" | translate}}'
          });
        }
      };

      //Displays the picture in a popup for confirmation before sending it to the server
      function displayPicture() {
        var myPopup = $ionicPopup.show({
          templateUrl: 'templates/cameraPopup.html',
          title: '<b>' + $filter('translate')('popup_send_pic_title') + '</b>',
          scope: $scope,
          buttons: [
            {
              text: $filter('translate')('popup_btn_cancel'),
              type: 'button-assertive'
            },
            {
              text: $filter('translate')('popup_btn_send'),
              type: 'button-balanced',
              onTap: function (e) {
                camCtrl.sendPicture();
              }
            }
          ]
        });
      }
    }
})();

