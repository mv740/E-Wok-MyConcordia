var newEvent = {
    name: 'testEvent',
    location: 'testLocation',
    room: 'testRoom',
    description: 'testDescription',
    timeBegin: '03/30/2017 00:00',
    timeEnd: '03/31/2017 00:00'
}

describe('Event tab event creation', function () {
    it('should create a new event', function () {
        browser.get('http://localhost:63342/WebApp/app/#event');
        var EC = protractor.ExpectedConditions;

        var searchResultsUl = element(by.css('.listResults'));

        browser.wait(EC.presenceOf(searchResultsUl), 5000).then(function () {

            var searchResults = element.all(by.css('li'));
            searchResults.count().then(function (oldCount) {

                element.all(by.css('[ng-click="eventTab.fpControls.slideDown()"]')).then(function (scrollDownList) {


                    scrollDownList[0].click().then(function () {

                        var typeInput = element(by.css('select'));
                        browser.wait(EC.visibilityOf(typeInput), 5000).then(function () {
                            typeInput.click().then(function () {

                                element(by.xpath('//option[contains(., "Open")]')).click().then(function () {
                                    scrollDownList[1].click();

                                    var nameInput = element(by.model("eventTab.creating.name"));
                                    browser.wait(EC.visibilityOf(nameInput), 5000).then(function () {
                                        nameInput.sendKeys(newEvent.name);
                                        scrollDownList[2].click();

                                        var locationInput = element(by.model("eventTab.creating.location"));
                                        browser.wait(EC.visibilityOf(locationInput), 5000).then(function () {
                                            locationInput.sendKeys(newEvent.location);
                                            scrollDownList[3].click();

                                            var roomInput = element(by.model("eventTab.creating.room"));
                                            browser.wait(EC.visibilityOf(roomInput), 5000).then(function () {
                                                roomInput.sendKeys(newEvent.room);
                                                scrollDownList[4].click();

                                                var descriptionInput = element(by.model("eventTab.creating.description"));
                                                browser.wait(EC.visibilityOf(descriptionInput), 5000).then(function () {
                                                    descriptionInput.sendKeys(newEvent.description);
                                                    scrollDownList[5].click();

                                                    var timeBeginInput = element(by.model("eventTab.creating.timeBegin"));
                                                    browser.wait(EC.visibilityOf(timeBeginInput), 5000).then(function () {
                                                        timeBeginInput.sendKeys(newEvent.timeBegin);
                                                        scrollDownList[6].click();

                                                        var timeEndInput = element(by.model("eventTab.creating.timeEnd"));
                                                        browser.wait(EC.visibilityOf(timeEndInput), 5000).then(function () {
                                                            timeEndInput.sendKeys(newEvent.timeEnd).then(function () {

                                                                element(by.css('[ng-click="eventTab.submit()"]')).click().then(function () {

                                                                    var createdEvent = element(by.xpath('//ul[contains(@class, "listResults")]//span[contains(., "' + newEvent.name + '")]'))
                                                                    browser.wait(EC.presenceOf(createdEvent), 5000).then(function () {
                                                                        element.all(by.css('li')).count().then(function (newCount) {
                                                                            expect(newCount).toEqual(oldCount + 1);
                                                                        });
                                                                    }, function (error) {
                                                                        console.log("created event did not display");
                                                                        expect(false).toBe(true);
                                                                    });
                                                                });
                                                            });
                                                        });
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        }, function (error) {
            console.log("event list did not display");
            expect(false).toBe(true);
        });
    });
});