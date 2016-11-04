/**
 * Created by Simon on 2016-11-01.
 */

angular.module('starter.controllers').controller('CameraCtrl', function($scope, $cordovaCamera, $cordovaFile, $cordovaFileTransfer) {
  $scope.pictureUrl = 'http://placehold.it/300x300';
  $scope.showSendPictureButton = false;
  $scope.takePictureButtonText = 'Take Picture';
  $scope.description="none";

  var visionObj = this;
  visionObj.current_image = '';
  visionObj.image_description = '';
  visionObj.detection_type = 'LABEL_DETECTION';

  visionObj.detection_types = {
    LABEL_DETECTION: 'label',
    TEXT_DETECTION: 'text',
    LOGO_DETECTION: 'logo',
    LANDMARK_DETECTION: 'landmark'
  };

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
    correctOrientation:true
  };

  $scope.takePicture = function () {


    options.sourceType=Camera.PictureSourceType.CAMERA;

    $cordovaCamera.getPicture(options)
      .then(function (imageData) {
        $scope.pictureUrl = "data:image/jpeg;base64," + imageData;
        $scope.takePictureButtonText = 'Retake Picture';
        $scope.showSendPictureButton = true;

        sendToVision(imageData);

      }, function (error) {

      })
  };

  $scope.loadPicture = function () {


    options.sourceType = Camera.PictureSourceType.PHOTOLIBRARY;

    $cordovaCamera.getPicture(options)
      .then(function (imageData) {
        $scope.pictureUrl = "data:image/jpeg;base64," + imageData;
        $scope.showSendPictureButton = true;


        sendToVision(imageData);

      }, function (error) {
          alert('Error occured while getting the camera');
      })
  };

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

  };

  function sendToVision(imageData){

    visionObj.current_image = "data:image/jpeg;base64," + imageData;
    visionObj.image_description = '';
    visionObj.locale = '';

    var vision_api_json = {
      "requests":[
        {
          "image":{
            "content": imageData
          },
          "features":[
            {
              "type": visionObj.detection_type,
              "maxResults": 1
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
    ).then(function(result){

      var headers = {
        'Content-Type': 'application/json'
      };

      options.headers = headers;

      var server = 'https://vision.googleapis.com/v1/images:annotate?key=' + google_api_key;
      var filePath = cordova.file.applicationStorageDirectory + 'file.json';

      $cordovaFileTransfer.upload(server, filePath, options, true)
        .then(function(result){

          var res = JSON.parse(result.response);
          var key = visionObj.detection_types[visionObj.detection_type] + 'Annotations';

          visionObj.image_description = res.responses[0][key][0].description;
          $scope.description = visionObj.image_description;

        }, function(err){
          alert(JSON.stringify(err));
          //alert('An error occurred while uploading the file');
        });
    }, function(err){
      alert('An error occurred while trying to write the file');
    });

  }

});
