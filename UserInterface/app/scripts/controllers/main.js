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
    vm.deletePost = deletePost;

    loadPosts();

    function deletePost(id){
      $http.delete("http://localhost:8080/posts/" + id).then(loadPosts);
    }

    function loadPosts(){
      $http.get("http://localhost:8080/posts").then(postsHaveLoaded);
    }

    function postsHaveLoaded(response){
      vm.posts = response.data;
    }

  });
