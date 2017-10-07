// app-countries.js

(function () {

    "use strict";

    //creating the module
    angular.module("app-countries", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {

            $routeProvider.when("/", {
                controller: "countriesController",
                controllerAs: "vm",
                templateUrl: "/views/countriesView.html"
            });

            $routeProvider.when("/editor/:countryName", {
                controller: "countryEditorController",
                controllerAs: "vm",
                templateUrl: "/views/countryEditorView.html"
            });


            $routeProvider.otherwise({ redirectTo: "/" })
        }).config(['$locationProvider', function ($locationProvider) {
            $locationProvider.hashPrefix('');
        }]);

})();