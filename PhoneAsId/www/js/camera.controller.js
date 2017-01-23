/**
 * Created by Simon on 2016-11-01.
 */


angular.module('starter.controllers').controller('CameraCtrl',['SessionService','$scope','$cordovaCamera', '$cordovaFile', '$cordovaFileTransfer', '$ionicPopup', '$location', '$rootScope', function (SessionService, $scope, $cordovaCamera, $cordovaFile, $cordovaFileTransfer, $ionicPopup, $location, $rootScope) {

  $scope.pictureUrl = 'http://placehold.it/300x300';
  $scope.takePictureButtonText = 'Camera';
  $scope.description = "";

  var visionObj = this;
  visionObj.current_image = '';
  visionObj.image_description = '';
  visionObj.detection_type = 'SAFE_SEARCH_DETECTION';

  visionObj.detection_types = {
    LABEL_DETECTION: 'label',
    TEXT_DETECTION: 'text',
    LOGO_DETECTION: 'logo',
    LANDMARK_DETECTION: 'landmark',
    SAFE_SEARCH_DETECTION: 'safeSearch'
  };

  visionObj.safe_search_result = {
    V_UNL: "VERY_UNLIKELY",
    UNL: "UNLIKELY",
    POS: "POSSIBLE",
    LIK: "LIKELY",
    V_LIK:"VERY_LIKELY"
  };

  var safeToSend = true;

  //For testing only
  var google_api_key = 'AIzaSyAhURhpm8tVY1k2RM2T1_I84bqC-lFJlZc';

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




  $scope.takePicture = function () {


    options.sourceType = Camera.PictureSourceType.CAMERA;

    $cordovaCamera.getPicture(options)
      .then(function (imageData) {
        $scope.pictureUrl = "data:image/jpeg;base64," + imageData;
        $scope.takePictureButtonText = 'Retake Picture';

        $scope.hideSendPictureButton = false;



        sendToVision(imageData);

        displayPicture();

      }, function (error) {

      })
  };

  $scope.loadPicture = function () {


    options.sourceType = Camera.PictureSourceType.PHOTOLIBRARY;

    $cordovaCamera.getPicture(options)
      .then(function (imageData) {
        $scope.pictureUrl = "data:image/jpeg;base64," + imageData;
        $scope.hideSendPictureButton = false;

        sendToVision(imageData);

        displayPicture();

      }, function (error) {
        alert('Error occured while getting the camera');
      })
  };

  $scope.sendPicture = function () {


    var ft = new FileTransfer();
    var options = new FileUploadOptions();


    var serverURL = encodeURI("https://myconcordiaid.azurewebsites.net");


    var token = 'Bearer ' + SessionService.getAccessToken();
    var headers = {
      'Authorization' : token
    };

    if(safeToSend){
      options.fileKey = "file";
      options.fileName = 'filename.jpg';
      options.mimeType = "image/jpeg";
      options.chunkedMode = false;
      options.headers = headers;
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

          console.log("camera upload error: " + e);

          //hide uploading spinner when callback failed
          $scope.$apply(function () {
            $scope.showUploadSpinnerGif = false;
            $scope.hideTakePictureButton = false;
            $scope.hideLoadPictureButton = false;
            $scope.hideSendPictureButton = false;
          });
        }, options);
    }else{
      var alertSafePopup = $ionicPopup.alert({
        title: 'Request Denied',
        template: 'The picture you tried to send contained content that may not be suitable for children under the age of 18. Please try again'
      });
    }
  }

  // ionic alert dialogues
  $scope.showAlert = function (alertType) {
    var alertPopup;

    if (alertType == 'upload-success') {

      $rootScope.canUpdate = false; //disable

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

  function sendToVision(imageData) {

    visionObj.current_image = "data:image/jpeg;base64," + imageData;
    visionObj.image_description = '';
    visionObj.locale = '';

    var vision_api_json = {
      "requests": [
        {
          "image": {
            "content": imageData
          },
          "features": [
            {
              "type": visionObj.detection_type,

            }
          ]
        }
      ]
    };

    var file_contents = JSON.stringify(vision_api_json);


    $cordovaFile.writeFile(
      cordova.file.applicationStorageDirectory,
      'file.json',
      file_contents,
      true
    ).then(function (result) {

      var headers = {
        'Content-Type': 'application/json'
      };

      options.headers = headers;

      var server = 'https://vision.googleapis.com/v1/images:annotate?key=' + google_api_key;
      var filePath = cordova.file.applicationStorageDirectory + 'file.json';

      $cordovaFileTransfer.upload(server, filePath, options, true)
        .then(function (result) {

          var res = JSON.parse(result.response);
          var key = visionObj.detection_types[visionObj.detection_type] + 'Annotation';

          visionObj.image_description = res.responses[0][key].adult;
          $scope.description = visionObj.image_description;
          if(visionObj.image_description ==  visionObj.safe_search_result['V_UNL'] ||  visionObj.image_description ==  visionObj.safe_search_result['UNL'] || visionObj.image_description ==  visionObj.safe_search_result['POS']){
            safeToSend = true;
          }else{
            safeToSend = false;
          }

        }, function (err) {
          alert(JSON.stringify(err));
          //alert('An error occurred while uploading the file');
        });
    }, function (err) {
      alert('An error occurred while trying to write the file');
    });

  }

  function displayPicture(){
    var myPopup = $ionicPopup.show({
      templateUrl: 'templates/cameraPopup.html',
      title: 'Send picture for approval?',
      scope: $scope,
      buttons: [
        { text: 'Cancel',
        type: 'button-assertive'},
        {
          text: 'Send',
          type: 'button-balanced',
          onTap: function(e) {
            $scope.sendPicture();
          }
        }
      ]
    });
  }
}]);

