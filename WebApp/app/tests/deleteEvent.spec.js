var newEvent = {
    name: 'testEvent',
    location: 'testLocation',
    room: 'testRoom',
    description: 'testDescription',
    timeBegin: '03/30/2017 00:00',
    timeEnd: '03/31/2017 00:00'
}

describe('event tab event cancellation', function () {
    it('should cancel an event with name testEvent', function () {
        browser.get('https://concordiaidclient.netlify.com/WebApp/app/event');
        var EC = protractor.ExpectedConditions;

        var searchResultsUl = element(by.css('.listResults'));

        browser.wait(EC.presenceOf(searchResultsUl), 5000).then(function () {

            var searchResults = element.all(by.css('li'));
            searchResults.count().then(function (oldCount) {

                var createdEventLi = element(by.xpath('//li[contains(., "' + newEvent.name + '")]'));
                var createdEventButton = createdEventLi.element(by.css('.button3D'));
                createdEventButton.click();

                var cancelButton = createdEventLi.element(by.css('[ng-click="eventTab.confirmCancelEvent(result)"]'));
                browser.wait(EC.visibilityOf(cancelButton), 5000).then(function () {
                    cancelButton.click();

                    var confirmCancelButton = createdEventLi.element(by.xpath('//button[contains(., "Yes, cancel it!")]'));
                    browser.wait(EC.presenceOf(confirmCancelButton), 5000).then(function () {
                        confirmCancelButton.click();

                        var searchResults = element.all(by.css('li'));
                        searchResults.count().then(function (newCount) {
                            expect(newCount).toEqual(oldCount - 1);
                        });
                    });
                });
            });
        }, function (error) {
            console.log("listResults did not display");
            expect(false).toBe(true);
        });
    });
});