var env = require('./environment.protractor.js');
var https = require('https');

exports.config = {
    framework: 'jasmine',
    seleniumAddress: env.seleniumAddress,
    specs: [
        'search.spec.js',
        'newEvent.spec.js',
        'deleteEvent.spec.js'
    ],
    onPrepare: function(){
        https.get(env.urlBackendInit).end(function(err, resp) {
        });
    }
};