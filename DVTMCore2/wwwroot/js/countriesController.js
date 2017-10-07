// countriesController.js

(function () {

    "use strict";

     //Getting the existing module
    angular.module("app-countries")
        .controller("countriesController", countriesController);

    function countriesController($http) {

        var vm = this;

        vm.countries = [];

        vm.newCountry = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/countries")
            .then(function (response) {
                //success
                angular.copy(response.data, vm.countries);
            }, function () {
                //fail
                console.log("fail");
                vm.errorMessage = "Failed to load data: ";
            })
            .finally(function () {
                 vm.isBusy = false;
            });

        vm.addCountry = function () {

            vm.isBusy = true;
            vm.errorMessage = "";
            var exists = false;
            var alreadyUsed = false;


            var name = capitalizeFirstLetter(vm.newCountry.nameCountry);
            vm.newCountry.nameCountry = name;

            for (var i = 0; i < country_list.length - 1; i++) {
                if (country_list[i] === name) {
                    console.log(country_list[i]);
                    console.log(name);
                    exists = true;
                }
            }

            vm.countries.forEach(function (country) {
                var countryName = country.nameCountry;
                if (countryName === name) {
                    alreadyUsed = true;
                }
            });

            if (exists && !alreadyUsed) {

                $http.post("/api/countries", vm.newCountry)
                .then(function(response){
                    //success
                    vm.countries.push(response.data);
                    vm.newCountry = {};
                }, function () {
                    //fail
                    vm.errorMessage = "Failed to save new country";
            });
            }
            else if (exists && alreadyUsed)
            {
                vm.errorMessage = "Country already in the database.";
            }
            else if (!exists ) {
                vm.errorMessage = name + " is not a real country.";
            }

            vm.isBusy = false;
        };

        function capitalizeFirstLetter(string) {
            var splitStr = string.toLowerCase().split(' ');
            for (var i = 0; i < splitStr.length; i++) {
                splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
            }
            return splitStr.join(' '); 
        }

    }

})();