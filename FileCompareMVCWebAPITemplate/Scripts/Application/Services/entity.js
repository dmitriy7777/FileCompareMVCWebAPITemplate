"use strict";
(function () {
    angular.module("application")
           .factory("entityService", ["akFileUploaderService", function (akFileUploaderService) {
			
           	var saveTutorial1 = function (tutorial, event) {
           		return akFileUploaderService.saveModel(tutorial, "/home/loadFile1");
           	};

           	var saveTutorial2 = function (tutorial, event) {
           		return akFileUploaderService.saveModel(tutorial, "/home/loadFile2");
           	};

           	var compareFiles = function (tutorial, event) {
           		return akFileUploaderService.saveModel(tutorial, "/home/compareFiles");
           	};
           
               return {
               	saveTutorial1: saveTutorial1,
               	saveTutorial2: saveTutorial2,
               	compareFiles: compareFiles
               };
           }]);
})();