'use strict';

/**
 * @ngdoc function
 * @name userInterfaceApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the userInterfaceApp
 */
angular.module('userInterfaceApp')
  .controller('MainCtrl', function ($http) {
    var vm = this;
    vm.posts = [];
    $http.get("http://localhost:8080/posts").then(function(response){
      vm.posts = response.data;
    });
  });
