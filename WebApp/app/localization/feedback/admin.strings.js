/**
 * Created by Harrison on 2017-03-09.
 */

angular
    .module('myApp')
    .constant("adminToastFeedback", {
        "submitUpdatePeriod": {
            responseMsg: "Period updated",
            failureMsg: {
                401: "Please Login",
                404: "Update Period not found",
                500: "Sorry! Our servers are down :("
            }
        },
        "getUpdatePeriod": {
            failureMsg: {
                401: "Please Login",
                404: "Update Period not found",
                500: "Sorry! Our servers are down :("
            }
        }
    });