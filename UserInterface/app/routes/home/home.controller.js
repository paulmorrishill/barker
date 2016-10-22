'use strict';

/**
 * @ngdoc function
 * @name userInterfaceApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the userInterfaceApp
 */
angular.module('userInterfaceApp')
  .controller('HomeCtrl', function ($http) {
    var vm = this;
    vm.posts = [];
    vm.deletePost = deletePost;
    vm.post = createPost;
    vm.barkBoxKeyPressed = barkBoxKeyPressed;
    vm.creatingPost = false;

    loadPosts();

    function createPost(content){
      vm.messageWasDeleted = false;
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
      vm.messageWasDeleted = false;
    }

    function showDeleteSuccessMessage(){
      vm.messageWasDeleted = true;
    }

    function deletePost(id){
      vm.lastResponse = null;
      $http.delete("http://localhost:8080/posts/" + id)
        .then(showDeleteSuccessMessage)
        .then(loadPosts);
    }

    function loadPosts(){
      $http.get("http://localhost:8080/posts").then(postsHaveLoaded);
    }

    function postsHaveLoaded(response){
      vm.posts = response.data;
    }

  });
