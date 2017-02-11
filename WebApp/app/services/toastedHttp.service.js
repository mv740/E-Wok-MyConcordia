'use strict';

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
        var topUrl = settings.topUrl;

        toast = $mdToast.show(
            $mdToast.simple()
                .textContent('Working...')
                .position("bottom right")
                .hideDelay(0)
        );

        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + topUrl + param).then(function (success) {
            deferred.resolve(success);
            $mdToast.hide(toast);
        },function (failure) {
            deferred.reject((failure));
            $mdToast.hide(toast);
        });

        return deferred.promise;
    }

    function post(data, topUrl){

        toast = $mdToast.show(
            $mdToast.simple()
                .textContent('Working...')
                .position("bottom right")
                .hideDelay(0)
        );

        var deferred = $q.defer();

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'POST',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success);
                $mdToast.hide(toast);
            },
            function (failure) {
                deferred.reject(failure);
                $mdToast.hide(toast);
            });

        return deferred.promise;
    }

    function put(data, topUrl){

        toast = $mdToast.show(
            $mdToast.simple()
                .textContent('Sending validation...')
                .position("bottom right")
                .hideDelay(0)
        );

        var deferred = $q.defer();

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'PUT',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success);
                $mdToast.hide(toast);
            },
            function (failure) {
                deferred.reject(failure);
                $mdToast.hide(toast);
            });

        return deferred.promise;
    }

    function del(data, topUrl){

        toast = $mdToast.show(
            $mdToast.simple()
                .textContent('Sending validation...')
                .position("bottom right")
                .hideDelay(0)
        );

        var deferred = $q.defer();

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'DELETE',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success);
                $mdToast.hide(toast);
            },
            function (failure) {
                deferred.reject(failure);
                $mdToast.hide(toast);
            });

        return deferred.promise;
    }

}