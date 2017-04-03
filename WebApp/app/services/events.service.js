'use strict';

angular
    .module('myApp')
    .factory('eventService', eventService);


eventService.$inject = ['$q', 'translateService', 'toastedHttpService', 'myConfig'];

function eventService($q, translateService, toastedHttp, myConfig) {

    var service = {
        getThisEvent: getThisEvent,
        getAllEvents: getAllEvents,
        submit: submit,
        updateEvent: updateEvent,
        setUserRole: setUserRole,
        getEventAttendees: getEventAttendees,
        addUser: addUser,
        cancelEvent: cancelEvent,
        deleteUser: deleteUser,
        getStats: getStats
    };

    return service;

    //////////////////////////////////////

    function getThisEvent(id) {
        var localizationPromises = {
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETTHISEVENT.failureMsg.401'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETTHISEVENT.failureMsg.404'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETTHISEVENT.failureMsg.500')
        };

        var settings = {
            param: id,
            topUrl: myConfig.getEvent,
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.get(settings);

            });
    }

    function submit(event){
        var localizationPromises = {
            responseMsg: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SUBMIT.responseMsg'),
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SUBMIT.failureMsg.401'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SUBMIT.failureMsg.500')
        };

        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: "",
            failureMsg: {
                401: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.post(settings);

            });
    }

    function getAllEvents() {
        var localizationPromises = {
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETALLEVENTS.failureMsg.401'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETALLEVENTS.failureMsg.500')
        };

        var settings = {
            topUrl: myConfig.getEvents,
            failureMsg: {
                401: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.get(settings);

            });
    }

    function updateEvent(event) {
        var localizationPromises = {
            responseMsg: translateService.getTranslation('TOASTFEEDBACK.EVENTS.UPDATEEVENT.responseMsg'),
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.UPDATEEVENT.failureMsg.401'),
            403: translateService.getTranslation('TOASTFEEDBACK.EVENTS.UPDATEEVENT.failureMsg.403'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.UPDATEEVENT.failureMsg.404'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.UPDATEEVENT.failureMsg.500')
        };

        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: "",
            failureMsg: {
                401: "",
                403: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[403] = translations[403];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.put(settings);
            });
    }

    function cancelEvent(event) {
        var localizationPromises = {
            responseMsg: translateService.getTranslation('TOASTFEEDBACK.EVENTS.CANCELEVENT.responseMsg'),
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.CANCELEVENT.failureMsg.401'),
            403: translateService.getTranslation('TOASTFEEDBACK.EVENTS.CANCELEVENT.failureMsg.403'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.CANCELEVENT.failureMsg.404'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.CANCELEVENT.failureMsg.500')
        };

        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: "",
            failureMsg: {
                401: "",
                403: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[403] = translations[403];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.del(settings);
            });
    }

    function setUserRole(user) {
        var localizationPromises = {
            responseMsg: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SETUSERROLE.responseMsg'),
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SETUSERROLE.failureMsg.401'),
            403: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SETUSERROLE.failureMsg.403'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SETUSERROLE.failureMsg.404'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.SETUSERROLE.failureMsg.500')
        };

        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: "",
            failureMsg: {
                401: "",
                403: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[403] = translations[403];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.put(settings);

            });
    }

    function getEventAttendees(id) {
        var localizationPromises = {
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETEVENTATTENDEES.failureMsg.401'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETEVENTATTENDEES.failureMsg.404'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETEVENTATTENDEES.failureMsg.500')
        };

        var settings = {
            topUrl: myConfig.eventAttendees.replace("IDTOKEN", id) + "/true",
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.get(settings);

            });
    }

    function addUser(user) {
        var localizationPromises = {
            responseMsg: translateService.getTranslation('TOASTFEEDBACK.EVENTS.ADDUSER.responseMsg'),
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.ADDUSER.failureMsg.401'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.ADDUSER.failureMsg.404'),
            409: translateService.getTranslation('TOASTFEEDBACK.EVENTS.ADDUSER.failureMsg.409'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.ADDUSER.failureMsg.500')
        };

        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: "",
            failureMsg: {
                401: "",
                404: "",
                409: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[409] = translations[409];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.post(settings);
            });
    }

    function deleteUser(user) {
        var localizationPromises = {
            responseMsg: translateService.getTranslation('TOASTFEEDBACK.EVENTS.DELETEUSER.responseMsg'),
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.DELETEUSER.failureMsg.401'),
            403: translateService.getTranslation('TOASTFEEDBACK.EVENTS.DELETEUSER.failureMsg.403'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.DELETEUSER.failureMsg.404'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.DELETEUSER.failureMsg.500')
        };

        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: "",
            failureMsg: {
                401: "",
                403: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[403] = translations[403];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.del(settings);

            });
    }

    function getStats(eventId){
        var localizationPromises = {
            401: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETSTATS.failureMsg.401'),
            404: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETSTATS.failureMsg.404'),
            500: translateService.getTranslation('TOASTFEEDBACK.EVENTS.GETSTATS.failureMsg.500')
        };

        var settings = {
            topUrl: myConfig.getEventStats.replace("IDTOKEN", eventId),
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.get(settings);

            });
    }
}