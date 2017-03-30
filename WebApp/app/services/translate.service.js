'use strict';

angular
    .module('myApp')
    .factory('translateService', translateService);


translateService.$inject = ['$q', '$translate'];

function translateService($q, $translate) {

    var service = {
        getTranslation: getTranslation
    };

    return service;

    //////////////////////////////////////

    function getTranslation(path) { // translates text given a path
        var deferred = $q.defer();

        $translate(path)
            .then(function(translation) {
                deferred.resolve(translation);
            });

        return deferred.promise;
    }
}
