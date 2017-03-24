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
            if (settings.failureMsg == null)
                showToast({msg: "Failed", delay: 3000, responseFailed: true});
            else if (failure.status == 401)
                showToast({msg: settings.failureMsg[401], delay: 3000, responseFailed: true});
            else if (failure.status == 404)
                showToast({msg: settings.failureMsg[409], delay: 3000, responseFailed: true});
            else if (failure.status == 500)
                showToast({msg: settings.failureMsg[500], delay: 3000, responseFailed: true});
        });

        return deferred.promise;
    }

    function post(settings){

        toast = showToast();

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'POST',
            url: myConfig.baseUrl + settings.topUrl,
            data: settings.data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                hideToast();
                $timeout(function(){
                    if (settings.responseMsg == null)
                        showToast({msg:"Sent", delay: 3000});
                    else
                        showToast({msg:settings.responseMsg, delay: 3000});
                }, 800);

            },
            function (failure) {
                deferred.reject(failure);
                hideToast();
                $timeout(function() {
                    if (settings.failureMsg == null)
                        showToast({msg: "Failed", delay: 3000});
                    else if (failure.status == 401)
                        showToast({msg: settings.failureMsg[401], delay: 3000, responseFailed: true});
                    else if (failure.status == 404)
                        showToast({msg: settings.failureMsg[404], delay: 3000, responseFailed: true});
                    else if (failure.status == 409)
                        showToast({msg: settings.failureMsg[409], delay: 3000, responseFailed: true});
                    else if (failure.status == 500)
                        showToast({msg: settings.failureMsg[500], delay: 3000, responseFailed: true});
                }, 800);

            });

        return deferred.promise;
    }

    function put(settings){

        toast = showToast();

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'PUT',
            url: myConfig.baseUrl + settings.topUrl,
            data: settings.data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                hideToast();
                $timeout(function(){
                    if (settings.responseMsg == null)
                        showToast({msg:"Modified", delay: 3000});
                    else
                        showToast({msg:settings.responseMsg, delay: 3000});
                }, 800);
            },
            function (failure) {
                deferred.reject(failure);
                hideToast();
                $timeout(function() {
                    if (settings.failureMsg == null)
                        showToast({msg: "Failed", delay: 3000, responseFailed: true});
                    else if (failure.status == 401)
                        showToast({msg: settings.failureMsg[401], delay: 3000, responseFailed: true});
                    else if (failure.status == 404)
                        showToast({msg: settings.failureMsg[404], delay: 3000, responseFailed: true});
                    else if (failure.status == 500)
                        showToast({msg: settings.failureMsg[500], delay: 3000, responseFailed: true});
                }, 800);
            });

        return deferred.promise;
    }

    function del(settings){

        toast = showToast();

        var deferred = $q.defer();

        $http({
            headers: defaultHeaders,
            method: 'DELETE',
            url: myConfig.baseUrl + settings.topUrl,
            data: settings.data
        }).then(
            function (success) {
                deferred.resolve(success.data);
                hideToast();
                $timeout(function(){
                    if (settings.responseMsg == null)
                        showToast({msg:"Deleted", delay: 3000});
                    else
                        showToast({msg:settings.responseMsg, delay: 3000});
                }, 800);
            },
            function (failure) {
                deferred.reject(failure);
                hideToast();
                $timeout(function() {
                    if (settings.failureMsg == null)
                        showToast({msg: "Failed", delay: 3000, responseFailed: true});
                    else if (failure.status == 401)
                        showToast({msg: settings.failureMsg[401], delay: 3000, responseFailed: true});
                    else if (failure.status == 404)
                        showToast({msg: settings.failureMsg[404], delay: 3000, responseFailed: true});
                    else if (failure.status == 500)
                        showToast({msg: settings.failureMsg[500], delay: 3000, responseFailed: true});
                }, 800);
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
        return $mdToast.show(buildSimple(options));
    }

    function buildSimple(options){
        var msg = "Loading...";
        var theme = "";
        var delay = 6000;
        if (options && options.responseFailed) theme = "error-toast";
        if (options && options.msg) msg = options.msg;
        if (options && options.delay) delay = options.delay;
        return $mdToast.simple()
            .textContent(msg)
            .position("bottom right")
            .hideDelay(delay)
            .theme(theme)
    }

}