'use strict';

var defaultHeaders = {
    'Accept': 'application/json',
    'Content-Type': 'application/json'
};

angular
    .module('myApp')
    .factory('toastedHttpService', toastedHttpService);

toastedHttpService.$inject = ['$http', '$q','$mdToast', 'myConfig'];

function toastedHttpService($http, $q, $mdToast, myConfig) {

    var service = {
        get: get,
        post: post,
        put: put,
        del: del
    };

    var toast;

    return service;
    /////////////////////


    function get(settings){
        var param = "";
        if (settings.param != undefined) param = settings.param;

        toast = $mdToast.show(buildSimple());

        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + settings.topUrl + param).then(function (success) {
            deferred.resolve(success.data);
            $mdToast.hide(toast);
        },function (failure) {
            deferred.reject((failure));
            $mdToast.hide(toast);
        });

        return deferred.promise;
    }

    function post(data, topUrl){

        toast = $mdToast.show(buildSimple());

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'POST',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                $mdToast.hide(toast);
            },
            function (failure) {
                deferred.reject(failure);
                $mdToast.hide(toast);
            });

        return deferred.promise;
    }

    function put(data, topUrl){

        toast = $mdToast.show(buildSimple());

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'PUT',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                $mdToast.hide(toast);
            },
            function (failure) {
                deferred.reject(failure);
                $mdToast.hide(toast);
            });

        return deferred.promise;
    }

    function del(data, topUrl){

        toast = $mdToast.show(buildSimple());

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'DELETE',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                $mdToast.hide(toast);
            },
            function (failure) {
                deferred.reject(failure);
                $mdToast.hide(toast);
            });

        return deferred.promise;
    }

    function buildSimple(msg){
        if (!msg) msg = "Working...";
        return $mdToast.simple()
            .textContent(msg)
            .position("bottom right")
            .hideDelay(0)
    }

}