// countryEditorController.js
var map;

(function () {
    "use strict";

    angular.module("app-countries")
        .controller("countryEditorController", countryEditorController);

    function countryEditorController($routeParams, $http) {
        var vm = this;

        vm.countryName = $routeParams.countryName;
        vm.cities = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newCity = {};

        var url = "/api/countries/" + vm.countryName + "/cities";


        $http.get(url)
            .then(function (response) {
                //success
                angular.copy(response.data, vm.cities);
                _showmap(vm.cities);
            }, function () {
                //fail
                console.log("fail");
                vm.errorMessage = "Failed to load data: ";
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addCity = function () {
             vm.errorMessage ="";
             vm.isBusy = true;

             var alreadyUsed = false;

             console.log(vm.newCity);
             var name = capitalizeFirstLetter(vm.newCity.nameCity);
             vm.newCity.nameCity = name;

             vm.cities.forEach(function (city) {
                 var cityName = city.nameCity;
                 if (cityName === name) {
                     alreadyUsed = true;
                 }
             });

             if (alreadyUsed) {
                 vm.errorMessage = "City already in the database.";
             }
             else {
                 $http.post(url, vm.newCity)
                     .then(function (response) {
                         //success
                         console.log(response);
                         vm.cities.push(response.data);
                         _markmap(response.data);
                         vm.newCity = {};
                     }, function (err) {
                         //fail
                        vm.errorMessage = "Failed to save new city - Either a server error or city not existing.";
                     });
             }
             vm.isBusy = false;
        };

    }

    function capitalizeFirstLetter(string) {
        var splitStr = string.toLowerCase().split(' ');
        for (var i = 0; i < splitStr.length; i++) {
            splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
        }
        return splitStr.join(' '); 
    }

    function _showmap(cities) {
        var centerLat = 51.476852;
        var centerLong = -0.000500;
        
        if (cities && cities.length > 0) {
            centerLat = cities[0].latitude;
            centerLong = cities[0].longitude;
        }
        var corner1 = L.latLng(85, 180),
            corner2 = L.latLng(-85, -180),
            bounds = L.latLngBounds(corner1, corner2);

        map = L.map('worldmap', {
            maxBounds: bounds,
            center: [centerLat, centerLong],
            zoom: 5
            
        });

        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 10,
            id: 'mapbox.pencil',
            //id: 'mapbox.satellite',
            accessToken: 'pk.eyJ1IjoibnZhbHNhbWlzIiwiYSI6ImNqN3VrYmZjZDU4ZGYzMnFrYnAwdzltMWsifQ.d9s0-yupcoWNsQIm-bXI6g'
        }).addTo(map);

        if (cities && cities.length > 0) {

            cities.forEach(function (city) {
                var name = city.nameCity;
                var lat = city.latitude;
                var long = city.longitude;
                var marker = L.marker([lat, long]).addTo(map);
                marker.bindPopup("<b>This is " + name + "!</b > <br>Latitude: " + lat + "<br>Longitude: " + long);
            });

            
        }
    }

    function _markmap(city) {
        var name = city.nameCity;
        var lat = city.latitude;
        var long = city.longitude;
        var marker = L.marker([lat, long]).addTo(map);
        marker.bindPopup("<b>This is " + name + "!</b > <br>Latitude: " + lat + "<br>Longitude: " + long);
        map.setView(new L.LatLng(lat, long), 5);
    }

})();