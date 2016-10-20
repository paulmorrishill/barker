'use strict';

angular.module('userInterfaceApp')
  .filter('timeAgo', function () {
    return function(date){
      var inputMoment = moment(date);
      return inputMoment.fromNow();
    };
  });
