/**
 * Created by franc on 1/13/2017.
 */

module.exports = {
    // The address of a running selenium server.
    seleniumAddress:
        (process.env.SELENIUM_URL || 'http://localhost:4444/wd/hub'),

    // A base URL for your application under test.
    urlBackendInit: 'https://myconcordiaid.azurewebsites.net/api/student'

};