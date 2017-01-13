var input = "61923560";

describe('student validation center student search', function() {
    it('should return a list of student corresponding to the search criteria', function() {
        browser.get('https://concordiaidclient.netlify.com/WebApp/app/review');

        element(by.id('searchterm')).sendKeys(input);
        element(by.id('search')).click();

        var searchResults = element.all(by.id('searchResults'));
        expect(searchResults.count()).toEqual(1);
    });
});