/**
 * Created by Harrison on 2017-03-09.
 */

angular
    .module('myApp')
    .constant("studentToastFeedback", {
        "sendValidation": {
            responseMsg: {
                valid: "Picture Validated",
                invalid: "Picture Revoked"
            },
            failureMsg: {
                401: "Please Login",
                404: "No picture found",
                500: "Sorry! Our servers are down :("
            }
        },
        "getStudentPictures": {
            failureMsg: {
                401: "Please Login",
                404: "No student found",
                500: "Sorry! Our servers are down :("
            }
        },
        "getStudentLogs": {
            failureMsg: {
                401: "Please Login",
                404: "No logs found",
                500: "Sorry! Our servers are down :("
            }
        },
        "getStudents": {
            failureMsg: {
                401: "Please Login",
                404: "Error",
                500: "Sorry! Our servers are down :("
            }
        },
        "search": {
            responseMsg: "Student found",
            failureMsg: {
                401: "Please Login",
                404: "Error",
                500: "Sorry! Our servers are down :("
            }
        },
        "validateArchived": {
            responseMsg: {
                valid: "Archive picture validated",
                invalid: "Archive picture revoked"
            },
            failureMsg: {
                401: "Please Login",
                404: "No archive picture found",
                500: "Sorry! Our servers are down :("
            }
        },
        "submitComment": {
            responseMsg: "Commented",
            failureMsg: {
                401: "Please Login",
                404: "Couldn't comment. Student not found",
                500: "Sorry! Our servers are down :("
            }
        }
    });