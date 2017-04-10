"use strict";
(function () {
    angular.module("application")
           .controller("homeCtrl", ["$scope", "entityService",
               function ($scope, entityService) {
               	$scope.saveTutorial1 = function (event, tutorial) {
               		entityService.saveTutorial1(tutorial, event)
                                    .then(function (data) {
									var myEl1 = angular.element(document.querySelector("#FileContent1"));
										myEl1.html(data);
                               
                                    });
               	};

               	$scope.saveTutorial2 = function (event, tutorial) {
               		entityService.saveTutorial2(tutorial, event)
                                    .then(function (data) {
                                    	var myEl2 = angular.element(document.querySelector("#FileContent2"));
                                    	myEl2.html(data);
                                    });
               	};

               	$scope.compareFiles = function (event, tutorial) {
               		entityService.compareFiles(tutorial, event)
                                    .then(function (data) {
                                    	var myEl2 = angular.element(document.querySelector("#FilesDiff"));
                                    	myEl2.html(data);
                                    });
               	};
               }]);
})();