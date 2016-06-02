'use strict';

// Declare app level module which depends on filters, and services
var learniApp = angular.module('learniApp', ['ngRoute', 'restangular', 'ui.bootstrap', 'angularFileUpload'])
    .config(function(RestangularProvider) {
        RestangularProvider.setBaseUrl('http://localhost:59643/api/');
        RestangularProvider.setRestangularFields({ id: "id" });
    })
    .config(['$routeProvider', function ($routeProvider) {
      $routeProvider.when('/login', { templateUrl: 'App/templates/Login.html', controller: 'LoginController' });
      $routeProvider.when('/categories', { templateUrl: 'App/templates/Categories.html', controller: 'CategoriesController' });
      $routeProvider.when('/packages', { templateUrl: 'App/templates/Packages.html', controller: 'PackagesController' });
      $routeProvider.when('/package/:packageId', { templateUrl: 'App/templates/PackageDetails.html', controller: 'PackageDetailsController' });
      $routeProvider.otherwise({ redirectTo: '/packages' });
    }])
    .run( function($rootScope, $location) {
        $rootScope.$on( "$routeChangeStart", function(event, next, current) {
            if ($rootScope.userAuthenticated != true) {
                if (next.templateUrl != "App/templates/Login.html") {
                    $location.path("/login");
                } 
            }         
        });
    })