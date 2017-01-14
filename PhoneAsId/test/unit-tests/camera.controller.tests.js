/**
 * Created by Simon on 2016-10-13.
 */

describe('CameraCtrl', function () {

    var controller,
    cordovaCameraMock,
    options={};

    beforeEach(module('starter'));
    beforeEach(module('ngCordovaMocks'));

    beforeEach(inject(function ($controller, $cordovaCamera) {

      cordovaCameraMock = jasmine.createSpyObj('$cordovaCamera spy',['getPicture']);

      controller = $controller('CameraCtrl',{'$cordovaCamera':cordovaCameraMock});
    }));

    beforeEach(inject(function (_$rootCcope_) {
      $rootScope=_$rootCcope_;
      controller.takePicture(options);
    }));


    it('should call take picture',function () {
      expect(cordovaCameraMock.getPicture(options).toHaveBeenCalled());
    });
});
