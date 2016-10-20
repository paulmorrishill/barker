'use strict';

describe('the timeAgo filter', function () {

  // load the controller's module
  beforeEach(module('userInterfaceApp'));

  var timeAgo;
  const oneMinute = 60;
  const oneHour = 60 * oneMinute;
  const oneDay = 24 * oneHour;
  const oneWeek = 7 * oneDay;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope, timeAgoFilter) {
    timeAgo = timeAgoFilter;
  }));

  it('can generate a nice time ago description of a date', function(){
    assertOutputMatches(new Date(), "a few seconds ago");
    assertNumberOfSecondsAgoOutputs(47, "a minute ago");
    assertNumberOfSecondsAgoOutputs(120, "2 minutes ago");
    assertNumberOfSecondsAgoOutputs(2 * oneHour, "2 hours ago");
    assertNumberOfSecondsAgoOutputs(9 * oneDay, "9 days ago");
    assertNumberOfSecondsAgoOutputs(46 * oneDay, "2 months ago");
    assertNumberOfSecondsAgoOutputs(366 * oneDay, "a year ago");
    assertNumberOfSecondsAgoOutputs(3 * 365 * oneDay, "3 years ago");
  });

  function assertNumberOfSecondsAgoOutputs(numberOfSecondsAgo, expectedOutput){
    assertOutputMatches(getDateNumberOfSecondsAgo(numberOfSecondsAgo), expectedOutput);
  }

  function getDateNumberOfSecondsAgo(numberOfSeconds){
    var numberOfSecondsAgoInMillisecondUnixTime = new Date().getTime() - numberOfSeconds * 1000;
    return new Date(numberOfSecondsAgoInMillisecondUnixTime);
  }

  function assertOutputMatches(dateInput, expectedOuput){
    expect(timeAgo(dateInput)).to.equal(expectedOuput);
  }

});
