'use strict';

/**
 * @ngdoc overview
 * @name userInterfaceApp
 * @description
 * # userInterfaceApp
 *
 * Main module of the application.
 */
angular
  .module('userInterfaceApp', [
    'ngAnimate',
    'ngRoute',
    'ngSanitize',
    'ngTouch'
  ])
  .config(function ($routeProvider) {
    $routeProvider
      .when('/', {
        templateUrl: 'routes/home/home.html',
        controller: 'HomeCtrl',
        controllerAs: 'vm'
      })
      .otherwise({
        redirectTo: '/'
      });
  });
