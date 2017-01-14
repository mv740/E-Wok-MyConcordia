

describe('student validation center student search', function() {
    it('should return a list of student corresponding to the search criteria', function() {
        browser.get('https://concordiaidclient.netlify.com/WebApp/app/review');
        var EC = protractor.ExpectedConditions;
        var id = "61923560";
        element(by.id('searchterm')).sendKeys(id);
        element(by.id('search')).click();


        var searchResultsUl = element(by.id('searchResults'));
        browser.wait(EC.presenceOf(searchResultsUl), 5000).then(function(){
            var searchResults = element.all(by.repeater('result in search.results'));
            expect(searchResults.count()).toEqual(1);

            //open the modal
            searchResults.each(function (el){
                el.click();
            });

            var spanWithId = element(by.xpath('//div[contains(@class, "validationModal")]//span[contains(., "61923560")]'));
            browser.wait(EC.presenceOf(spanWithId), 5000).then(function(){
                expect(spanWithId.getText()).toEqual(id);
            }, function (error) {
                console.log("validationModal did not display");
                expect(true).toBe(false);
            });
        }, function (error) {
            console.log("searchResults did not display");
            expect(false).toBe(true);
        });


    });
});