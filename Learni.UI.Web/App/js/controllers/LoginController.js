'use strict';

learniApp.controller('LoginController',
    function LoginController($scope, $rootScope, $location, $modal, usersService) {

        $scope.isError = false;
        $scope.login = function (user, loginForm) {
            if (loginForm.$valid) {
                usersService.login(user).then(function (loggedUser) {
                    $rootScope.userAuthenticated = true;
                    $location.path('/packages');
                }, function () {
                    $scope.isError = true;
                    console.log("There was an error while logging.");
                });
            }
        };

        $scope.register = function () {
            var modalInstance = $modal.open({
                templateUrl: 'App/templates/Register.html',
                controller: 'RegisterController'
            });

            modalInstance.result.then(function () {
                $location.path('/login');
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };

    });