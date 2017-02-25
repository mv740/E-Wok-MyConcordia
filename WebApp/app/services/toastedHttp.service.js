'use strict';

var defaultHeaders = {
    'Accept': 'application/json',
    'Content-Type': 'application/json'
};

angular
    .module('myApp')
    .factory('toastedHttpService', toastedHttpService);

toastedHttpService.$inject = ['$http', '$q', '$timeout', '$mdToast', 'myConfig'];

function toastedHttpService($http, $q, $timeout,  $mdToast, myConfig) {

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

        toast = showToast();

        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + settings.topUrl + param).then(function (success) {
            deferred.resolve(success.data);
            hideToast();
        },function (failure) {
            deferred.reject((failure));
            hideToast();
        });

        return deferred.promise;
    }

    function post(data, topUrl){

        toast = showToast();

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'POST',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                hideToast();
                $timeout(function(){
                    showToast({msg:"Sent", delay: 3000});
                }, 800);

            },
            function (failure) {
                deferred.reject(failure);
                hideToast();
            });

        return deferred.promise;
    }

    function put(data, topUrl){

        toast = showToast();

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'PUT',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                hideToast();
                $timeout(function(){
                    showToast({msg:"Modified", delay: 3000});
                }, 800);
            },
            function (failure) {
                deferred.reject(failure);
                hideToast();
            });

        return deferred.promise;
    }

    function del(data, topUrl){

        toast = showToast();

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'DELETE',
            url: myConfig.baseUrl + topUrl,
            data: data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                hideToast();
                $timeout(function(){
                    showToast({msg:"Deleted", delay: 3000});
                }, 800);
            },
            function (failure) {
                deferred.reject(failure);
                hideToast();
            });

        return deferred.promise;
    }

    function hideToast(){
        if (toast) {
            $mdToast.hide(toast);
            toast = null;
        }
    }

    function showToast(options){
        if (!toast) {
            return $mdToast.show(buildSimple(options));
        }
    }

    function buildSimple(options){
        var msg = "Loading...";
        var delay = 6000;
        if (options && options.msg) msg = options.msg;
        if (options && options.delay) delay = options.delay;
        return $mdToast.simple()
            .textContent(msg)
            .position("bottom right")
            .hideDelay(delay)
    }

}