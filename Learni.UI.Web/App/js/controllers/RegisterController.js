'use strict';

learniApp.controller('RegisterController',
    function RegisterController($scope, $modalInstance, usersService) {

        $scope.register = function (user, registerForm) {
            if (registerForm.$valid) {
                usersService.register(user).then(function () {
                    $modalInstance.close();
                }, function () {
                    console.log("There was an error while registering.");
                });
            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    });