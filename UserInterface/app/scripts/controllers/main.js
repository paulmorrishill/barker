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
    vm.post = createPost;
    vm.barkBoxKeyPressed = barkBoxKeyPressed;
    vm.creatingPost = false;

    loadPosts();

    function createPost(content){
      if(vm.creatingPost) return;
      vm.creatingPost = true;
      $http.post("http://localhost:8080/posts", {
        content: content
      }).then(handleCreatePostResponse);
    }

    function handleCreatePostResponse(response){
      vm.creatingPost = false;
      vm.lastResponse = response.data;
      if(!vm.lastResponse.Successful) return;
      vm.newPost = "";
      loadPosts();
    }

    function barkBoxKeyPressed(){
      vm.lastResponse = null;
    }

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
